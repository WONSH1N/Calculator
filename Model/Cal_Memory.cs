using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Data.Controls.ExpressionEditor;
using DevExpress.XtraRichEdit.Commands;
using MySqlConnector;

namespace Calculator.Model
{

    public class Cal_Memory
    {
        private readonly string _connectionString =
            "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";
        private readonly string _tableName = "memorystore";
        private readonly string _databaseName = "caldb";

        public void Save(double value)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    EnsureTable(conn);

                    string insertSql = "INSERT INTO memorystore (Value) VALUES (@val);";
                    using (var cmd = new MySqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@val", value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("[메모리 저장 실패]\n" + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public double? Load()
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    EnsureTable(conn);

                    string selectSql = "SELECT Value FROM memorystore ORDER BY Id DESC LIMIT 1;";
                    using (var cmd = new MySqlCommand(selectSql, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null ? (double?)Convert.ToDouble(result) : null;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("[메모리 불러오기 실패]\n" + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public void Clear()
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    EnsureTable(conn);

                    string deleteSql = "DELETE FROM memorystore;";
                    using (var cmd = new MySqlCommand(deleteSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("[메모리 삭제 실패]\n" + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EnsureTable(MySqlConnection conn)
        {
            string checkSql = @"
                SELECT COUNT(*)
                FROM information_schema.tables
                WHERE table_schema = @db AND table_name = @table;
            ";

            using (var checkCmd = new MySqlCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@db", _databaseName);
                checkCmd.Parameters.AddWithValue("@table", _tableName);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count == 0)
                {
                    string createSql = @"
                        CREATE TABLE memorystore (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Value DOUBLE
                        );
                    ";

                    using (var createCmd = new MySqlCommand(createSql, conn))
                    {
                        createCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("테이블이 존재하지 않아 새로 생성했습니다.", "정보", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}


