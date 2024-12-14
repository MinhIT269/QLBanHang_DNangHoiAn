using PBL6_QLBH.Models;
using System.ComponentModel.DataAnnotations;

namespace PBL6.Dto
{
    public class VideoDto
    {
        public Guid VideoId { get; set; }

   
        public string? VideoUrl { get; set; }
        public string? Description { get; set; }

      
    }
}
