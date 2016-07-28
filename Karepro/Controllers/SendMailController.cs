using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Karepro.Models;
using System.Net;
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