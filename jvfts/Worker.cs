using System;
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

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // instead of using the directory Processing I could have used GetTempFile method
                string sourceDirectory = "/Users/justus/source";

                // writing to direcotory inside project folder
                //TODO exclude Processing folder from .gitignore file
                string targetDirectory = Directory.GetCurrentDirectory() + "/Processing";
                string[] files = Directory.GetFiles(targetDirectory);

                if (files == null)
                {
                    Console.WriteLine("Files does not exis.");
                }

                else if (files.Length > 0)
                {
                    foreach (string fileName in files)
                    {
                        //removing column descriptions on the first line of each file
                        var filelines = File.ReadAllLines(fileName);
                        File.WriteAllLines(fileName, filelines.Skip(1).ToList());
                        filelines = File.ReadAllLines(fileName);

                        // check if the file has any content
                        if (filelines.Length > 0)
                        {
                            for (int i = 0; i < filelines.Length; i++)
                            {
                                //TODO Check what happens to last batch when less than 100

                                // batching 100 items together
                                var items = string.Join(";", filelines.Take(100));

                                // passing 100 items to the ColProcess where data get manipulated and passed to the db
                                var x = new ColProcess(items);

                                //skip 100 lines and write new file
                                filelines = File.ReadAllLines(fileName);
                                File.WriteAllLines(fileName, filelines.Skip(100).ToList());
                            }
                        }
                        else
                        {
                            //delete file if empty
                            Console.WriteLine("Deleted file: '{0}'.", fileName);
                            File.Delete(fileName);
                        }
                        Console.WriteLine("Next file");
                    }
                }

                // most propbaly unnecessary to create an instance of the BlockAndDelay class here
                //TODO congigure BlockAndDelay to run on its own terms
                var file = new BlockAndDelay(sourceDirectory);
                await Task.Delay(2000, stoppingToken);
            }
        }

    }
}
