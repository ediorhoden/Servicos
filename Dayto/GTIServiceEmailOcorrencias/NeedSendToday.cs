using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias
{
    public static class NeedSendToday
    {

        public static bool isNow(float Days, string relType,int timeTosend)
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
                                TYPEREL = '" + relType + "'";

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

                   
                    if (UltimoEnvio.Date <= DateTime.Now.Date.AddDays(Days) && DateTime.Now.Hour == timeTosend)
                    {
                        return true;
                    }

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

