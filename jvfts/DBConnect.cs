using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace jvfts
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            //TODO set environment variables for credentials to be retrieved
            server = "localhost";
            database = "jvfts";
            uid = "root";
            password = "pass";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Insert(List<string> Rows)
        {
            //TODO find better place to create new table if not exists
            string sql =
                    @" CREATE TABLE IF NOT EXISTS transactions
                    (id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                    entry_date DATE NOT NULL,
                    client_name VARCHAR(255) NOT NULL,
                    client_code VARCHAR(7) NOT NULL,
                    gl_account_number SMALLINT NOT NULL,
                    transaction_amount DECIMAL(19, 4) NOT NULL);";

            StringBuilder sCommand = new StringBuilder("INSERT INTO transactions (entry_date, client_name, client_code, gl_account_number, transaction_amount) VALUES");

            if (this.OpenConnection() == true)
            {
                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), connection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }

                this.CloseConnection();
            }
        }
    }
}
