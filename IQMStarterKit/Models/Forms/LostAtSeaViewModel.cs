using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    public class LostAtSeaViewModel
    {
        public OwnLostAtSeaActivityClass OwnLostAtSea { get; set; }
        public GroupLostAtSeaActivityClass GroupLostAtSea { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public LostAtSeaViewModel()
        {
            StudentActivity = new StudentActivity();
        }

    }
}