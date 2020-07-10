using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class AlbumsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private Manager m = new Manager();
        // GET: Albums
        public ActionResult Index(string SearchBy, string search)
        {
   //         ApplicationDbContext db = new ApplicationDbContext();
   //         if (SearchBy == "Name")
    //        {
               
    //            return View(db.Albums.Where(a => a.Name.Contains(search)||search==null).ToList());
    //        }
   //         else 
   //         {
    //            return View(db.Albums.Where(a => a.Genre.Contains(search) || search == null).ToList());
    //        }
            return View(m.AlbumGetAll());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.AlbumGetById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            return View(o);
        }


        
        //GET: artists/5/addalbum
        [Authorize(Roles = "Coordinator")]
        [Route("Albums/{id}/addtrack")]
        public ActionResult AddTrack(int? id)
        {
            TrackAddForm a = new TrackAddForm();

            //get the accosiated artist
            var o = m.AlbumGetById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }

            var selectedAlbums = new List<int>();
            selectedAlbums.Add(o.Id);

            //configure album's name
            a.AlbumName = o.Name;
            a.AlbumId = o.Id;

            //configure select lists
            a.GenreList = new SelectList(m.GenreGetAll(), "Id", "Name");

            return View(a);
        }

        //POST
        [Authorize(Roles = "Coordinator")]
        [Route("albums/{id}/addtrack")]
        [HttpPost]
        public ActionResult AddTrack(TrackAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.TrackAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                // Attention - Must redirect to the Albums controller
                return RedirectToAction("details", "Tracks", new { id = addedItem.Id });
            }
        }


        // GET: Albums/Edit/5
        public ActionResult Edit(int id)
        {
           
            //Artist artist= context.Artists.Single(art => art.Id == id);
             Album album = db.Albums.Find(id);
            return View(album);
        }

        // POST: Albums/Edit/5
        [HttpPost]
        public ActionResult Edit(Album album)
        {
           
            db.Entry(album).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
           
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int id)
        {
           
            Album album = db.Albums.Find(id);
            
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Album album = db.Albums.Single(alb => alb.Id == id);
            db.Albums.Remove(album);
            db.SaveChanges();

            return RedirectToAction("Index");
            
        }
    }
}
