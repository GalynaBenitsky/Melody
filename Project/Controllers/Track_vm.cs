using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class TrackBase
    {
        [Key]
        [HiddenInput]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Track Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Track's composer(s)")]
        public string Composer { get; set; }
        [Required]
        [Display(Name = "Genre")]
        public string Genre { get; set; }


    }

    public class TrackWithDetails : TrackBase
    {

        public TrackWithDetails()
        {
            AlbumNames = new List<string>();
        }
        [Display(Name = "Albums associated with the track")]
        public IEnumerable<string> AlbumNames { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Clerk who helps with album tasks")]
        public string Clerk { get; set; }

        [Display(Name = "Track")]
        public string AudioUrl {
                get
                {
                    return $"/audio/{Id}";
                }
            }
    }

    public class TrackAudio
    {
        public string AudioContentType { get; set; }
        public byte[] Audio { get; set; }
        public int Id { get; set; }
    }

    public class TrackAddForm 
    {

        public string AlbumName { get; set; }
        public SelectList GenreList { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int GenreId { get; set; }

        [StringLength(50)]
        public string Clerk { get; set; }

        [Required]
        [HiddenInput]
        public int AlbumId { get; set; }

        [Required]
        [StringLength(100)]
        public string Composers { get; set; }

        [Required]
        [Display(Name = "Track")]
        [DataType(DataType.Upload)]
        public string AudioUpload { get; set; }
    }

    public class TrackAdd
    {
        public TrackAdd()
        {

        }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int GenreId { get; set; }

        [StringLength(50)]
        public string Clerk { get; set; }

        [Required]
        [HiddenInput]
        public int AlbumId { get; set; }

        [Required]
        [StringLength(100)]
        public string Composers { get; set; }

        [Required]
        public HttpPostedFileBase AudioUpload { get; set; }
    }

}