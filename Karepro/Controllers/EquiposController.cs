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
    public class EquiposController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Equipos
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if (um.IsInRole(userId, "Administrador") || um.IsInRole(userId, "Tecnico"))
            {
                return View(db.Equipos.ToList());
            }
            else
            {
                var equipos = from misEquipos in db.Equipos
                              where misEquipos.IdUsuario == userId
                              select misEquipos;

                return View(equipos.ToList());
            }
        }

        // GET: Equipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = db.Equipos.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        // GET: Equipos/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Users, "Id", "Email");
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre");
            return View();
        }

        // POST: Equipos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEquipo,Nombre,Descripcion,Perio_mantenimiento,IdUsuario,IdInstitucion")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Equipos.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Users, "Id", "UserName", equipo.IdUsuario);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre");
            return View(equipo);
        }

        // GET: Equipos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = db.Equipos.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.Users, "Id", "Email", equipo.IdUsuario);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre");
            return View(equipo);
        }

        // POST: Equipos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEquipo,Nombre,Descripcion,Perio_mantenimiento,IdUsuario,Cod_inst")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.Users, "Id", "Email", equipo.IdUsuario);
            ViewBag.IdInstitucion = new SelectList(db.Instituciones, "IdInstitucion", "Nombre");
            return View(equipo);
        }

        // GET: Equipos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = db.Equipos.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipo equipo = db.Equipos.Find(id);
            db.Equipos.Remove(equipo);
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
