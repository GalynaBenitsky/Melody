using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    // Add your design model classes below

    // Follow these rules or conventions:

    // To ease other coding tasks, the name of the 
    //   integer identifier property should be "Id"
    // Collection properties (including navigation properties) 
    //   must be of type ICollection<T>
    // Valid data annotations are pretty much limited to [Required] and [StringLength(n)]
    // Required to-one navigation properties must include the [Required] attribute
    // Do NOT configure scalar properties (e.g. int, double) with the [Required] attribute
    // Initialize DateTime and collection properties in a default constructor

    public class Genre
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class RoleClaim
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class Artist
    {
        public Artist()
        {
            BirthName = "";
            BirthOrStartDate = DateTime.Now.AddYears(-20);
            Albums = new List<Album>();
            MediaItems = new List<MediaItem>();
        }

        public int Id { get; set; }

        // For an individual, can be birth name, or a stage/performer name
        // For a duo/band/group/orchestra, will typically be a stage/performer name
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Profile { get; set; }

        // For an individual, a birth name
        [StringLength(100)]
        public string BirthName { get; set; }

        // For an individual, a birth date
        // For all others, can be the date the artist started working together
        public DateTime BirthOrStartDate { get; set; }

        // Get from Apple iTunes Preview, Amazon, or Wikipedia
        [Required, StringLength(200)]
        public string UrlArtist { get; set; }

        [Required]
        public string Genre { get; set; }

        // User name who looks after this artist
        [Required, StringLength(200)]
        public string Executive { get; set; }

        public ICollection<MediaItem> MediaItems { get; set; } 

        public ICollection<Album> Albums { get; set; }
    }

    public class Album
    {
        public Album()
        {
            ReleaseDate = DateTime.Now;
            Artists = new List<Artist>();
            Tracks = new List<Track>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

        // Get from Apple iTunes Preview, Amazon, or Wikipedia
        [Required, StringLength(200)]
        public string UrlAlbum { get; set; }

        [Required]
        public string Genre { get; set; }

        // User name who looks after the album
        [Required, StringLength(200)]
        public string Coordinator { get; set; }

        public ICollection<Artist> Artists { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }

    public class Track
    {
        public Track()
        {
            Albums = new List<Album>();
        }

        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        // Simple comma-separated string of all the track's composers
        [Required, StringLength(500)]
        public string Composers { get; set; }

        [Required]
        public string Genre { get; set; }

        // User name who added/edited the track
        [Required, StringLength(200)]
        public string Clerk { get; set; }

        public byte[] Audio { get; set; }

        public string AudioContentType { get; set; }

        public ICollection<Album> Albums { get; set; }
    }

    public class MediaItem
    {
        public MediaItem()
        {
            TimeStamp = DateTime.Now;

            // StringId generator
            // Code is from Mads Kristensen
            // http://madskristensen.net/post/generate-unique-strings-and-numbers-in-c

            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            StringId = string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Caption { get; set; }

        public byte[] Content { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        [Required, StringLength(100)]
        public string StringId { get; set; }

        public DateTime TimeStamp { get; set; }

        [Required]
        public Artist Artist { get; set; }
    }

}
