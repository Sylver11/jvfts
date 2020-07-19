using System;
using System.Collections.Generic;

namespace jvfts
{
    public class ColProcess
    {
        public ColProcess(string transaction_batches)
        {
            List<string> Rows = new List<string>();
            string[] transactions = transaction_batches.Split(';');
            foreach (string transaction_string in transactions)
            {
                string[] parts = transaction_string.Split(',');
                string entry_date = DateTime.ParseExact(parts[0], "ddMMyyyy", null).ToString("yyyy/MM/dd");
                string client_name = parts[1];
                string client_code = parts[2];
                int gl_account_number = Convert.ToInt32(parts[3]);
                decimal transaction_amount = Convert.ToDecimal(parts[4]);
                Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}')", entry_date, client_name, client_code, gl_account_number, transaction_amount));
            }

            var db = new DBConnect();
            db.Insert(Rows);




        }
    }
}
