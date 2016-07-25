using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Karepro.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Karepro.Controllers
{
    [Authorize]
    public class AveriasController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if (um.IsInRole(userId, "Administrador") || um.IsInRole(userId, "Tecnico"))
            {
                return View(db.Averias.ToList());
            }else{
                var averias = from misAverias in db.Averias
                              join equipo in db.Equipos
                              on misAverias.IdEquipo equals equipo.IdEquipo
                              where equipo.IdUsuario == userId
                              select misAverias;

                return View(averias.ToList());
            }
        }

       
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

       
        public ActionResult Create()
        {

            var userId = User.Identity.GetUserId();
            var misEquipos = from equipo in db.Equipos
                             where equipo.IdUsuario == userId
                             select equipo;


            ViewBag.IdEquipo = new SelectList(misEquipos, "IdEquipo", "Nombre");
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre");
            ViewBag.IdUrgencia = new SelectList(db.NivelUrgencia, "IdUrgencia", "Nivel");
            return View();
        }

  
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

            var userId = User.Identity.GetUserId();
            var misEquipos = from equipo in db.Equipos
                             where equipo.IdUsuario == userId
                             select equipo;

            ViewBag.IdEquipo = new SelectList(misEquipos, "IdEquipo", "Nombre");
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
