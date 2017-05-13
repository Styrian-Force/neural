

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
                    List<ImageTaskStatus> statuses = ReadLogWithoutLock(imageTask);
                    if (statuses == null)
                    {
                        statuses = new List<ImageTaskStatus>();
                    }
                    statuses.Add(status);
                    //string json = status.ToJson();
                    string logs = ImageTaskStatus.ToJson(statuses);
                    File.WriteAllText(taskStatusLogPath, logs);
                }
            );
        }

        private List<ImageTaskStatus> ReadLogWithoutLock(ImageTask imageTask)
        {
            string taskStatusLogPath = _fileService.GetTaskStatusLogPath(imageTask);
            string logs = File.ReadAllText(taskStatusLogPath);

            List<ImageTaskStatus> imageTasks = ImageTaskStatus.FromJson(logs);
            return imageTasks;
        }

        public List<ImageTaskStatus> ReadLog(ImageTask imageTask)
        {
            string taskStatusLogPath = _fileService.GetTaskStatusLogPath(imageTask);
            List<ImageTaskStatus> unreadTasks = null;

            Lock(
                taskStatusLogPath,
                () =>
                {
                    List<ImageTaskStatus> imageTasks = ReadLogWithoutLock(imageTask);
                    if (imageTasks == null) return;
                    unreadTasks = imageTasks.Where(x => x.StatusRead == false).ToList();
                    unreadTasks.ForEach(x => x.StatusRead = true);

                    string logs = ImageTaskStatus.ToJson(imageTasks);
                    File.WriteAllText(taskStatusLogPath, logs);
                }
            );

            return unreadTasks;
        }
    }
}