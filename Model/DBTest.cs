using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model
{
    public static class DBTest
    {
        public static bool TestConnection(string connectionString)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("연결 성공!");
                    return true; // 연결 성공
                }
            }
            catch (Exception ex)
            {
                    Console.WriteLine("연결 실패: " + ex.Message);
                    return false;
            }
        }
        
    }
}
