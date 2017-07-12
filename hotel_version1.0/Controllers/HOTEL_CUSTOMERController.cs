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
    public class HOTEL_CUSTOMERController : Controller
    {
        private Entities db = new Entities();
        static bool[] ascMsg = new bool[7] { true, true, true, true, true, true, true };
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
        public ActionResult Index(int? page)
        {
            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

            var cus = from c in db.HOTEL_CUSTOMER select c;
            cus = cus.OrderBy(x => x.CUS_ID);
            IPagedList<HOTEL_CUSTOMER> cust = cus.ToPagedList(pagenumber, pageSize);
            

            ViewBag.SelCat = "customer";
            //return View(db.HOTEL_CUSTOMER.ToList());
            return View(cust);
        }



        [HttpPost]
        public ActionResult Index(String searchString)
        {
            ViewBag.SelCat = "customer";
            if (searchString == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var result = from a in db.HOTEL_CUSTOMER
                         where a.TEL_NUMBER == searchString || a.NAME == searchString|| a.CUS_ID == searchString
                         select a;
           
            if (result.Count() != 0)
                return View(result.ToList());
            else
                return RedirectToAction("Error");

        }
        [HttpPost]
        public ActionResult Sort(string message,int? page)
        {
            int pagenumber = page ?? 1;
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

            var sort_employee = db.HOTEL_CUSTOMER;

           

            IOrderedQueryable<HOTEL_CUSTOMER> result = null;
            if (message == "CUS_ID")
            {
                if (ascMsg[0])
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.CUS_ID ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.CUS_ID descending
                             select a;
                }
                ascMsg[0] = !ascMsg[0];

            }
            else if (message == "NAME")
            {
                if (ascMsg[1])
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.NAME ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.NAME descending
                             select a;
                }
                ascMsg[1] = !ascMsg[1];
            }
            else if (message == "REPUTATION")
            {
                if (ascMsg[2])
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.REPUTATION ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.REPUTATION descending
                             select a;
                }
                ascMsg[2] = !ascMsg[2];
            }
            else if (message == "CUS_TEL")
            {
                if (ascMsg[3])
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.TEL_NUMBER ascending
                             select a;
                }
                else
                {
                    result = from a in db.HOTEL_CUSTOMER
                             orderby a.TEL_NUMBER descending
                             select a;
                }
                ascMsg[3] = !ascMsg[3];
            }


            if (result != null && result.Count() != 0)
            {
                IPagedList<HOTEL_CUSTOMER> cust = result.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }
                
            else
            {
                IPagedList<HOTEL_CUSTOMER> cust = sort_employee.ToPagedList(pagenumber, pageSize);
                return View(cust);
            }
                


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
