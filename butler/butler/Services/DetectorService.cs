
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Butler.Interfaces;
using Butler.Models;
using Microsoft.Extensions.Logging;

namespace Butler.Services
{
    public class DetectorService : IDetectorService
    {
        ILogger<DetectorService> _logger;

        private Process detectorProcess;
        private Queue<ImageTask> queue;

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
            this.queue = new Queue<ImageTask>();
        }

        public void AddToQueue(ImageTask imageTask)
        {
            lock (queue)
            {
                queue.Enqueue(imageTask);
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
                ImageTask imageTask = null;

                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        imageTask = queue.Dequeue();
                    }
                }

                if (imageTask != null)
                {
                    StreamWriter inputWriter = this.detectorProcess.StandardInput;
                    StreamReader outputReader = this.detectorProcess.StandardOutput;

                    inputWriter.WriteLine(imageTask.InputFilePath);
                    inputWriter.WriteLine(imageTask.WorkingDir);
                    inputWriter.WriteLine(imageTask.SubDir);
                    inputWriter.Flush();

                    List<Image> croppedImages = new List<Image>();
                    while (true)
                    {
                        string message = outputReader.ReadLine();
                        Debug.WriteLine("DETECTOR_OUTPUT " + (counter++) + ": " + message);
                        if (message == "FINISHED_SUCCESSFULLY")
                        {
                            Debug.WriteLine(message);
                            break;
                        }
                        if (message.Contains("BOX"))
                        {
                            Image image = ParseImage(message);
                            croppedImages.Add(image);
                        }
                    }
                    imageTask.CroppedImages = croppedImages;
                    _logger.LogDebug(imageTask.InputFilePath + " successfully created.");
                    Console.WriteLine("ID:" + imageTask.JobId);
                    imageTask.task.Start();
                }
            }
        }

        private Image ParseImage(string message)
        {
            int index = message.IndexOf("BOX");
            if (index != -1)
            {
                message = message.Substring(3);
            }

            string[] fields = message.Trim().Split();

            Image image = new Image();
            image.Id = int.Parse(fields[0]);
            image.Left = int.Parse(fields[1]);
            image.Top = int.Parse(fields[2]);
            image.Width = int.Parse(fields[3]);
            image.Heigth = int.Parse(fields[4]);

            return image;
        }

    }
}