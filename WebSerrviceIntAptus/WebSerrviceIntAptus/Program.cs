using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;


namespace WebSerrviceIntAptus
{
    class Program
    {
        static void Main(string[] args)
        {


            if (retornohoracerta())
            {



                string retorno = SOAPManual("ObterRelatoriosPendentesResumo", "", "", "");

                DataTable dt = new DataTable();
                dt.Columns.Add("Codigo");
                string CodigoAux = "";
                string DataVencimento = "";
                string Cpf = "";
                string DataEmissao = "";





                for (int i = 0; i <= RetornaElementoDoXml(retorno.ToString(), "Codigo").Count - 1; i++)
                {
                    var Codigo = Convert.ToString(RetornaElementoDoXml(retorno.ToString(), "Codigo").ElementAt(i));

                    if (CodigoAux != Codigo)
                    {

                        DataVencimento = RetornaElementoDoXml(retorno.ToString(), "DataPrevisaoPagamento").ElementAt(i);
                        DataEmissao = Convert.ToString(RetornaElementoDoXml(retorno.ToString(), "DataEnvio").ElementAt(i));
                        Cpf = Convert.ToString(RetornaElementoDoXml(retorno.ToString(), "UsuarioCodigo").ElementAt(i));
                        var ValorTotal = RetornaElementoDoXml(retorno.ToString(), "ValorTotal").ElementAt(i);

                        var data = DataEmissao;
                        var dataCorrigida = data.Insert(4, "-").Insert(7, "-");
                        var dataConvertidaEmissao = Convert.ToDateTime(dataCorrigida).ToString("dd/MM/yyyy");

                        var datavencimento = DataEmissao;
                        var dataCorrigidavencimento = data.Insert(4, "-").Insert(7, "-");
                        var dataConvertidavencimento = Convert.ToDateTime(dataCorrigidavencimento).ToString("dd/MM/yyyy");

                        string retornook = "";

                        DateTime Emissao = Convert.ToDateTime(dataConvertidaEmissao);
                        DateTime Vencimento = Convert.ToDateTime(dataConvertidavencimento);


                        retornook = inserereembolso(Codigo, Cpf, Emissao, Vencimento, ValorTotal);

                        if (retornook == "OK")
                        {
                            SOAPManual("AtualizarIntegracaoSucesso", Codigo, Codigo, "");
                        }
                        else
                        {

                            SOAPManual("AtualizarIntegracaoProblema", Codigo, Codigo, "");
                        }

                    }


                    CodigoAux = Codigo;

                }
            }

            DateTime horainicial = DateTime.Now;

            if (horainicial.Hour == 19 && horainicial.Minute == 0)
            {


                DataTable DtPgto = new DataTable();
                DtPgto = RetornaPgtoReebolso("", "");
                DataRow Dtrw;

                foreach (DataRow linha in DtPgto.Rows)
                {
                    string CodigoIntegracao = Convert.ToString(linha["id_titulo_aptus"]);
                    string Mensagem = "Reembolso pago no dia: " + Convert.ToString(linha["dat_pagamento"]);

                    var Codigo = "";
                    string retornoSituacao = SOAPManual("AtualizarIntegradoSituacao", Codigo, CodigoIntegracao, Mensagem);
                    string mensagem = Convert.ToString(RetornaElementoDoXml(retornoSituacao.ToString(), "MensagemRetorno").ElementAt(0));

                    RetornaPgtoReebolso(CodigoIntegracao, mensagem);

                }

            }
        }
		

        public static String SOAPManual(string metodo, string codigo, string codigointegracao, string mensagem)
        {


            const string url = "https://mooz.aptuscloud.com.br/rd-api/IE_RD.asmx?WSDL";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);


            //const string action = "http://aptuscloud.com.br/rdapi/ObterRelatoriosPendentesResumo";
            string action = "http://aptuscloud.com.br/rdapi/" + metodo;
            XmlDocument soapEnvelopeXml;
            if (metodo == "ObterRelatoriosPendentesResumo")
            {
                soapEnvelopeXml = CreateEnvelopeRelResumido(codigo, codigointegracao, mensagem);
                webRequest = CreateWebRequest(url, action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            }

            if (metodo == "AtualizarIntegracaoSucesso")
            {
                soapEnvelopeXml = CreateXmlAtualizaSucesso(codigo, codigointegracao, mensagem);
                webRequest = CreateWebRequest(url, action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            }

            if (metodo == "AtualizarIntegracaoProblema")
            {

                soapEnvelopeXml = CreateXmlAtualizaProblema(codigo, codigointegracao, mensagem);
                webRequest = CreateWebRequest(url, action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            }

            if (metodo == "AtualizarIntegradoSituacao")
            {

                //P - Pago esse é o staus 
                soapEnvelopeXml = CreateEnvelopeAtualizaIntegracao("P", codigointegracao, mensagem);
                webRequest = CreateWebRequest(url, action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            }





            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {

                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream, System.Text.Encoding.Default
                        ).ReadToEnd();


                    result = responseStr;
                }
            }
            return result;
        }


        public static List<string> RetornaElementoDoXml(string plc_stringxml, string plc_elemento)
        {
            // Dicionário que irá armazenar as Tags primárias e seus valores
            List<string> listElementos = new List<string>();

            string stringxml = plc_stringxml;

            while (stringxml.IndexOf("<" + plc_elemento + ">") >= 0 && stringxml.IndexOf("</" + plc_elemento + ">") >= 0)
            {
                string filtrotagi = "<" + plc_elemento + ">";
                string filtrotagf = "</" + plc_elemento + ">";
                int filtrotagposicaoi = stringxml.IndexOf(filtrotagi);
                int filtrotagposicaof = stringxml.IndexOf(filtrotagf);

                if (!filtrotagposicaoi.Equals(-1) && !filtrotagposicaof.Equals(-1))
                {
                    string conteudo = (stringxml.Substring(filtrotagposicaoi, filtrotagposicaof - filtrotagposicaoi).Replace(filtrotagi, ""));
                    listElementos.Add(conteudo);

                    // Removo a Tag lida da string
                    stringxml = stringxml.Substring(filtrotagposicaof + filtrotagf.Length);
                }
            }

            // Caso não tenha localizado a tag, retorno um registro em branco
            if (listElementos.Count.Equals(0))
            {
                listElementos.Add("");
            }

            return listElementos;
        }



        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }



        private static XmlDocument CreateEnvelopeRelResumido(string codigo, string codigointegracao, string mensagem)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                    <ObterRelatoriosPendentesResumo   xmlns=""http://aptuscloud.com.br/rdapi/"">
                      <accessKey>093659e3-cdfa-4cff-b77b-871f3f1abe41</accessKey>
                      <empresa>1</empresa>
                    </ObterRelatoriosPendentesResumo>
                  </soap:Body>
                </soap:Envelope>");



            //soapEnvelopeXml.LoadXml(retorno);
            return soapEnvelopeXml;
        }

        private static XmlDocument CreateEnvelopeAtualizaIntegracao(string codigostatus, string codigointegracao, string mensagemstatus)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                    <AtualizarIntegradoSituacao   xmlns=""http://aptuscloud.com.br/rdapi/"">
                      <accessKey>093659e3-cdfa-4cff-b77b-871f3f1abe41</accessKey>
                      <empresa>1</empresa>
                      <codigoIntegracao>" + codigointegracao + "</codigoIntegracao>" +
                      "<codigoStatus>" + codigostatus + "</codigoStatus>" +
                      "<mensagemStatus>" + mensagemstatus + "</mensagemStatus>" +
                      "</AtualizarIntegradoSituacao>" +
                  "</soap:Body>" +
                "</soap:Envelope>");
            return soapEnvelopeXml;

        }

        private static XmlDocument CreateXmlAtualizaSucesso(string codigo, string codigointegracao, string mensagem)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                    <AtualizarIntegracaoSucesso   xmlns=""http://aptuscloud.com.br/rdapi/"">
                      <accessKey>093659e3-cdfa-4cff-b77b-871f3f1abe41</accessKey>
                      <empresa>1</empresa>
                      <codigo>" + codigo + "</codigo>" +
                      "<codigo>" + codigointegracao + "</codigo>" +
                      "</AtualizarIntegracaoSucesso>" +
                  "</soap:Body>" +
                "</soap:Envelope>");
            return soapEnvelopeXml;
        }

        private static XmlDocument CreateXmlAtualizaProblema(string codigo, string codigointegracao, string mensagem)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                    <AtualizarIntegracaoProblema   xmlns=""http://aptuscloud.com.br/rdapi/"">
                      <accessKey>093659e3-cdfa-4cff-b77b-871f3f1abe41</accessKey>
                      <empresa>1</empresa>
                      <codigo>" + codigo + "</codigo>" +
                      "<codigo>" + codigointegracao + "</codigo>" +
                       "<codigo>" + mensagem + "</codigo>" +
                      "</AtualizarIntegracaoProblema>" +
                  "</soap:Body>" +
                "</soap:Envelope>");
			
            return soapEnvelopeXml;
        }




        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {

                soapEnvelopeXml.Save(stream);
            }
        }

        public static string postXMLData(string destinationUrl, string requestXml)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream, System.Text.Encoding.Default
                    ).ReadToEnd();
                return responseStr;
            }
            return null;
        }

        public static string inserereembolso(string codigo, string cpf, DateTime Emissao, DateTime Vencimento, string Valor)
        {
            SqlDataReader _dtReader = null;
            SqlCommand Com = null;
            try
            {

                string resultado = "";
                var Conn = new clConexao().getConexao();
                string ComText = "EXEC DBO.SP_INSERE_TITULO_PAGAR @ID_INTEGRACAO, @pCpf, @Emissao, @Vencimento, @Valor";

                using (Com = new SqlCommand(ComText, Conn))
                {


                    Com.Parameters.Clear();

                    SqlParameter pCodigo = new SqlParameter("@ID_INTEGRACAO", System.Data.SqlDbType.VarChar);
                    pCodigo.Value = codigo;
                    Com.Parameters.Add(pCodigo);


                    SqlParameter pCpf = new SqlParameter("@pCpf", System.Data.SqlDbType.VarChar);
                    pCpf.Value = cpf;
                    Com.Parameters.Add(pCpf);


                    SqlParameter pEmissao = new SqlParameter("@Emissao", System.Data.SqlDbType.DateTime);
                    pEmissao.Value = Emissao;
                    Com.Parameters.Add(pEmissao);

                    SqlParameter pVencimento = new SqlParameter("@Vencimento", System.Data.SqlDbType.DateTime);
                    pVencimento.Value = Vencimento;
                    Com.Parameters.Add(pVencimento);

                    SqlParameter pValor = new SqlParameter("@Valor", System.Data.SqlDbType.VarChar);
                    pValor.Value = Valor;
                    Com.Parameters.Add(pValor);



                    Com.CommandTimeout = 300;
                    _dtReader = Com.ExecuteReader();

                    if (_dtReader.Read())
                    {
                        if (_dtReader.GetString(0) == "OK")
                        {
                            resultado = "OK";
                        }
                        else
                        {

                            resultado = "Erro";
                        }
                    }

                    return resultado;
                }
            }
            catch (Exception Ex)
            {
                throw;
                

            }

        }



        public static Boolean retornohoracerta()
        {
            try
            {
                DateTime horainicial = DateTime.Now;

                if ((horainicial.Hour >= 06 && horainicial.Minute >= 0) && (horainicial.Hour <= 18 && horainicial.Minute >= 0))
                {
                    var hora = horainicial.Hour + 1;

                    if (horainicial.Hour == hora && horainicial.Minute == 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception Ex)
            {
                throw;
                

            }

        }



        public static DataTable RetornaPgtoReebolso(string codigointegracao, string mensagem)
        {

            var Conn = new clConexao().getConexao();

            // CRIANDO OBJETO DE EXECUÇÃO
            SqlCommand ObjCmd = new SqlCommand("SP_CONSULTA_PAGAMENTOS_REEMBOLSO", Conn);

            // CRIANDO DATATABLE
            DataTable Objdt;
            DataRow Dtrw;


            // DEFININDO TIPO DE COMANDO COMO SP
            ObjCmd.CommandType = CommandType.StoredProcedure;
            ObjCmd.CommandTimeout = 9000;

            // PASSANDO PARAMETROS
            ObjCmd.Parameters.Add("@codigointegracao", SqlDbType.VarChar).Value = codigointegracao;
            ObjCmd.Parameters.Add("@mensagem", SqlDbType.VarChar).Value = mensagem;


            // CRIANDO OBJETO PARA RETORNO DE VALOR
            SqlDataAdapter ObjRetorno = new SqlDataAdapter(ObjCmd);

            DataSet Objds = new DataSet();

            // EXECUTANDO PROCEDURE
            ObjRetorno.Fill(Objds, "SP_CONSULTA_PAGAMENTOS_REEMBOLSO");


            // DESCARREGANDO DADOS NO DATATABLE
            Objdt = Objds.Tables["SP_CONSULTA_PAGAMENTOS_REEMBOLSO"];
            int i;
            i = 0;

            if ((Objdt.Rows.Count != 0))
            {
                Dtrw = Objdt.Rows[i];

                // FECHANDO CONEXÃO
                Conn.Close();
                return Objdt;
            }
            else
            {
                Conn.Close();
                return Objdt;
            }
        }

    }
}
