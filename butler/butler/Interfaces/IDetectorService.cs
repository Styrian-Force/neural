

using System.Diagnostics;
using System.IO;
using Butler.Models;

namespace Butler.Interfaces
{
    public interface IDetectorService
    {
        void AddToQueue(ImageTask imageTask);

    }
}