using System;
using System.Web;

namespace ClientManager.Models
{
    public class DocumentData
    {
        public int Id { get; set; }
        public string DocumentSource { get; set; }
        public int ReferenceRecId { get; set; }
        public string DocumentType { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase PostedFile { get; set; }
        public string FileExtension { get; set; }
        public string FileData { get; set; }
        public string URL { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
