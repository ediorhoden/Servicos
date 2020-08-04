using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSerrviceIntAptus
{
    public class clConexao
    {
        private SqlConnection _cConexao;
        private string _serverProducao = "Data Source=10.251.15.182; Initial Catalog=STARSOFT;User Id=hagana-apps;Password=blVDd!66rcQi;Connection Timeout=0;Connection Lifetime = 0";

        public SqlConnection getConexao()
        {

            if (_cConexao == null)
            {
                _cConexao = new SqlConnection(_serverProducao);
                _cConexao.Open();
            }
            else
            {
                if (_cConexao.State == ConnectionState.Closed)
                {
                    _cConexao.Open();
                }
            }

            return _cConexao;

        }


    }
}
