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
    [Authorize]
    public class RoomsController : Controller
    {
        public List<SelectListItem> TipiDiCamera
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "--- Seleziona ---", Value = "0" },
                    new SelectListItem { Text = "Singola", Value = "Singola" },
                    new SelectListItem { Text = "Doppia", Value = "Doppia" }
                };

                return list;
            }
        }

        public ActionResult AggiungiCamera()
        {
            ViewBag.TipiDiCamera = TipiDiCamera;
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiCamera(Camere c)
        {
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Camere VALUES (@numero, @descrizione, @tipologia)", sqlConnection);
                    cmd.Parameters.AddWithValue("numero", c.Numero);
                    cmd.Parameters.AddWithValue("descrizione", c.Descrizione);
                    cmd.Parameters.AddWithValue("tipologia", c.Tipologia);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();

                    if (inserimentoEffettuato > 0)
                    {
                        ViewBag.MessaggioDiSuccesso = "Camera creata correttamente";
                        sqlConnection.Close();
                        ModelState.Clear();
                        ViewBag.TipiDiCamera = TipiDiCamera;
                        return View();
                    }
                    else
                    {
                        ViewBag.TipiDiCamera = TipiDiCamera;
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
            }

            ViewBag.TipiDiCamera = TipiDiCamera;
            return View();
        }

        public ActionResult MostraCamere()
        {
            List<Camere> rooms = Camere.GetAllRooms();
            return View(rooms);
        }

        public ActionResult ModificaCamera(int id)
        {
            Camere c = Camere.GetRoomById(id);
            return View(c);
        }

        [HttpPost]
        public ActionResult ModificaCamera(Camere c)
        {
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Camere SET numero=@numero, descrizione=@descrizione, tipologia=@tipologia where idCamera=@idCamera", sqlConnection);
                    cmd.Parameters.AddWithValue("numero", c.Numero);
                    cmd.Parameters.AddWithValue("descrizione", c.Descrizione);
                    cmd.Parameters.AddWithValue("tipologia", c.Tipologia);
                    cmd.Parameters.AddWithValue("idCamera", c.Id);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();
                    if (inserimentoEffettuato > 0)
                    {
                        return RedirectToAction("MostraCamere", "Rooms");
                    }
                    else
                    {
                        ViewBag.messageError = "Errore nel complilamento del form";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.messageError = "Errore generico:" + ex;
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
                return View();
            }
        }

        public ActionResult EliminaCamera(int id)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

            SqlConnection conn = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM Camere WHERE idCamera=@id";
                cmd.Parameters.AddWithValue("id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            finally { conn.Close(); }

            return RedirectToAction("MostraCamere", "Rooms");
        }
    }
}