using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page36
    public class WinningLotteryClass
    {
        public string WinLot1 { get; set; }
        public string WinLot2 { get; set; }
        public string WinLot3 { get; set; }
        public string WinLot4 { get; set; }
        public string WinLot5 { get; set; }
        public string WinLot6 { get; set; }
        public string WinLot7 { get; set; }
        public string WinLot8 { get; set; }
        public string WinLot9 { get; set; }
        public string WinLot10 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public WinningLotteryClass()
        {
            StudentActivity = new StudentActivity();
        }
    }
}