using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOF;

namespace LOF.Controllers
{
    public class RegisterController : Controller
    {
        private LOFDbEntities6 db = new LOFDbEntities6();

        // GET: Register
        public ActionResult Index()
        {
            return View(db.RegisterTbls.ToList());
        }

        // GET: Register/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterTbl registerTbl = db.RegisterTbls.Find(id);
            if (registerTbl == null)
            {
                return HttpNotFound();
            }
            return View(registerTbl);
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,UserName,Password,ConfirmPassword,ContactNo,Address,AlternativeContactNo")] RegisterTbl registerTbl)
        {
            if (ModelState.IsValid)
            {
                db.RegisterTbls.Add(registerTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(registerTbl);
        }

        // GET: Register/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterTbl registerTbl = db.RegisterTbls.Find(id);
            if (registerTbl == null)
            {
                return HttpNotFound();
            }
            return View(registerTbl);
        }

        // POST: Register/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,UserName,Password,ConfirmPassword,ContactNo,Address,AlternativeContactNo")] RegisterTbl registerTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registerTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registerTbl);
        }

        // GET: Register/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterTbl registerTbl = db.RegisterTbls.Find(id);
            if (registerTbl == null)
            {
                return HttpNotFound();
            }
            return View(registerTbl);
        }

        // POST: Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegisterTbl registerTbl = db.RegisterTbls.Find(id);
            db.RegisterTbls.Remove(registerTbl);
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
