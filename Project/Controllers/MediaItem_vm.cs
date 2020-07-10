using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Controllers
{
    public class MediaItemAdd
    {
        [Required, StringLength(200)]
        public string Caption { get; set; }

        [Required]
        public HttpPostedFileBase ContentUpload { get; set; }

        public int ArtistId { get; set; }

    }

    public class MediaItemAddForm 
    {
        public string ArtistName { get; set; }
        
        public int ArtistId { get; set; }

        [Display(Name = "Media Item")]
        [DataType(DataType.Upload)]
        public string ContentUpload { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Descriptive caption")]
        public string Caption { get; set; }

    }

    public class MediaItemBase 
    {

        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Caption { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        [Required, StringLength(100)]
        public string StringId { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    public class MediaItemContent 
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }

        public string ContentType { get; set; }
    }
}