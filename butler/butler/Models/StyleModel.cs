
namespace Butler.Models
{
    public class StyleModel
    {
        public static readonly StyleModel UDNIE = new StyleModel("udnie.ckpt");

        public readonly string Name;

        private StyleModel(string name)
        {
            this.Name = name;
        }

    }
}