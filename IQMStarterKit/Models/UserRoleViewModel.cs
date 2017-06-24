using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models
{
    public class ExtendedUserCustom
    {

        [Display(Name = "Full Name")]
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Group Name")]
        public byte GroupId { get; set; }

        [Display(Name = "School Month")]
        public int MonthIntake { get; set; }

        [Display(Name = "School Year")]
        public int YearIntake { get; set; }

        [Required]
        public string Password { get; set; }

        public double ProgressValue { get; set; }

        public IEnumerable<UserRolesCustom> Roles { get; set; }

        public IEnumerable<GroupModel> Groups { get; set; }
    }

    public class UserRolesCustom
    {
        [Key]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class UserRoleCustom
    {
        [Key]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class RoleCustom
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class UserAndRolesCustom
    {
        [Key]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public List<UserRoleCustom> colUserRole { get; set; }
    }
}