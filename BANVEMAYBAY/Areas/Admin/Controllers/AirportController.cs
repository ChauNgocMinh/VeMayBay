using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVeMayBay.Models;
using BanVeMayBay.Common;
using BanVeMayBay.Models;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class AirportController : BaseController
    {
        private BanVeMayBayEntities db = new BanVeMayBayEntities();
        // GET: Admin/Tickets
        public ActionResult Index()
        {
            var airports = db.airports.ToList();
            ViewBag.airports = airports;
            return View();
        }


        // GET: Admin/Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Admin/Tickets/Create
        public ActionResult Create()
        {
            var countries = db.countries.Where(m =>m.status == 1).ToList();
            ViewBag.countries = countries;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(country countries)
        {           
            //if(db.countries.Where(m=>m.countryName.Contains(countries.countryName)) == null)
            //    {
                    db.countries.Add(countries);
                    Message.set_flash("Successfully added Country", "success");
                    db.SaveChanges();
                    return RedirectToAction("Index");
                //}
                //else
                //{
                //    Message.set_flash("Country already exist", "danger");
                //    return View("Create");
                //}

            
        }

        //GET: Admin/Tickets/Create
        public ActionResult CreateAirports()
        {
            var countries = db.countries.Where(m=>m.status == 1).ToList();
            ViewBag.countries = countries;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAirports(airport airports)
        {

            if (ModelState.IsValid)
            {
                db.airports.Add(airports);
                Message.set_flash("Successfully added airport", "success");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Message.set_flash("More failed airports", "danger");
            return View("Create");
        }


        public ActionResult Status(int id)
        {
            country countries = db.countries.Find(id);
            countries.status = (countries.status == 1) ? 2 : 1;
            db.Entry(countries).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Status change successful", "success");
            return RedirectToAction("Index");
        }
        //trash
        //public ActionResult trash()
        //{
        //    var list = db.countries.Where(m => m.status == 2).ToList();
        //    return View("Trash", list);
        //}
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Deltrash(int id)
        {
            country morder = db.countries.Find(id);
            morder.status = 2;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Delete successfully", "success");
            return RedirectToAction("Index");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Retrash(int id)
        {
            country morder = db.countries.Find(id);
            morder.status = 1;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Successful recovery", "success");
            return RedirectToAction("trash");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult deleteTrash(int id)
        {
            country morder = db.countries.Find(id);
            db.countries.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Permanently deleted 1 Order", "success");
            return RedirectToAction("trash");
        }


    }
}
