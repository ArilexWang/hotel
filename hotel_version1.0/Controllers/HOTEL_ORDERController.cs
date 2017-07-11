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
    public class HOTEL_ORDERController : Controller
    {
        private Entities db = new Entities();

        // GET: HOTEL_ORDER
        public ActionResult Index()
        {
            var hOTEL_ORDER = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(h => h.HOTEL_CUSTOMER).Include(h => h.HOTEL_ROOM);
            return View(hOTEL_ORDER.ToList());
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc, String searchString)
        {

            if (searchString == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = from a in db.HOTEL_ORDER
                          join b in db.HOTEL_CUSTOMER
            on a.CUS_ID equals b.CUS_ID
                          where b.NAME == searchString||a.ORDER_ID==searchString
                          select a;
            if (result.Count() != 0)
                return View(result.ToList());

            return RedirectToAction("Error");

        }

        // GET: HOTEL_ORDER/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_ORDER hOTEL_ORDER = db.HOTEL_ORDER.Find(id);
            if (hOTEL_ORDER == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_ORDER);
        }

        // GET: HOTEL_ORDER/Create
        public ActionResult Create()
        {
            ViewBag.ORDER_ID = new SelectList(db.HOTEL_BILL, "ORDER_ID", "ORDER_ID");
            ViewBag.CUS_ID = new SelectList(db.HOTEL_CUSTOMER, "CUS_ID", "NAME");
            ViewBag.ROOM_NUM = new SelectList(db.HOTEL_ROOM, "ROOM_NUM", "ROOM_TYPE");
            return View();
        }

        // POST: HOTEL_ORDER/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ORDER_ID,CHECK_IN_TIME,CHECK_OUT_TIME,RESERVE_TIME,STATUS,PRE_ORDER_ID,CUS_ID,ROOM_NUM,HOTEL_ID")] HOTEL_ORDER hOTEL_ORDER)
        {
            if (ModelState.IsValid)
            {
                db.HOTEL_ORDER.Add(hOTEL_ORDER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ORDER_ID = new SelectList(db.HOTEL_BILL, "ORDER_ID", "ORDER_ID", hOTEL_ORDER.ORDER_ID);
            ViewBag.CUS_ID = new SelectList(db.HOTEL_CUSTOMER, "CUS_ID", "NAME", hOTEL_ORDER.CUS_ID);
            ViewBag.ROOM_NUM = new SelectList(db.HOTEL_ROOM, "ROOM_NUM", "ROOM_TYPE", hOTEL_ORDER.ROOM_NUM);
            return View(hOTEL_ORDER);
        }

        // GET: HOTEL_ORDER/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_ORDER hOTEL_ORDER = db.HOTEL_ORDER.Find(id);
            if (hOTEL_ORDER == null)
            {
                return HttpNotFound();
            }
            ViewBag.ORDER_ID = new SelectList(db.HOTEL_BILL, "ORDER_ID", "ORDER_ID", hOTEL_ORDER.ORDER_ID);
            ViewBag.CUS_ID = new SelectList(db.HOTEL_CUSTOMER, "CUS_ID", "NAME", hOTEL_ORDER.CUS_ID);
            ViewBag.ROOM_NUM = new SelectList(db.HOTEL_ROOM, "ROOM_NUM", "ROOM_TYPE", hOTEL_ORDER.ROOM_NUM);
            return View(hOTEL_ORDER);
        }

        // POST: HOTEL_ORDER/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ORDER_ID,CHECK_IN_TIME,CHECK_OUT_TIME,RESERVE_TIME,STATUS,PRE_ORDER_ID,CUS_ID,ROOM_NUM,HOTEL_ID")] HOTEL_ORDER hOTEL_ORDER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOTEL_ORDER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ORDER_ID = new SelectList(db.HOTEL_BILL, "ORDER_ID", "ORDER_ID", hOTEL_ORDER.ORDER_ID);
            ViewBag.CUS_ID = new SelectList(db.HOTEL_CUSTOMER, "CUS_ID", "NAME", hOTEL_ORDER.CUS_ID);
            ViewBag.ROOM_NUM = new SelectList(db.HOTEL_ROOM, "ROOM_NUM", "ROOM_TYPE", hOTEL_ORDER.ROOM_NUM);
            return View(hOTEL_ORDER);
        }

        // GET: HOTEL_ORDER/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL_ORDER hOTEL_ORDER = db.HOTEL_ORDER.Find(id);
            if (hOTEL_ORDER == null)
            {
                return HttpNotFound();
            }
            return View(hOTEL_ORDER);
        }

        // POST: HOTEL_ORDER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOTEL_ORDER hOTEL_ORDER = db.HOTEL_ORDER.Find(id);
            db.HOTEL_ORDER.Remove(hOTEL_ORDER);
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
