using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    public class FirstThingFirst
    {
        public string Activities { get; set; }

        public string Matrix1 { get; set; }
        public string Matrix2 { get; set; }
        public string Matrix3 { get; set; }
        public string Matrix4 { get; set; }

        public string LargeTask { get; set; }

        public string SmallTask1 { get; set; }
        public string SmallTask2 { get; set; }
        public string SmallTask3 { get; set; }
        public string SmallTask4 { get; set; }
        public string SmallTask5 { get; set; }
        public string SmallTask6 { get; set; }
        public string SmallTask7 { get; set; }
        public string SmallTask8 { get; set; }
        public string SmallTask9 { get; set; }
        public string SmallTask10 { get; set; }



        public StudentActivity StudentActivity { get; set; }

        public FirstThingFirst()
        {
            StudentActivity = new StudentActivity();
        }
    }
}