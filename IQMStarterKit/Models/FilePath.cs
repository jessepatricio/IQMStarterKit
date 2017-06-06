using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models
{

    public enum FileType
    {
        Photo

    }


    public class FilePath
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte FileId { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        public FileType FileType { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public byte StudentActivityId { get; set; }


    }
}