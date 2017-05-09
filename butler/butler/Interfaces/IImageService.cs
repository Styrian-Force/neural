

using System.Diagnostics;
using System.IO;
using Butler.Models;

namespace Butler.Interfaces
{
    public interface IImageService
    {
        void MergeImages(ImageTask imageTask);

    }
}