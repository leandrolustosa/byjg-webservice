using ByJGWebService.Model;
using System;
using System.Web.Services;

namespace ByJGWebService
{
    /// <summary>
    /// Summary description for webservice
    /// </summary>
    [WebService(Namespace = "http://byjg.ainbox.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class webservice : System.Web.Services.WebService
    {

        [WebMethod]
        public LogradouroModel ObterLogradouro(string user, string pass, string cep)
        {
            var cepusername = System.Configuration.ConfigurationManager.AppSettings["cepusername"];
            var ceppassword = System.Configuration.ConfigurationManager.AppSettings["ceppassword"];

            var logradouro = new LogradouroModel();
            if (string.IsNullOrEmpty(cepusername) || string.IsNullOrEmpty(ceppassword))
            {
                logradouro.Erro = "Usuário e senha da Consulta de CEP não estão configurados";
                return logradouro;
            }

            if (!(user.Equals(cepusername) || pass.Equals(ceppassword)))
            {
                logradouro.Erro = "Usuário ou senha inválidos.";
                return logradouro;
            }

            try
            {                
                var service = new ByJGCepService.CEPService();
                var byjgusername = System.Configuration.ConfigurationManager.AppSettings["byjgusername"];
                var byjgpassword = System.Configuration.ConfigurationManager.AppSettings["byjgpassword"];

                if (string.IsNullOrEmpty(byjgusername) || string.IsNullOrEmpty(byjgpassword))
                {
                    logradouro.Erro = "Usuário e senha ByJG não estão configurados";
                    return logradouro;
                }

                var logradouroStr = service.obterLogradouroAuth(cep, byjgusername, byjgpassword);
                var logradouroSplit = logradouroStr.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (logradouroSplit.Length < 5)
                {
                    logradouro.Erro = string.Join(", ", logradouroSplit);
                    return logradouro;
                }

                var hifenSplit = logradouroSplit[0].Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                
                logradouro.Logradouro = hifenSplit[0].Trim();
                logradouro.Bairro = logradouroSplit[1].Trim();
                logradouro.Cidade = logradouroSplit[2].Trim();
                logradouro.UF = logradouroSplit[3].Trim();
                logradouro.Codigo = logradouroSplit[4].Trim();

                return logradouro;
            }
            catch (Exception ex)
            {
                logradouro.Erro = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
                return logradouro;
            }
        }
    }
}
