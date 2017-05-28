

using System.Diagnostics;
using System.IO;
using Butler.Models;

namespace Butler.Interfaces
{
    public interface IArtistService : Queueable<ImageTask>  
    {
        // add more if needed
    }
}