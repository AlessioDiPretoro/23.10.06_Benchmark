using Albergo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Albergo.Controllers
{
    public class ReservationController : Controller
    {
        public List<SelectListItem> ListaCamere
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>
                                {
                    new SelectListItem { Text = "--- Seleziona ---", Value = "0" },
                };
                List<Camere> listC = Camere.GetAllRooms();
                foreach (Camere c in listC)
                {
                    list.Add(new SelectListItem { Text = c.Descrizione, Value = c.Id.ToString() });
                }
                return list;
            }
        }

        [Authorize]
        public ActionResult AddReservation(int id)

        {
            TempData["id"] = id;
            ViewBag.Lc = ListaCamere;
            return View();
        }

        [HttpPost]
        public ActionResult AddReservation(Prenotazioni p)
        {
            int idCliente = Convert.ToInt16(TempData["id"]);
            p.DataPrenotazione = DateTime.Now;
            p.Anno = DateTime.Now.Year.ToString();
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Prenotazioni VALUES (@idCliente, @idCamera, @data, @anno, @periodoDal, @periodoAl, @caparra, @tariffa, @mezzaPensione, @pensioneCompleta, @primaColazione)", sqlConnection);
                    cmd.Parameters.AddWithValue("idCliente", idCliente);
                    cmd.Parameters.AddWithValue("idCamera", Convert.ToInt16(p.IdCamera));
                    cmd.Parameters.AddWithValue("data", p.DataPrenotazione);
                    cmd.Parameters.AddWithValue("anno", p.Anno);
                    cmd.Parameters.AddWithValue("periodoDal", p.PeriodoDal);
                    cmd.Parameters.AddWithValue("periodoAl", p.PeriodoAl);
                    cmd.Parameters.AddWithValue("caparra", p.Caparra);
                    cmd.Parameters.AddWithValue("tariffa", p.Tariffa);
                    cmd.Parameters.AddWithValue("mezzaPensione", p.MezzaPensione);
                    cmd.Parameters.AddWithValue("pensioneCompleta", p.PensioneCompleta);
                    cmd.Parameters.AddWithValue("primaColazione", p.PrimaColazione);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();

                    if (inserimentoEffettuato > 0)
                    {
                        ViewBag.MessaggioDiSuccesso = "Prenotazione creata correttamente";
                        sqlConnection.Close();
                        ModelState.Clear();
                        ViewBag.Lc = ListaCamere;
                        return View();
                    }
                    else
                    {
                        ViewBag.Lc = ListaCamere;
                        ViewBag.MessaggioDiErrore = "Errore nel salvataggio, riprovare";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.MessaggioDiErrore = "Errore generico:" + ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
                ViewBag.Lc = ListaCamere;
                return View();
            }
            else
            {
                ViewBag.Lc = ListaCamere;
                return View();
            }
        }

        public ActionResult ReservationList()
        {
            List<Prenotazioni> p = Prenotazioni.GetAllReservations();
            return View(p);
        }

        public ActionResult ShowReservationForIf()
        {
            List<Servizi> s = new List<Servizi>();
            return View();
        }

        public ActionResult EliminaPrenotazione(int id)
        {
            Prenotazioni.DeleteReservation(id);
            return RedirectToAction("ReservationList");
        }

        public ActionResult ModificaPrenotazione(int id)
        {
            ViewBag.Lc = ListaCamere;
            Prenotazioni p = Prenotazioni.GetReservationsById(id);
            return View(p);
        }

        [HttpPost]
        public ActionResult ModificaPrenotazione(Prenotazioni p)
        {
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Prenotazioni SET primaColazione=@primaColazione, mezzaPensione=@mezzaPensione, pensioneCompleta=@pensioneCompleta, idCamera=@idCamera, periodoDal=@periodoDal, periodoAl=@periodoAl, caparra=@caparra, tariffa=@tariffa where idPrenotazione=@idPrenotazione", sqlConnection);
                    cmd.Parameters.AddWithValue("idCamera", p.IdCamera);
                    cmd.Parameters.AddWithValue("periodoDal", p.PeriodoDal);
                    cmd.Parameters.AddWithValue("periodoAl", p.PeriodoAl);
                    cmd.Parameters.AddWithValue("caparra", p.Caparra);
                    cmd.Parameters.AddWithValue("tariffa", p.Tariffa);
                    cmd.Parameters.AddWithValue("mezzaPensione", p.MezzaPensione);
                    cmd.Parameters.AddWithValue("pensioneCompleta", p.PensioneCompleta);
                    cmd.Parameters.AddWithValue("primaColazione", p.PrimaColazione);
                    cmd.Parameters.AddWithValue("idPrenotazione", p.IdPrenotazione);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();
                    if (inserimentoEffettuato > 0)
                    {
                        return RedirectToAction("ReservationList");
                    }
                    else
                    {
                        ViewBag.messageError = "Errore nel complilamento del form";
                        ViewBag.Lc = ListaCamere;
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.messageError = "Errore generico:" + ex;
                    ViewBag.Lc = ListaCamere;
                    return View();
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            else
            {
                ViewBag.messageError = "Errore nel complilamento del form";
                ViewBag.Lc = ListaCamere;
                return View();
            }
        }

        public ActionResult CheckOut(int id)
        {
            Prenotazioni p = Prenotazioni.GetReservationsById(id);
            List<Servizi> s = Servizi.GetServiceById(id);
            Clienti c = Clienti.GetClientId(p.IdCliente);
            p.cliente = c;
            p.ListaServizi = s;
            double totServizi = 0;
            foreach (Servizi servizi in s)
            {
                totServizi += (servizi.Quantita * servizi.Prezzo);
            }
            TempData["totServizi"] = totServizi;

            return View(p);
        }
    }
}