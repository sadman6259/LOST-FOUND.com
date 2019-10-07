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
    public class AdminApprovalController : Controller
    {
        private LOFDbEntities6 db = new LOFDbEntities6();

        // GET: AdminApproval
        public ActionResult Index()
        {
            var adminApprovalTbls = db.AdminApprovalTbls.Include(a => a.Category1).Include(a => a.Location1).Include(a => a.SubCategory1).Include(a => a.SubLocation1);
            return View(adminApprovalTbls.ToList());
        }

        // GET: AdminApproval/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminApprovalTbl adminApprovalTbl = db.AdminApprovalTbls.Find(id);
            if (adminApprovalTbl == null)
            {
                return HttpNotFound();
            }
            return View(adminApprovalTbl);
        }

        // GET: AdminApproval/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            ViewBag.SubLocationId = new SelectList(db.SubLocations, "SubLocationId", "SubLocationName");
            return View();
        }

        // POST: AdminApproval/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Image,Type,Category,SubCategory,Location,SubLocation,DateOfFound,DateOfLost,Details,OwnerName,CellNo,CategoryId,SubCategoryId,LocationId,SubLocationId")] AdminApprovalTbl adminApprovalTbl)
        {
            if (ModelState.IsValid)
            {
                db.AdminApprovalTbls.Add(adminApprovalTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", adminApprovalTbl.CategoryId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", adminApprovalTbl.LocationId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", adminApprovalTbl.SubCategoryId);
            ViewBag.SubLocationId = new SelectList(db.SubLocations, "SubLocationId", "SubLocationName", adminApprovalTbl.SubLocationId);
            return View(adminApprovalTbl);
        }

        // GET: AdminApproval/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminApprovalTbl adminApprovalTbl = db.AdminApprovalTbls.Find(id);
            if (adminApprovalTbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", adminApprovalTbl.CategoryId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", adminApprovalTbl.LocationId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", adminApprovalTbl.SubCategoryId);
            ViewBag.SubLocationId = new SelectList(db.SubLocations, "SubLocationId", "SubLocationName", adminApprovalTbl.SubLocationId);
            return View(adminApprovalTbl);
        }

        // POST: AdminApproval/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Image,Type,Category,SubCategory,Location,SubLocation,DateOfFound,DateOfLost,FoundId,LostId,Details,OwnerName,CellNo,CategoryId,SubCategoryId,LocationId,SubLocationId,OwnerAddress,UniqueKey")] AdminApprovalTbl appoval, AllProductsTbl adminApprovalTbl,Foundtbl foundtbl,Losttbl losttbl,Topfoundtbl topfoundtbl,TopLosttbl toplosttbl)
        {
            if (ModelState.IsValid)
            {

                if(appoval.Type == "FOUNDED")
                {

                    db.AllProductsTbls.Add(adminApprovalTbl);
                    db.Foundtbls.Add(foundtbl);

                }
                if (appoval.Type == "TOP FOUNDED")
                {

                    db.AllProductsTbls.Add(adminApprovalTbl);
                    db.Foundtbls.Add(foundtbl);

                    db.Topfoundtbls.Add(topfoundtbl);

                }
                if (appoval.Type == "LOSTED")
                {

                    db.AllProductsTbls.Add(adminApprovalTbl);
                    db.Losttbls.Add(losttbl);

                }
                if (appoval.Type == "TOP LOSTED")
                {
                    db.AllProductsTbls.Add(adminApprovalTbl);
                    db.Losttbls.Add(losttbl);

                    db.TopLosttbls.Add(toplosttbl);

                }
           //     db.AllProductsTbls.Add(adminApprovalTbl);

              //  db.Entry(adminApprovalTbl).State = EntityState.Modified;
                db.SaveChanges();
           
                
                    return RedirectToAction("CreateConfirm");
                
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", adminApprovalTbl.CategoryId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", adminApprovalTbl.LocationId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", adminApprovalTbl.SubCategoryId);
            ViewBag.SubLocationId = new SelectList(db.SubLocations, "SubLocationId", "SubLocationName", adminApprovalTbl.SubLocationId);
            return View(appoval);
        }

        // GET: AdminApproval/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminApprovalTbl adminApprovalTbl = db.AdminApprovalTbls.Find(id);
            if (adminApprovalTbl == null)
            {
                return HttpNotFound();
            }
            return View(adminApprovalTbl);
        }

        // POST: AdminApproval/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminApprovalTbl adminApprovalTbl = db.AdminApprovalTbls.Find(id);
            db.AdminApprovalTbls.Remove(adminApprovalTbl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateConfirm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateForm()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult CreateForm(AdminLoginTbl adminlogin)
        {
            if (adminlogin.Username == "admin" && adminlogin.Password == "admin")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Invalid");
            }
          //  return View(adminlogin);
        }
        public ActionResult Invalid()
        {
            return View();
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
