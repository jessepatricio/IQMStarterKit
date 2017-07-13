using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    public class VarkView
    {
        public StudentActivity StudentActivity { get; set; }

        public VarkView()
        {
            StudentActivity = new StudentActivity();

        }
    }
}