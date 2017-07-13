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
    public class HOTEL_ORDERController : Controller
    {
        private Entities db = new Entities();
        static bool[] ascMsg = new bool[7] { true, true, true, true, true, true, true };
        // GET: HOTEL_ORDER
        public ActionResult Index(int? page)
        {
            ViewBag.SelCat = "order";
            var hOTEL_ORDER = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(h => h.HOTEL_CUSTOMER).Include(h => h.HOTEL_ROOM);

            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

            var cus = from c in hOTEL_ORDER select c;
            cus = cus.OrderBy(x => x.ORDER_ID);
            IPagedList<HOTEL_ORDER> cust = cus.ToPagedList(pagenumber, pageSize);

            //return View(hOTEL_ORDER.ToList());
            return View(cust);
        }

        public ActionResult Error()
        {
            ViewBag.SelCat = "order";
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc, String searchString,int? page)
        {
            ViewBag.SelCat = "order";
            if (searchString == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = from a in db.HOTEL_ORDER
                         join b in db.HOTEL_CUSTOMER
           on a.CUS_ID equals b.CUS_ID
                         where b.NAME == searchString || a.ORDER_ID == searchString
                         select a;


            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

            result = result.OrderBy(x => x.CUS_ID);
            IPagedList<HOTEL_ORDER> cust = result.ToPagedList(pagenumber, pageSize);
            

            if (result.Count() != 0)
                return View(cust);

            return RedirectToAction("Error");

        }

        public ActionResult Sort(string message, string currentSort, int? page)
        {
            var sort_ORDER = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(h => h.HOTEL_CUSTOMER).Include(h => h.HOTEL_ROOM);

            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);
            var sort_employee = db.HOTEL_CUSTOMER;
            ViewBag.CurrentSort = message;
            message = string.IsNullOrEmpty(message) ? "CUS_ID" : message;

            IOrderedQueryable<HOTEL_ORDER> result = null;
            if (message == "ORDER_ID")
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
                    result = from a in sort_ORDER
                             orderby a.ORDER_ID ascending
                             select a;
                }
                else
                {
                    result = from a in sort_ORDER
                             orderby a.ORDER_ID descending
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
                    result = from a in sort_ORDER
                             orderby a.HOTEL_CUSTOMER.NAME ascending
                             select a;
                }
                else
                {
                    result = from a in sort_ORDER
                             orderby a.HOTEL_CUSTOMER.NAME descending
                             select a;
                }
            }
            else if (message == "ROOM_TYPE")
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
                    result = from a in sort_ORDER
                             orderby a.HOTEL_ROOM.ROOM_TYPE ascending
                             select a;
                }
                else
                {
                    result = from a in sort_ORDER
                             orderby a.HOTEL_ROOM.ROOM_TYPE descending
                             select a;
                }

            }
            else if (message == "RESERVE_TIME")
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
                    result = from a in sort_ORDER
                             orderby a.RESERVE_TIME ascending
                             select a;
                }
                else
                {
                    result = from a in sort_ORDER
                             orderby a.RESERVE_TIME descending
                             select a;
                }
            }
            else if (message == "ORDER_STATUS")
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
                    result = from a in sort_ORDER
                             orderby a.STATUS ascending
                             select a;
                }
                else
                {
                    result = from a in sort_ORDER
                             orderby a.STATUS descending
                             select a;
                }
            }

            if (result != null && result.Count() != 0)
            {
                IPagedList<HOTEL_ORDER> cust = result.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }
            else
            {
                IPagedList<HOTEL_ORDER> cust = sort_ORDER.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }

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
