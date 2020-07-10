using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class ArtistBase
    {
        public int Id { get; set; }

        [Display(Name ="Artist name")]
        public string Name { get; set; }

        [Display(Name ="Birth name")]
        public string BirthName { get; set; }

        [Display(Name = "Birth date")]
        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime BirthOrStartDate { get; set; }

        [Display(Name = "Artist photo")]
        public string UrlArtist { get; set; }

        [Display(Name = "Genre")]
        public string Genre { get; set; }
    }

    public class ArtistWithDetails : ArtistBase
    {

        [StringLength(50)]
        [Display(Name = "Executive")]
        public string Executive { get; set; }

        [Display(Name ="Artist profile")]
        public string Profile { get; set; }

        public IEnumerable<AlbumBase> Albums { get; set; }
    }

    public class ArtistWithMediaInfo : ArtistBase
    {
        [StringLength(50)]
        [Display(Name = "Executive")]
        public string Executive { get; set; }

        [Display(Name = "Artist profile")]
        public string Profile { get; set; }

        public IEnumerable<MediaItemBase> MediaItems { get; set; }

        public IEnumerable<AlbumBase> Albums { get; set; }
    }

    public class ArtistAddForm : ArtistAdd
    {
        [Required]
        [Display(Name = "Genre")]
        public SelectList GenreList { get; set; }

    }

    public class ArtistAdd
    {
        public ArtistAdd()
        {
            BirthOrStartDate = DateTime.Now;
        }
        [Required]
        [StringLength(50)]
        [Display(Name = "Artist name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Date of birth")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BirthOrStartDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Executive")]
       public string Executive { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Photo link")]
        public string UrlArtist { get; set; }

       [Display(Name = "Given Name")]
        public string BirthName { get; set; }

        [StringLength(2000), DataType(DataType.MultilineText), Display()]
       public string Profile { get; set; }

    }
}