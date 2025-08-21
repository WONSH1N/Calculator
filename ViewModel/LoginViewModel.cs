using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using MySqlConnector;

namespace Calculator.ViewModel
{
    public class LoginViewModel
    {
        private readonly string _connectionString =
            "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";

        public bool Login(string id, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT  COUNT(*) FROM account WHERE id = @id AND password = @pw;";
                    
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@pw", password);

                        var result = Convert.ToInt32(cmd.ExecuteScalar());
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[로그인 오류]\n" + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
