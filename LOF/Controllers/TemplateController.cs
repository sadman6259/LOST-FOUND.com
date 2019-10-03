using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOF;
using LOF.ViewModel;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using PagedList;

namespace LOF.Controllers
{
    public class TemplateController : Controller
    {
        LOFDbEntities5 obj = new LOFDbEntities5();
        public ActionResult CommonIndex(string subcategorysrch,string locationsrch)
        {
            var commonmodel = new CommonIndex();
            var movies = from m in obj.Foundtbls
                         select m;
            var cinema = from n in obj.Losttbls
                         select n;

            if (!string.IsNullOrEmpty(subcategorysrch))
            {
              movies = movies.Where(m => m.SubCategory.Contains(subcategorysrch));
               cinema = cinema.Where(n => n.SubCategory.Contains(subcategorysrch));
                //   return View(movies.ToList());

            }
            if (!string.IsNullOrEmpty(locationsrch))
            {
           movies = movies.Where(x => x.Location == locationsrch);
            cinema = cinema.Where(x => x.Location == locationsrch);

            }
            commonmodel.found = obj.Foundtbls.ToList();
            commonmodel.lost = obj.Losttbls.ToList();

            return View(commonmodel);
        }
        // GET: Template
        public ActionResult Index()
        {
            
                var mymodel = new Home();
          
            mymodel.  Topfound = obj.Topfoundtbls.ToList();
          mymodel.  Toplost = obj.TopLosttbls.ToList();

        
            return View(mymodel);
        }
        [Authorize]

        // GET: FoundProducts/Create
        public ActionResult Create()
        {
            ViewBag.username = User.Identity.Name;
            //  ViewBag.username = (string)Session["username"];
            //  ViewBag.username = (from u in db.LoginTbls
            //           select u.RegisterTbl.UserName).Max();
            //   ViewBag.cellno = (from u in db.LoginTbls
            //   select u.RegisterTbl.ContactNo).Max();
            var lastrecord = (from c in obj.LoginTbls orderby c.Id descending select c).First();
            ViewBag.cellno = lastrecord.RegisterTbl.ContactNo;
            ViewBag.address = lastrecord.RegisterTbl.Address;
            bindLocation();
            bindCategory();
            return View();
        }
        public void bindLocation()
        {
            var location = obj.Locations.ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Location", Value = "0" });
            foreach (var m in location)
            {



                li.Add(new SelectListItem { Text = m.LocationName, Value = m.LocationId.ToString() });
                ViewBag.location = li;
                // ViewBag.Category = new SelectList(li, m.CategoryName,m.CategoryName);
            }
        }
        public void bindCategory()
        {
            var category = obj.Categories.ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Category", Value = "0" });
            foreach (var m in category)
            {



                li.Add(new SelectListItem { Text = m.CategoryName, Value = m.CategoryId.ToString() });
                ViewBag.category = li;
                // ViewBag.Category = new SelectList(li, m.CategoryName,m.CategoryName);
            }
        }

        public JsonResult getSubCategory(int id)
        {
            var ddlSubCategory = obj.SubCategories.Where(x => x.CategoryId == id).ToList();
            List<SelectListItem> lisub = new List<SelectListItem>();

            lisub.Add(new SelectListItem { Text = "Select SubCategory", Value = "0" });
            if (ddlSubCategory != null)
            {
                foreach (var x in ddlSubCategory)
                {
                    lisub.Add(new SelectListItem { Text = x.SubCategoryName, Value = x.SubCategoryId.ToString() });
                }
            }
            return Json(new SelectList(lisub, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult getSubLocation(int id)
        {
            var ddlSubLocation = obj.SubLocations.Where(x => x.LocationId == id).ToList();
            List<SelectListItem> lisub = new List<SelectListItem>();

            lisub.Add(new SelectListItem { Text = "Select SubLocation", Value = "0" });
            if (ddlSubLocation != null)
            {
                foreach (var x in ddlSubLocation)
                {
                    lisub.Add(new SelectListItem { Text = x.SubLocationName, Value = x.SubLocationId.ToString() });
                }
            }
            return Json(new SelectList(lisub, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        // POST: Foundstbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(AdminApprovalTbl approve)
        {


            string fileName = Path.GetFileNameWithoutExtension(approve.ImageFile.FileName);

            //  string fileName = Path.GetFileNameWithoutExtension(loststbl.ImageFile.FileName);
            // string fileNames = Path.GetFileNameWithoutExtension(losttbl.ImageFile.FileName);

            string extension = Path.GetExtension(approve.ImageFile.FileName);

            //  string extension = Path.GetExtension(loststbl.ImageFile.FileName);
            //   string extensions = Path.GetExtension(losttbl.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;


            // fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //   fileNames = fileNames + DateTime.Now.ToString("yymmssfff") + extensions;

            approve.Image = "~/Image/" + fileName;

            //  loststbl.Image = "~/Image/" + fileName;
            //  losttbl.Image = "~/Image/" + fileNames;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);

            //  fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            //   fileNames = Path.Combine(Server.MapPath("~/Image/"), fileNames);
            approve.ImageFile.SaveAs(fileName);

            //   loststbl.ImageFile.SaveAs(fileName);
            //   losttbl.ImageFile.SaveAs(fileNames);


            if (ModelState.IsValid)
            {
                approve.CategoryId = Int32.Parse(approve.Category);
                approve.SubCategoryId = Int32.Parse(approve.SubCategory);

                approve.LocationId = Int32.Parse(approve.Location);
                approve.SubLocationId = Int32.Parse(approve.SubLocation);


                //     loststbl.CategoryId = Int32.Parse(loststbl.Category);
                //    loststbl.SubCategoryId = Int32.Parse(loststbl.SubCategory);

                //    loststbl.LocationId = Int32.Parse(loststbl.Location);
                //      loststbl.SubLocationId = Int32.Parse(loststbl.SubLocation);

                //   losttbl.CategoryId = Int32.Parse(losttbl.Category);
                //   losttbl.SubCategoryId = Int32.Parse(losttbl.SubCategory);

                //     losttbl.LocationId = Int32.Parse(loststbl.Location);
                //     losttbl.SubLocationId = Int32.Parse(losttbl.Sublocation);
                obj.AdminApprovalTbls.Add(approve);

                //   db.Foundtbls.Add(loststbl);
                //   db.AllProductsTbls.Add(losttbl);
                try
                {
                    obj.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Success");
            }

            return View(approve);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topfoundtbl foundtbl = obj.Topfoundtbls.Find(id);
            if (foundtbl == null)
            {
                return HttpNotFound();
            }
            return View(foundtbl);
        }
        public ActionResult DetailsToplost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopLosttbl foundtbl = obj.TopLosttbls.Find(id);
            if (foundtbl == null)
            {
                return HttpNotFound();
            }
            return View(foundtbl);
        }
        [Authorize]

        // GET: FoundProducts/Create
        public ActionResult CreateTopLost()
        {
            ViewBag.username = User.Identity.Name;
            //  ViewBag.username = (string)Session["username"];
            //  ViewBag.username = (from u in db.LoginTbls
            //           select u.RegisterTbl.UserName).Max();
            //   ViewBag.cellno = (from u in db.LoginTbls
            //   select u.RegisterTbl.ContactNo).Max();
            var lastrecord = (from c in obj.LoginTbls orderby c.Id descending select c).First();
            ViewBag.cellno = lastrecord.RegisterTbl.ContactNo;
            ViewBag.address = lastrecord.RegisterTbl.Address;
            bindCategory();
            bindLocation();
            return View();
        }


        // POST: Foundstbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateTopLost(AdminApprovalTbl approve)
        {
            string fileName = Path.GetFileNameWithoutExtension(approve.ImageFile.FileName);

            //  string fileName = Path.GetFileNameWithoutExtension(loststbl.ImageFile.FileName);
            // string fileNames = Path.GetFileNameWithoutExtension(losttbl.ImageFile.FileName);

            string extension = Path.GetExtension(approve.ImageFile.FileName);

            //  string extension = Path.GetExtension(loststbl.ImageFile.FileName);
            //   string extensions = Path.GetExtension(losttbl.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;


            // fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //   fileNames = fileNames + DateTime.Now.ToString("yymmssfff") + extensions;

            approve.Image = "~/Image/" + fileName;

            //  loststbl.Image = "~/Image/" + fileName;
            //  losttbl.Image = "~/Image/" + fileNames;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);

            //  fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            //   fileNames = Path.Combine(Server.MapPath("~/Image/"), fileNames);
            approve.ImageFile.SaveAs(fileName);

            //   loststbl.ImageFile.SaveAs(fileName);
            //   losttbl.ImageFile.SaveAs(fileNames);


            if (ModelState.IsValid)
            {
                approve.CategoryId = Int32.Parse(approve.Category);
                approve.SubCategoryId = Int32.Parse(approve.SubCategory);

                approve.LocationId = Int32.Parse(approve.Location);
                approve.SubLocationId = Int32.Parse(approve.SubLocation);


                //     loststbl.CategoryId = Int32.Parse(loststbl.Category);
                //    loststbl.SubCategoryId = Int32.Parse(loststbl.SubCategory);

                //    loststbl.LocationId = Int32.Parse(loststbl.Location);
                //      loststbl.SubLocationId = Int32.Parse(loststbl.SubLocation);

                //   losttbl.CategoryId = Int32.Parse(losttbl.Category);
                //   losttbl.SubCategoryId = Int32.Parse(losttbl.SubCategory);

                //     losttbl.LocationId = Int32.Parse(loststbl.Location);
                //     losttbl.SubLocationId = Int32.Parse(losttbl.Sublocation);
                obj.AdminApprovalTbls.Add(approve);

                //   db.Foundtbls.Add(loststbl);
                //   db.AllProductsTbls.Add(losttbl);
                try
                {
                    obj.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Success");
            }

            return View(approve);
        }
        public ActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AjaxMethod()
        {
            string query = "SELECT (Location.LocationName) TotalLo, COUNT(AllProductsTbl.Id) TotalId";
            query += " FROM AllProductsTbl,Location WHERE Location.LocationId = AllProductsTbl.LocationId GROUP BY Location.LocationName";
             
                string constr = "Data Source=DELL-PC;Initial Catalog=LOFDb;Integrated Security=True";
                List<object> chartData = new List<object>();
                chartData.Add(new object[]
                                {
                            "Location.LocationName", "TotalId"
                                });
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                chartData.Add(new object[]
                                {
                            sdr["TotalLo"], sdr["TotalId"]
                                });
                            }
                        }

                        con.Close();
                    }
                }

                return Json(chartData);
            } 
    }
}