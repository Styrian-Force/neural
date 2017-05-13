
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
    public class ArtistService : IArtistService
    {
        private static readonly Device DEVICE = Device.GPU;
        private static readonly int MAX_ITERATIONS = 2;
        private static readonly int PRINT_ITERATIONS = 1;
        private static readonly bool ORIGINAL_COLORS = false;
        private static readonly string style_model = "wave.ckpt";

        private ILogger<ArtistService> _logger;
        private IFileService _fileService;

        private Queue<ImageTask> queue;

        public ArtistService(
            ILogger<ArtistService> logger,
            IFileService fileService
        )
        {
            Console.WriteLine("ALERT_SERVICE: Artist Service constructor");

            this._logger = logger;
            this._fileService = fileService;

            this.queue = new Queue<ImageTask>();

            Thread artistThread = new Thread(HandleQueue);
            artistThread.Start();
        }

        public void AddToQueue(ImageTask imageTask)
        {
            lock (queue)
            {
                queue.Enqueue(imageTask);
            }
        }

        private ProcessStartInfo GetArtistProcessInfo(ImageTask imageTask)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            info.CreateNoWindow = false;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            info.WorkingDirectory = ButlerConfig.NEURAL_GIT_DIR + "artist";
            info.FileName = "/usr/bin/python3";

            string arguments = "evaluate.py";
            arguments += " --allow-different-dimensions";
            arguments += " --device " + DEVICE.Id;
            arguments += " --checkpoint style_models/" + style_model;
            arguments += " --in-path " + this._fileService.GetDetectorDir(imageTask);

            //arguments += " --print_iterations " + PRINT_ITERATIONS;

            arguments += " --out-path " + this._fileService.GetArtistDir(imageTask);

            info.Arguments = arguments;

            return info;
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
                    Process artistProcess = new Process();
                    artistProcess.StartInfo = GetArtistProcessInfo(imageTask);
                    artistProcess.Start();

                    StreamReader outputReader = artistProcess.StandardOutput;

                    while (true)
                    {
                        string message = outputReader.ReadLine();
                        if (message != null)
                        {
                            Debug.WriteLine("ARTIST_OUTPUT: " + message);
                        }

                        if (message == "FINISHED_SUCCESSFULLY")
                        {
                            break;
                        }
                    }

                    Console.WriteLine("ID:" + imageTask.JobId);
                    imageTask.task.Start();
                }
            }
        }

    }

}