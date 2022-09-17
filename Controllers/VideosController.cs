using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nakazadde.Models;

namespace nakazadde.Controllers
{
    public class VideosController : Controller
    {
        private nakazaddeEntities db = new nakazaddeEntities();

        // GET: Videos
        public ActionResult Index()
        {
            return View(db.Videos.ToList());
        }

        // GET: Videos/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult Create([Bind(Include = "VideoId,Description,ShortNotes,UserId,DatePosted,Source,IsVerified")] Video video, HttpPostedFileBase reportfile)
        {
            //saving changes           
            if (ModelState.IsValid)
            {
                try
                {
                    //FILE PROCESSING 
                    if (reportfile.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(reportfile.FileName);
                        string filepath = Path.Combine(Server.MapPath("~/App_Data/videos/"), filename);
                        video.ShortNotes = reportfile.FileName;
                        //userReportsTable.Title = filepath;
                        reportfile.SaveAs(filepath);

                        video.DatePosted = DateTime.Now;
                        video.UserId = 2;
                        video.Source = filepath;
                        db.Videos.Add(video);
                        db.SaveChanges();
                        ViewBag.upload = "file uploadded successfully";
                    }
            
                }
                catch (Exception e)
                {
                    TempData["reporterror"] = e.Message;
                }

                //return RedirectToAction("Index");
            }

            return View(video);
        }


        public ActionResult DownloadFile(string fileName)
        {
            string fullName = Server.MapPath("~/App_Data/videos/" + fileName);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
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

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VideoId,Description,ShortNotes,UserId,DatePosted,Source,IsVerified")] Video video)
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

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
            db.SaveChanges();
            return RedirectToAction("Index");
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
