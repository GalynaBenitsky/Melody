using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;

namespace Project
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            // Add map creation statements here
            // Mapper.CreateMap< FROM , TO >();

            // Disable AutoMapper v4.2.x warnings
            #pragma warning disable CS0618

            

            Mapper.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();

            // Album
            Mapper.CreateMap<Models.Album, Controllers.AlbumBase>();
            Mapper.CreateMap<Controllers.AlbumAdd, Models.Album>();
            Mapper.CreateMap<Models.Album, Controllers.AlbumWithDetails>();
           
            //Artist
            Mapper.CreateMap<Models.Artist, Controllers.ArtistBase>();
            Mapper.CreateMap<Controllers.ArtistAdd, Models.Artist>();
            Mapper.CreateMap<Models.Artist,Controllers.ArtistWithDetails>();
            Mapper.CreateMap<Models.Artist,Controllers.ArtistWithMediaInfo>();

   
            //Tracks
            Mapper.CreateMap<Models.Track, Controllers.TrackBase>();
            Mapper.CreateMap<Controllers.TrackAdd, Models.Track>();
            Mapper.CreateMap<Models.Track, Controllers.TrackWithDetails>();
            Mapper.CreateMap<Models.Track,Controllers.TrackAudio>();

            //Genre
            Mapper.CreateMap<Models.Genre, Controllers.GenreBase>();

            //MediaItem
            Mapper.CreateMap<Models.MediaItem, Controllers.MediaItemBase>();
            Mapper.CreateMap<Controllers.MediaItemAdd, Models.MediaItem>();
            Mapper.CreateMap<Models.MediaItem, Controllers.MediaItemContent>();



#pragma warning restore CS0618
        }
    }
}