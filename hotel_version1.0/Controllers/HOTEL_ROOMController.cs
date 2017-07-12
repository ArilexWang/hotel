using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_version1._0.Controllers
{
    public class HOTEL_ROOMController : Controller
    {
        private Entities db = new Entities();
        // GET: Test


        public ActionResult Index()
        {
            ViewBag.SelCat = "room";
            var str = new List<string>();
            var date = new List<string>();
            var strQuery = from a in db.HOTEL_ROOM
                           orderby a.HOTEL_ID
                           select a.HOTEL_ID;

            for (int i = 0; i < 7; i++)
                date.Add(System.DateTime.Now.AddDays(i).ToShortDateString());
            str.AddRange(strQuery.Distinct());

            ViewBag.str = new SelectList(str);
            ViewBag.date = new SelectList(date);



            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            var hotel_id = Request.Form["str"];
            var selectDate = Request.Form["date"];

            var result = from a in db.HOTEL_ROOM
                         where a.HOTEL_ID == hotel_id
                         orderby a.ROOM_NUM
                         select new { a.ROOM_NUM };


            var deleteList = from a in result
                             join b in db.HOTEL_OCCUPIED on a.ROOM_NUM equals b.ROOM_NUM
                             where b.HOTEL_ID == hotel_id
                             orderby a.ROOM_NUM
                             select new { a.ROOM_NUM, b.PRE_DATE };


            var all = result.ToList();
            var temp = result.ToList();

            var delete = deleteList.ToList();
            for (int i = 0; i < delete.Count(); i++)
            {
                if (delete[i].PRE_DATE.ToShortDateString() == selectDate)
                {
                    for (int j = 0; j < temp.Count(); j++)
                    {
                        if (temp[j].ROOM_NUM == delete[i].ROOM_NUM)
                        {
                            temp.RemoveAt(j);
                            break;
                        }
                    }
                }
            }

            string requiredStr = "";
            foreach (var x in all)
            {
                bool flag = false;
                foreach (var y in temp)
                    if (x.ROOM_NUM == y.ROOM_NUM)
                    {
                        requiredStr += "1";
                        flag = true;
                        break;
                    }
                if (flag == false)
                {
                    requiredStr += "2";
                    flag = false;
                }

            }

            var str = new List<string>();
            var date = new List<string>();
            var strQuery = from a in db.HOTEL_ROOM
                           orderby a.HOTEL_ID
                           select a.HOTEL_ID;

            for (int i = 0; i < 7; i++)
                date.Add(System.DateTime.Now.AddDays(i).ToShortDateString());
            str.AddRange(strQuery.Distinct());

            ViewBag.str = new SelectList(str);
            ViewBag.date = new SelectList(date);
            ViewBag.requiredStr = requiredStr;
            return View();
        }


        public String HelloWorld(FormCollection form)
        {
            return "HelloWorld";
        }

        [HttpPost]
        public ActionResult Lu(FormCollection form, string myButton)
        {
            var buttonId = Request.Form["myButton"];
            ViewBag.button = buttonId;

            return View();
        }
    }
}