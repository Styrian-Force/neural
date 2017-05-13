

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Butler.Interfaces;
using Butler.Models;

namespace Butler.Services
{
    public class ImageTaskStatusService : IImageTaskStatusService
    {
        private readonly IFileService _fileService;

        public ImageTaskStatusService(
            IFileService fileService
        )
        {
            this._fileService = fileService;
        }

        public static void Lock(string path, Action action)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            while (true)
            {
                try
                {
                    using (FileStream file = File.Open(
                                                path,
                                                FileMode.OpenOrCreate,
                                                FileAccess.ReadWrite,
                                                FileShare.Write))
                    {
                        action();
                        break;
                    }
                }
                catch (IOException)
                {
                    var fileSystemWatcher =
                        new FileSystemWatcher(Path.GetDirectoryName(path))
                        {
                            EnableRaisingEvents = true
                        };

                    fileSystemWatcher.Changed +=
                        (o, e) =>
                            {
                                if (Path.GetFullPath(e.FullPath) == Path.GetFullPath(path))
                                {
                                    autoResetEvent.Set();
                                }
                            };

                    autoResetEvent.WaitOne();
                }
            }
        }

        public void RewriteLog(ImageTask imageTask, List<ImageTaskStatus> statuses)
        {
            string taskStatusLogPath = _fileService.GetTaskStatusLogPath(imageTask);
            Lock(
                taskStatusLogPath,
                () =>
                {
                    string logs = ImageTaskStatus.ToJson(statuses);
                    File.WriteAllText(taskStatusLogPath, logs);
                }
            );
        }

        public void AddToLog(ImageTask imageTask, ImageTaskStatus status)
        {
            string taskStatusLogPath = _fileService.GetTaskStatusLogPath(imageTask);

            Lock(
                taskStatusLogPath,
                () =>
                {
                    string json = status.ToJson();
                    File.AppendAllText(taskStatusLogPath, json);
                }
            );
        }

        public List<ImageTaskStatus> ReadLog(ImageTask imageTask)
        {
            string taskStatusLogPath = _fileService.GetTaskStatusLogPath(imageTask);
            List<ImageTaskStatus> unreadTasks = null;

            Lock(
                taskStatusLogPath,
                () =>
                {
                    string logs = File.ReadAllText(taskStatusLogPath);

                    List<ImageTaskStatus> imageTasks = ImageTaskStatus.FromJson(logs);

                    unreadTasks = (List<ImageTaskStatus>)imageTasks.Where(x => x.StatusRead == false);
                    unreadTasks.ForEach(x => x.StatusRead = true);

                    logs = ImageTaskStatus.ToJson(imageTasks);
                    File.WriteAllText(taskStatusLogPath, logs);
                }
            );

            return unreadTasks;
        }
    }
}