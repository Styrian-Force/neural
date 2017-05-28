

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Butler.Interfaces;
using Butler.Models;
using Newtonsoft.Json;

namespace Butler.Services
{
    public class ImageTaskStatusService : IImageTaskStatusService
    {
        private ILogger<ImageTaskStatusService> _logger;
        private readonly IFileService _fileService;

        private static readonly Object LOCK = new Object();

        public ImageTaskStatusService(
            ILogger<ImageTaskStatusService> logger,
            IFileService fileService
        )
        {
            this._logger = logger;
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
            this.SerializeImageTask(imageTask);
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
            this.SerializeImageTask(imageTask);
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

        public void SerializeImageTask(ImageTask imageTask)
        {
            lock (LOCK)
            {
                String json = imageTask.ToJson();
                String filePath = this._fileService.GetImageTaskPath(imageTask);

                try
                {
                    using (FileStream file = File.Open(filePath, FileMode.Create))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(file))
                        {
                            streamWriter.WriteLine(json);
                            streamWriter.Dispose();
                            this._logger.LogDebug(json);
                        }
                    }
                }
                catch (IOException e)
                {
                    //this._logger.LogError("Erro when serializing ImageTask: " + e.StackTrace);
                    throw e;
                }
            }
        }

        public ImageTask DeserializeImageTask(string id)
        {
            lock (LOCK)
            {
                ImageTask temp = new ImageTask();
                temp.JobId = id;
                String filePath = this._fileService.GetImageTaskPath(temp);
                if (!File.Exists(filePath))
                {
                    return null;
                }

                try
                {
                    using (StreamReader reader = File.OpenText(filePath))
                    {
                        string json = reader.ReadToEnd();
                        ImageTask imageTask = JsonConvert.DeserializeObject<ImageTask>(json);
                        //this._logger.LogDebug(json);
                        return imageTask;
                    }
                }
                catch (IOException e)
                {
                    this._logger.LogError("Erro when deserializing ImageTask: " + e.StackTrace);
                    throw e;
                }

            }
        }
    }
}