using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator.Model
{
    public class AccountDb
    {
        private readonly string _connectionString =
            "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";
        private readonly string _tableName = "account";
        private readonly string _databaseName = "caldb";

        public AccountDb()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                EnsureTable(conn);
            }
        }

        // 로그인 시도
        public AccountControl Login(string id, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM account WHERE id = @id AND password = @pw LIMIT 1;";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@pw", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new AccountControl
                                {
                                    Account = reader.GetInt32("account"),
                                    Id = reader.GetString("id"),
                                    Password = reader.GetString("password"),
                                    Date = reader.GetDateTime("date"),
                                    Authority = reader.GetString("authority")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그인 오류: " + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

 
        // 테이블 존재 확인 및 생성
        private void EnsureTable(MySqlConnection conn)
        {
            string checkSql = @"
                SELECT COUNT(*)
                FROM information_schema.tables
                WHERE table_schema = @db AND table_name = @account;";

            using (var checkCmd = new MySqlCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@db", _databaseName);
                checkCmd.Parameters.AddWithValue("@table", _tableName);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    string createSql = @"
                        CREATE TABLE account (
                            account INT AUTO_INCREMENT PRIMARY KEY,
                            id VARCHAR(50) NOT NULL,
                            password VARCHAR(100) NOT NULL,
                            date DATETIME,
                            authority VARCHAR(20)
                        );";

                    using (var createCmd = new MySqlCommand(createSql, conn))
                    {
                        createCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("account 테이블이 존재하지 않아 생성했습니다.", "정보", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // 모든 계정 가져오기
        public List<AccountControl> GetAllAccounts()
        {
            var list = new List<AccountControl>();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM account ORDER BY account ASC;";
                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new AccountControl
                            {
                                Account = reader.GetInt32("account"),
                                Id = reader.GetString("id"),
                                Password = reader.GetString("password"),
                                Date = reader.GetDateTime("date"),
                                Authority = reader.GetString("authority")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("계정 조회 오류: " + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }
    }
}
