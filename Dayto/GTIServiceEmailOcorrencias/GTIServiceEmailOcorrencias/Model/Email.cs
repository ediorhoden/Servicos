using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias.Model
{
    class Email
    {
        public string EmailToSend { get; set; }

        public string NR_HAWB { get; set; }

        public string Nome { get; set; }


        public List<OcorrenciaConhecimento> Ocorrencias { get; set; }


    }
}
