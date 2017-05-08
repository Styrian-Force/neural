

using System.Diagnostics;
using System.IO;
using butler.Models;

namespace butler.Interfaces
{
    public interface IDetectorService
    {
        void AddToQueue(Image image);

    }
}