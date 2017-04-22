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
        public static Process detectorProcess; 

        public static void Main(string[] args)
        {
            startDetector();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                // TODO: comment this line on publish
                .UseUrls("http://localhost:5001")
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
