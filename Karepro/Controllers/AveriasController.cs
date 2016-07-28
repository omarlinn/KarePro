using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Karepro.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Net.Mail;


namespace Karepro.Controllers
{
    public class SendMailController : Controller
    {
        // GET: SendMail
        public ActionResult Index()
        {

            return View();
        }
        public bool send(string user, string pass, string host, int port, string toEmail, string subject, string body)
        {
            try
            {
                using (var stmClient = new SmtpClient())
                {
                    stmClient.EnableSsl = true;
                    stmClient.Host = host;
                    stmClient.Port = port;
                    stmClient.UseDefaultCredentials = true;
                    stmClient.Credentials = new NetworkCredential(user, pass);
                    var msg = new MailMessage
                    {

                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(user),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal



                    };
                    msg.To.Add(toEmail);
                    stmClient.Send(msg);
                    return true;

                }






            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}






namespace Karepro.Controllers
{
    [Authorize]
    public class AveriasController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            //var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if (User.IsInRole("Administrador"))
            {
                var averias = from averia in db.Averias
                              where averia.EstadoAveria == "No Resuelta"
                              orderby averia.Nivel_urgencia.IdUrgencia descending
                              select averia;

                return View(averias.ToList());
            }

            if (User.IsInRole("Tecnico"))
            {
                var averias = from a in db.Averias
                              where a.IdTecnico == userId
                              select a;


                return View(averias.ToList());
            }

            var ave = from misAverias in db.Averias
                              join equipo in db.Equipos
                              on misAverias.IdEquipo equals equipo.IdEquipo
                              where equipo.IdUsuario == userId
                              orderby misAverias.Nivel_urgencia.Nivel descending
                              select misAverias;

                return View(ave.ToList());
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


        public ActionResult Asignar(int id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var tenicosId = roleManager.FindByName("Tecnico").Users;
            var tecnicos = new List<ApplicationUser>();

            tenicosId.ToList().ForEach(e => tecnicos.Add(db.Users.Find(e.UserId)));
            ViewBag.IdTecnico = new SelectList(tecnicos, "Id", "UserName");
           // ViewBag.IdAveria = new SelectList(db.Averias.Find(id), "IdAveria", "IdAveria");

            var averia = db.Averias.Find(id);

            return View(new AsignarAveriaViewModel { idAveria = id, Averia = averia});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Asignar(AsignarAveriaViewModel model)
        {

            var averia = db.Averias.Find(model.idAveria);
            if (ModelState.IsValid)
            {

                averia.IdTecnico = model.idTecnico;
                var email = db.Users.Find(model.idTecnico).Email;
                enviarMensajeATenicos(averia, email);

                db.Entry(averia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var tenicosId = roleManager.FindByName("Tecnico").Users;
            var tecnicos = new List<ApplicationUser>();
            var averias = new List<Averia>();
            averias.Add(averia);

            tenicosId.ToList().ForEach(e => tecnicos.Add(db.Users.Find(e.UserId)));
            ViewBag.idTecnico = new SelectList(tecnicos, "Id", "UserName");
            ViewBag.idAveria = new SelectList(averias, "IdAveria", "Descripcion");

            return View(new AsignarAveriaViewModel { idAveria = averia.IdAveria, Averia = averia });

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
                var estado = db.EstadoAveria.Where(e => e.Estado == "No Resuelta").FirstOrDefault();
                averia.EstadoAveria = "No Resuelta";
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

            var equipoReportado = (from equipo in db.Equipos 
                                  where equipo.IdEquipo == averia.IdEquipo
                                  select equipo).FirstOrDefault();

            string use = "elleurielmenor12@gmail.com"; //Aqui va el usuario logeado, solo que el correo debe existir para que se pueda enviar el msj
            string pass = "michael1922"; //El pass no deberia estar escrito aqui, luego implementamos cuestiones de seguridad
            string host = "smtp.gmail.com";
            int port = 25;

            string nombreEncargadoEquipo = equipoReportado.Usuario.Name + equipoReportado.Usuario.LastName;
            
            //Cuando se logre saber los tecnicos de una empresa el mismo correo se enviara a cu de ellos
            string subject = "KarePro, Reporte de averias";

            string body = string.Format("Reporte de averias, KarePro. <br>Distinguido administrador, lamentamos decirle "
                + "el equipo {0} del Usuario {1} ha sido reportado con una averia asi que le pedimos asignar esta averia"
                + " a un técnico lo mas rapido posible. <br><strong>Descripcion problema: </strong><br>{2}", equipoReportado.Nombre, nombreEncargadoEquipo, averia.Descripcion);

           SendMailController email = new SendMailController(); //Esta clase gestiona la config necesaria para enviar msj
           
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var tenicos = roleManager.FindByName("Administrador").Users; //Devuelve todos los usuarios con el rol tecnico

            //Enviar el correo a todos los tecnicos
            tenicos.ToList().ForEach(t => 
                email.send(use, pass, host, port, db.Users.Find(t.UserId).Email, subject, body)
            ); 
        }

        public void enviarMensajeATenicos(Averia averia, string emailTecnico)
        {
            //Este metodo gestiona el content del mensaje de las averias registradas

            var equipoReportado = (from equipo in db.Equipos
                                   where equipo.IdEquipo == averia.IdEquipo
                                   select equipo).FirstOrDefault();

            string use = "elleurielmenor12@gmail.com"; //Aqui va el usuario logeado, solo que el correo debe existir para que se pueda enviar el msj
            string pass = "michael1922"; //El pass no deberia estar escrito aqui, luego implementamos cuestiones de seguridad
            string host = "smtp.gmail.com";
            int port = 25;

            string nombreEncargadoEquipo = equipoReportado.Usuario.Name + equipoReportado.Usuario.LastName;

            //Cuando se logre saber los tecnicos de una empresa el mismo correo se enviara a cu de ellos
            string subject = "KarePro, Reporte de averias";

            string body = string.Format("Reporte de averias, KarePro. <br>Distinguido Tecnico, lamentamos decirle "
                + "el equipo {0} del Usuario {1} ha sido reportado con una averia asi que le pedimos resolver esta averia"
                + " a un técnico lo mas rapido posible. <br><strong>Descripcion problema: </strong><br>{2}", equipoReportado.Nombre, nombreEncargadoEquipo, averia.Descripcion);

            SendMailController email = new SendMailController(); //Esta clase gestiona la config necesaria para enviar msj
            email.send(use, pass, host, port, emailTecnico, subject, body);
            
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



    public class SendMailController : Controller
    {
        // GET: SendMail
        public ActionResult Index()
        {

            return View();
        }
        public bool send(string user, string pass, string host, int port, string toEmail, string subject, string body)
        {
            try
            {
                using (var stmClient = new SmtpClient())
                {
                    stmClient.EnableSsl = true;
                    stmClient.Host = host;
                    stmClient.Port = port;
                    stmClient.UseDefaultCredentials = true;
                    stmClient.Credentials = new NetworkCredential(user, pass);
                    var msg = new MailMessage
                    {

                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(user),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal



                    };
                    msg.To.Add(toEmail);
                    stmClient.Send(msg);
                    return true;

                }


            }
            catch (Exception e)
            {
                return false;
            }

        }
    }

