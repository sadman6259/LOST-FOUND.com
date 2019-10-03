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
    public class LoginController : Controller
    {
        private LOFDbEntities5 db = new LOFDbEntities5();

        // GET: Login
        public ActionResult Index()
        {
            var loginTbls = db.LoginTbls.Include(l => l.RegisterTbl);
            return View(loginTbls.ToList());
        }

        // GET: Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginTbl loginTbl = db.LoginTbls.Find(id);
            if (loginTbl == null)
            {
                return HttpNotFound();
            }
            return View(loginTbl);
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            ViewBag.RegisterId = new SelectList(db.RegisterTbls, "Id", "Email");
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,RegisterId")] LoginTbl loginTbl,string userName,string password,RegisterTbl registerTbl,int? id)
        {
            if (ModelState.IsValid)
            {
                var query = (from u in db.RegisterTbls
                             where u.UserName == userName && u.Password == password
                             select u).FirstOrDefault();
               

               
                if (query != null)
                {
                    loginTbl.RegisterId = query.Id;
                    db.LoginTbls.Add(loginTbl);

                    db.SaveChanges();

                    Session["username"] = loginTbl.UserName;

                    return RedirectToAction("Index");
                }
                else

                {
                    return RedirectToAction("Create");
                }
            }

            ViewBag.RegisterId = new SelectList(db.RegisterTbls, "Id", "Email", loginTbl.RegisterId);
            return View(loginTbl);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginTbl loginTbl = db.LoginTbls.Find(id);
            if (loginTbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegisterId = new SelectList(db.RegisterTbls, "Id", "Email", loginTbl.RegisterId);
            return View(loginTbl);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,RegisterId")] LoginTbl loginTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loginTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegisterId = new SelectList(db.RegisterTbls, "Id", "Email", loginTbl.RegisterId);
            return View(loginTbl);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginTbl loginTbl = db.LoginTbls.Find(id);
            if (loginTbl == null)
            {
                return HttpNotFound();
            }
            return View(loginTbl);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoginTbl loginTbl = db.LoginTbls.Find(id);
            db.LoginTbls.Remove(loginTbl);
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
