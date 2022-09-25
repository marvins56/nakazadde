using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nakazadde.Models;

namespace nakazadde.Controllers
{
    [HandleError]
    public class VideosController : Controller
    {
        private nakazaddeEntities db = new nakazaddeEntities();

        public ActionResult Index()
        {
            return View(db.Videos.Where(p => p.ContentType == "video/mp4").ToList());
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "Description,ShortNotes,UserId,DatePosted")] Video video, HttpPostedFileBase postedFile)
        {
            try
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }

                db.Videos.Add(new Video
                {
                    Name = Path.GetFileName(postedFile.FileName),
                    ContentType = postedFile.ContentType,
                    Data = bytes,
                    Description = video.Description,
                    ShortNotes = video.ShortNotes,
                    UserId = 2,
                    DatePosted = DateTime.Now

                });
                db.SaveChanges();
            
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public FileResult DownloadFile(int? fileId)
        {
         
            Video file = db.Videos.ToList().Find(p => p.Id == fileId.Value);
            return File(file.Data, file.ContentType, file.Name);
        }
        // GET: Videos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VideoId,Description,ShortNotes,UserId,DatePosted,IsVerified,FilePath,FileSize,Name")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(video);
        }

        // GET: Videos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VideoId,Description,ShortNotes,UserId,DatePosted,IsVerified,FilePath,FileSize,Name")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(video);
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video viddetails = db.Videos.Find(id);
            if (viddetails == null)
            {
                return HttpNotFound();
            }
            return View(viddetails);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
