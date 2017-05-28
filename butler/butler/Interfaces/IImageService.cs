

using System.Diagnostics;
using System.IO;
using Butler.Models;
using Microsoft.AspNetCore.Http;

namespace Butler.Interfaces
{
    public interface IImageService : Queueable<ImageTask>  
    {
        
        bool FileTypeSupported(IFormFile file);

    }
}