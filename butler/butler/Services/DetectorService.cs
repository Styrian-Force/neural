
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Butler.Config;
using Butler.Interfaces;
using Butler.Models;
using Microsoft.Extensions.Logging;

namespace Butler.Services
{
    public class DetectorService : IDetectorService
    {
        public static Weights WEIGHTS = Weights.TINY_YOLO_VOC; 

        private ILogger<DetectorService> _logger;
        private IFileService _fileService;

        private Process detectorProcess;
        private Queue<ImageTask> queue;

        public DetectorService(
            ILogger<DetectorService> logger,
            IFileService fileSerice
        )
        {
            Console.WriteLine("DETECTOR_SERVICE: DetectorService konstruktor");
            this._logger = logger;
            this._fileService = fileSerice;
            this.queue = new Queue<ImageTask>();

            // start detector process
            Process process = new Process();
            process.StartInfo = GetDetectorProcessInfo();
            this.detectorProcess = process;

            Thread detectorThread = new Thread(StartDetector);
            detectorThread.Start();          
        }

        ~DetectorService() {
            this.detectorProcess.Kill();
        }

        public void AddToQueue(ImageTask imageTask)
        {
            lock (queue)
            {
                queue.Enqueue(imageTask);
            }
        }

        private ProcessStartInfo GetDetectorProcessInfo()
        {
            ProcessStartInfo info = new ProcessStartInfo();

            info.CreateNoWindow = false;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            info.WorkingDirectory = ButlerConfig.NEURAL_GIT_DIR + "detector";
            info.FileName = info.WorkingDirectory + "/darknet";

            string arguments = "detect";
            arguments += " " + WEIGHTS.CfgPath;
            arguments += " " + WEIGHTS.WeightsPath;

            info.Arguments = arguments;

            return info;
        }

        private void StartDetector()
        {            
            this.detectorProcess.Start();

            StreamReader stdout = this.detectorProcess.StandardOutput;

            while (true)
            {
                string line = stdout.ReadLine();
                if(line == null) {
                    continue;
                }

                _logger.LogDebug("DETECTOR_OUTPUT: " + line);

                if (line == "DETECTOR_READY")
                {
                    Console.WriteLine("DETECTOR IS READY");
                    break;
                }
            }

            Console.WriteLine("DoWork thread finished! DetectorReady!");
            this.HandleQueue();
        }

        private void HandleQueue()
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

                    string originalImagePath = this._fileService.GetOriginalImagePath(imageTask);
                    string detectorImagePath = this._fileService.GetDetectorImagePath(imageTask);
                    string detectorDir = this._fileService.GetDetectorDir(imageTask);
                    string transparentImagePath =  this._fileService.GetTransparentImagePath(imageTask);

                    inputWriter.WriteLine(originalImagePath);
                    inputWriter.WriteLine(detectorImagePath);
                    inputWriter.WriteLine(detectorDir);
                    inputWriter.WriteLine(transparentImagePath);
                    inputWriter.Flush();

                    List<Image> croppedImages = new List<Image>();
                    while (true)
                    {
                        string message = outputReader.ReadLine();
                        if(message == null) {
                            continue;
                        }
                        Debug.WriteLine("DETECTOR_OUTPUT: " + message);
                        if (message == "FINISHED_SUCCESSFULLY")
                        {                            
                            break;
                        }
                        if (message.Contains("BOX"))
                        {
                            Image image = ParseImage(message);
                            croppedImages.Add(image);
                        }
                    }
                    imageTask.CroppedImages = croppedImages;
                    _logger.LogDebug(originalImagePath + " successfully created.");
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