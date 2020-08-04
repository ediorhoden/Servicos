using ExcelLibrary.SpreadSheet;
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
    class ProcessaEDI
    {
        private static string attachmentFilename;
      
        public void Processar()
        {

            WriteToAnEventLog log = new WriteToAnEventLog();
           if (NeedSendToday.isNow(-1,"7", 22))
            {

                var Ocorrencias = new List<OcorrenciaConhecimento>();

                try
                {

                    Ocorrencias = GenerateCSV();
                    if (Ocorrencias != null && Ocorrencias.Count() != 0)
                    {
                        var EmailToSend = NeedSendToday.GetEmailByConhecimentoUkey(Ocorrencias.FirstOrDefault().UkeyConhecimento);

                        var body = @"
                                    <strong>Atencao:</strong>
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
                            _cmd = _cmd.Replace("@TYPEREL", "7");


                            _cmd = _cmd.Replace("@DATA", String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                            _cmd = _cmd.Replace("@MENSAGEM", "Enviado Ok");


                            ExecSql.execsqlDr(_cmd);
                        }
                    }
                }
                catch (Exception e)
                {
                    log.RegistraLog("Erro Processa EDI" + e.StackTrace, 3);

                }

            }

        }

        public static List<OcorrenciaConhecimento> GenerateCSV()
        {
            WriteToAnEventLog log = new WriteToAnEventLog();
            var OcorrenciasToRegisterLog = new List<OcorrenciaConhecimento>();
            try
            {


                //BITTENCOURT AUDIO E VIDEO -CENTRAL SUPORTES
                var _rCmd = @"exec SP_RETORNA_DADOS_RELATORIO_OCORRENCIA 'EDI_PLANILHA'";
                SqlDataReader _dtReader = ExecSql.execsqlDr(_rCmd);

               


                // Data / Hawb / NF / Valor / Destinatário / Link Rastreio


                Workbook workbook = new Workbook();

                Worksheet worksheet = new Worksheet("Confirmacao de envio");
                worksheet.Cells[0, 0] = new Cell("CTE");
                worksheet.Cells[0, 1] = new Cell("ETD");
                worksheet.Cells[0, 2] = new Cell("ETA");
                worksheet.Cells[0, 3] = new Cell("order_number");
                worksheet.Cells[0, 4] = new Cell("numero de rastreio");
                worksheet.Cells[0, 5] = new Cell("origem");
                worksheet.Cells[0, 6] = new Cell("destino");
                worksheet.Cells[0, 7] = new Cell("conta_consignatario");
                worksheet.Cells[0, 8] = new Cell("consignatario");
                worksheet.Cells[0, 9] = new Cell("endereco _consignatario1");
                worksheet.Cells[0, 10] = new Cell("endereco _consignatario2");
                worksheet.Cells[0, 11] = new Cell("endereco _consignatario3");
                worksheet.Cells[0, 12] = new Cell("cidade_consignatario");
                worksheet.Cells[0, 13] = new Cell("estado_consignatario");
                worksheet.Cells[0, 14] = new Cell("cep_consignatario");
                worksheet.Cells[0, 15] = new Cell("pais-consignatario");
                worksheet.Cells[0, 16] = new Cell("email-consignatario");
                worksheet.Cells[0, 17] = new Cell("telefone_consignatario");
                worksheet.Cells[0, 18] = new Cell("celular_consignatario");
                worksheet.Cells[0, 19] = new Cell("CPF/CNPJ_consignatario");
                worksheet.Cells[0, 20] = new Cell("peças");
                worksheet.Cells[0, 21] = new Cell("peso bruto");
                worksheet.Cells[0, 22] = new Cell("pesoliquido");
                worksheet.Cells[0, 23] = new Cell("unidade_de_medida");
                worksheet.Cells[0, 24] = new Cell("altura");
                worksheet.Cells[0, 25] = new Cell("comprimento");
                worksheet.Cells[0, 26] = new Cell("largura");
                worksheet.Cells[0, 27] = new Cell("descricao_mercadoria");
                worksheet.Cells[0, 28] = new Cell("valor_mercadoria");
                worksheet.Cells[0, 29] = new Cell("frete");
                worksheet.Cells[0, 30] = new Cell("moeda");
                worksheet.Cells[0, 31] = new Cell("tipo de serviçp");
                worksheet.Cells[0, 32] = new Cell("conta_embarcador");
                worksheet.Cells[0, 33] = new Cell("nome_embarcador");
                worksheet.Cells[0, 34] = new Cell("endereço_embarcador1");
                worksheet.Cells[0, 35] = new Cell("endereço_embarcador2");
                worksheet.Cells[0, 36] = new Cell("cidade_embarcador");
                worksheet.Cells[0, 37] = new Cell("estado_embarcador");
                worksheet.Cells[0, 38] = new Cell("cep_embarcador");
                worksheet.Cells[0, 39] = new Cell("pais-embarcador");
                worksheet.Cells[0, 40] = new Cell("email-embarcador");
                worksheet.Cells[0, 41] = new Cell("telefone_embarcador");





                int i = 1;

                while (_dtReader.Read())
                {
                    string CTE = "";
                    string ETD = "";
                    string ETA = "";
                    string ORDER_NUMBER = "";
                    string NUMERO_RASTREIO = "";
                    string ORIGEM = "";
                    string DESTINO = "";
                    string conta_consignatario = "";
                    string consignatario = "";
                    string endereco_consignatario1 = "";
                    string endereco_consignatario2 = "";
                    string endereco_consignatario3 = "";
                    string cidade_consignatario = "";
                    string estado_consignatario = "";
                    string cep_consignatario = "";
                    string pais_consignatario = "";
                    string email_consignatario = "";
                    string telefone_consignatario = "";
                    string celular_consignatario = "";
                    string CPF_CNPJ_consignatario = "";
                    Int32  pecas = 0;
                    decimal pesobruto = 0;
                    decimal pesoliquido = 0;
                    string unidade_de_medida = "";
                    decimal altura = 0;
                    decimal comprimento = 0;
                    decimal largura = 0 ;
                    string descricao_mercadoria = "";
                    decimal valor_mercadoria = 0;
                    decimal frete = 0;
                    string moeda = "";
                    string tipodeservico = "";
                    string conta_embarcador = "";
                    string nome_embarcador = "";
                    string endereco_embarcador1 = "";
                    string endereco_embarcador2 = "";
                    string cidade_embarcador = "";
                    string estado_embarcador = "";
                    string cep_embarcador = "";
                    string pais_embarcador = "";
                    string email_embarcador = "";
                    string telefone_embarcador = "";

                    try
                    {
                        CTE = _dtReader.GetString(_dtReader.GetOrdinal("CTE")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        ETD = _dtReader.GetString(_dtReader.GetOrdinal("ETD")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        ETA = _dtReader.GetString(_dtReader.GetOrdinal("ETA")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        ORDER_NUMBER = _dtReader.GetString(_dtReader.GetOrdinal("ORDER_NUMBER")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        NUMERO_RASTREIO = _dtReader.GetString(_dtReader.GetOrdinal("NUMERO_RASTREIO")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        ORIGEM = _dtReader.GetString(_dtReader.GetOrdinal("ORIGEM")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        DESTINO = _dtReader.GetString(_dtReader.GetOrdinal("DESTINO")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        conta_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("conta_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        consignatario = _dtReader.GetString(_dtReader.GetOrdinal("consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        endereco_consignatario1 = _dtReader.GetString(_dtReader.GetOrdinal("endereco_consignatario1")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        endereco_consignatario2 = _dtReader.GetString(_dtReader.GetOrdinal("endereco_consignatario2")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        endereco_consignatario3 = _dtReader.GetString(_dtReader.GetOrdinal("endereco_consignatario3")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        cidade_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("cidade_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        estado_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("estado_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        cep_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("cep_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        pais_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("pais_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        email_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("email_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        telefone_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("telefone_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        celular_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("celular_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        CPF_CNPJ_consignatario = _dtReader.GetString(_dtReader.GetOrdinal("CPF_CNPJ_consignatario")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        pecas = (Int32)_dtReader.GetInt32(_dtReader.GetOrdinal("pecas"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        pesobruto =_dtReader.GetDecimal(_dtReader.GetOrdinal("pesobruto"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        pesoliquido =_dtReader.GetDecimal(_dtReader.GetOrdinal("pesoliquido"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        unidade_de_medida = _dtReader.GetString(_dtReader.GetOrdinal("unidade_de_medida"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        altura =_dtReader.GetDecimal(_dtReader.GetOrdinal("altura"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        comprimento =_dtReader.GetDecimal(_dtReader.GetOrdinal("comprimento"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        largura =_dtReader.GetDecimal(_dtReader.GetOrdinal("largura"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        descricao_mercadoria = _dtReader.GetString(_dtReader.GetOrdinal("descricao_mercadoria")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        valor_mercadoria = _dtReader.GetDecimal(_dtReader.GetOrdinal("VALOR_MERCADORIA"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        frete =_dtReader.GetDecimal(_dtReader.GetOrdinal("frete"));
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        moeda = _dtReader.GetString(_dtReader.GetOrdinal("moeda")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        tipodeservico = _dtReader.GetString(_dtReader.GetOrdinal("tipodeservico")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        conta_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("conta_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        nome_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("nome_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        endereco_embarcador1 = _dtReader.GetString(_dtReader.GetOrdinal("endereco_embarcador1")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        endereco_embarcador2 = _dtReader.GetString(_dtReader.GetOrdinal("endereco_embarcador2")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        cidade_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("cidade_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        estado_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("estado_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        cep_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("cep_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        pais_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("pais_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        email_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("email_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {
                    }
                    try
                    {
                        telefone_embarcador = _dtReader.GetString(_dtReader.GetOrdinal("telefone_embarcador")).Trim();
                    }
                    catch (Exception e)
                    {

                    }



                    worksheet.Cells[i, 0] = new Cell(CTE);
                    worksheet.Cells[i, 1] = new Cell(ETD);
                    worksheet.Cells[i, 2] = new Cell(ETA);
                    worksheet.Cells[i, 3] = new Cell(ORDER_NUMBER);
                    worksheet.Cells[i, 4] = new Cell(NUMERO_RASTREIO);
                    worksheet.Cells[i, 5] = new Cell(ORIGEM);
                    worksheet.Cells[i, 6] = new Cell(DESTINO);
                    worksheet.Cells[i, 7] = new Cell(conta_consignatario);
                    worksheet.Cells[i, 8] = new Cell(consignatario);
                    worksheet.Cells[i, 9] = new Cell(endereco_consignatario1);
                    worksheet.Cells[i, 10] = new Cell(endereco_consignatario2);
                    worksheet.Cells[i, 11] = new Cell(endereco_consignatario3);
                    worksheet.Cells[i, 12] = new Cell(cidade_consignatario);
                    worksheet.Cells[i, 13] = new Cell(estado_consignatario);
                    worksheet.Cells[i, 14] = new Cell(cep_consignatario);
                    worksheet.Cells[i, 15] = new Cell(pais_consignatario);
                    worksheet.Cells[i, 16] = new Cell(email_consignatario);
                    worksheet.Cells[i, 17] = new Cell(telefone_consignatario);
                    worksheet.Cells[i, 18] = new Cell(celular_consignatario);
                    worksheet.Cells[i, 19] = new Cell(CPF_CNPJ_consignatario);
                    worksheet.Cells[i, 20] = new Cell(pecas);
                    worksheet.Cells[i, 21] = new Cell(pesobruto);
                    worksheet.Cells[i, 22] = new Cell(pesoliquido);
                    worksheet.Cells[i, 23] = new Cell(unidade_de_medida);
                    worksheet.Cells[i, 24] = new Cell(altura);
                    worksheet.Cells[i, 25] = new Cell(comprimento);
                    worksheet.Cells[i, 26] = new Cell(largura);
                    worksheet.Cells[i, 27] = new Cell(descricao_mercadoria);
                    worksheet.Cells[i, 28] = new Cell(valor_mercadoria);
                    worksheet.Cells[i, 29] = new Cell(frete);
                    worksheet.Cells[i, 30] = new Cell(moeda);
                    worksheet.Cells[i, 31] = new Cell(tipodeservico);
                    worksheet.Cells[i, 32] = new Cell(conta_embarcador);
                    worksheet.Cells[i, 33] = new Cell(nome_embarcador);
                    worksheet.Cells[i, 34] = new Cell(endereco_embarcador1);
                    worksheet.Cells[i, 35] = new Cell(endereco_embarcador2);
                    worksheet.Cells[i, 36] = new Cell(cidade_embarcador);
                    worksheet.Cells[i, 37] = new Cell(estado_embarcador);
                    worksheet.Cells[i, 38] = new Cell(cep_embarcador);
                    worksheet.Cells[i, 39] = new Cell(pais_embarcador);
                    worksheet.Cells[i, 40] = new Cell(email_embarcador);
                    worksheet.Cells[i, 41] = new Cell(telefone_embarcador);






                    OcorrenciasToRegisterLog.Add(new OcorrenciaConhecimento
                    {
                        Seq = 1,
                        CodigoOcorrencia = 75,
                        SatusEnviado = 1,
                        UkeyConhecimento = _dtReader.GetString(_dtReader.GetOrdinal("UKEY")).Trim(),
                        LogRegister = 1
                    }); ;


                    i++;

                }

                _dtReader.Close();


                //Resolve problema: O Excel encontrou conteúdo ilegível / Invalid or corrupt file (unreadable content)
                for (int j = i; j < 100; j++)
                {
                    worksheet.Cells[i, 1] = new Cell("");
                    worksheet.Cells[i, 2] = new Cell("");
                    worksheet.Cells[i, 3] = new Cell("");
                    worksheet.Cells[i, 4] = new Cell("");
                    worksheet.Cells[i, 5] = new Cell("");
                    worksheet.Cells[i, 6] = new Cell("");
                    worksheet.Cells[i, 7] = new Cell("");
                    worksheet.Cells[i, 8] = new Cell("");
                    worksheet.Cells[i, 9] = new Cell("");
                    worksheet.Cells[i, 10] = new Cell("");
                    worksheet.Cells[i, 11] = new Cell("");
                    worksheet.Cells[i, 12] = new Cell("");
                    worksheet.Cells[i, 13] = new Cell("");
                    worksheet.Cells[i, 14] = new Cell("");
                    worksheet.Cells[i, 15] = new Cell("");
                    worksheet.Cells[i, 16] = new Cell("");
                    worksheet.Cells[i, 17] = new Cell("");
                    worksheet.Cells[i, 18] = new Cell("");
                    worksheet.Cells[i, 19] = new Cell("");
                    worksheet.Cells[i, 20] = new Cell("");
                    worksheet.Cells[i, 21] = new Cell("");
                    worksheet.Cells[i, 22] = new Cell("");
                    worksheet.Cells[i, 23] = new Cell("");
                    worksheet.Cells[i, 24] = new Cell("");
                    worksheet.Cells[i, 25] = new Cell("");
                    worksheet.Cells[i, 26] = new Cell("");
                    worksheet.Cells[i, 27] = new Cell("");
                    worksheet.Cells[i, 28] = new Cell("");
                    worksheet.Cells[i, 29] = new Cell("");
                    worksheet.Cells[i, 30] = new Cell("");
                    worksheet.Cells[i, 31] = new Cell("");
                    worksheet.Cells[i, 32] = new Cell("");
                    worksheet.Cells[i, 33] = new Cell("");
                    worksheet.Cells[i, 34] = new Cell("");
                    worksheet.Cells[i, 35] = new Cell("");
                    worksheet.Cells[i, 36] = new Cell("");
                    worksheet.Cells[i, 37] = new Cell("");
                    worksheet.Cells[i, 38] = new Cell("");
                    worksheet.Cells[i, 39] = new Cell("");
                    worksheet.Cells[i, 40] = new Cell("");
                    worksheet.Cells[i, 41] = new Cell("");
                    worksheet.Cells[i, 42] = new Cell("");
                }



                if (OcorrenciasToRegisterLog.Count() == 0)
                {
                    return null;
                }
                //var filePath = "G:\\DAYTONA\\LOG_ENVIO_ENTRADA_ENTREGA\\" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".html";
                //var filePath = "C:\\"+CNPJ+"_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".XLS";


                // aqui produçao
               var filePath = "G:\\Daytona\\LogOcorrencias\\EDI_PLANILHA_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".xls";
                // pra testes leandro
            //    var filePath = "C:\\TEMP\\EDI_PLANILHA_" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".XLS";

                // geracao antiga 
                // ds.Tables.Add(dt);
                // ExcelLibrary.DataSetHelper.CreateWorkbook(filePath, ds);
                workbook.Worksheets.Add(worksheet);
                workbook.Save(filePath);

                attachmentFilename = filePath;
            }
            catch (Exception e)
            {
                log.RegistraLog("Erro GeraCSV grupo ELG" + e.StackTrace, 3);
            }

            return OcorrenciasToRegisterLog;
        }




    }
}
