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
    public class HOTEL_BILLController : Controller
    {
        private Entities db = new Entities();

        // GET: HOTEL_BILL
        public ActionResult Index()
        {
            var all_bill = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(m => m.HOTEL_ORDER_SERVICE).Include(j => j.HOTEL_ITEM_USED);

            return View(all_bill.ToList());

        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            if (searchString == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var all_bill = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(m => m.HOTEL_ORDER_SERVICE).Include(j => j.HOTEL_ITEM_USED);

            var test = from p in all_bill
                       where p.HOTEL_CUSTOMER.NAME == searchString
                       select p;

            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test.ToList());
        }



        // GET: HOTEL_BILL/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var all_bill = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(m => m.HOTEL_ORDER_SERVICE).Include(j => j.HOTEL_ITEM_USED);

            var test = from p in all_bill
                       where p.ORDER_ID == id
                       select p;

            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test.ToList());


        }

        // GET: HOTEL_BILL/Create
        public ActionResult Create()
        {
            ViewBag.ORDER_ID = new SelectList(db.HOTEL_ORDER, "ORDER_ID", "STATUS");
            return View();
        }

        // POST: HOTEL_BILL/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ORDER_ID,SERVICE_PRICE,ROOM_PRICE")] HOTEL_BILL hOTEL_BILL)
        {
            if (ModelState.IsValid)
            {
                db.HOTEL_BILL.Add(hOTEL_BILL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ORDER_ID = new SelectList(db.HOTEL_ORDER, "ORDER_ID", "STATUS", hOTEL_BILL.ORDER_ID);
            return View(hOTEL_BILL);
        }

        // GET: HOTEL_BILL/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_BILL hOTEL_BILL = db.HOTEL_BILL.Find(id);
            if (hOTEL_BILL == null)
            {
                return HttpNotFound();
            }
            ViewBag.ORDER_ID = new SelectList(db.HOTEL_ORDER, "ORDER_ID", "STATUS", hOTEL_BILL.ORDER_ID);
            return View(hOTEL_BILL);
        }

        // POST: HOTEL_BILL/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ORDER_ID,SERVICE_PRICE,ROOM_PRICE")] HOTEL_BILL hOTEL_BILL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOTEL_BILL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ORDER_ID = new SelectList(db.HOTEL_ORDER, "ORDER_ID", "STATUS", hOTEL_BILL.ORDER_ID);
            return View(hOTEL_BILL);
        }

        // GET: HOTEL_BILL/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_BILL hOTEL_BILL = db.HOTEL_BILL.Find(id);
            if (hOTEL_BILL == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_BILL);
        }

        // POST: HOTEL_BILL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOTEL_BILL hOTEL_BILL = db.HOTEL_BILL.Find(id);
            db.HOTEL_BILL.Remove(hOTEL_BILL);
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
