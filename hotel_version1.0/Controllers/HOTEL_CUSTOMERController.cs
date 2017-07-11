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
    public class HOTEL_CUSTOMERController : Controller
    {
        private Entities db = new Entities();

        public bool IsNumberic(string oText)
        {
            try
            {
                int var1 = Convert.ToInt32(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public ActionResult Error()
        {
            return View(db.HOTEL_CUSTOMER.ToList());
        }


        // GET: HOTEL_CUSTOMER
        public ActionResult Index()
        {
            return View(db.HOTEL_CUSTOMER.ToList());
        }

        [HttpPost]
        public ActionResult Index(String searchString)
        {
            if (searchString == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (searchString.Length == 11 && IsNumberic(searchString))
            {
                var result = from a in db.HOTEL_CUSTOMER
                             where a.TEL_NUMBER == searchString
                             select a;
                return View(result.ToList());
            }
            else if (searchString.Length == 5 && IsNumberic(searchString))
            {
                var result = from a in db.HOTEL_CUSTOMER
                             where a.CUS_ID == searchString
                             select a;
                return View(result.ToList());
            }

            var result2 = from a in db.HOTEL_CUSTOMER
                          where a.NAME == searchString
                          select a;
            if (result2.Count() != 0)
                return View(result2.ToList());
            else
                return RedirectToAction("Error");

        }



        // GET: HOTEL_CUSTOMER/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var hOTEL_ORDER = db.HOTEL_ORDER.Include(h => h.HOTEL_CUSTOMER);
            var test = from p in hOTEL_ORDER
                       where p.CUS_ID == id
                       select p;

            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // GET: HOTEL_CUSTOMER/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HOTEL_CUSTOMER/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CUS_ID,NAME,REPUTATION,TEL_NUMBER")] HOTEL_CUSTOMER hOTEL_CUSTOMER)
        {
            if (ModelState.IsValid)
            {
                db.HOTEL_CUSTOMER.Add(hOTEL_CUSTOMER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hOTEL_CUSTOMER);
        }

        // GET: HOTEL_CUSTOMER/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_CUSTOMER hOTEL_CUSTOMER = db.HOTEL_CUSTOMER.Find(id);
            if (hOTEL_CUSTOMER == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_CUSTOMER);
        }

        // POST: HOTEL_CUSTOMER/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CUS_ID,NAME,REPUTATION,TEL_NUMBER")] HOTEL_CUSTOMER hOTEL_CUSTOMER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOTEL_CUSTOMER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hOTEL_CUSTOMER);
        }

        // GET: HOTEL_CUSTOMER/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_CUSTOMER hOTEL_CUSTOMER = db.HOTEL_CUSTOMER.Find(id);
            if (hOTEL_CUSTOMER == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_CUSTOMER);
        }

        // POST: HOTEL_CUSTOMER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOTEL_CUSTOMER hOTEL_CUSTOMER = db.HOTEL_CUSTOMER.Find(id);
            db.HOTEL_CUSTOMER.Remove(hOTEL_CUSTOMER);
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
