using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Karepro.Models;
using Karepro.Controllers;
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
                enviarMensaje(averia);  //Cuando se reporta la averia ese reporte se envia por msj a los tecnicos a traves de este metodo
               
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

        public void enviarMensaje(Averia averia)
        {
            //Este metodo gestiona el content del mensaje de las averias registradas

            var equipoReportado = from equipo in db.Equipos //Resivar esto
                                  where equipo.IdEquipo == averia.IdEquipo
                                  select equipo.Nombre;



            string use = "elleurielmenor12@gmail.com"; //Aqui va el usuario logeado, solo que el correo debe existir para que se pueda enviar el msj
            string pass = "michael1922"; 
            string host = "smtp.gmail.com";
            int port = 25;

            int idEquipo = averia.IdEquipo; //Realmente lo que se quiere es el nombre del equipo, se puso esto como prueba ya que el linq no me funciona 

            string toEmail = "20142469@itla.edu.do";
            //Cuando se logre saber los tecnicos de una empresa el mismo correo se enviara a cu de ellos
            string subject = "KarePro, Reporte de averias";

            string body = string.Format("Reporte de averias, KarePro. <br>Distinguido tecnico, lamentamos decirle "
                + "el equipo {0} del Se;or: {1} ha sido reportado con una averia asi que le pedimos pasar a revisar"
                + " lo mas rapido posible. <br><strong>Descripcion problema: </strong><br>{2}", idEquipo, "NOMBRE DEL TECNICO", averia.Descripcion);

            SendMailController email = new SendMailController(); //Esta clase gestiona la config necesaria para enviar msj

            bool msg = email.send(use, pass, host, port, toEmail, subject, body);
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
