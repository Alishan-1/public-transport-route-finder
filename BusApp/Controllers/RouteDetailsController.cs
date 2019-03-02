using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusApp.Models;

namespace BusApp.Controllers
{
    [Authorize]
    public class RouteDetailsController : Controller
    {
        private readonly PublicTransportRouteFinderEntities db = new PublicTransportRouteFinderEntities();
        [AllowAnonymous]
        // GET: RouteDetails
        public ActionResult Index()
        {
            var routeDetails = db.RouteDetails.Include(r => r.Route).Include(r => r.Stop);
            return View(routeDetails.ToList());
        }
        [AllowAnonymous]
        // GET: RouteDetails/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteDetail routeDetail = db.RouteDetails.Find(id);
            if (routeDetail == null)
            {
                return HttpNotFound();
            }
            return View(routeDetail);
        }

        // GET: RouteDetails/Create
        public ActionResult Create()
        {
            ViewBag.RouteID = new SelectList(db.Routes, "ID", "Name");
            ViewBag.StopID = new SelectList(db.Stops, "ID", "Name");
            return View();
        }

        // POST: RouteDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RouteID,StopNo,StopID,DistanceFromPrevStop")] RouteDetail routeDetail)
        {
            if (ModelState.IsValid)
            {
                db.RouteDetails.Add(routeDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RouteID = new SelectList(db.Routes, "ID", "Name", routeDetail.RouteID);
            ViewBag.StopID = new SelectList(db.Stops, "ID", "Name", routeDetail.StopID);
            return View(routeDetail);
        }

        // GET: RouteDetails/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteDetail routeDetail = db.RouteDetails.Find(id);
            if (routeDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.RouteID = new SelectList(db.Routes, "ID", "Name", routeDetail.RouteID);
            ViewBag.StopID = new SelectList(db.Stops, "ID", "Name", routeDetail.StopID);
            return View(routeDetail);
        }

        // POST: RouteDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RouteID,StopNo,StopID,DistanceFromPrevStop")] RouteDetail routeDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routeDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RouteID = new SelectList(db.Routes, "ID", "Name", routeDetail.RouteID);
            ViewBag.StopID = new SelectList(db.Stops, "ID", "Name", routeDetail.StopID);
            return View(routeDetail);
        }

        // GET: RouteDetails/Delete/5
        public ActionResult Delete(decimal id)
        {
            
            RouteDetail routeDetail = db.RouteDetails.Find(id);
            if (routeDetail == null)
            {
                return HttpNotFound();
            }
            return View(routeDetail);
        }

        // POST: RouteDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            RouteDetail routeDetail = db.RouteDetails.Find(id);
            db.RouteDetails.Remove(routeDetail);
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
