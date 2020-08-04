using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias.Model
{
    class OcorrenciaConhecimento
    {
        public DateTime DataOcorrencia { get; set; }

        public int Seq { get; set; }

        public int CodigoOcorrencia { get; set; }

        public string Local { get; set; }

        public string DescricaoOcorrencia { get; set; }

        public string RecebidoPor { get; set; }

        public int SatusEnviado { get; set; }

        public int LogRegister { get; set; }

        public string UkeyConhecimento { get; set; }
        //Previsão de entrega, Nome do destinatário, CEP, UF e Município
        public int Prev_Entrega { get; set; }

        public string NOME_RECIBIMENTO { get; set; }

        public string CEP_DESTINATARIO { get; set; }

        public string ESTADO_DESTINATARIO { get; set; }

        public string MUNICIPIO_DESTINATARIO { get; set; }
    }
}
