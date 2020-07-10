using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using Project.Models;
using System.Security.Claims;

namespace Project.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // Declare a property to hold the user account for the current request
        // Can use this property here in the Manager class to control logic and flow
        // Can also use this property in a controller 
        // Can also use this property in a view; for best results, 
        // near the top of the view, add this statement:
        // var userAccount = new ConditionalMenu.Controllers.UserAccount(User as System.Security.Claims.ClaimsPrincipal);
        // Then, you can use "userAccount" anywhere in the view to render content
        public UserAccount UserAccount { get; private set; }

        public Manager()
        {
            // If necessary, add constructor code here

            // Initialize the UserAccount property
            UserAccount = new UserAccount(HttpContext.Current.User as ClaimsPrincipal);

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }


        public IEnumerable<string> RoleClaimGetAll()
        {
            return ds.RoleClaims.Select(l=> l.Name);
        }


        // artists
        public IEnumerable<ArtistBase> ArtistGetAll()
        {
            return Mapper.Map<IEnumerable<ArtistBase>>(ds.Artists);
        }

        public ArtistWithDetails ArtistAdd(ArtistAdd newItem)
        {
            var addedItem = ds.Artists.Add(Mapper.Map<Artist>(newItem));

            addedItem.Executive = HttpContext.Current.User.Identity.Name;
            addedItem.Genre = GenreGetById(newItem.GenreId).Name;

            ds.SaveChanges();

            return (addedItem == null) ? null : Mapper.Map<ArtistWithDetails>(addedItem);
        }

        public ArtistWithDetails ArtistGetById(int id)
        {
            var r = ds.Artists.Where(l => l.Id == id).SingleOrDefault();

            return (r == null) ? null : Mapper.Map<ArtistWithDetails>(r);
        }

        public ArtistWithMediaInfo ArtistGetByIdWithMedia(int id)
        {
            var r = ds.Artists.Include("Albums").Include("MediaItems").Where(l => l.Id == id).SingleOrDefault();

            return (r == null) ? null : Mapper.Map<ArtistWithMediaInfo>(r);
        }
                       
        // albums
        public IEnumerable<AlbumBase> AlbumGetAll()
        {
            return Mapper.Map<IEnumerable<AlbumBase>>(ds.Albums);
        }

        public AlbumWithDetails AlbumGetById(int id)
        {
            var r = ds.Albums.Include("Artists").Include("Tracks").Where(l => l.Id == id).SingleOrDefault();

            if (r == null)
            {
                return null;
            }


            var result = Mapper.Map<AlbumWithDetails>(r);
            

            return result;
        }

        public AlbumWithDetails AlbumAdd(AlbumAdd newItem)
        {
            var addedItem = ds.Albums.Add(Mapper.Map<Album>(newItem));

            // For each one, add to the fetched object's collection
            addedItem.Artists.Add(ds.Artists.Find(newItem.ArtistId));

            addedItem.Genre = GenreGetById(newItem.GenreId).Name;

            addedItem.Coordinator = HttpContext.Current.User.Identity.Name;
            // Save changes
            ds.SaveChanges();

            return (addedItem == null) ? null : Mapper.Map<AlbumWithDetails>(addedItem);
        }

        // tracks
        public TrackWithDetails TrackAdd(TrackAdd newItem)
        {

            var a = ds.Albums.SingleOrDefault(l => newItem.AlbumId == l.Id);
            if (a == null)
            {
                return null;
            }
            
            var addedItem = ds.Tracks.Add(Mapper.Map<Track>(newItem));

            addedItem.Albums.Add(a);
            addedItem.Genre = ds.Genres.SingleOrDefault(l => l.Id == newItem.GenreId).Name;

            addedItem.Clerk = HttpContext.Current.User.Identity.Name;

            
            a.Tracks.Add(addedItem);

            addedItem.Albums.Add(a);

            //configure audio content
            var AudioBytes = new byte[newItem.AudioUpload.ContentLength];
            newItem.AudioUpload.InputStream.Read(AudioBytes,0,newItem.AudioUpload.ContentLength);
            addedItem.AudioContentType = newItem.AudioUpload.ContentType;
            addedItem.Audio = AudioBytes;

            // Save changes
            ds.SaveChanges();
            return (addedItem == null) ? null : Mapper.Map<TrackWithDetails>(addedItem);
        }

        public IEnumerable<TrackBase> TrackGetAll()
        {
            return Mapper.Map<IEnumerable<TrackBase>>(ds.Tracks);
        }

        public TrackAudio TrackGetByIdAudio(int id)
        {
            var o = ds.Tracks.Find(id);

            return (o == null) ? null : Mapper.Map<TrackAudio>(o);
        }

        public TrackWithDetails TrackGetById(int id)
        {
            var r = ds.Tracks.Include("Albums").Where(l => l.Id == id).SingleOrDefault();

            if (r == null)
            {
                return null;
            }

            var result = Mapper.Map<TrackWithDetails>(r);

            result.AlbumNames = r.Albums.Select(l => l.Name);

            return result;
        }


        // MediaType
        public MediaItemContent MediaItemGetByIdContent (string id)
        {
            var o = ds.MediaItems.SingleOrDefault(l => l.StringId == id);

            return (o == null) ? null : Mapper.Map<MediaItemContent>(o);
        }

        public MediaItemBase MediaItemAdd(MediaItemAdd newItem)
        {
            var artist = ds.Artists.Find(newItem.ArtistId);

            if(artist == null)
            {
                return null;
            }

            var addedItem = ds.MediaItems.Add(Mapper.Map<MediaItem>(newItem));

            if(addedItem == null)
            {
                return null;
            }

            //create association between the artist and the new item
            addedItem.Artist = artist;

            var bytes = new byte[newItem.ContentUpload.ContentLength];

            addedItem.ContentType = newItem.ContentUpload.ContentType;
            
            //copy submited bytes to the temp bytes
            newItem.ContentUpload.InputStream.Read(bytes,0,newItem.ContentUpload.ContentLength);

            //add the bytes to the data store
            addedItem.Content = bytes;

            //associate new item with the artist
            artist.MediaItems.Add(addedItem);

            ds.SaveChanges();

            return Mapper.Map<MediaItemBase>(addedItem);
        }

        // Genre
        public IEnumerable<GenreBase> GenreGetAll()
        {
            return Mapper.Map<IEnumerable<GenreBase>>(ds.Genres.OrderBy(l => l.Id));
        }

        public GenreBase GenreGetById(int id)
        {
            return Mapper.Map<GenreBase>(ds.Genres.SingleOrDefault(l => l.Id == id));
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool LoadData()
        {
            
            var user = HttpContext.Current.User.Identity.Name;
            bool isValid = false;

           
            // Genre
            if(ds.RoleClaims.Count() == 0)
            {
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive"});
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Admin" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Guest" });

            }

            if (ds.Genres.Count() == 0)
            {
                ds.Genres.Add(new Genre { Name = "Alternative" });
                ds.Genres.Add(new Genre { Name = "Classical" });
                ds.Genres.Add(new Genre { Name = "Country" });
                ds.Genres.Add(new Genre { Name = "Easy Listening" });
                ds.Genres.Add(new Genre { Name = "Hip-Hop/Rap" });
                ds.Genres.Add(new Genre { Name = "Jazz" });
                ds.Genres.Add(new Genre { Name = "Pop" });
                ds.Genres.Add(new Genre { Name = "R&B" });
                ds.Genres.Add(new Genre { Name = "Rock" });
                ds.Genres.Add(new Genre { Name = "Soundtrack" });

                ds.SaveChanges();
                isValid = true;
            }

            if (ds.Artists.Count() == 0)
            {
                ds.Artists.Add(new Artist
                {
                    Name = "The Beatles",
                    BirthOrStartDate = new DateTime(1962, 8, 15),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/9/9f/Beatles_ad_1965_just_the_beatles_crop.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Adele",
                    BirthName = "Adele Adkins",
                    BirthOrStartDate = new DateTime(1988, 5, 5),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "http://www.billboard.com/files/styles/article_main_image/public/media/Adele-2015-close-up-XL_Columbia-billboard-650.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Bryan Adams",
                    BirthOrStartDate = new DateTime(1959, 11, 5),
                    Executive = user,
                    Genre = "Rock",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Bryan_Adams_Hamburg_MG_0631_flickr.jpg"
                });

                ds.SaveChanges();
                isValid = true;
            }

            
            // Album
            if (ds.Albums.Count() == 0)
            {

                var bryan = ds.Artists.SingleOrDefault(a => a.Name == "Bryan Adams");

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "Reckless",
                    ReleaseDate = new DateTime(1984, 11, 5),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/5/56/Bryan_Adams_-_Reckless.jpg"
                });

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "So Far So Good",
                    ReleaseDate = new DateTime(1993, 11, 2),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/pt/a/ab/So_Far_so_Good_capa.jpg"
                });

                ds.SaveChanges();
                isValid = true;
            }

            // Track
            if (ds.Tracks.Count() == 0)
            {
                var reck = ds.Albums.SingleOrDefault(a => a.Name == "Reckless");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Run To You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Heaven",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Somebody",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Summer of '69",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Kids Wanna Rock",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                var so = ds.Albums.SingleOrDefault(a => a.Name == "So Far So Good");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Straight from the Heart",
                    Composers = "Bryan Adams, Eric Kagna",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "It's Only Love",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "This Time",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "(Everything I Do) I Do It for You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Heat of the Night",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.SaveChanges();
                isValid = true;
            }

            return isValid;
        }

    }

    // New "UserAccount" class for the authenticated user
    // Includes many convenient members to make it easier to render user account info
    // Study the properties and methods, and think about how you could use it
    public class UserAccount
    {
        // Constructor, pass in the security principal
        public UserAccount(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        // Add other role-checking properties here as needed
        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

}