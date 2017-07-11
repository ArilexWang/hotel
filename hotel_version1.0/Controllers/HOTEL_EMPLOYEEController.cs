using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using hotel_version1._0;

namespace hotel_version1._0.Controllers
{
    public class HOTEL_EMPLOYEEController : Controller
    {
        private Entities db = new Entities();

        // GET: HOTEL_EMPLOYEE
        public ActionResult Index()
        {
            var hOTEL_EMPLOYEE = db.HOTEL_EMPLOYEE.Include(h => h.HOTEL_EMPLOYEE_TYPE);
            return View(hOTEL_EMPLOYEE.ToList());
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
