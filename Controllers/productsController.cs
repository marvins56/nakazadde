﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nakazadde.Models;

namespace nakazadde.Controllers
{
    public class productsController : Controller
    {
        private nakazaddeEntities db = new nakazaddeEntities();

        
        public ActionResult Index()
        {
            return View(db.products.ToList());
        }
        [HttpPost]
        public ActionResult Index([Bind(Include = "id,Title,Description,price,Data,ContentType")] product productmodel, HttpPostedFileBase postedFile)
        {
            try
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }

                db.products.Add(new product
                {
                    Title = productmodel.Title,
                    ContentType = postedFile.ContentType,
                    Data = bytes,
                    Description = productmodel.Description,
                    price = productmodel.price 
                });
                db.SaveChanges();

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public FileResult DownloadFile(int? fileId)
        {

            product file = db.products.ToList().Find(p => p.id == fileId.Value);
            return File(file.Data, file.ContentType);
        }




        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product products = db.products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }
      


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Title,Description,price,Data,ContentType")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
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
    public class Item
    {
        public product Product
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

    }
}
