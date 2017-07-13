using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using hotel_version1._0.Models;

namespace hotel_version1._0.Controllers
{
    public class HOTEL_USERController : Controller
    {
        private Entities db = new Entities();

        // GET: HOTEL_USER
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(string searchString1, string searchString2)
        {


            var test = from p in db.HOTEL_USER
                       where p.USER_ID == searchString1 && p.PASSWORD == searchString2
                       select p;
            Session["USER_ID"] = null;

            if (test.Count() == 0)
            {
                return RedirectToAction("Error");
            }
            else
            {
                Session["USER_ID"] = searchString1;


                return RedirectToAction("Index", "HOME", new { currentusername = searchString1 });
            }


        }

        public ActionResult Error()
        {


            return View();
        }
        public ActionResult Success()
        {


            return View();
        }

        //logout
        public ActionResult LogOut()
        {
            return View();
        }


        // GET: HOTEL_USER/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_USER hOTEL_USER = db.HOTEL_USER.Find(id);
            if (hOTEL_USER == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_USER);
        }

        // GET: HOTEL_USER/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HOTEL_USER/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_ID,PASSWORD")] HOTEL_USER hOTEL_USER)
        {
            if (ModelState.IsValid)
            {
                db.HOTEL_USER.Add(hOTEL_USER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hOTEL_USER);
        }

        // GET: HOTEL_USER/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_USER hOTEL_USER = db.HOTEL_USER.Find(id);
            if (hOTEL_USER == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_USER);
        }

        // POST: HOTEL_USER/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID,PASSWORD")] HOTEL_USER hOTEL_USER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOTEL_USER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hOTEL_USER);
        }

        // GET: HOTEL_USER/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_USER hOTEL_USER = db.HOTEL_USER.Find(id);
            if (hOTEL_USER == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_USER);
        }

        // POST: HOTEL_USER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOTEL_USER hOTEL_USER = db.HOTEL_USER.Find(id);
            db.HOTEL_USER.Remove(hOTEL_USER);
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
