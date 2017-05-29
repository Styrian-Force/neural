
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using ImageSharp;
using ImageSharp.Formats;
using ImageSharp.Processing;

using Butler.Models;
using Butler.Interfaces;
using Butler.Config;

namespace Butler.Services
{
    public class ImageService : IImageService
    {
        private static readonly int JPG_QUALITY = 80;
        private ILogger<ImageService> _logger;
        private IFileService _fileService;
        private IImageTaskStatusService _imageTaskStatusService;

        private readonly Queue<ImageTask> queue;

        public ImageService(
            ILogger<ImageService> logger,
            IFileService fileService,
            IImageTaskStatusService imageTaskStatusService
        )
        {
            this._logger = logger;
            _logger.LogDebug("IMAGE_SERVICE: Image Service constructor");
            this._fileService = fileService;
            this._imageTaskStatusService = imageTaskStatusService;

            Configuration.Default.AddImageFormat(new JpegFormat());
            Configuration.Default.AddImageFormat(new PngFormat());
            Configuration.Default.AddImageFormat(new GifFormat());
            Configuration.Default.AddImageFormat(new BmpFormat());

            this.queue = new Queue<ImageTask>();

            Thread imageServiceThread = new Thread(HandleQueue);
            imageServiceThread.Start();
        }

        public void AddToQueue(ImageTask imageTask)
        {
            lock (queue)
            {
                queue.Enqueue(imageTask);
                imageTask.Status = ImageTaskStatusCode.ImageInMergeQueue;
                this._imageTaskStatusService.AddToLog(
                    imageTask,
                   ImageTaskStatus.ImageInMergeQueue()
                );
            }
        }

        private void MergeTransparent(Image<Rgba32> finalImage, ImageTask imageTask)
        {
            string transparentImagePath = this._fileService.GetTransparentImagePathWithExt(imageTask);

            using (PixelAccessor<Rgba32> pixels = finalImage.Lock())
            {
                using (FileStream transparent = File.OpenRead(transparentImagePath))
                {
                    Image<Rgba32> transparentImage = ImageSharp.Image.Load(transparent);
                    using (PixelAccessor<Rgba32> transparentPixels = transparentImage.Lock())
                    {
                        for (int i = 0; i < transparentPixels.Width; i++)
                        {
                            for (int j = 0; j < transparentPixels.Height; j++)
                            {
                                var a = transparentPixels[i, j];

                                if (transparentPixels[i, j].R == 1 && transparentPixels[i, j].B == 1 && transparentPixels[i, j].G == 1 && transparentPixels[i, j].A == 255)
                                {
                                    continue;
                                }
                                pixels[i, j] = transparentPixels[i, j];
                            }
                        }
                    }
                }
            }
        }

        private Image<Rgba32> MergeImages(ImageTask imageTask)
        {
            string originalImagePath = this._fileService.GetOriginalImagePath(imageTask);
            string workingDir = this._fileService.GetWorkingDir(imageTask);
            string artistDir = this._fileService.GetArtistDir(imageTask);

            using (var original = File.OpenRead(originalImagePath))
            {
                Image<Rgba32> finalImage = ImageSharp.Image.Load(original);

                using (PixelAccessor<Rgba32> pixels = finalImage.Lock())
                {
                    foreach (Models.Image image in imageTask.CroppedImages)
                    {
                        string croppedPath = artistDir + image.Id + ".png";

                        using (var cropped = File.OpenRead(croppedPath))
                        {
                            var croppedImage = ImageSharp.Image.Load(cropped);
                            using (PixelAccessor<Rgba32> croppedPixels = croppedImage.Lock())
                            {
                                for (int i = 0; i < croppedPixels.Width; i++)
                                {
                                    for (int j = 0; j < croppedPixels.Height; j++)
                                    {
                                        int pixelI = i + image.Left;
                                        int pixelJ = j + image.Top;

                                        if (pixelI < 0 || pixelI >= pixels.Width || pixelJ < 0 || pixelJ >= pixels.Height)
                                        {
                                            continue;
                                        }
                                        pixels[pixelI, pixelJ] = croppedPixels[i, j];
                                    }
                                }
                            }
                        }
                    }


                    return finalImage;
                }
            }
        }

        public bool FileTypeSupported(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            try
            {
                ImageSharp.Image.Load(stream);
            }
            catch (NotSupportedException exception)
            {
                _logger.LogDebug(exception.Message);
                return false;
            }
            return true;
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
                    imageTask.Status = ImageTaskStatusCode.ImageMerging;
                    this._imageTaskStatusService.AddToLog(
                        imageTask,
                       ImageTaskStatus.ImageMerging()
                    );

                    Image<Rgba32> finalImage = this.MergeImages(imageTask);
                    MergeTransparent(finalImage, imageTask);

                    string mergedImagePath = this._fileService.GetMergedImagePathWithExt(imageTask);

                    using (var output = File.OpenWrite(mergedImagePath))
                    {
                        JpegEncoderOptions options = new JpegEncoderOptions();
                        options.Quality = JPG_QUALITY;
                        finalImage.SaveAsJpeg(output, options);
                    }

                    imageTask.Status = ImageTaskStatusCode.ImageFinished;
                    this._imageTaskStatusService.AddToLog(
                        imageTask,
                       ImageTaskStatus.ImageFinished()
                    );
                }
                else {
                    Thread.Sleep(20);
                }
            }
        }
    }

    /*private class JpegEncoderOptions : IJpegEncoderOptions
    {
        private int Quality;
        private bool IgnoreMetadata;
        private JpegSubsample Subsample;
        JpegEncoderOptions(int Quality, bool IgnoreMetadata, JpegSubsample Subsample) {
            this.Quality = Quality;
            this.IgnoreMetadata = IgnoreMetadata;
            this.Subsample = Subsample;
        }

        public int Quality => throw new NotImplementedException();

        public JpegSubsample? Subsample => throw new NotImplementedException();

        public bool IgnoreMetadata => throw new NotImplementedException();
    }*/
}