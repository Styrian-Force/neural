using System.Collections.Generic;
using Newtonsoft.Json;

namespace Butler.Models
{
    public class ImageTaskStatus
    {
        public ImageTaskStatusCode StatusCode {get;set;}
        public string Message {get;set;}
        public bool StatusRead {get; set;}

        public ImageTaskStatus(ImageTaskStatusCode statusCode, string message) {
            this.StatusCode = statusCode;
            this.Message = message;
            this.StatusRead = false;
        }

        public static ImageTaskStatus ImageUploaded() {
            return new ImageTaskStatus(
                ImageTaskStatusCode.ImageUploaded, 
                "Image successfully uploaded. "
            );
        }
        public static ImageTaskStatus ImageInDetectorQueue() {
            return new ImageTaskStatus(
                ImageTaskStatusCode.ImageInDetectorQueue, 
                "Image is in Detector queue. "
            );
        }
        public static ImageTaskStatus ImageInDetector() {
            return new ImageTaskStatus(
                ImageTaskStatusCode.ImageInDetector, 
                "Image is in Detector. "
            );
        }

        public static ImageTaskStatus DetectorFoundSomethingf(string message) {
            return new ImageTaskStatus(
                ImageTaskStatusCode.ImageUploaded, 
                "Detector found: "+ message
            );
        }

        public static string ToJson(List<ImageTaskStatus> statuses) {
            return JsonConvert.SerializeObject(statuses);
        }

        public string ToJson() {
            return JsonConvert.SerializeObject(this);
        }

        public static List<ImageTaskStatus> FromJson(string json) {
            return JsonConvert.DeserializeObject<List<ImageTaskStatus>>(json);
        }
    }
}
