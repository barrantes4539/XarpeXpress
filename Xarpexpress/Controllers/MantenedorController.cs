using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xarpexpress.Controllers
{

    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Pagar()
        {
            return View();
        }

        public ActionResult ProductosAdmin()
        {
            return View();
        }
    }
}