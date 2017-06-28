using IQMStarterKit.Models.Forms;

namespace IQMStarterKit.Models
{
    public class FirstThingFirstViewModel
    {
        public FilePath FilePath { get; set; }
        public FirstThingFirst FirstThingFirst { get; set; }

        public FirstThingFirstViewModel()
        {
            FilePath = new FilePath();
            FirstThingFirst = new FirstThingFirst();
        }
    }
}