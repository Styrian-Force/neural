
namespace Butler.Models
{
    public class Device
    {
        public static Device CPU = new Device("/cpu:0");
        public static Device GPU = new Device("/gpu:0");

        public string Id;

        public Device(string id)
        {
            this.Id = id;
        }

    }
}