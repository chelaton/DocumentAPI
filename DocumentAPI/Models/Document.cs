using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        [Required]
        public string ExternalDocumentId { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
        [Required]
        public string Data { get; set; }
    }
}
