using GTIServiceEmailOcorrencias.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Data;
using ExcelLibrary.SpreadSheet;
using GTIServiceEmailOcorrencias.GrupoELG;

namespace GTIServiceEmailOcorrencias
{
    class clEnvioEmailConfirmacaoEntradaEntrega
    {



       
        private static string attachmentFilename;

        public static void processaEmail()
        {

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Hour >= 17)
            {
                //  if(false)
                if (!WasSent(2))
                {
                    var A03_UKEYS = new List<string>
                    {
                        "F7CDA95A-A8E9-4C2B-B"
                    };


                    foreach (var A03_UKEY in A03_UKEYS)
                    {
                        var OcorrenciasConfirmadas = new List<OcorrenciaConhecimento>(); //Ocorrencias com entrega
                        var OcorrenciasEmAberto = new List<OcorrenciaConhecimento>(); //Ocorrencias que não foram entregas

                        var FileNamePendente = string.Empty;

                        OcorrenciasEmAberto = GenerateCSV2(0, A03_UKEY, "PENDENTE_");
                        FileNamePendente = attachmentFilename;
                        OcorrenciasConfirmadas = GenerateCSV2(1, A03_UKEY, "ENVIO_7_DIAS_");


                        if ((OcorrenciasEmAberto != null && OcorrenciasEmAberto.Count() != 0) || (OcorrenciasConfirmadas != null && OcorrenciasConfirmadas.Count() != 0))
                        {
                            try
                            {
                                var EmailToSend = string.Empty;

                                if (OcorrenciasConfirmadas != null && OcorrenciasConfirmadas.Count() != 0)
                                {
                                    EmailToSend = RetornaConhecimento.GetEmailByConhecimentoUkey(OcorrenciasConfirmadas.FirstOrDefault().UkeyConhecimento);
                                }
                                else if (OcorrenciasEmAberto != null && OcorrenciasEmAberto.Count() != 0)
                                {
                                    EmailToSend = RetornaConhecimento.GetEmailByConhecimentoUkey(OcorrenciasEmAberto.FirstOrDefault().UkeyConhecimento);

                                }
                                else
                                    throw new Exception("Nenhuma ocorrencia encontrada para buscar email de enivo! Entre em contato com o desenvolvedor!");


                                var body = @"
                                            <strong>Atenção:</strong>
                                            Este é um envio de e-mail automático via sistema e não deve ser respondido.
                                            Qualquer dúvida, entre em contato diretamente com a Daytona Express 
                                            ";

                                using (var smtpClient = new SmtpClient("mail.daytonaexpress.com.br"))
                                {
                                    smtpClient.UseDefaultCredentials = false;
                                    smtpClient.Host = "mail.daytonaexpress.com.br";
                                    smtpClient.Port = 587;
                                    smtpClient.EnableSsl = false;
                                    smtpClient.Credentials = new NetworkCredential("ocorrenciaonline@daytonaexpress.com.br", "dayt@011");




                                    var emailMensage = new MailMessage
                                    {
                                        IsBodyHtml = true,
                                        From = new MailAddress("ocorrenciaonline@daytonaexpress.com.br"),
                                        Body = body,
                                        Subject = "Informativo de Ocorrência",



                                    };
                                    //emailMensage.To.Add(new MailAddress(Email.EmailToSend));
                                    //emailMensage.To.Add(new MailAddress(EmailToSend));
                                    //emailMensage.To.Add(new MailAddress("igor@gtiit.com.br"));
                                    //emailMensage.To.Add(new MailAddress("leandro@gtiit.com.br"));
                                    emailMensage.To.Add(new MailAddress("andre@gtiit.com.br"));
                                    emailMensage.To.Add(new MailAddress("saulo@daytonaexpress.com.br"));
                                    emailMensage.To.Add(new MailAddress("joederli.barreto@gtiit.com.br"));


                                    try
                                    {

                                        if (OcorrenciasConfirmadas != null && OcorrenciasConfirmadas.Count() != 0)
                                        {
                                            Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                                            ContentDisposition disposition = attachment.ContentDisposition;
                                            disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                                            disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                                            disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                                            disposition.FileName = Path.GetFileName(attachmentFilename);
                                            disposition.Size = new FileInfo(attachmentFilename).Length;
                                            disposition.DispositionType = DispositionTypeNames.Attachment;
                                            emailMensage.Attachments.Add(attachment);
                                        }
                                        if (OcorrenciasEmAberto != null && OcorrenciasEmAberto.Count() != 0)
                                        {
                                            Attachment attachmentPendente = new Attachment(FileNamePendente, MediaTypeNames.Application.Octet);
                                            ContentDisposition disposition = attachmentPendente.ContentDisposition;
                                            disposition.CreationDate = File.GetCreationTime(FileNamePendente);
                                            disposition.ModificationDate = File.GetLastWriteTime(FileNamePendente);
                                            disposition.ReadDate = File.GetLastAccessTime(FileNamePendente);
                                            disposition.FileName = Path.GetFileName(FileNamePendente);
                                            disposition.Size = new FileInfo(FileNamePendente).Length;
                                            disposition.DispositionType = DispositionTypeNames.Attachment;
                                            emailMensage.Attachments.Add(attachmentPendente);
                                        }


                                        smtpClient.Send(emailMensage);
                                    }catch(Exception e)
                                    {

                                    }
                                }


                                if (OcorrenciasConfirmadas != null && OcorrenciasConfirmadas.Count() != 0)
                                {
                                    foreach (var OcorrenciaConhecimento in OcorrenciasConfirmadas.Where(x => x.LogRegister == 1).ToList())
                                    {

                                        var _cmd = "INSERT INTO  OCORRENCIAS_ENVIADAS_ENTRADA_ENTREGA  VALUES(@SEQ,'@UKEYCONHECIMENTO',@CODIGO,@STATUS,CONVERT(DATETIME,'@DATA',120),'@MENSAGEM',@typeRel)";

                                        _cmd = _cmd.Replace("@SEQ", OcorrenciaConhecimento.Seq.ToString());
                                        _cmd = _cmd.Replace("@UKEYCONHECIMENTO", OcorrenciaConhecimento.UkeyConhecimento);
                                        _cmd = _cmd.Replace("@CODIGO", OcorrenciaConhecimento.CodigoOcorrencia.ToString());
                                        _cmd = _cmd.Replace("@STATUS", "1");
                                        _cmd = _cmd.Replace("@typeRel", "2");


                                        //2017-11-29 15:51:29
                                        _cmd = _cmd.Replace("@DATA", String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                                        _cmd = _cmd.Replace("@MENSAGEM", "Enviado Ok");

                                        ExecSql.execsqlCmd(_cmd);
                                    }
                                }
                                if (OcorrenciasEmAberto != null && OcorrenciasEmAberto.Count() != 0)
                                {
                                    foreach (var OcorrenciaConhecimento in OcorrenciasEmAberto.Where(x => x.LogRegister == 1).ToList())
                                    {

                                        var _cmd = "INSERT INTO OCORRENCIAS_ENVIADAS_ENTRADA_ENTREGA VALUES(@SEQ,'@UKEYCONHECIMENTO',@CODIGO,@STATUS,CONVERT(DATETIME,'@DATA',120),'@MENSAGEM',@typeRel)";

                                        _cmd = _cmd.Replace("@SEQ", OcorrenciaConhecimento.Seq.ToString());
                                        _cmd = _cmd.Replace("@UKEYCONHECIMENTO", OcorrenciaConhecimento.UkeyConhecimento);
                                        _cmd = _cmd.Replace("@CODIGO", OcorrenciaConhecimento.CodigoOcorrencia.ToString());
                                        _cmd = _cmd.Replace("@STATUS", "1");
                                        _cmd = _cmd.Replace("@typeRel", "2");


                                        //2017-11-29 15:51:29
                                        _cmd = _cmd.Replace("@DATA", String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                                        _cmd = _cmd.Replace("@MENSAGEM", "Enviado Ok");

                                        ExecSql.execsqlCmd(_cmd);
                                    }
                                }
                            }
                            catch (Exception exRel2)
                            {
                                var erro = exRel2.Message;
                            }
                        }

                    }
                }
            }



            
 /***********Processo Selbetti layou propio*************************/
           new ProcessaSelBetti().Processar();
/*******************************************************************/

            /**************************************************************************/
            /*Grupo ELG sao 3 empresas, sao emails separados com arquivos por empresa*/
            List<Empresas> listaGrupoELG = new List<Empresas>();

            Empresas BITTENCOURT = new Empresas();
            //ukey do cadastro do cliente A03
            BITTENCOURT.CNPJ = "FC23CED5_8E37_4F83_B";// " CNPJ 18125970000189";// - BITTENCOURT AUDIO E VIDEO EIRELI ME
            BITTENCOURT.relType = "4";
            BITTENCOURT.emails.Add("centralsuportes@centralsuportes.com.br");
            listaGrupoELG.Add(BITTENCOURT);

            Empresas ELG = new Empresas();
            ELG.CNPJ = "STAR_NPZ43_2DG0UE35S";// CPNJ "05012368000193";// -ELG PEDESTAIS LTDA
            ELG.relType = "5";
            ELG.emails.Add("elgstore@elgstore.com.br");
            listaGrupoELG.Add(ELG);

            Empresas LP = new Empresas();
            LP.CNPJ = "C72E5AA6-F339-4384-B"; // CNPJ "21851194000109";// -LP COMERCIO DE ELETRONICOS LTDA
            LP.relType = "6";
            LP.emails.Add("marcos.pesssoa@elgpedestais.com.br");
            LP.emails.Add("robeson.salles@elgpedestais.com.br");

            listaGrupoELG.Add(LP);

            foreach (var empresas in listaGrupoELG.ToList()) {
                new ProcessarELG().Processar(empresas);
            }
            /***************************************************************************************/
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

            SqlCommand sqlCmd = new SqlCommand(_sComando, ExecSql.retornaConexao());

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

        public static bool WasSent(int typeRel)
        {
            SqlDataReader _dtReader = null;
            try
            {
                var _rCmd = @"SELECT
	                              MAX(DATA_ENVIO) AS ULTIMO_ENVIO
                              FROM
	                              OCORRENCIAS_ENVIADAS_ENTRADA_ENTREGA
                              WHERE
                                  TYPEREL=" + typeRel.ToString();

                _dtReader =  ExecSql.execsqlDr(_rCmd);
                DateTime UltimoEnvio = new DateTime();
                try
                {
                    while (_dtReader.Read())
                    {
                        UltimoEnvio = _dtReader.GetDateTime(_dtReader.GetOrdinal("ULTIMO_ENVIO"));

                    }
                }
                catch
                {
                    _rCmd = @"  SELECT
	                                Count(1) AS COUNTER
                                FROM
	                                OCORRENCIAS_ENVIADAS_ENTRADA_ENTREGA
                                WHERE
                                    TYPEREL=" + typeRel.ToString();

                    _dtReader =  ExecSql.execsqlDr(_rCmd);

                    try
                    {
                        int qtd = 0;
                        while (_dtReader.Read())
                        {
                            qtd = _dtReader.GetInt32(_dtReader.GetOrdinal("COUNTER"));

                        }
                        if (qtd == 0)
                        {
                            _dtReader.Close();
                            return false;

                        }
                    }
                    catch
                    {

                    }

                }


                _dtReader.Close();


                if (UltimoEnvio.Day == DateTime.Now.Day &&
                    UltimoEnvio.Month == DateTime.Now.Month &&
                    UltimoEnvio.Year == DateTime.Now.Year)
                {
                    return true;
                }

            }
            catch
            {

            }
            return false;
        }


        public static List<OcorrenciaConhecimento> GenerateCSV2(int relType, string A03_UKEY, string fileInitialName)
        {


            var _rCmd = @"exec SP_RETORNA_DADOS_RELATORIO_OCORRENCIA_COMPLETO " + relType.ToString() + ", '" + A03_UKEY + "'";



            SqlDataReader _dtReader =  ExecSql.execsqlDr(_rCmd);
            var newLineChar = (char)13;
            //Previsão de entrega, Nome do destinatário, CEP, UF e Município
            var FileContent = "DATA_COLETA;NR_NOTA_FISCAL;LINK_RASTREIO;DATA_ENTREGA;PREVISAO_ENTREGA;NOME_DESTINATARIO;CEP_DESTINATARIO;UF_DESTINATARIO;MUNICIPIO_DESTINATARIO" + newLineChar;

            var CNPJ = "";

            var OcorrenciasToRegisterLog = new List<OcorrenciaConhecimento>();

            while (_dtReader.Read())
            {
                try
                {
                    CNPJ = _dtReader.GetString(_dtReader.GetOrdinal("A03_010_C")).Trim();
                }
                catch
                {

                }
                FileContent = FileContent + string.Format("{0:dd/MM/yyyy}", _dtReader.GetDateTime(_dtReader.GetOrdinal("DT_COLETA"))) + ";";

                try
                {
                    FileContent = FileContent + _dtReader.GetString(_dtReader.GetOrdinal("NR_NF")).Trim() + ";";

                }
                catch
                {
                    FileContent = FileContent + "" + ";";

                }
                FileContent = FileContent + _dtReader.GetString(_dtReader.GetOrdinal("LINK")).Trim() + ";";


                if (relType == 0)
                {
                    OcorrenciasToRegisterLog.Add(new OcorrenciaConhecimento
                    {
                        Seq = 1,
                        CodigoOcorrencia = 75,
                        SatusEnviado = 1,
                        UkeyConhecimento = _dtReader.GetString(_dtReader.GetOrdinal("UKEY")).Trim(),
                        LogRegister = _dtReader.GetInt32(_dtReader.GetOrdinal("LogRegister")),
                        CEP_DESTINATARIO = _dtReader.GetString(_dtReader.GetOrdinal("CEP_DESTINATARIO")).Trim(),
                        ESTADO_DESTINATARIO = _dtReader.GetString(_dtReader.GetOrdinal("ESTADO_DESTINATARIO")).Trim(),
                        NOME_RECIBIMENTO = _dtReader.GetString(_dtReader.GetOrdinal("NOME_RECIBIMENTO")).Trim(),
                        MUNICIPIO_DESTINATARIO = _dtReader.GetString(_dtReader.GetOrdinal("MUNICIPIO_DESTINATARIO")).Trim(),
                        Prev_Entrega = _dtReader.GetInt32(_dtReader.GetOrdinal("Prev_Entrega"))
                    });
                }


                try
                {
                    FileContent = FileContent + string.Format("{0:dd/MM/yyyy}", _dtReader.GetDateTime(_dtReader.GetOrdinal("DT_ENTREGA"))) + ";";
                    OcorrenciasToRegisterLog.Add(new OcorrenciaConhecimento
                    {
                        Seq = 2,
                        CodigoOcorrencia = 1,
                        SatusEnviado = 1,
                        UkeyConhecimento = _dtReader.GetString(_dtReader.GetOrdinal("UKEY")).Trim(),
                        LogRegister = _dtReader.GetInt32(_dtReader.GetOrdinal("LogRegister")),
                        CEP_DESTINATARIO = _dtReader.GetString(_dtReader.GetOrdinal("CEP_DESTINATARIO")).Trim(),
                        ESTADO_DESTINATARIO = _dtReader.GetString(_dtReader.GetOrdinal("ESTADO_DESTINATARIO")).Trim(),
                        NOME_RECIBIMENTO = _dtReader.GetString(_dtReader.GetOrdinal("NOME_RECIBIMENTO")).Trim(),
                        MUNICIPIO_DESTINATARIO = _dtReader.GetString(_dtReader.GetOrdinal("MUNICIPIO_DESTINATARIO")).Trim(),
                        Prev_Entrega = _dtReader.GetInt32(_dtReader.GetOrdinal("Prev_Entrega"))
                    });
                }
                catch
                {
                    FileContent = FileContent + "" + ";";
                    //FileContent = FileContent + "" + newLineChar;
                }

                try
                {
                    FileContent = FileContent + _dtReader.GetInt32(_dtReader.GetOrdinal("Prev_Entrega")).ToString() + ";";

                }
                catch
                {
                    FileContent = FileContent + "" + ";";

                }

                try
                {
                    FileContent = FileContent + _dtReader.GetString(_dtReader.GetOrdinal("NOME_RECIBIMENTO")) + ";";

                }
                catch
                {
                    FileContent = FileContent + "" + ";";

                }

                try
                {
                    FileContent = FileContent + _dtReader.GetString(_dtReader.GetOrdinal("CEP_DESTINATARIO")) + ";";

                }
                catch
                {
                    FileContent = FileContent + "" + ";";

                }

                try
                {
                    FileContent = FileContent + _dtReader.GetString(_dtReader.GetOrdinal("ESTADO_DESTINATARIO")) + ";";

                }
                catch
                {
                    FileContent = FileContent + "" + ";";

                }

                try
                {
                    FileContent = FileContent + _dtReader.GetString(_dtReader.GetOrdinal("MUNICIPIO_DESTINATARIO")) + newLineChar;

                }
                catch
                {
                    FileContent = FileContent + "" + newLineChar;

                }
            }
            _dtReader.Close();


            if (OcorrenciasToRegisterLog.Count() == 0)
            {
                return null;
            }
            var filePath = "G:\\Daytona\\LogOcorrencias\\" + fileInitialName + CNPJ + "_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".CSV";

            try
            {
                System.IO.File.WriteAllText(filePath, FileContent);

            }
            catch
            {
                filePath = "C:\\" + fileInitialName + CNPJ + "_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".CSV";

                System.IO.File.WriteAllText(filePath, FileContent);

            }


            attachmentFilename = filePath;

            return OcorrenciasToRegisterLog;
        }


       

       

        public static List<Email> GetConhecimentosComOcorrenciasSemEnviar()
        {
            SqlDataReader _dtReader = null;
            var listUkeyOcorrencia = new List<Email>();
            string _sComando = @"SELECT * FROM [VW_EMAIL_CONFIRMACAO_ENTRADA_ENTREGA] ORDER BY DT_HAWB";

            _dtReader =  ExecSql.execsqlDr(_sComando);

            while (_dtReader.Read())
            {
                var _Email = new Email
                {
                    Ocorrencias = new List<OcorrenciaConhecimento>()
                };


                _Email.EmailToSend = string.IsNullOrEmpty(_Email.EmailToSend) ? _dtReader.GetString(_dtReader.GetOrdinal("EMAIL")).ToString().Trim() : _Email.EmailToSend;

                _Email.Nome = string.IsNullOrEmpty(_Email.Nome) ? _dtReader.GetString(_dtReader.GetOrdinal("NOME")).ToString().Trim() : _Email.Nome;

                _Email.NR_HAWB = string.IsNullOrEmpty(_Email.NR_HAWB) ? "HAWB - " + _dtReader.GetString(_dtReader.GetOrdinal("NR_HAWB")).ToString().Trim() : _Email.NR_HAWB;


                _Email.Ocorrencias.Add(new OcorrenciaConhecimento
                {
                    UkeyConhecimento = _dtReader.GetString(_dtReader.GetOrdinal("UKEY")),
                    CodigoOcorrencia = 75,
                    Local = "",
                    DataOcorrencia = _dtReader.GetDateTime(_dtReader.GetOrdinal("DT_HAWB")),
                    DescricaoOcorrencia = "Coleta solicitada"
                });



                var DT_ENTREGA = new DateTime();

                try
                {
                    DT_ENTREGA = _dtReader.GetDateTime(_dtReader.GetOrdinal("DT_ENTREGA"));
                }
                catch
                {

                }


                if (DT_ENTREGA != null && DT_ENTREGA != new DateTime())
                {
                    _Email.Ocorrencias.Add(new OcorrenciaConhecimento
                    {
                        UkeyConhecimento = _dtReader.GetString(_dtReader.GetOrdinal("UKEY")),
                        CodigoOcorrencia = 1,
                        Local = _dtReader.GetString(_dtReader.GetOrdinal("LOCAL_ENTREGA")),
                        RecebidoPor = _dtReader.GetString(_dtReader.GetOrdinal("DS_RECEBIDO")),
                        DescricaoOcorrencia = "Entregue",

                        DataOcorrencia = DT_ENTREGA,
                    });
                }


                listUkeyOcorrencia.Add(_Email);

            }

            _dtReader.Close();

            return listUkeyOcorrencia;
        }

     

        #endregion
    }
}
