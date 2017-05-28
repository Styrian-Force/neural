
namespace Butler.Models
{
    public class Device
    {
        public static readonly Device CPU = new Device("/cpu:0");
        public static readonly Device GPU = new Device("/gpu:0");

        public readonly string Id;

        private Device(string id)
        {
            this.Id = id;
        }

    }
}