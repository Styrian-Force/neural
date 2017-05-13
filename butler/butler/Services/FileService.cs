
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;

using Butler.Config;
using Butler.Interfaces;
using Butler.Models;

namespace Butler.Services
{
    public class FileService : IFileService
    {
        private ILogger<FileService> _logger;

        public FileService(
            ILogger<FileService> logger
        )
        {
            Console.WriteLine("FILE_SERVICE: File Service constructor");
            this._logger = logger;
            CreateDir(ButlerConfig.DATABASE_DIR);
        }

        public string GetWorkingDir(ImageTask imageTask)
        {
            string workingDir = ButlerConfig.DATABASE_DIR + imageTask.JobId + "/";
            return workingDir;
        }

        public string GetDetectorDir(ImageTask imageTask)
        {
            string workingDir = GetWorkingDir(imageTask);
            string detectorDir = workingDir + ButlerConfig.DETECTOR_SUBDIR;
            return detectorDir;
        }

        public string GetArtistDir(ImageTask imageTask)
        {
            string workingDir = GetWorkingDir(imageTask);
            string artistDir = workingDir + ButlerConfig.ARTIST_SUBDIR;
            return artistDir;
        }

        public void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string GetOriginalImagePath(ImageTask imageTask)
        {
            string workingDir = GetWorkingDir(imageTask);
            string originalImagePath = workingDir + ButlerConfig.ORIGINAL_FILENAME + imageTask.OriginalExtension;
            return originalImagePath;
        }

        public string GetDetectorImagePath(ImageTask imageTask)
        {
            string workingDir = GetWorkingDir(imageTask);
            string detectorImagePath = workingDir + ButlerConfig.DETECTOR_OUTPUT;
            return detectorImagePath;
        }

        public string GetDetectorImagePathWithExt(ImageTask imageTask)
        {
            string workingDir = GetWorkingDir(imageTask);
            string detectorImagePath = workingDir + ButlerConfig.DETECTOR_OUTPUT_WITH_EXT;
            return detectorImagePath;
        }

        public string GetMergedImagePath(ImageTask imageTask)
        {
            string workingDir = GetWorkingDir(imageTask);
            string mergedImagePath = workingDir + ButlerConfig.MERGED_OUTPUT;
            return mergedImagePath;
        }

       public string GetTransparentImagePath(ImageTask imageTask) {
           string workingDir = GetWorkingDir(imageTask);
            string transparentImagePath = workingDir + ButlerConfig.TRANSPARENT_OUTPUT;
            return transparentImagePath;
       }

       public string GetTransparentImagePathWithExt(ImageTask imageTask)
        {
            string workingDir = GetWorkingDir(imageTask);
            string transparentImagePath = workingDir + ButlerConfig.TRANSPARENT_OUTPUT_WITH_EXT;
            return transparentImagePath;
        }
        
    }

}