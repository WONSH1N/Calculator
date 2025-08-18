using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Calculator.Model
{
    public class Cal_Memory
    {
        private readonly string _connectionString =
            "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";

        public void Save(double value)
        {
            using (var conn = new MySqlConnection(_connectionString))
            { 
            conn.Open();
            var cmd = new MySqlCommand("INSERT INTO memorystore (Value) VALUES (@val)", conn);
            cmd.Parameters.AddWithValue("@val", value);
            cmd.ExecuteNonQuery();
            }
        }
        

        public double? Load()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT Value FROM memorystore ORDER BY Id DESC LIMIT 1", conn);
                var result = cmd.ExecuteScalar();
                return result != null ? (double?)Convert.ToDouble(result) : null;
            }
          
        }

        public void Clear()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM MemoryStore", conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
