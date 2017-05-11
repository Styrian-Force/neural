
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
        private ILogger<ImageService> _logger;
        private IFileService _fileService;

        public ImageService(
            ILogger<ImageService> logger,
            IFileService fileService
        )
        {
            Console.WriteLine("IMAGE_SERVICE: Image Service constructor");
            this._logger = logger;
            this._fileService = fileService;

            Configuration.Default.AddImageFormat(new JpegFormat());
            Configuration.Default.AddImageFormat(new PngFormat());
            Configuration.Default.AddImageFormat(new GifFormat());
            Configuration.Default.AddImageFormat(new BmpFormat());
        }

        public void MergeImages(ImageTask imageTask)
        {
            string originalImagePath = this._fileService.GetOriginalImagePath(imageTask);
            string workingDir = this._fileService.GetWorkingDir(imageTask);
            string artistDir = this._fileService.GetArtistDir(imageTask);
            string mergedImagePath = this._fileService.GetMergedImagePath(imageTask);

            using (var original = File.OpenRead(originalImagePath))
            {
                using (var output = File.OpenWrite(mergedImagePath))
                {
                    var finalImage = ImageSharp.Image.Load(original);

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

                                            if(pixelI < 0 || pixelI >= pixels.Width || pixelJ < 0 || pixelJ >= pixels.Height ) {
                                                continue;
                                            }
                                            pixels[pixelI, pixelJ] = croppedPixels[i, j];
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //image.Quality = quality;
                    finalImage.SaveAsPng(output);
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
    }

}