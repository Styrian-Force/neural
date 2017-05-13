

using System.Diagnostics;
using System.IO;
using Butler.Models;

namespace Butler.Interfaces
{
    public interface IArtistService
    {
        void AddToQueue(ImageTask imageTask);

    }
}