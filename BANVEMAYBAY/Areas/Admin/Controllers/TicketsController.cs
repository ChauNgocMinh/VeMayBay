﻿using System;
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

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class TicketsController : BaseController
    {
        private BanVeMayBayEntities db = new BanVeMayBayEntities();

        // GET: Admin/Tickets
        public ActionResult Index()
        {
            
            var tickets = db.tickets.Include(x => x.airport).ToList();
            ViewBag.tickets = tickets;
            
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
            var airports = db.airports.ToList();
            ViewBag.airports = airports;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ticket ticket)
        {
            //ticket.flightCode = "NB_"+ticket.departure_date;
            ticket.img = "img.jpg";
            ticket.Sold = 0;
            if (ModelState.IsValid)
            {
                //HttpPostedFileBase file;
                //file = Request.Files["airline"];
                //string filename = "";
               // string ExtensionFile = Mystring.GetFileExtension(filename);
               // string namefilenew = Mystring.ToSlug(ticket.departure_date.Year.ToString())+ "." + ExtensionFile;
                //var path = Path.Combine(Server.MapPath("~/Public/images/flight"), filename);
                //file.SaveAs(path);
                ticket.airline = "";
                ticket.created_at = DateTime.Now;
                ticket.updated_at = DateTime.Now;
                ticket.created_by = int.Parse(Session["Admin_id"].ToString());
                ticket.updated_by = int.Parse(Session["Admin_id"].ToString());
                ticket.priceSale = ticket.price;
             
                db.tickets.Add(ticket);
                Message.set_flash("Successfully added ticket", "success");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Message.set_flash("More failed tickets", "danger");
            return View("Create");
        }
        

        public ActionResult Status(int id)
        {
            ticket tickets = db.tickets.Find(id);
            tickets.status = (tickets.status == 1) ? 2 : 1;
            db.Entry(tickets).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Status change successful", "success");
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.tickets.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Deltrash(int id)
        {
            ticket morder = db.tickets.Find(id);
           
            morder.status = 0;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Delete successfully", "success");
            return RedirectToAction("Index");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Retrash(int id)
        {
            ticket morder = db.tickets.Find(id);
            morder.status = 1;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Successful recovery", "success");
            return RedirectToAction("trash");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult deleteTrash(int id)
        {
            ticket morder = db.tickets.Find(id);
            db.tickets.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Permanently deleted 1 Order", "success");
            return RedirectToAction("trash");
        }
        // GET: Admin/Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            var countries = db.airports.ToList();
            ViewBag.airports = countries;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ticket mticket = db.tickets.Find(id);
            if (mticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.listticket = db.tickets.Where(m => m.status == 1).ToList();
            return View(mticket);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ticket mticket)
        {
            var countries = db.countries.ToList();
            ViewBag.countries = countries;
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file;
                file = Request.Files["airline"];
                string filename = "";
                if (filename.Equals("") == false)
                {
                   
                    var path = Path.Combine(Server.MapPath("~/Public/images/flight"), filename);
                 
                    file.SaveAs(path);
                    mticket.airline = filename;
                }
              
                  
                mticket.created_at = DateTime.Now;
                mticket.created_by = int.Parse(Session["Admin_id"].ToString());
                
                mticket.priceSale = mticket.price;
                mticket.updated_at = DateTime.Now;
                mticket.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.Entry(mticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listticket = db.tickets.Where(m => m.status ==1).ToList();
            return View(mticket);
        }


    }
}
