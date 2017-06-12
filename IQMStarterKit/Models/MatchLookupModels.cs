using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models
{
    public class MatchLookupModels
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte MatchId { get; set; }

        [Required]
        public int QuestionNumber { get; set; }

        [StringLength(100)]
        public string CorrectAnswer { get; set; }

    }
}