using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    // Page17
    public class KiwianaClass
    {
        [Display(Name = "Paua Shell")]
        [DataType(DataType.MultilineText)]
        public string Paua { get; set; }

        [Display(Name = "Pavlova")]
        [DataType(DataType.MultilineText)]
        public string Pavlova { get; set; }

        [Display(Name = "Hokey Pokey Ice Cream")]
        [DataType(DataType.MultilineText)]
        public string HokeyPokey { get; set; }

        [Display(Name = "Fish & Chips")]
        [DataType(DataType.MultilineText)]
        public string FishNChips { get; set; }

        [DataType(DataType.MultilineText)]
        public string Jandals { get; set; }

        [DataType(DataType.MultilineText)]
        public string Swandri { get; set; }

        [DataType(DataType.MultilineText)]
        public string BuzzyBee { get; set; }

        [DataType(DataType.MultilineText)]
        public string BlackSinglet { get; set; }

        [DataType(DataType.MultilineText)]
        public string Rugby { get; set; }

        [DataType(DataType.MultilineText)]
        public string KiwiFruit { get; set; }

        [DataType(DataType.MultilineText)]
        public string PineappleLumps { get; set; }

        [DataType(DataType.MultilineText)]
        public string LnP { get; set; }



        public StudentActivity StudentActivity { get; set; }

        public KiwianaClass()
        {
            StudentActivity = new StudentActivity();
        }

    }
}