﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Butler.Models
{
    public class ImageTask
    {
        public string JobId { get; set; }
        public List<Image> CroppedImages { get; set; }
        public string WorkingDir { get; set; }
        public string DetectorDir { get; set; }
        public string ArtistDir { get; set; }
        public string OriginalImagePath { get; set; }
        public Task task { get; set; }
    }
}
