using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    public class Disc
    {
        public string DISC1 { get; set; }
        public string DISC2 { get; set; }
        public string DISC3 { get; set; }
        public string DISC4 { get; set; }
        public string DISC5 { get; set; }
        public string DISC6 { get; set; }
        public string DISC7 { get; set; }
        public string DISC8 { get; set; }
        public string DISC9 { get; set; }
        public string DISC10 { get; set; }
        public string DISC11 { get; set; }
        public string DISC12 { get; set; }
        public string DISC13 { get; set; }
        public string DISC14 { get; set; }
        public string DISC15 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public Disc()
        {
            StudentActivity = new StudentActivity();

        }
    }
}