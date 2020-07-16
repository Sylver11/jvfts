using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace jvfts
{
    public class ColProcess
    {
        public ColProcess(string path)
        {

            List<ColData> allClientInfo = new List<ColData>();
            List<string> filelines = File.ReadAllLines(path).ToList();
            filelines.RemoveAt(0);
            foreach (string line in filelines)
            {
                string[] parts = line.Split(',');

                ColData transaction = new ColData();
                transaction.entry_date = parts[0];
                transaction.client_name = parts[1];
                transaction.client_code = parts[2];
                transaction.gl_account_number = Convert.ToInt32(parts[3]);
                transaction.transaction_amount = Convert.ToDouble(parts[4]);
                //onvert.ToDecimal(parts[4]);

                allClientInfo.Add(transaction);
            }
            foreach (var item in allClientInfo)
            {
                Console.WriteLine("A client name: '{0}'.", item.client_name);
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
