using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Butler.Models
{
    public class ImageTask
    {
        public string JobId { get; set; }
        public List<Image> CroppedImages { get; set; }
        public string OriginalExtension { get; set; }
        public ImageTaskStatusCode Status;

        public string ToJson() {
            String json = JsonConvert.SerializeObject(this, Formatting.Indented);
            return json;
        }
    }
}
