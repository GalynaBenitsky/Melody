using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    [Authorize]
    public class TracksController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private Manager m = new Manager();
        // GET: Tracks
        public ActionResult Index()
        {
            return View(m.TrackGetAll());
        }

        // GET: Tracks/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.TrackGetById(id.GetValueOrDefault());
            if( o == null)
            {
                return HttpNotFound();
            }
            return View(o);
        }

        [Route("audio/{id}")]
        public ActionResult DetailsAudio(int? id)
        {
           var o = m.TrackGetByIdAudio(id.GetValueOrDefault());

            try {
                return File(o.Audio, o.AudioContentType);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        // GET: Tracks/Create
       

        // GET: Tracks/Edit/5
        public ActionResult Edit(int id)
        {

            //Artist artist= context.Artists.Single(art => art.Id == id);
            Track track = db.Tracks.Single(t => t.Id == id);
           // Track track = db.Tracks.Find(id);
            return View(track);
        }

        // POST: Tracks/Edit/5
        [HttpPost]
        public ActionResult Edit(Track track)
        {
            
            db.Entry(track).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: Tracks/Delete/5
        public ActionResult Delete(int id)
        {
           
            Track track = db.Tracks.Find(id);
            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Track track = db.Tracks.Single(tr => tr.Id == id);
            db.Tracks.Remove(track);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
