using IQMStarterKit.Models.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IQMStarterKit.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //addtional field for user
        [StringLength(100)]
        [Required]
        public string FullName { get; set; }

        public byte GroupId { get; set; }

        [Required]
        [Display(Name = "School Month")]
        [DefaultValue(0)]
        public int MonthIntake { get; set; }

        [Required]
        [Display(Name = "School Year")]
        [DefaultValue(0)]
        public int YearIntake { get; set; }

        [DefaultValue(0.00)]
        public double OverallProgress { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public DbSet<TempWorkbook> TempWorkbooks { get; set; }
        public DbSet<TempModule> TempModules { get; set; }
        public DbSet<TempActivity> TempActivities { get; set; }


        public DbSet<StudentActivity> StudentActivities { get; set; }
        public DbSet<FilePath> FilePaths { get; set; }
        public DbSet<MatchLookupModels> MatchLookup { get; set; }

        public DbSet<GroupModel> GroupModels { get; set; }
        public DbSet<GroupTutorModel> GroupTutorModels { get; set; }

        public DbSet<GroupActivityConfig> GroupActivityConfig { get; set; }

        public DbSet<ProgramSurveyModel> ProgramSurveyModel { get; set; }
        public DbSet<TutorSurveyModel> TutorSurveyModel { get; set; }
        public DbSet<PresentationEvaluationModel> PresentationEvaluationModel { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<IQMStarterKit.Models.UserAndRolesCustom> UserAndRolesCustoms { get; set; }
    }
}