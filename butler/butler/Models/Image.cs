using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace butler.Models
{
    public class Image
    {
        public int JobId { get; set; }
        public string WorkingDir { get; set; }
        public string SubDir { get; set; }
        public string InputFilePath { get; set; }
        public Task task {get; set;}
    }
}
