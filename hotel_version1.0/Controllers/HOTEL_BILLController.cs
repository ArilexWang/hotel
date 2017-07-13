﻿using System;
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
    public class HOTEL_BILLController : Controller
    {
        private Entities db = new Entities();
        static bool[] ascMsg = new bool[7] { true, true, true, true, true, true, true };


        // GET: HOTEL_BILL
        public ActionResult Index(int? page)
        {
            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);


            var all_bill = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(m => m.HOTEL_ORDER_SERVICE).Include(j => j.HOTEL_ITEM_USED);
            var cus = from c in all_bill select c;
            cus = cus.OrderBy(x => x.ORDER_ID);
            IPagedList<HOTEL_ORDER> cust = cus.ToPagedList(pagenumber, pageSize);
            ViewBag.SelCat = "bill";
            //return View(all_bill.ToList());
            return View(cust);
        }


        public ActionResult Error()
        {
            ViewBag.SelCat = "bill";
            return View();
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
                       where p.HOTEL_CUSTOMER.NAME == searchString || p.ORDER_ID == searchString || p.HOTEL_ROOM.ROOM_TYPE == searchString || p.STATUS == searchString
                       select p;

            if (test.Count() == 0)
            {
                return RedirectToAction("Error");
            }

            return View(test.ToList());
        }

        

        [HttpPost]
        public ActionResult Sort(string message,int? page)
        {
            var sort_BILL = db.HOTEL_ORDER.Include(h => h.HOTEL_BILL).Include(m => m.HOTEL_ORDER_SERVICE).Include(j => j.HOTEL_ITEM_USED);

            IOrderedQueryable<HOTEL_ORDER> result = null;

            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);
            if (message == "ORDER_ID")
            {
                if (ascMsg[0])
                {
                    result = from a in sort_BILL
                             orderby a.ORDER_ID ascending
                             select a;
                }
                else
                {
                    result = from a in sort_BILL
                             orderby a.ORDER_ID descending
                             select a;
                }
                ascMsg[0] = !ascMsg[0];

            }
            else if (message == "SUM")
            {
                if (ascMsg[1])
                {
                    result = from a in sort_BILL
                             orderby a.HOTEL_BILL.ROOM_PRICE + a.HOTEL_BILL.SERVICE_PRICE + a.HOTEL_BILL.ITEM_PRICE ascending
                             select a;
                }
                else
                {
                    result = from a in sort_BILL
                             orderby a.HOTEL_BILL.ROOM_PRICE + a.HOTEL_BILL.SERVICE_PRICE + a.HOTEL_BILL.ITEM_PRICE descending
                             select a;
                }
                ascMsg[1] = !ascMsg[1];
            }
            else if (message == "NAME")
            {
                if (ascMsg[2])
                {
                    result = from a in sort_BILL
                             orderby a.HOTEL_CUSTOMER.NAME ascending
                             select a;
                }
                else
                {
                    result = from a in sort_BILL
                             orderby a.HOTEL_CUSTOMER.NAME descending
                             select a;
                }
                ascMsg[2] = !ascMsg[2];
            }


            if (result != null && result.Count() != 0)
            {
                IPagedList<HOTEL_ORDER> cust = result.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }
                
            else
            {
                IPagedList<HOTEL_ORDER> cust = sort_BILL.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }
                


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
