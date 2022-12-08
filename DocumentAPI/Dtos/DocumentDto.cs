using DocumentAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Dtos
{
    public class DocumentDto
    {
        public string Id { get; set; }
        public List<String>? Tags { get; set; }
        [Required]
        public string Data { get; set; }
    }
}
