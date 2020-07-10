using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class AlbumBase
    {
        public int Id { get; set; }

        [Display(Name = "Album name")]
        public string Name { get; set; }

        [Display(Name = "Release date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ReleaseDate { get; set; }

        // Get from Apple iTunes Preview, Amazon, or Wikipedia
        [Display(Name = "Album cover art")]
        public string UrlAlbum { get; set; }

        [Display(Name = "Album's primary genre")]
        public string Genre { get; set; }
    }

    public class AlbumWithDetails : AlbumBase
    {

        public AlbumWithDetails()
        {

        }

        [Display(Name ="Album description")]
        public string Description { get; set; }

    }

    public class AlbumAddForm : AlbumAdd
    {
        public string ArtistName { get; set; }

        [Display(Name = "Available genres")]
        public SelectList GenreList { get; set; }



    }

    public class AlbumAdd
    {
        public AlbumAdd()
        {
            ReleaseDate = DateTime.Now;
        }
        [Required]
        public int GenreId { get; set; }

        [StringLength(50)]
        public string Coordinator { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Release date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [StringLength(200)]
        public string UrlAlbum { get; set; }
        public int ArtistId { get; set; }

        [DataType(DataType.MultilineText),StringLength(2000)]
        public string Description { get; set; }
    }
}