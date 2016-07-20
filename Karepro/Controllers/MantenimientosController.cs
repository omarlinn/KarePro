using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Karepro.Models;

namespace Karepro.Controllers
{
    public class MantenimientosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Mantenimientos
        public ActionResult Index()
        {
            var mantenimientos = db.Mantenimientos.Include(m => m.Averia).Include(m => m.Institucion);
            return View(mantenimientos.ToList());
        }

        // GET: Mantenimientos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimiento mantenimiento = db.Mantenimientos.Find(id);
            if (mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(mantenimiento);
        }

        // GET: Mantenimientos/Create
        public ActionResult Create()
        {
            ViewBag.IdAveria = new SelectList(db.Averias, "IdAveria", "Tipo_averia");
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre");
            return View();
        }

        // POST: Mantenimientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMantenimiento,IdAveria,Tipo_mantenimiento,Nivel_urgencia,Descripcion,IdInstitucion")] Mantenimiento mantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.Mantenimientos.Add(mantenimiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAveria = new SelectList(db.Averias, "IdAveria", "Tipo_averia", mantenimiento.IdAveria);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre", mantenimiento.IdInstitucion);
            return View(mantenimiento);
        }

        // GET: Mantenimientos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimiento mantenimiento = db.Mantenimientos.Find(id);
            if (mantenimiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAveria = new SelectList(db.Averias, "IdAveria", "Tipo_averia", mantenimiento.IdAveria);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre", mantenimiento.IdInstitucion);
            return View(mantenimiento);
        }

        // POST: Mantenimientos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMantenimiento,IdAveria,Tipo_mantenimiento,Nivel_urgencia,Descripcion,IdInstitucion")] Mantenimiento mantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mantenimiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAveria = new SelectList(db.Averias, "IdAveria", "Tipo_averia", mantenimiento.IdAveria);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre", mantenimiento.IdInstitucion);
            return View(mantenimiento);
        }

        // GET: Mantenimientos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimiento mantenimiento = db.Mantenimientos.Find(id);
            if (mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(mantenimiento);
        }

        // POST: Mantenimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mantenimiento mantenimiento = db.Mantenimientos.Find(id);
            db.Mantenimientos.Remove(mantenimiento);
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
