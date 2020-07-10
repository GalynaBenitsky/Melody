using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class ArtistsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private Manager m = new Manager();
        // GET: Artists
        public ActionResult Index()
        {
            return View(m.ArtistGetAll());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            var a = m.ArtistGetByIdWithMedia(id.GetValueOrDefault());

            if (a == null)
            {
                return HttpNotFound();
            }
            return View(a);
        }

        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            ArtistAddForm a = new ArtistAddForm();

            a.GenreList = new SelectList(m.GenreGetAll(), "Id", "Name");

            return View(a);
        }

        // POST: Artists/Create
        [HttpPost]
        [Authorize(Roles = "Executive"),ValidateInput(false)]
        public ActionResult Create(ArtistAdd newItem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("create");
                }

                var addedItem = m.ArtistAdd(newItem);

                if (addedItem == null)
                {
                    return View(newItem);
                }

                return RedirectToAction("Details", new { id = addedItem.Id });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //GET: artists/5/addalbum
        [Authorize(Roles = "Coordinator")]
        [Route("Artists/{id}/addalbum")]
        public ActionResult AddAlbum(int? id)
        {
            AlbumAddForm a = new AlbumAddForm();

            
            var o = m.ArtistGetById(id.GetValueOrDefault());
            a.ArtistName = o.Name;

           
            a.GenreList = new SelectList(m.GenreGetAll(), "Id", "Name");


            return View(a);
        }

        // POST
        [Authorize(Roles = "Coordinator"), ValidateInput(false)]
        [Route("artists/{id}/addalbum")]
        [HttpPost]
        public ActionResult AddAlbum(AlbumAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.AlbumAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                // Attention - Must redirect to the Albums controller
                return RedirectToAction("details", "Albums", new { id = addedItem.Id });
            }
        }

        //GET: artists/5/addmediaitem
        [Authorize(Roles = "Coordinator")]
        [Route("Artists/{id}/addMediaItem")]
        public ActionResult addMediaItem(int? id)
        {
            var artist = m.ArtistGetById(id.GetValueOrDefault());

            if(artist == null)
            {
                return HttpNotFound();
            }

            MediaItemAddForm media = new MediaItemAddForm();

            media.ArtistId = artist.Id;
            media.ArtistName = artist.Name;

            return View(media);
        }

        // GET: artists/5/addmediaitem
        [Authorize(Roles = "Coordinator")]
        [Route("Artists/{id}/addMediaItem")]
        [HttpPost]
        public ActionResult addMediaItem(MediaItemAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            var addedItem = m.MediaItemAdd(newItem);

            if(addedItem == null)
            {
                return View(newItem);
            }
            

            return RedirectToAction("details",new { id = newItem.ArtistId});
        }


        [HttpGet]// GET: Artists/Edit/5
        public ActionResult Edit(int id)
        {
            
            //Artist artist= context.Artists.Single(art => art.Id == id);
            Artist artist = db.Artists.Find(id);


            return View(artist);
        }

        // POST: Artists/Edit/5
        [HttpPost]
        public ActionResult Edit(Artist artist)
        {
            
            db.Entry(artist).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            
            return RedirectToAction("Index");
            
                
            
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int id)
        {
            
            Artist artist= db.Artists.Find(id);

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Artist artist = db.Artists.Single(art => art.Id == id);
            db.Artists.Remove(artist);
            db.SaveChanges();

            return RedirectToAction("Index");
           
        }
    }
}
