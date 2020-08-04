using GTIServiceEmailOcorrencias.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias
{
    static class clEnvioEmail
    {
        private static SqlConnection _conexao;

        public static void processaEmail()
        {


            var ListUkeyConhecimentos = GetConhecimentosComOcorrenciasSemEnviar();

            var strLog = @"<strong>[[@HORA]]</strong>
                           <table border=""1"">
                                <tr>
                                    <th>
                                        Conhecimento
                                    </th>
                                    <th>
                                        Email
                                    </th>
                                    <th>
                                        Quantidade de solicita&ccedil;&otilde;es enviadas
                                    </th>
                                    <th>
                                        Data
                                    </th>
                                    <th>
                                        Status
                                    </th>
                                    <th>
                                        Mensagem
                                    </th>
                                </tr>";

            strLog = strLog.Replace("[[@HORA]]", DateTime.Now.ToString());

            foreach ( var ukey in ListUkeyConhecimentos)
            {

                Email Email = null;


                
                
                


                try
                {
                    Email = GetEmailOcorrencias(ukey);
                    var body = @"<table width=""740px"" >
                              <tr style=""background-color: #E1E8F1"">
                                <td>
                                    <table>
                                        <tr style=""height:35px"">
                                            <td valign=""top"">
                                              <strong><span style=""color:#2C4C9D"">Detalhes:</span></strong>
                                            </td>
                                            <td valign=""top"">
                                              <strong><span style=""color:#0079FF"">&nbsp;[[NR_HAWB]]</span></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                              </tr>
                              [[HOUVEENTREGA]]
                              <tr>
                                <table style=""border:none;border-collapse: collapse;"" width=""740px"">
                                  <tr style=""background-color: #E1E8F1;height:35px"">
                                    <th valign=""top"" width=""130px"" style=""text-align: left;""><span style=""color:#2C4C9D"">Data</span></th>
                                    <th valign=""top"" width=""130px"" style=""text-align: left;""><span style=""color:#2C4C9D"">Hora</span></th>
                                    <th valign=""top"" width=""250px"" style=""text-align: left;""><span style=""color:#2C4C9D"">Localização</span></th>
                                    <th valign=""top"" width=""350px"" style=""text-align: left;""><span style=""color:#2C4C9D"">Situação</span></th>
                                  </tr>
                                  [[OCORRENCIAS]]
                                </table>
                              </tr>
                            </table><br><br><br>
                            <p>Este e-mail é somente informativo, por favor não responder.</p>
                            <p>Para demais informações consulte a Daytona mais próxima de você ou acesse: <a href=""https://www.daytonaexpress.com.br"">www.daytonaexpress.com.br</a></p>";
                    body = body.Replace("[[NR_HAWB]]", Email.NR_HAWB);

                    var entrega = Email.Ocorrencias.Where(x => x.CodigoOcorrencia == 1).FirstOrDefault();
                    //HORA
                    //NOME
                    //DATA
                    if (entrega != null)
                    {
                        var houveentrega = @" <tr style=""background-color: #F9FAFC"">
                                            <table>
                                              <tr>
                                                <td>Recebido por:</td>
                                                <td>&nbsp;<strong>[[NOME]]</strong></td>
                                              </tr>
                                              <tr>
                                                <td>Data:</td>
                                                <td>&nbsp;<strong>[[DATA]]</strong></td>
                                              </tr>
                                              <tr>
                                                <td>Hora:</td>
                                                <td>&nbsp;<strong>[[HORA]]</strong></td>
                                              </tr>
                                              <tr>
                                                <td></td>
                                                <td></td>
                                              </tr>
                                              <tr>
                                                <td></td>
                                                <td></td>
                                              </tr>
                                            </table>
                                          </tr>";

                        houveentrega = houveentrega.Replace("[[NOME]]", entrega.RecebidoPor);
                        houveentrega = houveentrega.Replace("[[DATA]]", String.Format("{0:dd/MM/yyyy}", entrega.DataOcorrencia));
                        houveentrega = houveentrega.Replace("[[HORA]]", String.Format("{0:HH:mm}", entrega.DataOcorrencia));

                        body = body.Replace("[[HOUVEENTREGA]]", houveentrega);
                    }
                    else
                    {
                        body = body.Replace("[[HOUVEENTREGA]]", "");
                    }
                    //[[DATA]]
                    //HORA
                    //LOCAL
                    //DESCRICAO

                    var Line = "";

                    foreach (var ocorrencia in Email.Ocorrencias.OrderByDescending(x => x.Seq).ToList())
                    {
                        Line += @"<tr style=""background-color: #F9FAFC;height:25px"" >
                                <td>
                                  [[DATA]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                  [[HORA]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                  [[LOCAL]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                  [[DESCRICAO]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                              </tr>";
                        Line = Line.Replace("[[DATA]]", String.Format("{0:dd/MM/yyyy}", ocorrencia.DataOcorrencia));
                        Line = Line.Replace("[[HORA]]", String.Format("{0:HH:mm}", ocorrencia.DataOcorrencia));
                        Line = Line.Replace("[[LOCAL]]", ocorrencia.Local);
                        Line = Line.Replace("[[DESCRICAO]]", ocorrencia.DescricaoOcorrencia);
                    }


                    body = body.Replace("[[OCORRENCIAS]]", Line);

                    using (var smtpClient = new SmtpClient("mail.daytonaexpress.com.br"))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Host = "mail.daytonaexpress.com.br";
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;
                            smtpClient.Credentials = new NetworkCredential("ocorrenciaonline@daytonaexpress.com.br", "dayt@011");


                        var emailMensage = new MailMessage
                        {
                            IsBodyHtml = true,
                            From = new MailAddress("financeiro.cwb@daytonaexpress.com.br"),
                            Body = body,
                            Subject = "Informativo de Ocorrência",
                            
                            
                            
                        };
                        emailMensage.To.Add(new MailAddress(Email.EmailToSend));


                        smtpClient.Send(emailMensage);
                        foreach (var ocorrencia in Email.Ocorrencias.Where(y => y.SatusEnviado == 0).OrderBy(x => x.Seq).ToList())
                        {
                            InsereLogTabela(ocorrencia, "1", "Enviado Ok");
                        }

                        strLog += @"<tr>
                                            <td>
                                                HAWB-[[NR_HAWB]]
                                            </td>
                                            <td>
                                                [[EMAIL]]
                                            </td>
                                            <td>
                                                [[QTD]]
                                            </td>
                                            <td>
                                                [[DATA]]
                                            </td>
                                            <td>
                                                [[STATUS]]
                                            </td>
                                            <td>
                                                [[MENSAGEM]]
                                            </td>
                                        </tr>";

                        strLog = strLog.Replace("[[NR_HAWB]]", Email.NR_HAWB);
                        strLog = strLog.Replace("[[EMAIL]]", Email.EmailToSend);
                        strLog = strLog.Replace("[[QTD]]", Email.Ocorrencias.Count().ToString());
                        strLog = strLog.Replace("[[DATA]]", DateTime.Now.ToString());
                        strLog = strLog.Replace("[[STATUS]]", "Enviado");
                        strLog = strLog.Replace("[[MENSAGEM]]", "Enviado Ok");

                    }
                }
                catch(Exception ex)
                {
                    if(Email!=null)
                    {
                        foreach (var ocorrencia in Email.Ocorrencias.Where(y => y.SatusEnviado == 0).OrderBy(x => x.Seq).ToList())
                        {
                            InsereLogTabela(ocorrencia, "0", ex.Message+(ex.InnerException!=null? "; " + ex.InnerException.Message +(ex.InnerException.InnerException!=null? "; " + ex.InnerException.InnerException.Message:"") :""));
                        }

                        strLog += @"<tr>
                                            <td>
                                                [[NR_HAWB]]
                                            </td>
                                            <td>
                                                [[EMAIL]]
                                            </td>
                                            <td>
                                                [[QTD]]
                                            </td>
                                            <td>
                                                [[DATA]]
                                            </td>
                                            <td>
                                                [[STATUS]]
                                            </td>
                                            <td>
                                                [[MENSAGEM]]
                                            </td>
                                        </tr>";

                        strLog = strLog.Replace("[[NR_HAWB]]", Email.NR_HAWB);
                        strLog = strLog.Replace("[[EMAIL]]", Email.EmailToSend);
                        strLog = strLog.Replace("[[QTD]]", Email.Ocorrencias.Count().ToString());
                        strLog = strLog.Replace("[[DATA]]", DateTime.Now.ToString());
                        strLog = strLog.Replace("[[STATUS]]", "Erro");
                        strLog = strLog.Replace("[[MENSAGEM]]", ex.Message + (ex.InnerException != null ? "; " + ex.InnerException.Message + (ex.InnerException.InnerException != null ? "; " + ex.InnerException.InnerException.Message : "") : ""));

                    }
                }

                
            }
            strLog += "</table><br><br><br><br>";

            string ExisteFile = "";
            //var filePath = "C:\\" + string.Format("{0:yyyy_MM_dd_HH_mm}", DateTime.Now) + ".html";
            var filePath = "G:\\DAYTONA\\LOG_ENVIO_OCORRENCIAS\\" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".html";
            try
            {
                ExisteFile = System.IO.File.ReadAllText(filePath);
            }
            catch
            {

            }

            if(ListUkeyConhecimentos.Count()!=0)
            {
                System.IO.File.WriteAllText(filePath, ExisteFile + strLog);
            }

            //System.IO.File.WriteAllText("G:\\DAYTONA\\LOG_AUTOMATICO_FATURAMENTO\\" + string.Format("yyyy_MM_dd_HH_mm", DateTime.Now) + ".html", strLog);

        }
        private static SmtpClient retConfigSmtp()
        {
            SmtpClient smtp = null;

            string _sComando = @"SELECT 
	                                T77_013_C AS ENDSMTP,
	                                T77_018_N AS PORTSMTP,
	                                T77_014_N AS AUTSMTP,
									T77_015_C AS USUARIOSTMP,
	                                'daytcwb01' AS SENHASMTP,
									1 AS CRIPTSSL
                                FROM 
	                                T77 (NOLOCK)
                                WHERE 
	                                UKEY =  'MQ1G4STAR__3PE0Y3BSM'";

            SqlCommand sqlCmd = new SqlCommand(_sComando, retornaConexao());

            SqlDataReader sqlDr = sqlCmd.ExecuteReader();
            if (sqlDr.Read())
            {
                smtp = new SmtpClient(sqlDr.GetString(sqlDr.GetOrdinal("ENDSMTP")));
                if (sqlDr.GetInt32(sqlDr.GetOrdinal("AUTSMTP")) == 1)
                {
                    smtp.Credentials = new NetworkCredential(sqlDr.GetString(sqlDr.GetOrdinal("USUARIOSTMP")).Trim(), sqlDr.GetString(sqlDr.GetOrdinal("SENHASMTP")).Trim());
                    smtp.Port = sqlDr.GetInt32(sqlDr.GetOrdinal("PORTSMTP"));
                    smtp.EnableSsl = sqlDr.GetInt32(sqlDr.GetOrdinal("CRIPTSSL")) > 0;
                }
            }
            return smtp;
        }

        #region SQLFunctions

        public static void InsereLogTabela(OcorrenciaConhecimento OcorrenciaConhecimento,string Status,string Message)
        {
            var _cmd = "INSERT INTO OCORRENCIAS_ENVIADAS VALUES(@SEQ,'@UKEYCONHECIMENTO',@CODIGO,@STATUS,CONVERT(DATETIME,'@DATA',120),'@MENSAGEM')";

            _cmd = _cmd.Replace("@SEQ", OcorrenciaConhecimento.Seq.ToString());
            _cmd = _cmd.Replace("@UKEYCONHECIMENTO", OcorrenciaConhecimento.UkeyConhecimento);
            _cmd = _cmd.Replace("@CODIGO", OcorrenciaConhecimento.CodigoOcorrencia.ToString());
            _cmd = _cmd.Replace("@STATUS", Status);

            //2017-11-29 15:51:29
            _cmd = _cmd.Replace("@DATA", String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            _cmd = _cmd.Replace("@MENSAGEM", Message);
            execsqlDr(_cmd);
        }


        public static SqlDataReader execsqlDr(String comando)
        {
            SqlConnection conexao = retornaConexao();
            SqlCommand sqlc;
            SqlDataReader dr;

            sqlc = new SqlCommand(comando, conexao);
            sqlc.CommandTimeout = 120;

            try
            {
                dr = sqlc.ExecuteReader();

            }
            catch (SqlException)
            {
                dr = null;
            }
            catch (Exception)
            {
                dr = null;
            }
            return dr;

        }

        public static SqlConnection retornaConexao()
        {
            String _rConnString_c = @"Data Source=db.infra.gtiit.com.br;Initial Catalog=STARSOFT;User ID=dex_star;Password=$Day_starsoft";
                //"Appliaction Name=GTIServiceEmailOcorrencias;Data Source=.;Initial Catalog=STARSOFT;Persist Security Info=True;User ID=dex_star;Password=$Day_starsoft";

            if (_conexao == null)

                _conexao = new SqlConnection(_rConnString_c);
            else
                _conexao.Close();

            _conexao.Open();
            return _conexao;
        }

        public static List<string> GetConhecimentosComOcorrenciasSemEnviar()
        {
            SqlDataReader _dtReader = null;
            var listUkeyOcorrencia = new List<string>();
            string _sComando = @"EXEC [SP_BUSCA_CONHECIMENTOS_COM_OCORRENCIAS_NAO_ENVIADA]";
            _dtReader = execsqlDr(_sComando);
            while (_dtReader.Read())
            {
                listUkeyOcorrencia.Add(_dtReader.GetString(_dtReader.GetOrdinal("UKEY")).ToString().Trim());
            }

            _dtReader.Close();

            return listUkeyOcorrencia;
        }

        public static Email GetEmailOcorrencias(string Ukey)
        {
            SqlDataReader _dtReader = null;
            var _Email = new Email
            {
                Ocorrencias = new List<OcorrenciaConhecimento>()
            };
            string _sComando = @"EXEC [SP_OCORRENCIAS_ENVIO_EMAIL] '@UKEY'";
            _sComando = _sComando.Replace("@UKEY", Ukey);

            _dtReader = execsqlDr(_sComando);
            while (_dtReader.Read())
            {
                _Email.EmailToSend = string.IsNullOrEmpty(_Email.EmailToSend) ? _dtReader.GetString(_dtReader.GetOrdinal("OED_EMAIL")).ToString().Trim(): _Email.EmailToSend;

                _Email.Nome = string.IsNullOrEmpty(_Email.Nome) ? _dtReader.GetString(_dtReader.GetOrdinal("NOME")).ToString().Trim(): _Email.Nome;

                _Email.NR_HAWB = string.IsNullOrEmpty(_Email.NR_HAWB) ? "HAWB - " + _dtReader.GetString(_dtReader.GetOrdinal("NR_HAWB")).ToString().Trim(): _Email.NR_HAWB;


                _Email.Ocorrencias.Add(new OcorrenciaConhecimento
                {
                    UkeyConhecimento = _dtReader.GetString(_dtReader.GetOrdinal("UKEY")),
                    CodigoOcorrencia = _dtReader.GetInt32(_dtReader.GetOrdinal("CODIGO")),
                    DataOcorrencia = _dtReader.GetDateTime(_dtReader.GetOrdinal("DATA")),
                    DescricaoOcorrencia = _dtReader.GetString(_dtReader.GetOrdinal("DESCRICAO")),
                    Local = _dtReader.GetString(_dtReader.GetOrdinal("LOCAL")),
                    RecebidoPor = _dtReader.GetString(_dtReader.GetOrdinal("DS_RECEBIDO")),
                    SatusEnviado = _dtReader.GetInt32(_dtReader.GetOrdinal("STATUS")),
                    Seq = _dtReader.GetInt32(_dtReader.GetOrdinal("SEQ"))
                });

            }
            _dtReader.Close();


            return _Email;
        }

        #endregion
    }
}
