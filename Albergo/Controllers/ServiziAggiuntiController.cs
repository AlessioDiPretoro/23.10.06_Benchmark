using Albergo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Albergo.Controllers
{
    public class ServiziAggiuntiController : Controller
    {
        // GET: ServiziAggiunti
        public ActionResult AddService(int id)
        {
            TempData["id"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddService(Servizi s)
        {
            int idPrenotazione = Convert.ToInt16(TempData["id"]);

            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Servizi VALUES (@idPrenotazione, @descrizioneServizio, @prezzoServizio, @data, @quantita)", sqlConnection);
                    cmd.Parameters.AddWithValue("idPrenotazione", idPrenotazione);
                    cmd.Parameters.AddWithValue("descrizioneServizio", s.Descrizione);
                    cmd.Parameters.AddWithValue("prezzoServizio", s.Prezzo);
                    cmd.Parameters.AddWithValue("data", s.Data);
                    cmd.Parameters.AddWithValue("quantita", s.Quantita);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();

                    if (inserimentoEffettuato > 0)
                    {
                        ViewBag.MessaggioDiSuccesso = "Prenotazione creata correttamente";
                        sqlConnection.Close();
                        ModelState.Clear();
                        return RedirectToAction("ReservationList", "Reservation");
                    }
                    else
                    {
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
                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult ShowService(int id)
        {
            List<Servizi> s = Servizi.GetServiceById(id);
            return View(s);
        }

        public ActionResult DeleteService(int id)
        {
            Servizi.DeleteService(id);
            return RedirectToAction("ReservationList", "Reservation");
        }

        public ActionResult EditService(int id)
        {
            Servizi s = Servizi.GetThisServiceById(id);
            return View(s);
        }

        [HttpPost]
        public ActionResult EditService(Servizi s)
        {
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Servizi SET descrizioneServizio=@descrizioneServizio, prezzoServizio=@prezzoServizio, data=@data, quantita=@quantita where idServizio=@idServizio", sqlConnection);
                    cmd.Parameters.AddWithValue("descrizioneServizio", s.Descrizione);
                    cmd.Parameters.AddWithValue("prezzoServizio", s.Prezzo);
                    cmd.Parameters.AddWithValue("data", s.Data);
                    cmd.Parameters.AddWithValue("quantita", s.Quantita);
                    cmd.Parameters.AddWithValue("idServizio", s.Id);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();
                    if (inserimentoEffettuato > 0)
                    {
                        return RedirectToAction("ReservationList", "Reservation");
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    return View();
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            else
            {
                return View();
            }
        }
    }
}