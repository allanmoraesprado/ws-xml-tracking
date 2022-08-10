using System;
using DTO.Entidades;

namespace DAL
{
    public class Query : Conexao
    {
        private string clienteid = System.Configuration.ConfigurationManager.AppSettings["clienteid"];
        private string tipointegracao = System.Configuration.ConfigurationManager.AppSettings["tipointegracao"];
        private string idimportacao = System.Configuration.ConfigurationManager.AppSettings["idimportacao"];

        public DadosIntegracao ConsultaDadosIntegracao()
        {
            var retorno = new DadosIntegracao();
            try
            {
                string sqlQuery = $@"select * from Integracoes WITH(NOLOCK) where cliente = {clienteid} and tipointegracao = '{tipointegracao}' and idimportacao = {idimportacao};";
                retorno = ExecutaSelect<DadosIntegracao>(sqlQuery);
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
            }
            return retorno;
        }
    }
}

