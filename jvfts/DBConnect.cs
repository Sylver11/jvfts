using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace jvfts
{
    class DBConnect 
    {
        
        public static int ConnectionAttempts;
        private MySqlConnection connection;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + UserSecrets.db_address + ";" + "DATABASE=" +
            UserSecrets.db_name + ";" + "UID=" + UserSecrets.db_user + ";" + "PASSWORD=" + UserSecrets.db_password + ";";
            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            //problem of using a while loop for trying to access the db is that one
            //cannot insert return false statement at end of exception catching
            //which means that for the first two attempts no error is returned
            //exept from closing the connection which prevents anythings from being deleted in the
            // txt files but still doesn feel like a good solution to this
            while (ConnectionAttempts != 2)
            {
                try
                {
                    connection.Open();
                    if (ConnectionAttempts > 2)
                    {
                        //send email comnfiming everything is in order again
                        var x = new SendMail("DB connection was re-established.");
                    }

                    //in case there were failed attempts before
                    ConnectionAttempts = 0;
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);       
                    ConnectionAttempts++;

                    if (ConnectionAttempts == 2)
                    {
                        //sending email with error message
                        var SendErrorEmail = new SendMail(ex.Message);
                    }
                }

                //let the failed connection attempt sleep for 1 minute
                Thread.Sleep(1000 * 60);
            }

            ConnectionAttempts++;
            return false;
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
