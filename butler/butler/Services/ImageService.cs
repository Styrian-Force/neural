
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
    public class ImageService : IImageService
    {
        ILogger<ImageService> _logger;

        public ImageService(
            ILogger<ImageService> logger
        )
        {
            Console.WriteLine("IMAGE_SERVICE: Image Service constructor");

            this._logger = logger;
        }

        public void MergeImages(ImageTask imageTask)
        {
            Console.WriteLine("MergeImages function");
        }
    }

}