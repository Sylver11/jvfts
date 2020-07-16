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
            {   // instead of using the directory Processing I could have used GetTempFile method
                string sourceDirectory = "/Users/justus/source";
                string targetDirectory = System.IO.Directory.GetCurrentDirectory() + "/Processing";
                string[] files = Directory.GetFiles(targetDirectory);
                foreach (string fileName in files)
                {
                    List<string> filelines = File.ReadAllLines(fileName).ToList();
                    for (int i = 0; i < filelines.Count; i = i + 100)
                    {
                        // batching 100 items together and processing them with ColProcess
                        var items = string.Join(",", filelines.Skip(i).Take(100));

                        //after success taking the the batch and deleting the lines from the temporary file in the processing directory 
                        string[] filteredLines = File.ReadAllLines(fileName).Where(l => !l.Contains(items)).ToArray();

                        Console.WriteLine("Batches of 100: '{0}'.", items);
                    }
                    
                    Console.WriteLine("Next file");
                }
                
                // most propbaly unnecessary to create an instance of the BlockAndDelay class here
                //TODO congigure BlockAndDelay to run on its own terms
                var file = new BlockAndDelay(sourceDirectory);
                await Task.Delay(2000, stoppingToken);
            }
        }

    }
}
