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
    //gestisce le operazioni relative alle anagrafiche clienti
    [Authorize]
    public class ClientsController : Controller
    {
        public ActionResult CreateClient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateClient(Clienti c)
        {
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Clienti VALUES (@codiceFiscale, @cognome, @nome, @citta, @provincia, @email, @telefono, @cellulare)", sqlConnection);
                    cmd.Parameters.AddWithValue("codiceFiscale", c.CodiceFiscale);
                    cmd.Parameters.AddWithValue("cognome", c.Cognome);
                    cmd.Parameters.AddWithValue("nome", c.Nome);
                    cmd.Parameters.AddWithValue("citta", c.Citta);
                    cmd.Parameters.AddWithValue("provincia", c.Provincia);
                    cmd.Parameters.AddWithValue("email", c.Email);
                    if (c.Telefono != null)
                    {
                        cmd.Parameters.AddWithValue("telefono", c.Telefono);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("telefono", "");
                    }
                    cmd.Parameters.AddWithValue("cellulare", c.Cellulare);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();

                    if (inserimentoEffettuato > 0)
                    {
                        ViewBag.MessaggioDiSuccesso = "Cliente creato correttamente";
                        sqlConnection.Close();
                        ModelState.Clear();
                        return View();
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
            }
            return View();
        }

        public ActionResult ListaClient(Clienti c)
        {
            List<Clienti> clienti = Clienti.GetAllClients();
            return View(clienti);
        }

        public ActionResult ModificaCliente(int id)
        {
            Clienti c = Clienti.GetClientId(id);
            return View(c);
        }

        [HttpPost]
        public ActionResult ModificaCliente(Clienti c)
        {
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Clienti SET codiceFiscale=@codiceFiscale, cognome=@cognome, nome=@nome, citta=@citta, provincia=@provincia, email=@email where idCliente=@idCliente", sqlConnection);
                    cmd.Parameters.AddWithValue("codiceFiscale", c.CodiceFiscale);
                    cmd.Parameters.AddWithValue("cognome", c.Cognome);
                    cmd.Parameters.AddWithValue("nome", c.Nome);
                    cmd.Parameters.AddWithValue("citta", c.Citta);
                    cmd.Parameters.AddWithValue("provincia", c.Provincia);
                    cmd.Parameters.AddWithValue("email", c.Email);
                    cmd.Parameters.AddWithValue("idCliente", c.Id);

                    int inserimentoEffettuato = cmd.ExecuteNonQuery();
                    if (inserimentoEffettuato > 0)
                    {
                        return RedirectToAction("ListaClient");
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

        public ActionResult EliminaCliente(int id)
        {
            Clienti.DeleteClient(id);
            return RedirectToAction("ListaClient");
        }
    }
}