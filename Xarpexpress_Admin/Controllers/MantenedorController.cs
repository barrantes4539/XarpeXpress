using Business;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xarpexpress_Admin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Index()
        {
            return View();
        }

        //Falta hacerle la vista a Usuarios
        public ActionResult Usuarios()
        {
            return View();
        }

  
    }
}