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

            ViewBag.hotelID = Request.Form["str"];
            ViewBag.dateTime = Request.Form["date"];
            return View();

        }




        [HttpPost]
        public ActionResult Lu(FormCollection form, string myButton, string hotelID, string dateTime)
        {

            var room_num = Request.Form["myButton"];

            var checkInList = new List<String>();
            var checkOutList = new List<String>();
            var statusList = new List<String>();


            for (int i = 0; i < 7; i++)
            {

                checkOutList.Add(System.DateTime.Now.AddDays(i).ToShortDateString());
            }


            statusList.Add("已入住");
            statusList.Add("已预约");


            ViewBag.checkInTime = dateTime;
            ViewBag.hotelID = hotelID;
            ViewBag.roomNum = room_num;
            ViewBag.checkOutList = new SelectList(checkOutList);
            ViewBag.statusList = new SelectList(statusList);


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(string id)
        {

            var checkInTime = Request.Form["checkInTime"];
            var checkOutTime = Request.Form["checkOutList"];
            var status = Request.Form["statusList"];
            var roomNum = Request.Form["roomNum"];
            var roomNumDec = Convert.ToDecimal(roomNum);
            var hotelID = Request.Form["hotelID"];
            var name = Request.Form["name"];
            var tel = Request.Form["tel"];
            var orderID = Request.Form["orderID"];
            var cusID = generateCustomerID();
            var checkInDateTime = Convert.ToDateTime(checkInTime);
            var checkOutDateTime = Convert.ToDateTime(checkOutTime);
            bool flag = false;
            if (DateTime.Compare(checkInDateTime, checkOutDateTime) > 0)
                flag = true;

            var result = from a in db.HOTEL_OCCUPIED
                         where a.HOTEL_ID == hotelID && a.ROOM_NUM == roomNumDec
                         select new { a.PRE_DATE };

            foreach (var x in result)
            {
                if (DateTime.Compare(x.PRE_DATE, checkInDateTime) >= 0 && DateTime.Compare(x.PRE_DATE, checkOutDateTime) <= 0)
                    flag = true;
            }

            if (flag == true)
            {
                ViewBag.error = "时间错误或房间不可预定";
                return View();
            }

            var newOrder = new HOTEL_ORDER() { CUS_ID = cusID, ORDER_ID = orderID, CHECK_IN_TIME = checkInDateTime, CHECK_OUT_TIME = checkOutDateTime, RESERVE_TIME = System.DateTime.Now, STATUS = status, PRE_ORDER_ID = null, ROOM_NUM = 101, HOTEL_ID = "100001" };
            var newCustomer = new HOTEL_CUSTOMER() { CUS_ID = cusID, TEL_NUMBER = tel, REPUTATION = "normal", NAME = name };

            var ts = checkOutDateTime - checkInDateTime;
            for (int i = 0; i < ts.TotalDays + 1; i++)
            {
                var roomOccupied = new HOTEL_OCCUPIED() { HOTEL_ID = hotelID, ROOM_NUM = roomNumDec, PRE_DATE = checkInDateTime.AddDays(i) };
                db.HOTEL_OCCUPIED.Add(roomOccupied);
            }

            db.HOTEL_ORDER.Add(newOrder);
            db.HOTEL_CUSTOMER.Add(newCustomer);

            db.SaveChanges();

            //catch(Exception )
            //{
            //    ViewBag.error = "失败";
            //    return View();
            //}
            ViewBag.error = "成功";
            return View();
        }





        private String generateCustomerID()
        {
            var days = DateTime.Now.AddYears(-1).DayOfYear.ToString();
            var min = DateTime.Now.Minute.ToString();
            var result = days + min;
            String finalResult = "3";
            for (int i = 1; i < result.Count(); i++)
            {
                finalResult += result[i];
            }

            return finalResult;
        }



        public ActionResult priceEdit(String id)
        {
            if (id == null)
            {
                return View();
            }
            var roomid = from a in db.HOTEL_ROOM
                         where a.ROOM_TYPE == id
                         select a;
            ViewBag.roomid = roomid.ToList();
            ViewBag.type = id;
            return View();
        }

        public ActionResult submit(string message)
        {
            string price = Request.Form["price"];
            int pr = Convert.ToInt32(price);
            var result = from a in db.HOTEL_ROOM
                         where a.ROOM_TYPE == message
                         select a;
            foreach (var item in result)
            {
                item.PRICE = pr;
            }
            db.SaveChanges();
            return RedirectToAction("Price");
        }

        public ActionResult Price()
        {
            ViewBag.SelCat = "roomType";
            var hOTEL_ROOM = from a in db.HOTEL_ROOM
                             orderby a.ROOM_TYPE, a.PRICE
                             select new { a.ROOM_TYPE, a.PRICE };
            var h2 = hOTEL_ROOM.Distinct(); h2.OrderBy(h => h.PRICE);
            List<string> room = new List<string>();
            List<decimal> price = new List<decimal>();
            foreach (var item in h2)
            {
                room.Add(item.ROOM_TYPE);
                price.Add((decimal)item.PRICE);
            }
            var hid = from a in db.HOTEL_ROOM
                      select new { a.HOTEL_ID, a.PRICE };
            hid.Distinct(); hid.OrderBy(a => a.HOTEL_ID);
            ViewBag.room = room; ViewBag.price = price;

            return View();

        }

    }
}