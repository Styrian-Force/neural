

using System.Diagnostics;
using System.IO;
using Butler.Models;
using Microsoft.AspNetCore.Http;

namespace Butler.Interfaces
{
    public interface IImageService
    {
        void MergeImages(ImageTask imageTask);

        bool FileTypeSupported(IFormFile file);

    }
}