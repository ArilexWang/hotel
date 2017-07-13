using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using hotel_version1._0;
using PagedList;

namespace hotel_version1._0.Controllers
{
    public class HOTEL_EMPLOYEEController : Controller
    {
        private Entities db = new Entities();
        static bool[] ascMsg = new bool[7] { true, true, true, true,true,true,true};
        // GET: HOTEL_EMPLOYEE

        public ActionResult Error()
        {
            ViewBag.SelCat = "employee";
            return View();
        }
        public ActionResult Index(int? page)
        {


            ViewBag.SelCat = "employee";
            var hOTEL_EMPLOYEE = db.HOTEL_EMPLOYEE.Include(h => h.HOTEL_EMPLOYEE_TYPE);
            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

            var cus = from c in hOTEL_EMPLOYEE select c;
            cus = cus.OrderBy(x => x.EMP_ID);
            IPagedList<HOTEL_EMPLOYEE> cust = cus.ToPagedList(pagenumber, pageSize);


            //return View(hOTEL_EMPLOYEE.ToList());
            return View(cust);
        }

        [HttpPost]
        public ActionResult Index(String searchString,int? page)
        {
            ViewBag.SelCat = "employee";
            if (searchString == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var result = from a in db.HOTEL_EMPLOYEE
                         where a.NAME == searchString || a.EMP_ID == searchString || a.PHONENUMBER == searchString || a.EMP_TYPE == searchString
                         select a;
            result = result.OrderBy(x => x.EMP_ID);

            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);
            IPagedList<HOTEL_EMPLOYEE> cust = result.ToPagedList(pagenumber, pageSize);

            if (result.Count() != 0)
                return View(cust);
            else
                return RedirectToAction("Error");

        }

        [HttpPost]
       
        public ActionResult Sort(string message,string currentSort,int? page)
        {
            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);


            var sort_employee = db.HOTEL_EMPLOYEE.Include(h => h.HOTEL_EMPLOYEE_TYPE);

            ViewBag.CurrentSort = message;
            message = string.IsNullOrEmpty(message) ? "EMP_ID" : message;

            IOrderedQueryable<HOTEL_EMPLOYEE> result = null;
            if (message == "EMP_ID")
            {
                if (message.Equals(currentSort) && pagenumber == 1)
                {
                    ascMsg[0] = !ascMsg[0];
                }
                else if (string.IsNullOrEmpty(currentSort) && pagenumber == 1)
                {
                    ascMsg[0] = true;
                }

                if (ascMsg[0])
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.EMP_ID ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.EMP_ID descending
                             select a;
                }
                

            }
            else if (message == "NAME")
            {
                if (message.Equals(currentSort) && pagenumber == 1)
                {
                    ascMsg[1] = !ascMsg[1];
                }
                else if (string.IsNullOrEmpty(currentSort) && pagenumber == 1)
                {
                    ascMsg[1] = true;
                }

                if (ascMsg[1])
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.NAME ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.NAME descending
                             select a;
                }
            }
            else if (message == "SALARY")
            {
                if (message.Equals(currentSort) && pagenumber == 1)
                {
                    ascMsg[2] = !ascMsg[2];
                }
                else if (string.IsNullOrEmpty(currentSort) && pagenumber == 1)
                {
                    ascMsg[2] = true;
                }
                if (ascMsg[2])
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.HOTEL_EMPLOYEE_TYPE.SALARY ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.HOTEL_EMPLOYEE_TYPE.SALARY descending
                             select a;
                }
                
            }
            else if (message == "BONUS")
            {
                if (message.Equals(currentSort) && pagenumber == 1)
                {
                    ascMsg[3] = !ascMsg[3];
                }
                else if (string.IsNullOrEmpty(currentSort) && pagenumber == 1)
                {
                    ascMsg[3] = true;
                }
                if (ascMsg[3])
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.BONUS ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.BONUS descending
                             select a;
                }
              
            }
            else if (message == "EMP_TYPE")
            {
                if (message.Equals(currentSort) && pagenumber == 1)
                {
                    ascMsg[4] = !ascMsg[4];
                }
                else if (string.IsNullOrEmpty(currentSort) && pagenumber == 1)
                {
                    ascMsg[4] = true;
                }
                if (ascMsg[4])
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.EMP_TYPE ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_EMPLOYEE
                             orderby a.EMP_TYPE descending
                             select a;
                }
               
            }

            if (result != null && result.Count() != 0)
            {
                IPagedList<HOTEL_EMPLOYEE> cust = result.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }
            else
            {
                IPagedList<HOTEL_EMPLOYEE> cust = sort_employee.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }


        }


       
 


        // GET: HOTEL_EMPLOYEE/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_EMPLOYEE hOTEL_EMPLOYEE = db.HOTEL_EMPLOYEE.Find(id);
            if (hOTEL_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_EMPLOYEE);
        }

        // GET: HOTEL_EMPLOYEE/Create
        public ActionResult Create()
        {
            ViewBag.EMP_TYPE = new SelectList(db.HOTEL_EMPLOYEE_TYPE, "EMP_TYPE", "TYPE_LEVEL");
            return View();
        }

        // POST: HOTEL_EMPLOYEE/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EMP_ID,NAME,PHONENUMBER,EMP_TYPE,BONUS")] HOTEL_EMPLOYEE hOTEL_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.HOTEL_EMPLOYEE.Add(hOTEL_EMPLOYEE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EMP_TYPE = new SelectList(db.HOTEL_EMPLOYEE_TYPE, "EMP_TYPE", "TYPE_LEVEL", hOTEL_EMPLOYEE.EMP_TYPE);
            return View(hOTEL_EMPLOYEE);
        }

        // GET: HOTEL_EMPLOYEE/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_EMPLOYEE hOTEL_EMPLOYEE = db.HOTEL_EMPLOYEE.Find(id);
            if (hOTEL_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMP_TYPE = new SelectList(db.HOTEL_EMPLOYEE_TYPE, "EMP_TYPE", "TYPE_LEVEL", hOTEL_EMPLOYEE.EMP_TYPE);
            return View(hOTEL_EMPLOYEE);
        }

        // POST: HOTEL_EMPLOYEE/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMP_ID,NAME,PHONENUMBER,EMP_TYPE,BONUS")] HOTEL_EMPLOYEE hOTEL_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOTEL_EMPLOYEE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMP_TYPE = new SelectList(db.HOTEL_EMPLOYEE_TYPE, "EMP_TYPE", "TYPE_LEVEL", hOTEL_EMPLOYEE.EMP_TYPE);
            return View(hOTEL_EMPLOYEE);
        }

        // GET: HOTEL_EMPLOYEE/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_EMPLOYEE hOTEL_EMPLOYEE = db.HOTEL_EMPLOYEE.Find(id);
            if (hOTEL_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_EMPLOYEE);
        }

        // POST: HOTEL_EMPLOYEE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOTEL_EMPLOYEE hOTEL_EMPLOYEE = db.HOTEL_EMPLOYEE.Find(id);
            db.HOTEL_EMPLOYEE.Remove(hOTEL_EMPLOYEE);
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
