using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace butler
{
    public class Program
    {
        private static readonly int SERVER_DEFAULT_PORT = 5026;
        private static readonly string SERVER_URL = "http://localhost";
        public static Process detectorProcess; 

        public static void Main(string[] args)
        {
            //startDetector();
            int port = SERVER_DEFAULT_PORT;
            if(args.Length > 0) {
                port = Int32.Parse(args[0]);
            }            

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                // Server port should be the same as in /etc/nginx/sites-available/default 
                .UseUrls(SERVER_URL + ":" + port)
                .Build();

            host.Run();
        }

        private static void startDetector()
        {
            ProcessStartInfo info = new ProcessStartInfo("/home/administrator/dev/git/neural/other/a.out");
            info.CreateNoWindow = false;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            
            Process process = new Process();
            process.StartInfo = info;

            process.Start();

            Program.detectorProcess = process;
        }
    }
}
