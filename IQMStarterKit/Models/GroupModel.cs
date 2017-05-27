using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models
{
    public interface IGroupModel
    {
        byte GroupId { get; set; }

        string GroupName { get; set; }

        string TutorId { get; set; }

        string Description { get; set; }

        IEnumerable<ApplicationUser> Tutors { get; set; }

    }

    public class GroupModel : IGroupModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte GroupId { get; set; }

        [Required]
        [StringLength(255)]
        public string GroupName { get; set; }
     
        [StringLength(100)]
        [Display(Name = "Assigned Tutor")]
        public string TutorId { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public IEnumerable<ApplicationUser> Tutors { get; set; }

        //system fields
        [Required]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime ModifiedDateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        [DefaultValue(0)]
        public bool IsRemoved { get; set; }
    }
}