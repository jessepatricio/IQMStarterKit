using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IQMStarterKit.Models
{
    public class ExtendedUserCustom
    {
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Email  { get; set; }

        public string Password { get; set; }

        [Display(Name = "Lockout End Date")]
        public DateTime? LockoutEndDateUtc { get; set; }

        public int AccessFailedCount { get; set; }

        public string PhoneNumber { get; set; }
        
        public IEnumerable<UserRolesCustom> Roles { get; set; }
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
        [Display(Name = "User Name")]
        public string UserName { get; set; }

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
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public List<UserRoleCustom> colUserRole { get; set; }
    } 
}