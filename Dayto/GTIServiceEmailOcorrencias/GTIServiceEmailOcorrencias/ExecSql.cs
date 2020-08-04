using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias
{
    public static class ExecSql
    {
        private static SqlConnection _conexao;

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

        public static bool execsqlCmd(string cmd)
        {


            SqlConnection conexao = retornaConexao();
            SqlCommand sqlc;

            sqlc = new SqlCommand(cmd, conexao);
            sqlc.CommandTimeout = 120;

            try
            {
                sqlc.ExecuteNonQuery();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

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
    }
}
