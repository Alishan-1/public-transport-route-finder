using BusApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BusApp.Controllers
{
    public class FindRouteController : Controller
    {
        private PublicTransportRouteFinderEntities db = new PublicTransportRouteFinderEntities();
        // GET: FindRouteInput
        public ActionResult Index()
        {
            ViewBag.Error = "";
            var stops = db.Stops.ToList();
            ViewBag.stops = stops;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string start, string dest)
        {
            ViewBag.Error = "";
            var stops = db.Stops.ToList();
            ViewBag.stops = stops;
            if (string.IsNullOrWhiteSpace( start))
            {
                ViewBag.Error = "Select Starting Stop Correctly";
                return View();
            }
            if (string.IsNullOrWhiteSpace(dest))
            {
                ViewBag.Error = "Select Destination Stop Correctly";
                return View();
            }
            FindRouteInput input = new FindRouteInput();
            input.startPoint = db.Stops.Where(x => x.Name == start).FirstOrDefault();
            if (input.startPoint == null)
            {
                ViewBag.Error = "Select Starting Stop Correctly";
                return View();
            }
            input.destinationPoint = db.Stops.Where(x => x.Name == dest).FirstOrDefault();
            if (input.destinationPoint == null)
            {
                ViewBag.Error = "Select Destination Stop Correctly";
                return View();
            }
            RouteFinder finder = new RouteFinder();
            List< RouteSearchResult> r = finder.find(input);
            
            
            return Json(r, JsonRequestBehavior.AllowGet);
            
        }

        // GET: FindRouteInput/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FindRouteInput/Create
        public ActionResult Create()
        {
            return View("dhgdhg");
        }

        // POST: FindRouteInput/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FindRouteInput/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FindRouteInput/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FindRouteInput/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FindRouteInput/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
