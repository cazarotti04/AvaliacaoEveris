using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AvaliacaoEverisAPI.Controllers
{
    public class HomeController : Controller
    {

        /**
        *   @brief Exibe a view inicial do sistema
        *
        *   @return Exibição da página Index
        */
        public ActionResult Index()
        {
            return View();
        }

    }
}