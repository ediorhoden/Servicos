using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSerrviceIntAptus.XmlModelo
{
    public static class XmlModelo
    {

        public static string getXML()
        {
            return "<?xml version =\"1.0\" encoding=\"utf-8\"?> " +
                   "<soap:Envelope xmlns:soap =" + "http://www.w3.org/2003/05/soap-envelope" + "xmlns: rdap = " + "http://aptuscloud.com.br/rdapi/" + " >" +
                   "<soap:Header/>" +
                   "<soap:Body>" +
                   "< rdap:ObterAdiantamentosPendentes >" +
                   "< rdap:accessKey > 093659e3 - cdfa - 4cff - b77b - 871f3f1abe41 </ rdap:accessKey >" +
                   "< rdap:empresa > 1 </ rdap:empresa >" +
                   "</ rdap:ObterAdiantamentosPendentes >" +
                   "</ soap:Body >" +
                   "</soap:Envelope>http://www.w3.org/2003/05/soap-envelopehttp://www.w3.orgPra";



        }

    }
}
