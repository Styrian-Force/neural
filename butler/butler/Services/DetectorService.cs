
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using butler.Interfaces;
using butler.Models;
using Microsoft.Extensions.Logging;

namespace butler.Services
{
    public class DetectorService : IDetectorService
    {
        ILogger<DetectorService> _logger;

        private Process detectorProcess;
        private Queue<Image> queue;

        public DetectorService(
            ILogger<DetectorService> logger
        )
        {
            Console.WriteLine("HERE!: DetectorService konstruktor");

            // start detector process
            Process process = new Process();
            process.StartInfo = InitDetectorProcessInfo();
            this.detectorProcess = process;

            Thread detectorThread = new Thread(StartDetector);
            detectorThread.Start(process);

            this._logger = logger;
            this.queue = new Queue<Image>();
        }

        public void AddToQueue(Image image)
        {
            lock (queue)
            {
                queue.Enqueue(image);
            }
        }

        private ProcessStartInfo InitDetectorProcessInfo()
        {
            ProcessStartInfo info = new ProcessStartInfo();

            info.CreateNoWindow = false;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            info.WorkingDirectory = "/home/administrator/dev/git/neural/detector";
            info.FileName = info.WorkingDirectory + "/darknet";
            info.Arguments = "detect cfg/tiny-yolo-voc.cfg weights/tiny-yolo-voc.weights";

            return info;
        }

        private void StartDetector(object data)
        {
            Process process = (Process)data;
            process.Start();

            StreamReader stdout = process.StandardOutput;
            while (true)
            {
                string line = stdout.ReadLine();
                if (line != null && line == "DETECTOR_READY")
                {
                    Console.WriteLine("DETECTOR IS READY");
                    break;
                }
            }

            Console.WriteLine("DoWork thread finished! DetectorReady!");
            this.HandleImages();
        }

        private int counter = 0;

        private void HandleImages()
        {
            while (true)
            {
                Image image = null;

                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        image = queue.Dequeue();
                    }
                }

                if (image != null)
                {
                    StreamWriter inputWriter = this.detectorProcess.StandardInput;
                    StreamReader outputReader = this.detectorProcess.StandardOutput;

                    inputWriter.WriteLine(image.InputFilePath);
                    inputWriter.WriteLine(image.WorkingDir);
                    inputWriter.WriteLine(image.SubDir);
                    inputWriter.Flush();
                    while (true)
                    {
                        string message = outputReader.ReadLine();
                        Debug.WriteLine("POT_DO_DATOTEKE " + (counter++) + ": " + message);
                        if (message == "FINISHED_SUCCESSFULLY")
                        {
                            Debug.WriteLine(message);
                            break;
                        }
                    }
                    _logger.LogDebug(image.InputFilePath + " successfully created.");
                    image.task.Start();
                }
            }
        }

    }
}