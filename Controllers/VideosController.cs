using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
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

        // GET: Videos
        //public ActionResult Index()
        //{
        //    return View(db.Videos.ToList());
        //}

        //[HttpGet]
        //public ActionResult UploadVideo()
        //{
        //    List<Video> videolist = new List<Video>();
        //    string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(CS))
        //    {
        //        SqlCommand cmd = new SqlCommand("spGetAllVideoFile", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            Video video = new Video();
        //            video.VideoId = Convert.ToInt32(rdr["VideoId"]);
        //            video.Name = rdr["Name"].ToString();
        //            video.FileSize = Convert.ToInt32(rdr["FileSize"]);
        //            video.FilePath = rdr["FilePath"].ToString();

        //            videolist.Add(video);
        //        }
        //    }
        //    return View(videolist);
        //}
        //[HttpPost]
        //public ActionResult UploadVideo([Bind(Include = "VideoId,Description,ShortNotes,UserId,DatePosted,IsVerified")] Video video, HttpPostedFileBase fileupload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (fileupload != null)
        //     {
        //        string fileName = Path.GetFileName(fileupload.FileName);
        //        int fileSize = fileupload.ContentLength;
        //        int Size = fileSize / 1000;
        //        fileupload.SaveAs(Server.MapPath("App_Data/videos/" + fileName));

        //        string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //        using (SqlConnection con = new SqlConnection(CS))
        //        {
        //            SqlCommand cmd = new SqlCommand("spAddNewVideoFile", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            con.Open();
        //            cmd.Parameters.AddWithValue("@Name", fileName);
        //            cmd.Parameters.AddWithValue("@Description", video.Description);
        //            cmd.Parameters.AddWithValue("@ShortNotes", video.ShortNotes);
        //            cmd.Parameters.AddWithValue("@UserId", 21);
        //            cmd.Parameters.AddWithValue("@DatePosted", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@FileSize", Size);
        //            cmd.Parameters.AddWithValue("FilePath", "App_Data/videos/" + fileName);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    }
        //    return RedirectToAction("UploadVideo");
        //}
        //// GET: Videos/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Video video = db.Videos.Find(id);
        //    if (video == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(video);
        //}
        // GET: Home
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
                    Description = "DESCRIBE",
                    ShortNotes = "DESCRIBE",
                    UserId = 2,
                    DatePosted = DateTime.Now

                });
                db.SaveChanges();
            
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
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

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
