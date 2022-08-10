using System.Collections.Generic;

namespace DTO.Entidades
{
    public class Produto
    {
        public class Dados
        {
            public string usuario { get; set; }
            public string senha { get; set; }
            public NewProduto produto { get; set; }
        }

        public class NewProduto
        {
            public string cd_empresa { get; set; }
            public string cd_deposito { get; set; }
            public string cd_produto { get; set; }
            public string ds_produto { get; set; }
            public string ds_reduzida { get; set; }
            public string cd_unidade_medida { get; set; }
            public string ds_unidade_medida { get; set; }
            public string id_aceita_decimal { get; set; }
            public string cd_embalagem { get; set; }
            public string ds_embalagem { get; set; }
            public string qt_unidade_embalagem { get; set; }
            public string cd_produto_master { get; set; }
            public string qt_itens { get; set; }
            public string cd_familia { get; set; }
            public string ds_familia { get; set; }
            public string vl_altura { get; set; }
            public string vl_largura { get; set; }
            public string vl_profundidade { get; set; }
            public string ps_liquido { get; set; }
            public string ps_bruto { get; set; }
            public string qt_max_palete { get; set; }
            public string cd_situacao { get; set; }
            public string cd_rotatividade { get; set; }
            public string cd_classe { get; set; }
            public string ds_classe { get; set; }
            public string qt_dias_validade { get; set; }
            public string qt_dias_remonte { get; set; }
            public string id_controle_lote { get; set; }
            public string id_controle_serie { get; set; }
            public string id_controle_validade { get; set; }
            public string qt_caixa_fechada { get; set; }
            public string cd_fornecedor { get; set; }
            public string cd_cnpj_fornecedor { get; set; }
            public string cd_produto_fornecedor { get; set; }
            public string cd_linha { get; set; }
            public string ds_linha { get; set; }
            public string cd_grupo { get; set; }
            public string ds_grupo { get; set; }
            public string cd_subgrupo { get; set; }
            public string ds_subgrupo { get; set; }
            public string cd_modelo { get; set; }
            public string ds_modelo { get; set; }
            public string tp_armazenagem_produto { get; set; }
            public string dt_addrow { get; set; }
            public string id_processado { get; set; }
            public string dt_processado { get; set; }
            public string ps_infAdicional1 { get; set; }
            public string ps_infAdicional2 { get; set; }
            public string ps_infAdicional3 { get; set; }
            public string cd_produto_relacionado { get; set; }
            public string cd_Campanha { get; set; }
            public string cd_ncm { get; set; }
        }
        public class NewDados
        {
            public string usuario { get; set; }
            public string senha { get; set; }
            public Recebimento recebimento { get; set; }
        }
        public class Recebimento
        {
            public string cd_empresa { get; set; }
            public string cd_deposito { get; set; }
            public string cd_agenda { get; set; }
            public string nu_nota { get; set; }
            public string nu_serie_nota { get; set; }
            public string cd_porta { get; set; }
            public string cd_transportadora { get; set; }
            public string ds_transportadora { get; set; }
            public string cd_cnpj_transportadora { get; set; }
            public string cd_fornecedor { get; set; }
            public string cd_cnpj_fornecedor { get; set; }
            public string dt_emissao { get; set; }
            public string ds_placa { get; set; }
            public string dt_agendamento { get; set; }
            public string cd_situacao { get; set; }
            public string cd_tipo_nota { get; set; }
            public string nu_doc_erp { get; set; }
            public string ds_area_erp { get; set; }
            public string cd_rav { get; set; }
            public string dt_addrow { get; set; }
            public string id_processado { get; set; }
            public string dt_processado { get; set; }
            public string cd_campanhaId { get; set; }
            public string cd_InfosAdicionais2 { get; set; }
            public string cd_InfosAdicionais3 { get; set; }
            public string cd_InfosAdicionais4 { get; set; }
            public string cd_InfosAdicionais5 { get; set; }
            public string cd_InfosAdicionais6 { get; set; }
            public string nu_chaveNotaFiscal { get; set; }
            public string cd_tipo_doc_ori { get; set; }
            public string nu_ori_doc_erp { get; set; }
            public string nu_CFOP { get; set; }
            public string cd_tipo_doc { get; set; }
            public string cd_cia { get; set; }
            public List<Detalhe> detalhe { get; set; }
        }

        public class Detalhe
        {
            public string cd_empresa { get; set; }
            public string cd_deposito { get; set; }
            public string cd_cliente { get; set; }
            public string cd_agenda { get; set; }
            public string nu_nota { get; set; }
            public string nu_serie_nota { get; set; }
            public string cd_fornecedor { get; set; }
            public string cd_cgc_fornecedor { get; set; }
            public string cd_situacao { get; set; }
            public string nu_item_corp { get; set; }
            public string cd_produto { get; set; }
            public string qt_produto { get; set; }
            public string nu_lote { get; set; }
            public string nu_lote_fornecedor { get; set; }
            public string dt_fabricacao { get; set; }
            public string dt_addrow { get; set; }
            public string id_processado { get; set; }
            public string dt_processado { get; set; }
            public string nu_valor_unitario { get; set; }
            public string cd_unidademedida { get; set; }
            public string nu_info01 { get; set; }
            public string nu_lin_doc { get; set; }
            public string ds_classe_prod { get; set; }
            public string cd_tipo_doc { get; set; }
            public string cd_cia { get; set; }
            public string dt_vencimento { get; set; }
        }
    }
}
