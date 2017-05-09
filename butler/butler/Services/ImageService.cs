
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Butler.Interfaces;
using Microsoft.Extensions.Logging;
using ImageSharp;
using ImageSharp.Formats;
using ImageSharp.Processing;
using Butler.Models;

namespace Butler.Services
{
    public class ImageService : IImageService
    {
        ILogger<ImageService> _logger;

        public ImageService(
            ILogger<ImageService> logger
        )
        {
            Console.WriteLine("IMAGE_SERVICE: Image Service constructor");
            this._logger = logger;

            Configuration.Default.AddImageFormat(new JpegFormat());
            Configuration.Default.AddImageFormat(new PngFormat());
        }

        public void MergeImages(ImageTask imageTask)
        {
            using (var original = File.OpenRead(imageTask.OriginalImagePath))
            {
                using (var output = File.OpenWrite(imageTask.WorkingDir + "butler_artist.png"))
                {
                    var finalImage = ImageSharp.Image.Load(original);

                    using (PixelAccessor<Rgba32> pixels = finalImage.Lock())
                    {
                        foreach (Models.Image image in imageTask.CroppedImages)
                        {
                            string croppedPath = imageTask.ArtistDir + image.Id + ".png";

                            using (var cropped = File.OpenRead(croppedPath))
                            {
                                var croppedImage = ImageSharp.Image.Load(cropped);
                                using (PixelAccessor<Rgba32> croppedPixels = croppedImage.Lock())
                                {
                                    for (int i = 0; i < image.Width; i++)
                                    {
                                        for (int j = 0; j < image.Heigth; j++)
                                        {
                                            pixels[i + image.Left, j + image.Top] = croppedPixels[i, j];
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
    }

}