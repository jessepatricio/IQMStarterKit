using System.Web;

namespace IQMStarterKit.Models
{
    public class UploadFileModel
    {
        [FileSize(10240)]
        [FileTypes("jpg,jpeg,png")]
        public HttpPostedFileBase File { get; set; }
    }
}