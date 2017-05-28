

using System.Diagnostics;
using System.IO;
using Butler.Models;

namespace Butler.Interfaces
{
    public interface IDetectorService : Queueable<ImageTask>  
    {
        // add more if needed

    }
}