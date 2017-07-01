using System.Collections.Generic;
using System.Web.Mvc;

namespace IQMStarterKit
{
    public class DDLHelper
    {

        public static IList<SelectListItem> GetVarkList()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "", Text = "" });
            _result.Add(new SelectListItem { Value = "Visual", Text = "Visual" });
            _result.Add(new SelectListItem { Value = "Auditory", Text = "Auditory" });
            _result.Add(new SelectListItem { Value = "Reading/Writing", Text = "Reading/Writing" });
            _result.Add(new SelectListItem { Value = "Kinesthetic", Text = "Kinesthetic" });
            return _result;
        }


        public static IList<SelectListItem> GetDopeList()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "", Text = "" });
            _result.Add(new SelectListItem { Value = "Dove", Text = "Dope" });
            _result.Add(new SelectListItem { Value = "Owl", Text = "Owl" });
            _result.Add(new SelectListItem { Value = "Peacock", Text = "Peacock" });
            _result.Add(new SelectListItem { Value = "Eagle", Text = "Eagle" });
            return _result;
        }

        public static string GetRating(int rating)
        {
            string retval = string.Empty;
            switch (rating)
            {
                case 1: retval = "Below Satisfactory"; break;
                case 2: retval = "Satisfactory"; break;
                case 3: retval = "Good"; break;
                case 4: retval = "Excellent"; break;
            }

            return retval;
        }


        public static string GetStarRating(int rating)
        {
            string retval = string.Empty;


            for (int i = 0; i < rating; i++)
            {
                retval += "<span class='glyphicon glyphicon-star - empty'></span>";
            }

            return retval;
        }



        public static string GetOtherRating(int rating)
        {
            string retval = string.Empty;
            switch (rating)
            {
                case 1: retval = "Too Short"; break;
                case 2: retval = "Just Right"; break;
                case 3: retval = "Too Long"; break;

            }

            return retval;
        }
    }
}