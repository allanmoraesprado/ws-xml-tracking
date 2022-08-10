using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using Limilabs.Mail.MIME;
using DAL;
using DTO.Entidades;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Net;
using RestSharp;

namespace BLL
{
    public class Processar
    {
        public class XML { public string xml { get; set; }}
        public string ProcessarXML()
        {
            try
            {
                var dadosIntegracao = new Query().ConsultaDadosIntegracao();
                if (dadosIntegracao.cliente > 0)
                {
                    var arquivos = GetArquivo(dadosIntegracao);
                    foreach (var item in arquivos)
                    {
                        var xml = item.xml;
                        var produto = MontaProduto(xml);
                        var jsonProd = EnviaJsonProduto(produto);
                        if (jsonProd == true)
                        {
                            var recebimento = MontaRecebimento(xml);
                            var jsonRec = EnviaJsonRecebimento(recebimento);
                            if (jsonRec == true)
                                MoveArquivoProcessado(dadosIntegracao, xml);
                            else
                                MoveArquivoErro(dadosIntegracao, xml);
                        }
                        else
                        {
                            MoveArquivoErro(dadosIntegracao, xml);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                var erro = ex.Message;
            }
            return "";
        }

        public List<XML> GetArquivo(DadosIntegracao dados)
        {
            var lista = new List<XML>();
            try
            {
                using (Imap imap = new Imap())
                {
                    var host = System.Configuration.ConfigurationManager.AppSettings["HOST"];
                    var user = System.Configuration.ConfigurationManager.AppSettings["EMAIL_LOGIN"];
                    var pass = System.Configuration.ConfigurationManager.AppSettings["EMAIL_SENHA"];

                    var dir = dados.diretorioentrada.Replace(@"\\", @"\");
                    var bkpDir = $@"{dados.diretorioentrada}Backup\".Replace(@"\\", @"\");

                    imap.ConnectSSL(host);
                    imap.UseBestLogin(user, pass);
                    imap.SelectInbox();
                    List<long> uids = imap.Search(Flag.Unseen);

                    foreach (long uid in uids)
                    {
                        try
                        {
                            var token = DateTime.Now.Ticks;
                            IMail email_ = new MailBuilder().CreateFromEml(imap.GetMessageByUID(uid));
                            foreach (MimeData mime in email_.Attachments.Where(x => x.SafeFileName.Contains("")))
                            {
                                string extensao = Path.GetExtension(mime.SafeFileName);

                                if (extensao == ".xml")
                                {
                                    if (!Directory.Exists(dir))
                                        Directory.CreateDirectory(dir);
                                    var arquivo = dir + token + "_" + mime.SafeFileName;
                                    var backup = bkpDir + token + "_" + mime.SafeFileName;
                                    mime.Save(arquivo);
                                    mime.Save(backup);
                                    var newXml = new XML() { xml = arquivo };                                   
                                    lista.Add(newXml);                                    
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var erro = ex.Message;
                            continue;
                        }
                    }
                    imap.Close();
                }
            }
            catch (Exception ex)
            {
                var erro = ex.Message;   
            }
            return lista;
        }
        public string MontaProduto(string arquivo)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arquivo);
                var serializer = new XmlSerializer(typeof(PedidoXml.NfeProc));
                var buffer = Encoding.UTF8.GetBytes(doc.OuterXml);
                var reader = new StringReader(Encoding.UTF8.GetString(buffer));
                string s = reader.ReadToEnd();
                PedidoXml.NfeProc nf = null;
                nf = Deserialize<PedidoXml.NfeProc>(s);
                PedidoXml.NfeProc nfProc = null;
                nfProc = Deserialize<PedidoXml.NfeProc>(s);

                var dateNow = DateTime.Now;
                var cdProduto = "";
                var dsProduto = "";
                var dsReduzida = "";
                var cdUnidadeMedida = "";
                var dsUnidadeMedida = "";
                var cdProdutoMaster = "";

                foreach (var item in nf.NFe.InfNFe.Det)
                {
                    cdProduto = item.Prod.CProd;
                    dsProduto = item.Prod.XProd;
                    dsReduzida = item.Prod.XProd;
                    cdUnidadeMedida = item.Prod.QCom;
                    dsUnidadeMedida = item.Prod.QCom;
                    cdProdutoMaster = item.Prod.CProd;
                }

                var produto = new Produto.Dados()
                {
                    usuario = "allan.moraes|1|2|0",
                    senha = "xmltracking55",
                    produto = new Produto.NewProduto()
                    {
                        cd_empresa = "1",
                        cd_deposito = "1",
                        cd_produto = cdProduto,
                        ds_produto = dsProduto,
                        ds_reduzida = dsReduzida,
                        cd_unidade_medida = cdUnidadeMedida,
                        ds_unidade_medida = dsUnidadeMedida,
                        id_aceita_decimal = "N",
                        cd_embalagem = null,
                        ds_embalagem = null,
                        qt_unidade_embalagem = null,
                        cd_produto_master = cdProdutoMaster,
                        qt_itens = "",
                        cd_familia = null,
                        ds_familia = null,
                        vl_altura = null,
                        vl_largura = null,
                        vl_profundidade = null,
                        ps_liquido = "0",
                        ps_bruto = "0",
                        qt_max_palete = null,
                        cd_situacao = "1",
                        cd_rotatividade = "NC",
                        cd_classe = null,
                        ds_classe = null,
                        qt_dias_validade = null,
                        qt_dias_remonte = null,
                        id_controle_lote = null,
                        id_controle_serie = null,
                        id_controle_validade = null,
                        qt_caixa_fechada = null,
                        cd_fornecedor = "",
                        cd_produto_fornecedor = null,
                        cd_cnpj_fornecedor = nfProc.NFe.InfNFe.Emit.CNPJ.ToString(),                        
                        cd_linha = null,
                        ds_linha = null,
                        cd_grupo = null,
                        ds_grupo = null,
                        cd_subgrupo = null,
                        ds_subgrupo = null,
                        cd_modelo = null,
                        ds_modelo = null,
                        tp_armazenagem_produto = null,
                        dt_addrow = dateNow.ToString(),
                        id_processado = "N",
                        dt_processado = null,
                        ps_infAdicional1 = null,
                        ps_infAdicional2 = null,
                        ps_infAdicional3 = null,
                        cd_produto_relacionado = "",
                        cd_Campanha = null,
                        cd_ncm = "1"
                    }
                };
                var json = JsonConvert.SerializeObject(produto);

                return json;
            }
            catch(Exception ex)
            {
                var erro = ex.Message;
            }
            return "";
        }
        public bool EnviaJsonProduto(string json)
        {
            try
            {
                var client = new RestClient("https://apiflux.sequoialog.com.br/interfaceflux/Entrada/produtoV2");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                if (!string.IsNullOrWhiteSpace(json))                
                    request.AddParameter("application/json", json, ParameterType.RequestBody);                
                IRestResponse response = client.Execute(request);
                var res = response.Content;
                var status = response.StatusCode;

                if (!String.IsNullOrEmpty(response.Content))
                {
                    if (status == HttpStatusCode.OK && res.Contains("PROCESSAMENTO EFETUADO COM SUCESSO"))
                        return true;
                }
            }
            catch(Exception ex)
            {
                var erro = ex.Message;
            }
            return false;
        }
        public string MontaRecebimento(string arquivo)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arquivo);
                var serializer = new XmlSerializer(typeof(PedidoXml.NfeProc));
                var buffer = Encoding.UTF8.GetBytes(doc.OuterXml);
                var reader = new StringReader(Encoding.UTF8.GetString(buffer));
                string s = reader.ReadToEnd();
                PedidoXml.NfeProc nf = null;
                nf = Deserialize<PedidoXml.NfeProc>(s);
                PedidoXml.NfeProc nfProc = null;
                nfProc = Deserialize<PedidoXml.NfeProc>(s);
                Produto prod = new Produto();

                var dateNow = DateTime.Now;
                         
                var recebimento = new Produto.NewDados()
                {
                    usuario = "thiago.schultz|1|2|0",
                    senha = "8585Gta85",
                    recebimento = new Produto.Recebimento()
                    {
                        cd_empresa = "1",
                        cd_deposito = "1",
                        cd_agenda = "1",
                        nu_nota = nfProc.NFe.InfNFe.Ide.NNF.ToString(),
                        nu_serie_nota = nfProc.NFe.InfNFe.Ide.Serie.ToString(),
                        cd_porta = "1",
                        cd_transportadora = null,
                        ds_transportadora = null,
                        cd_cnpj_transportadora = null,
                        cd_fornecedor = "1", //ajustar
                        cd_cnpj_fornecedor = nfProc.NFe.InfNFe.Emit.CNPJ.ToString(),
                        dt_emissao = dateNow.ToString(),
                        ds_placa = null,
                        dt_agendamento = null,
                        cd_situacao = null,
                        cd_tipo_nota = null,
                        nu_doc_erp = "1", //ajustar
                        ds_area_erp = null,
                        cd_rav = null,
                        dt_addrow = null,
                        id_processado = null,
                        dt_processado = null,
                        cd_campanhaId = null,
                        cd_InfosAdicionais2 = null,
                        cd_InfosAdicionais3 = null,
                        cd_InfosAdicionais4 = null,
                        cd_InfosAdicionais5 = null,
                        cd_InfosAdicionais6 = null,
                        nu_chaveNotaFiscal = null,
                        cd_tipo_doc_ori = null,
                        nu_ori_doc_erp = null,
                        nu_CFOP = null,
                        cd_tipo_doc = "1", //ajustar
                        cd_cia = nfProc.NFe.InfNFe.Emit.CNPJ.ToString(),
                        detalhe = new List<Produto.Detalhe>()
                        {
                            new Produto.Detalhe()
                            {
                                cd_empresa = "1",
                                cd_deposito = "1",
                                cd_cliente = null,
                                cd_agenda = "1",
                                nu_nota = nfProc.NFe.InfNFe.Ide.NNF.ToString(),
                                nu_serie_nota = nfProc.NFe.InfNFe.Ide.Serie.ToString(),
                                cd_fornecedor = "1", //ajustar
                                cd_cgc_fornecedor = null,
                                cd_situacao = "1", //ajustar 
                                nu_item_corp = "1", //ajustar
                                cd_produto = null,
                                qt_produto = "1", //ajustar
                                nu_lote = null,
                                nu_lote_fornecedor = null,
                                dt_fabricacao = null,
                                dt_addrow = null,
                                id_processado = null,
                                dt_processado = null,
                                nu_valor_unitario = null,
                                cd_unidademedida = null,
                                nu_info01 = null,
                                nu_lin_doc = null,
                                ds_classe_prod = null,
                                cd_tipo_doc = null,
                                cd_cia = null,
                                dt_vencimento = null
                            }
                        }
                    }
                };
                var json = JsonConvert.SerializeObject(recebimento);

                return json;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
            }
            return "";
        }
        public bool EnviaJsonRecebimento(string json)
        {
            try
            {
                var client = new RestClient("https://apiflux.sequoialog.com.br/interfaceflux/Entrada/recebimento");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                if (!string.IsNullOrWhiteSpace(json))
                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var res = response.Content;
                var status = response.StatusCode;

                if (!String.IsNullOrEmpty(response.Content))
                {
                    if (status == HttpStatusCode.OK && res.Contains("PROCESSAMENTO EFETUADO COM SUCESSO"))
                        return true;
                }
            }
            catch(Exception ex)
            {
                var erro = ex.Message;
            }
            return false;
        }
        public void MoveArquivoProcessado(DadosIntegracao dados, string arquivo)
        {
            if(File.Exists(arquivo))
            {
                try
                {
                    var dir = dados.diretorioentrada;
                    var dest = $@"{dir}Processados\{arquivo.Replace(dir, "")}".Replace(@"\\", @"\");
                    File.Copy(arquivo, dest);
                    File.Delete(arquivo);
                }
                catch(Exception ex)
                {
                    var erro = ex.Message;
                }
            }
        }
        public void MoveArquivoErro(DadosIntegracao dados, string arquivo)
        {
            if (File.Exists(arquivo))
            {
                try
                {
                    var dir = dados.diretorioentrada;
                    var dest = $@"{dir}Erros\{arquivo.Replace(dir, "")}".Replace(@"\\", @"\");
                    File.Copy(arquivo, dest);
                    File.Delete(arquivo);
                }
                catch(Exception ex)
                {
                    var erro = ex.Message;
                }
            }
        }
        public static T Deserialize<T>(string xml)
        {
            Object entity = null;
            xml = RemoveXmlRoot(xml);          
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (TextReader stream = new StringReader(xml))
                {
                    entity = (T)ser.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
            }
            return (T)entity;
        }
        public static string RemoveXmlRoot(string item, string replace = "", string value = "")
        {
            const string root = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            int posicao = 0;
            posicao = item.IndexOf(root);
            if (posicao > -1)
                item = item.Substring((posicao - 1) + (root.Length + 1));

            if (item.Contains("<?xml version"))
            {
                item = item.TrimStart().Substring(38);
            }

            if ((replace.Length > 0) && (value.Length > 0))
            {
                item = item.Replace(replace, value);
            }
            return item;
        }
    }
}

