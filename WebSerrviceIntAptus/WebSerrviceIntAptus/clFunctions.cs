using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

using System.Text;

using System.Xml.Serialization;

namespace WebSerrviceIntAptus
{
    public static class clFunctions
    {

        private static SqlConnection _conexao = null;

        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        /// <summary>
        /// Este método realiza a leitura de uma string XML e gera um List contendo os
        /// conteúdos dos elementos informados.
        /// </summary>
        /// <param name="plc_stringxml">Texto no formato XML</param>
        /// <param name="plc_elemento">Nome do elemento que se deseja extrair os valores</param>
        /// <returns></returns>
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




        public static string formataCPNJ(string cnpj)
        {
            string sCNPJ = cnpj;
            sCNPJ = sCNPJ.Substring(0, 2) + "." + sCNPJ.Substring(2, 3) + "." + sCNPJ.Substring(5, 3) + "/" + sCNPJ.Substring(8, 4) + "-" + sCNPJ.Substring(12, 2);
            return sCNPJ;
        }

        public static string formataCPF(string cpf)
        {
            string sCPF = cpf;
            sCPF = sCPF.Substring(0, 3) + "." + sCPF.Substring(3, 3) + "." + sCPF.Substring(6, 3) + "-" + sCPF.Substring(9, 2);
            return sCPF;
        }

        public static string formataCEP(string cep)
        {
            string scep = cep.Replace("-", "");
            scep = scep.Substring(0, 5) + "-" + scep.Substring(5, 3);
            return scep;
        }

        public static string toExtenso(decimal valor)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " DE";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                valor_por_extenso += " DE";
                            else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                valor_por_extenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valor_por_extenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valor_por_extenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTAVOS";
                }
                return valor_por_extenso;
            }
        }

        static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }


        public static string fncNewUkey(SqlConnection sqlConn)
        {
            string sSql = "";
            SqlCommand sqlCmd = null;
            SqlDataReader sqlDr = null;

            // Gera uma nova ukey
            sSql = "SELECT REPLACE(LEFT(CONVERT(VARCHAR(36), NEWID()), 20), '-', '_') AS UKEY";
            sqlCmd = new SqlCommand(sSql, sqlConn);
            try
            {
                sqlDr = sqlCmd.ExecuteReader();
                if (sqlDr.Read())
                    return sqlDr.GetString(sqlDr.GetOrdinal("UKEY"));

            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {

                if (sqlCmd != null)
                    sqlCmd.Dispose();

                if (sqlDr != null)
                    sqlDr.Close();
            }
            return "";




        }

        //public async static Task<Endereco> ConsultaCEP(string p_strCEP)
        //{
        //    p_strCEP = p_strCEP.Replace("-", "").Trim();
        //    p_strCEP = p_strCEP.Replace(".", "");

        //    string url = string.Format("https://viacep.com.br/ws/{0}/json/", p_strCEP);


        //    try
        //    {
        //        // Create the web request
        //        HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;

        //        // Set type to GET
        //        request.Method = "GET";
        //        request.ContentType = "application/json";

        //        using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
        //        {

        //            StreamReader reader = new StreamReader(response.GetResponseStream());
        //            var result = reader.ReadToEnd();
        //            reader.Close();

        //            return JsonConvert.DeserializeObject<Endereco>(result);
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return null;
        //}


        //public static Endereco ConsultaCEPXML(string p_strCEP)
        //{
        //    p_strCEP = p_strCEP.Replace("-", "").Trim();
        //    p_strCEP = p_strCEP.Replace(".", "");
        //    Endereco objEndereco = new Endereco();
        //    try
        //    {

        //        using (var ws = new WebservicesCorreios.AtendeClienteClient())
        //        {
        //            var resultato = ws.consultaCEP(p_strCEP);
        //            if (resultato != null)
        //            {
        //                objEndereco.UF = resultato.uf;
        //                objEndereco.Cidade = resultato.cidade;
        //                objEndereco.Bairro = resultato.bairro;

        //                objEndereco.Logradouro = RemoverAcentos(resultato.end.ToUpper());
        //                objEndereco.cep = p_strCEP;
        //            }

        //        }
        //    }




        //    catch (Exception ex)
        //    {
        //        objEndereco.Logdeerro = "Erro ao buscar CEP informado. ERROR: " + ex.Message.ToString();
        //        //throw new Exception("Erro ao buscar CEP informado. ERROR: " + ex.Message);

        //    }

        //    return objEndereco;
        //}


        public static string RemoverAcentos(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            else
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(input);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }

        public static object deserialize(XmlSerializer t, string xml)
        {
            StringReader rdr = new StringReader(xml);
            return t.Deserialize(rdr);

        }
    }
}
