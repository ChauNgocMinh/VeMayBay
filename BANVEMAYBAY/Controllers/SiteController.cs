using BanVeMayBay.Models;
using BanVeMayBay.Common;
using BanVeMayBay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BanVeMayBay.Controllers
{
    public class SiteController : Controller
    {
        BanVeMayBayEntities db = new BanVeMayBayEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]        
        public ActionResult flightSearch(FormCollection fc, int? page)
        {
            
            string typeTicket = fc["typeticket"];
            if (page == null) 
            { 
                page = 1; 
            }
            int pageSize = 4;
          
            int songuoi1 = int.Parse(fc["songuoi1"]);
            int songuoi2 = int.Parse(fc["songuoi2"]);
            int songuoi3 = int.Parse(fc["songuoi3"]);
            int tong = songuoi1 + songuoi2 + songuoi3;
            int songuoi = tong;
            ViewBag.songuoi = songuoi;
            string noiBay = fc["departure_address"];
            string noiVe = fc["arrival_address"];
            string ngaybay = fc["departure_date"];
           
            ViewBag.url = "chuyen-bay";
            //convert sang mm/dd/yy cho may hieu 
            DateTime ngaybay1 = DateTime.ParseExact(ngaybay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //sang mm/dd/yy
            string ngaybay2 = ngaybay1.ToString("MM-dd-yyyy");
            DateTime ngaybay3 = DateTime.Parse(ngaybay2);
            ViewBag.noiBay = noiBay;
            ViewBag.noiVe = noiVe;
            ViewBag.ngaybay = ngaybay;
           
            // neu la ve 2 chieu
            if (typeTicket.Equals("enable"))
            {
                string ngayve = fc["arrival_date"];
                DateTime ngayden1 = DateTime.ParseExact(ngayve, "d/M/yyyy", CultureInfo.InvariantCulture);
                string ngayden2 = ngayden1.ToString("MM-dd-yyyy");
                DateTime ngayden3 = DateTime.Parse(ngayden2);
                ViewBag.ngayden = ngayve;
                ViewBag.date = ngayden3;

                if (ngaybay1 > ngayden1)
                {
                    Message.set_flash("Return date must be greater than or equal to departure date!", "danger");
                    return Redirect("~/Home/Index");
                }
                var list = db.tickets.Where(m => m.airport.airportName.Contains(noiBay) && m.airport1.airportName.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m => m.status == 1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchReturn", list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //ve 1 chieu
                var list = db.tickets.Where(m => m.airport.airportName.Contains(noiBay) && m.airport1.airportName.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m=>m.status==1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchOnway", list.ToPagedList(pageNumber, pageSize));
            
            }

        }
        
        public ActionResult return_ticket(DateTime date,string noibay, string noiden)
        {
           
            var list = db.tickets.Where(m => m.airport.airportName.Contains(noiden) && m.airport1.airportName.Contains(noibay)).
               Where(m => m.departure_date == date).Where(m => m.status == 1).ToList();
            return View("_returnTicket", list);
        }
        public ActionResult AllChuyenBay(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            var singleC = db.topics.Where(m => m.status == 1).Where(m => m.status == 1).First();
            ViewBag.url = "all-chuyen-bay";
            int pageNumber = (page ?? 1);
            //không biết sử dụng
            //ViewBag.breadcrumb = "Tất cả chuyến bay";//không biết sử dụng
            var list_flight = db.tickets.Where(m => m.status == 1).ToList();
            return View("allflight", list_flight.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult postOftoPic(int? page, string slug)
        {
            if (page == null) page = 1;
            int pageSize = 4;
            var singleC = db.topics.Where(m => m.status == 1 && m.slug == slug).Where(m => m.status == 1).First();
            ViewBag.nameTopic = slug;
            ViewBag.url = "tin-tuc/" + slug + "";
            int pageNumber = (page ?? 1);
            var listPost = db.posts.Where(m => m.status == 1 && m.topid == singleC.ID).OrderByDescending(m => m.ID).ToList();
            return View("postOftoPic", listPost.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CancelTicket(int id)
        {
            var order = db.orders.FirstOrDefault(o => o.ID == id);
            if (order != null && order.status == 0) 
            {
                order.status = 2;  
                db.SaveChanges();
            }
            return RedirectToAction("OrderDetail", "Site", new { id = id });
        }


        public ActionResult topic()
        {

            var list = db.topics.Where(m => m.status == 1).Where(m => m.status == 1).ToList();
            return View("_topic", list);
        }

        public ActionResult postSearch(string keyw, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.url = "tim-kiem-bai-viet?keyw=" + keyw + "";
            @ViewBag.nameTopic = "Tim kiếm từ khóa: " + keyw;
            var list = db.posts.Where(m => m.title.Contains(keyw) || m.detail.Contains(keyw)).Where(m => m.status == 1).OrderBy(m => m.ID);
            return View("postOftoPic", list.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PostDetal(string slug)
        {

            var single = db.posts.Where(m => m.status == 1 && m.slug == slug).First();
            ViewBag.Bra = single.title;
            return View("PostDetal", single);
        }
        
        public ActionResult flightDetail(int id)
        {
            var single = db.tickets.Where(m => m.status == 1 && m.id == id).First();
            return View("flightDetail", single);
        }
        public ActionResult lienHe()
        {
            var single = db.posts.Where(m => m.status == 1 && m.slug.Equals("Liên Hê")).First();
            return View("PostDetal", single);
        }

        public ActionResult CreateOrder(int ticketId)
        {
            var ticket = db.tickets.FirstOrDefault(t => t.status == 1 && t.id == ticketId);
            if (ticket == null)
            {
                return HttpNotFound("Ticket not found or invalid status.");
            }
            ticket.Sold = ticket.Sold + 1;
            var userId = Session["id"] as int?;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Customer");
            }

            var order = new order
            {
                CusId = userId.Value,
                total = ticket.price ?? 0,
                created_at = DateTime.Now,
                status = 0,
                ticketId = ticket.id,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        ticketId = ticket.id
                    }
                }
            };
            return View(order);
        }


        [HttpPost]
        public ActionResult ConfirmOrder(order model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateOrder", model);
            }

            var userId = (int?)Session["id"];
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Customer");
            }

            model.CusId = userId.Value;
            model.created_at = DateTime.Now;
            model.status = 0;

            foreach (var orderDetail in model.OrderDetails)
            {
                orderDetail.orderId = model.ID;
                var existingTicket = db.tickets.Find(orderDetail.ticketId);

                orderDetail.ticketId = orderDetail.ticketId; 
                orderDetail.ticket = existingTicket;
                //db.OrderDetails.Add(orderDetail);
            }
            db.orders.Add(model);
            db.SaveChanges();
            ViewBag.OrderId = model.ID;
            ViewBag.TotalAmount = model.total;
            return View();
        }

        public ActionResult OrderDetail(int id)
        {
            if (Session["id"] == null || !int.TryParse(Session["id"].ToString(), out int userId))
            {
                return RedirectToAction("Login", "Customer");
            }

            var order = db.orders
                .Include(o => o.OrderDetails.Select(od => od.ticket))
                .Include(o => o.OrderDetails.Select(od => od.ticket.airport))
                .Include(o => o.OrderDetails.Select(od => od.ticket.airport1))
                .FirstOrDefault(o => o.ID == id && o.CusId == userId);

            if (order == null)
            {
                return HttpNotFound("Order not found or you do not have permission to view this order.");
            }

            ViewBag.FlightCode = order.OrderDetails.FirstOrDefault()?.ticket?.flightCode ?? "N/A";
            ViewBag.Airline = order.OrderDetails.FirstOrDefault()?.ticket?.airline ?? "N/A";
            ViewBag.AirportName = order.OrderDetails.FirstOrDefault()?.ticket?.airport?.airportName ?? "N/A";
            ViewBag.Airport1Name = order.OrderDetails.FirstOrDefault()?.ticket?.airport1?.airportName ?? "N/A";

            return View(order);
        }

    }
}