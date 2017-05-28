
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
        private static readonly string STYLE_MODEL = "udnie.ckpt";

        private readonly ILogger<ArtistService> _logger;
        private readonly IFileService _fileService;
        private readonly IImageTaskStatusService _imageTaskStatusService;
        private readonly IImageService _imageService;

        private readonly Queue<ImageTask> queue;

        public ArtistService(
            ILogger<ArtistService> logger,
            IFileService fileService,
            IImageTaskStatusService taskStatusService,
            IImageService imageService
        )
        {
            this._logger = logger;
            _logger.LogDebug("ALERT_SERVICE: Artist Service constructor");            
            this._fileService = fileService;
            this._imageTaskStatusService = taskStatusService;
            this._imageService = imageService;

            this.queue = new Queue<ImageTask>();

            Thread artistThread = new Thread(HandleQueue);
            artistThread.Start();
        }

        public void AddToQueue(ImageTask imageTask)
        {
            lock (queue)
            {
                imageTask.Status = ImageTaskStatusCode.ImageInArtistQueue;
                this._imageTaskStatusService.AddToLog(
                    imageTask,
                    ImageTaskStatus.ImageInArtistQueue()
                );
                queue.Enqueue(imageTask);
            }
        }

        private ProcessStartInfo GetArtistProcessInfo(ImageTask imageTask)
        {
            imageTask.Status = ImageTaskStatusCode.ImageInArtist;
            this._imageTaskStatusService.AddToLog(
                imageTask,
                ImageTaskStatus.ImageInArtist()
            );

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
            arguments += " --checkpoint style_models/" + STYLE_MODEL;
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
                    bool wasStarted = artistProcess.Start();
                    _logger.LogInformation("Artist process was started: " + wasStarted);

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

                    _logger.LogDebug("ID:" + imageTask.JobId);
                    //imageTask.Task.Start();
                    this._imageService.AddToQueue(imageTask);
                }
            }
        }

    }

}