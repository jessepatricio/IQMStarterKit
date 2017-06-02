using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    // Page17
    public class AboutMeClass
    {
        public string Name { get; set; }
        public string Place { get; set; }

        [DataType(DataType.MultilineText)]
        public string Hobby { get; set; }
        public string Movie { get; set; }
        public string Important { get; set; }
        public string Model { get; set; }
        public string Happy { get; set; }
        public string Dislike { get; set; }
        public string Famous { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public AboutMeClass()
        {
            StudentActivity = new StudentActivity();
        }

    }
}