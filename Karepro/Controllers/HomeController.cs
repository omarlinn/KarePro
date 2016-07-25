using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karepro.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Homr
        public ActionResult Index()
        {
            return View();
        }
    }
}