using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace jvfts
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration Configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            UserSecrets.db_password = Configuration["DB:password"];
            UserSecrets.db_user = Configuration["DB:user"];
            UserSecrets.db_name = Configuration["DB:name"];
            UserSecrets.db_address = Configuration["DB:address"];
            UserSecrets.email_address = Configuration["EMAIL:address"];
            UserSecrets.email_password = Configuration["EMAIL:password"];
            UserSecrets.email_mail_server = Configuration["EMAIL:mail_server"];
            UserSecrets.source_path = Configuration["source_path"];
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // instead of using the directory Processing I could use GetTempFile method
                string sourceDirectory = UserSecrets.source_path;

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
                        //TODO find more efficient way to remove first line
                        var filelines = File.ReadAllLines(fileName);
                        File.WriteAllLines(fileName, filelines.Skip(1).ToList());


                        //TODO inefficient way of reading the data
                        //more efficient would be with ReadLines instead of ReadAllLines

                        // check if the file has any content
                        if (filelines.Length > 0)
                        {
                            for (int i = 0; i < filelines.Length; i++)
                            {
                                filelines = File.ReadAllLines(fileName);

                                // batching 100 items together
                                var items = string.Join(";",filelines.Take(100));

                                // passing 100 items to the ColProcess where data get manipulated and passed to the db
                                var x = new ColProcess(items);

                                //skip 100 lines and write new file
                                File.WriteAllLines(fileName, filelines.Skip(100));
                            }
                        }
                        else
                        {
                            //delete file if empty
                            File.Delete(fileName);
                            Console.WriteLine("Deleted file: '{0}'.", fileName);

                            //not sure if the service should also delete the files in the source file
                            //otherwise that would also happen here

                        }
                    Console.WriteLine("Next file");
                    }
                }
                // most propbaly unnecessary to create an instance of the BlockAndDelay class here
                //TODO configure BlockAndDelay to run in its own terms
                var file = new BlockAndDelay(sourceDirectory);
                await Task.Delay(2000, stoppingToken);
            }
        }

    }
}
