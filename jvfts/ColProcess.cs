using System;
using System.Collections.Generic;

namespace jvfts
{
    public class ColProcess
    {
        public ColProcess(string transaction_batches)
        {
            List<ColData> TransactionInfo = new List<ColData>();
            string[] transactions = transaction_batches.Split(';');
            foreach (string transaction_string in transactions)
            {
                string[] parts = transaction_string.Split(',');
                ColData transaction = new ColData();
                transaction.entry_date = parts[0];
                transaction.client_name = parts[1];
                transaction.client_code = parts[2];
                transaction.gl_account_number = Convert.ToInt32(parts[3]);
                transaction.transaction_amount = Convert.ToDouble(parts[4]);
                TransactionInfo.Add(transaction);
            }

            int i = 0;
            foreach (var item in TransactionInfo)
            {
                //Console.WriteLine("this runs" );
                //Console.WriteLine("A client name: '{0}'.", i);
                i++;
            }
        }
        class ColData
        {
            public string entry_date { get; set; }
            public string client_name { get; set; }
            public string client_code { get; set; }
            public int gl_account_number { get; set; }
            public double transaction_amount { get; set; }
        }
    }
}
