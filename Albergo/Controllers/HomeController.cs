using Albergo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Albergo.Controllers
{
    //solo per login e logout
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Utenti u)
        {
            string conn = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("Select * FROM Utenti WHERE username=@username And password=@password", sqlConnection);
                cmd.Parameters.AddWithValue("username", u.username);
                cmd.Parameters.AddWithValue("password", u.password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(u.username, true);
                    sqlConnection.Close();

                    return RedirectToAction("UtentiHome", "Utenti");
                }
                else
                {
                    ViewBag.AuthError = "Autenticazione non riuscita";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.AuthError = "Errore generico" + ex;
            }
            finally
            {
                sqlConnection.Close();
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}