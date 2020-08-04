using ExcelLibrary.SpreadSheet;
using GTIServiceEmailOcorrencias.GrupoELG;
using GTIServiceEmailOcorrencias.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias
{
    class ProcessarELG
    {
        private static string attachmentFilename;
        /*Porque voce está ? vc tem uma demanda solicitada pelo Saulo, Cassia ou Deus?
 se nao tem , nao mude nada nessa classe, pois podera cair sobre você uma terrivel maldição
 se vc leu isso, vc está tendo uma chance de nao sofrer isso

    Porém se vc teve a solicitação  das pessoas acima  Leia descrição do que essa classe faz

    1ª  no metodo NeedSendToday recebe o parametro -1 que é a quantidade de Dias sem enviar o email, e vc ja percebeu aqui que 
    essa classe manda email, nao so manda como envia com anexo, esse anexo é um xls, XLS NAO UM CSV.
    com as ocorrencias do Dia para empresas do grupo ELG ,  ele manda para 3 empresas. 


     */
        public void Processar(Empresas empresa) {

           if (NeedSendToday(-1,empresa.relType)) { 
           
                var Ocorrencias = new List<OcorrenciaConhecimento>();

                try
                {

                    Ocorrencias = GenerateCSV(empresa.CNPJ);
                    if (Ocorrencias != null && Ocorrencias.Count() != 0)
                    {
                        var EmailToSend = GetEmailByConhecimentoUkey(Ocorrencias.FirstOrDefault().UkeyConhecimento);

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
                            
                          
                            emailMensage.To.Add(new MailAddress("joederli.barreto@gtiit.com.br"));
                            emailMensage.To.Add(new MailAddress("saulo@daytonaexpress.com.br"));
                            emailMensage.To.Add(new MailAddress("andre@gtiit.com.br"));

                            foreach (var email in empresa.emails)
                            {
                                emailMensage.To.Add(new MailAddress(email));

                            }

                            

                            Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                            ContentDisposition disposition = attachment.ContentDisposition;
                            disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                            disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                            disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                            disposition.FileName = Path.GetFileName(attachmentFilename);
                            disposition.Size = new FileInfo(attachmentFilename).Length;
                            disposition.DispositionType = DispositionTypeNames.Attachment;
                            emailMensage.Attachments.Add(attachment);

                            smtpClient.Send(emailMensage);
                        }

                        foreach (var OcorrenciaConhecimento in Ocorrencias.Where(x => x.LogRegister == 1).ToList())
                        {

                            var _cmd = "INSERT INTO OCORRENCIAS_ENVIADAS_ENTRADA_ENTREGA VALUES(@SEQ,'@UKEYCONHECIMENTO',@CODIGO,@STATUS,CONVERT(DATETIME,'@DATA',120),'@MENSAGEM','@TYPEREL')";

                            _cmd = _cmd.Replace("@SEQ", OcorrenciaConhecimento.Seq.ToString());
                            _cmd = _cmd.Replace("@UKEYCONHECIMENTO", OcorrenciaConhecimento.UkeyConhecimento);
                            _cmd = _cmd.Replace("@CODIGO", OcorrenciaConhecimento.CodigoOcorrencia.ToString());
                            _cmd = _cmd.Replace("@STATUS", "1");
                            _cmd = _cmd.Replace("@TYPEREL",empresa.relType);


                            _cmd = _cmd.Replace("@DATA", String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                            _cmd = _cmd.Replace("@MENSAGEM", "Enviado Ok");


                            

                            ExecSql.execsqlDr(_cmd);
                        }
                    }
                }
                catch (Exception ex)
                {


                }

            }

        }

        public static List<OcorrenciaConhecimento> GenerateCSV(string UKEY)
        {
            //BITTENCOURT AUDIO E VIDEO -CENTRAL SUPORTES
           var _rCmd = @"exec SP_RETORNA_DADOS_RELATORIO_OCORRENCIA '"+UKEY+"'";
            SqlDataReader _dtReader = ExecSql.execsqlDr(_rCmd);

            var CNPJ = "";

            var OcorrenciasToRegisterLog = new List<OcorrenciaConhecimento>();

           // Data / Hawb / NF / Valor / Destinatário / Link Rastreio


            Workbook workbook = new Workbook();

            Worksheet worksheet = new Worksheet("Confirmação de envio");
            worksheet.Cells[0, 0] = new Cell("DATA COLETA");
            worksheet.Cells[0, 1] = new Cell("NR HAWB");
            worksheet.Cells[0, 2] = new Cell("NR NF");
            worksheet.Cells[0, 3] = new Cell("VALOR");
            worksheet.Cells[0, 4] = new Cell("DESTINATARIO");
            worksheet.Cells[0, 5]= new Cell("LINK_RASTREIO");



            int i = 1;

            while (_dtReader.Read())
            {

                try
                {
                    CNPJ = _dtReader.GetString(_dtReader.GetOrdinal("A03_010_C")).Trim();
                }
                catch
                {

                }

                var NR_NF = "";
                var NR_HAWB = "";
                var A03_003_C_DES = "";
                DateTime DT_COLETA;
                var LINK = "";
                double VALOR = 0.00;


                try
                {
                    NR_HAWB = _dtReader.GetString(_dtReader.GetOrdinal("nr_hawb")).Trim();
                }
                catch
                { }

                try
                {
                    NR_NF = _dtReader.GetString(_dtReader.GetOrdinal("NR_NF")).Trim();
                }


                catch
                { }




                try
                {
                    LINK = _dtReader.GetString(_dtReader.GetOrdinal("LINK")).Trim();
                }
                catch
                { }

                try
                {
                    VALOR = (double)_dtReader.GetDecimal(_dtReader.GetOrdinal("vl_total"));
                }
                catch
                {

                }
                 try
                {
                    A03_003_C_DES = _dtReader.GetString(_dtReader.GetOrdinal("A03_003_C_DES")).Trim();
                }
                catch
                {

                }
               

             

                DT_COLETA = _dtReader.GetDateTime(_dtReader.GetOrdinal("DT_COLETA"));
            
                // Data / Hawb / NF / Valor / Destinatário / Link Rastreio

                worksheet.Cells[i, 0] = new Cell(DT_COLETA, @"DD/MM/YYYY");
                worksheet.Cells[i, 1] = new Cell(NR_HAWB);
                worksheet.Cells[i, 2] = new Cell(NR_NF);
                worksheet.Cells[i, 3] = new Cell(VALOR);
                worksheet.Cells[i, 4] = new Cell(A03_003_C_DES);
                worksheet.Cells[i, 5] = new Cell(LINK);

                OcorrenciasToRegisterLog.Add(new OcorrenciaConhecimento
                {
                    Seq = 1,
                    CodigoOcorrencia = 75,
                    SatusEnviado = 1,
                    UkeyConhecimento = _dtReader.GetString(_dtReader.GetOrdinal("UKEY")).Trim(),
                    LogRegister = _dtReader.GetInt32(_dtReader.GetOrdinal("LogRegister"))
                });


                i++;

            }

            _dtReader.Close();


            //Resolve problema: O Excel encontrou conteúdo ilegível / Invalid or corrupt file (unreadable content)
            for (int j = i; j < 100; j++)
            {
                worksheet.Cells[j, 0] = new Cell("");
                worksheet.Cells[j, 1] = new Cell("");
                worksheet.Cells[j, 2] = new Cell("");
                worksheet.Cells[j, 3] = new Cell("");
                worksheet.Cells[j, 4] = new Cell("");
            }



            if (OcorrenciasToRegisterLog.Count() == 0)
            {
                return null;
            }
            //var filePath = "G:\\DAYTONA\\LOG_ENVIO_ENTRADA_ENTREGA\\" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".html";
            //var filePath = "C:\\"+CNPJ+"_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".XLS";


            // aqui produçao
            var filePath = "G:\\Daytona\\LogOcorrencias\\" + CNPJ + "_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".xls";
            // pra testes leandro
       //  var filePath = "C:\\TEMP\\" + CNPJ + "_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".XLS";

            // geração antiga 
            // ds.Tables.Add(dt);
            // ExcelLibrary.DataSetHelper.CreateWorkbook(filePath, ds);
            workbook.Worksheets.Add(worksheet);
            workbook.Save(filePath);

            attachmentFilename = filePath;

            return OcorrenciasToRegisterLog;
        }

        public static bool NeedSendToday(float Days,string relType)
        {
            // somente para testes
            //return true;

            SqlDataReader _dtReader = null;
            try
            {
                var _rCmd = @"  SELECT
	                            MAX(DATA_ENVIO) AS ULTIMO_ENVIO
                            FROM
	                            OCORRENCIAS_ENVIADAS_ENTRADA_ENTREGA
                            WHERE
                                TYPEREL = '"+ relType + "'";

                _dtReader = ExecSql.execsqlDr(_rCmd);
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
	                                OCORRENCIAS_ENVIADAS_ENTRADA_ENTREGA";

                    _dtReader = ExecSql.execsqlDr(_rCmd);

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
                            return true;

                        }
                    }
                    catch
                    {

                    }

                }

                _dtReader.Close();

                // Alterado Joederli - 28/05/2019 - chamado 1490 - incluir envio as 16:00 e alterado para enviar as 9 e não a partir das 9
                if (UltimoEnvio.Date <= DateTime.Now.Date.AddDays(Days) && DateTime.Now.Hour == 21)
                {
                    return true;
                }

                //if (DateTime.Now.Hour == 21 && DateTime.Now.Minute <= 05)
                //{
                //    return true;
                //}
            }
            catch
            {

            }
            return false;
        }

        public static string GetEmailByConhecimentoUkey(string Ukey)
        {
            var _rCmd = @"  SELECT 
	                            A03_043_C
                            FROM
	                            CONHECIMENTO_DAYTONA
	                            INNER JOIN A03 ON CONHECIMENTO_DAYTONA.A03_UKEY_REMET = A03.UKEY
                            WHERE
	                            CONHECIMENTO_DAYTONA.UKEY ='@UKEY'";

            _rCmd = _rCmd.Replace("@UKEY", Ukey);

            var _dtReader = ExecSql.execsqlDr(_rCmd);
            var email = "";
            while (_dtReader.Read())
            {
                email = _dtReader.GetString(_dtReader.GetOrdinal("A03_043_C"));
            }
            _dtReader.Close();

            return email.Trim();
        }

    }
}
