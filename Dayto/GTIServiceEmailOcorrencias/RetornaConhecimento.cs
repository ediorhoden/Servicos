using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias
{
    public static class RetornaConhecimento
    {
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
