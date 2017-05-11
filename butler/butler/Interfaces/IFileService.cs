

using System.Diagnostics;
using System.IO;
using Butler.Models;
using Microsoft.AspNetCore.Http;

namespace Butler.Interfaces
{
    public interface IFileService
    {
        string GetWorkingDir(ImageTask imageTask);
        string GetDetectorDir(ImageTask imageTask);
        string GetArtistDir(ImageTask imageTask);
        string GetOriginalImagePath(ImageTask imageTask);
        string GetDetectorImagePath(ImageTask imageTask);
        string GetDetectorImagePathWithExt(ImageTask imageTask);
        string GetMergedImagePath(ImageTask imageTask);
        void CreateDir(string path);
    }
}