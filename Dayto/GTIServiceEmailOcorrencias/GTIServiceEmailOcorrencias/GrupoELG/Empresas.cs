using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias.GrupoELG
{
    class Empresas
    {
        public string CNPJ { get; set; }
        public List<string> emails = new List<string>();
        public string relType { get; set; }


    }
}
