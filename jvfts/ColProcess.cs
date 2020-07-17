using System;
using System.Collections.Generic;
using System.Globalization;

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
                transaction.entry_date = DateTime.ParseExact(parts[0], "ddMMyyyy", null).ToString("yyyy/MM/dd");
                transaction.client_name = parts[1];
                transaction.client_code = parts[2];
                transaction.gl_account_number = Convert.ToInt32(parts[3]);
                transaction.transaction_amount = Convert.ToDouble(parts[4]);
                TransactionInfo.Add(transaction);
            }

            //TODO parsing the data to the db here
            foreach (var item in TransactionInfo)
            {
                Console.WriteLine("A account numbver: '{0}'.", item.entry_date);
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
