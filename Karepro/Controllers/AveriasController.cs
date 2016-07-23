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
    public class AveriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Averias
        public ActionResult Index()
        {
            var averias = db.Averias.Include(a => a.Equipo).Include(a => a.Institucion);
            return View(averias.ToList());
        }

        // GET: Averias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Averia averia = db.Averias.Find(id);
            if (averia == null)
            {
                return HttpNotFound();
            }
            return View(averia);
        }

        // GET: Averias/Create
        public ActionResult Create()
        {
            ViewBag.IdEquipo = new SelectList(db.Equipos, "IdEquipo", "Nombre");
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre");
            ViewBag.IdUrgencia = new SelectList(db.NivelUrgencia, "IdUrgencia", "Nivel");
            return View();
        }

        // POST: Averias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAveria,IdEquipo,Tipo_averia,IdUrgencia,Nivel_urgencia,Descripcion,IdInstitucion")] Averia averia)
        {
            if (ModelState.IsValid)
            {
                db.Averias.Add(averia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEquipo = new SelectList(db.Equipos, "IdEquipo", "Nombre", averia.IdEquipo);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre", averia.IdInstitucion);
            ViewBag.IdUrgencia = new SelectList(db.NivelUrgencia, "IdUrgencia", "Nivel", averia.IdUrgencia);
            return View(averia);
        }

        // GET: Averias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Averia averia = db.Averias.Find(id);
            if (averia == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEquipo = new SelectList(db.Equipos, "IdEquipo", "Nombre", averia.IdEquipo);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre", averia.IdInstitucion);
            ViewBag.IdUrgencia = new SelectList(db.NivelUrgencia, "IdUrgencia", "Nivel", averia.IdUrgencia);
            return View(averia);
        }

        // POST: Averias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAveria,IdEquipo,Tipo_averia,IdUrgencia,Nivel_urgencia,Descripcion,IdInstitucion")] Averia averia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(averia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdEquipo = new SelectList(db.Equipos, "IdEquipo", "Nombre", averia.IdEquipo);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre", averia.IdInstitucion);
            ViewBag.IdUrgencia = new SelectList(db.NivelUrgencia, "IdUrgencia", "Nivel", averia.IdUrgencia);
            return View(averia);
        }

        // GET: Averias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Averia averia = db.Averias.Find(id);
            if (averia == null)
            {
                return HttpNotFound();
            }
            return View(averia);
        }

        // POST: Averias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Averia averia = db.Averias.Find(id);
            db.Averias.Remove(averia);
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
