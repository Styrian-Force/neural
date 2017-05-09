
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
    public class ArtistService : IArtistService
    {
        private static readonly string DEVICE = "/cpu:0";
        private static readonly int MAX_ITERATIONS = 3;
        private static readonly int PRINT_ITERATIONS = 1;

        ILogger<ArtistService> _logger;

        //private Process artistProcess;
        private Queue<ImageTask> queue;

        public ArtistService(
            ILogger<ArtistService> logger
        )
        {
            Console.WriteLine("ALERT_SERVICE: Artist Service constructor");

            // start artist process
            //Process process = new Process();
            //process.StartInfo = GetArtistProcessInfo();
            //this.artistProcess = process;

            this._logger = logger;
            this.queue = new Queue<ImageTask>();

            //Thread artistThread = new Thread(StartArtist);
            //artistThread.Start(process);
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

        private ProcessStartInfo GetArtistProcessInfo(ImageTask imageTask, Image image)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            info.CreateNoWindow = false;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            info.WorkingDirectory = "/home/administrator/dev/git/neural-style-tf";
            info.FileName = "/usr/bin/python3";

            string arguments = "neural_style.py";
            arguments += " --content_img " + image.Id + ".png";
            arguments += " --content_img_dir " + imageTask.DetectorDir;
            arguments += " --style_imgs starry-night.jpg";
            arguments += " --style_imgs_dir ./styles";
            arguments += " --device " + DEVICE;
            arguments += " --max_iterations " + MAX_ITERATIONS;
            arguments += " --print_iterations " + PRINT_ITERATIONS;
            //arguments += " --original_colors";
            arguments += " --img_name " + image.Id;
            arguments += " --img_output_dir " + imageTask.ArtistDir;
            arguments += " --verbose";

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
                    foreach (Image image in imageTask.CroppedImages)
                    {
                        Process artistProcess = new Process();
                        artistProcess.StartInfo = GetArtistProcessInfo(imageTask, image);
                        artistProcess.Start();

                        //StreamWriter inputWriter = artistProcess.StandardInput;
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
                    }

                    Console.WriteLine("ID:" + imageTask.JobId);
                    imageTask.task.Start();
                }
            }
        }

    }

}