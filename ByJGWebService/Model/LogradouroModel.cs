using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ByJGWebService.Model
{
    [Serializable]
    public class LogradouroModel
    {
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Codigo { get; set; }
        public string Erro { get; set; }
    }
}