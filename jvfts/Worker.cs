using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace jvfts
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string[] arr = new string[] { };
        private string[] bigList = new string[] { };

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string sourceDirectory = "/Users/justus/source";
                string targetDirectory = System.IO.Directory.GetCurrentDirectory() + "/Processing";
                string[] files = Directory.GetFiles(targetDirectory);
                foreach (string fileName in files)
                {
                    List<string> filelines = File.ReadAllLines(fileName).ToList();
                    for (int i = 0; i < filelines.Count; i = i + 100)
                    {
                        var items = string.Join(",", filelines.Skip(i).Take(100));

                        //var items = bigList.Skip(i).Take(100);
           
                        //string[] parts = items.Split(',');
                        //var split_items = items.Split(',')
                        //ColData transaction = new ColData();
                        //transaction.entry_date = parts[0];
                        //transaction.client_name = parts[1];
                        //transaction.client_code = parts[2];
                        //transaction.gl_account_number = Convert.ToInt32(parts[3]);
                        //transaction.transaction_amount = Convert.ToDouble(parts[4]);
                        Console.WriteLine("Wachting for changes in '{0}'.", items);

                        // Do something with 100 or remaining items
                    }
                    Console.WriteLine("Next file");
                }
                
                //foreach (string fileName in fileEntries)
                //{
                    //List<string> filelines = File.ReadAllLines(fileName).ToList();
                    //_ = new ColProcess(fileName);
                //}
                //Console.WriteLine("Wachting for changes in '{0}'.", fileName);
                //var x = ProcessDirectory(targetPath);
                var file = new BlockAndDelay(sourceDirectory);
                await Task.Delay(2000, stoppingToken);
            }
        }

    }
}
