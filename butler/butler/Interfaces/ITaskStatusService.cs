

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Butler.Models;
using Microsoft.AspNetCore.Http;

namespace Butler.Interfaces
{
    public interface IImageTaskStatusService
    {
        void RewriteLog(ImageTask imageTask, List<ImageTaskStatus> statuses);
        void AddToLog(ImageTask imageTask, ImageTaskStatus status);
        List<ImageTaskStatus> ReadLog(ImageTask imageTask);

        void SerializeImageTask(ImageTask imageTask);
        ImageTask DeserializeImageTask(string id);
    }
}