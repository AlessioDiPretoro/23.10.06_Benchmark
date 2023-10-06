using Albergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Albergo.Controllers
{
    //gestisce le operazioni fattibili dal dipendente (sorta di menù per i dipendenti)
    [Authorize]
    public class UtentiController : Controller
    {
        // GET: Utenti
        public ActionResult UtentiHome()
        {
            return View();
        }

        public JsonResult CercaDaCodFisc(string inputVal)
        {
            List<Prenotazioni> prenotazioneCF = Prenotazioni.GetReservationsByCodiFisc(inputVal);

            return Json(prenotazioneCF, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PensioneCompleta()
        {
            List<Prenotazioni> pensioneC = Prenotazioni.GetReservationsComplete();

            return Json(pensioneC, JsonRequestBehavior.AllowGet);
        }
    }
}