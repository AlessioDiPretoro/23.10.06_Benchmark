using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Albergo.Models
{
    public class Prenotazioni
    {
        public int IdPrenotazione { get; set; }
        public int IdCliente { get; set; }

        [Display(Name = "Seleziona Camera ")]
        public int IdCamera { get; set; }

        public Clienti cliente { get; set; }
        public Camere camera { get; set; }
        public List<Servizi> ListaServizi = new List<Servizi>();

        public DateTime DataPrenotazione { get; set; }
        public string Anno { get; set; }
        public DateTime PeriodoDal { get; set; }
        public DateTime PeriodoAl { get; set; }
        public double Caparra { get; set; }
        public double Tariffa { get; set; }
        public bool MezzaPensione { get; set; }
        public bool PensioneCompleta { get; set; }
        public bool PrimaColazione { get; set; }

        public static List<Prenotazioni> GetAllReservations()
        {
            List<Prenotazioni> reservations = new List<Prenotazioni>();
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Prenotazioni as P INNER JOIN Clienti as D ON P.idCliente = D.idCliente INNER JOIN Camere as C ON P.idCamera = C.idCamera", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Clienti c = new Clienti();
                        c.Id = Convert.ToInt16(reader["idCliente"].ToString());
                        c.CodiceFiscale = reader["codiceFiscale"].ToString();
                        c.Cognome = reader["cognome"].ToString();
                        c.Nome = reader["nome"].ToString();
                        c.Citta = reader["citta"].ToString();
                        c.Provincia = reader["provincia"].ToString();
                        c.Email = reader["email"].ToString();
                        c.Telefono = reader["telefono"].ToString();
                        c.Cellulare = reader["cellulare"].ToString();

                        Camere camera = new Camere();
                        camera.Numero = reader["numero"].ToString();
                        camera.Descrizione = reader["descrizione"].ToString();
                        camera.Tipologia = reader["tipologia"].ToString();

                        Prenotazioni p = new Prenotazioni();
                        p.cliente = c;
                        p.camera = camera;
                        p.IdPrenotazione = Convert.ToInt16(reader["idPrenotazione"].ToString());
                        p.IdCamera = Convert.ToInt32(reader["idCamera"]);
                        p.DataPrenotazione = Convert.ToDateTime(reader["data"]);
                        p.Anno = reader["anno"].ToString();
                        p.PeriodoDal = Convert.ToDateTime(reader["periodoDal"]);
                        p.PeriodoAl = Convert.ToDateTime(reader["periodoAl"]);
                        p.Caparra = Convert.ToDouble(reader["caparra"]);
                        p.Tariffa = Convert.ToDouble(reader["tariffa"]);
                        p.MezzaPensione = Convert.ToBoolean(reader["mezzaPensione"]);
                        p.PensioneCompleta = Convert.ToBoolean(reader["pensioneCompleta"]);
                        p.PrimaColazione = Convert.ToBoolean(reader["primaColazione"]);

                        reservations.Add(p);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return reservations;
        }

        public static Prenotazioni GetReservationsById(int id)
        {
            Prenotazioni p = new Prenotazioni();
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select * from Prenotazioni as P INNER JOIN Clienti as D ON P.idCliente = D.idCliente INNER JOIN Camere as C ON P.idCamera = C.idCamera WHERE P.idPrenotazione={id}", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Clienti c = new Clienti();
                        c.Id = Convert.ToInt16(reader["idCliente"].ToString());
                        c.CodiceFiscale = reader["codiceFiscale"].ToString();
                        c.Cognome = reader["cognome"].ToString();
                        c.Nome = reader["nome"].ToString();
                        c.Citta = reader["citta"].ToString();
                        c.Provincia = reader["provincia"].ToString();
                        c.Email = reader["email"].ToString();
                        c.Telefono = reader["telefono"].ToString();
                        c.Cellulare = reader["cellulare"].ToString();

                        Camere camera = new Camere();
                        camera.Numero = reader["numero"].ToString();
                        camera.Descrizione = reader["descrizione"].ToString();
                        camera.Tipologia = reader["tipologia"].ToString();

                        p.cliente = c;
                        p.camera = camera;
                        p.IdPrenotazione = Convert.ToInt16(reader["idPrenotazione"].ToString());
                        p.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                        p.IdCamera = Convert.ToInt32(reader["idCamera"]);
                        p.DataPrenotazione = Convert.ToDateTime(reader["data"]);
                        p.Anno = reader["anno"].ToString();
                        p.PeriodoDal = Convert.ToDateTime(reader["periodoDal"]);
                        p.PeriodoAl = Convert.ToDateTime(reader["periodoAl"]);
                        p.Caparra = Convert.ToDouble(reader["caparra"]);
                        p.Tariffa = Convert.ToDouble(reader["tariffa"]);
                        p.MezzaPensione = Convert.ToBoolean(reader["mezzaPensione"]);
                        p.PensioneCompleta = Convert.ToBoolean(reader["pensioneCompleta"]);
                        p.PrimaColazione = Convert.ToBoolean(reader["primaColazione"]);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return p;
        }

        public static void DeleteReservation(int id)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

            SqlConnection conn = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                //aggiungere la cancellazione di tutti servizi collegati alla prenotazione prima di fare questo...
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM Prenotazioni WHERE idPrenotazione=@id";
                cmd.Parameters.AddWithValue("id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            finally { conn.Close(); }
        }

        public static List<Prenotazioni> GetReservationsComplete()
        {
            List<Prenotazioni> reservations = new List<Prenotazioni>();
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Prenotazioni as P INNER JOIN Clienti as D ON P.idCliente = D.idCliente INNER JOIN Camere as C ON P.idCamera = C.idCamera where pensioneCompleta = 1", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Clienti c = new Clienti();
                        c.Id = Convert.ToInt16(reader["idCliente"].ToString());
                        c.CodiceFiscale = reader["codiceFiscale"].ToString();
                        c.Cognome = reader["cognome"].ToString();
                        c.Nome = reader["nome"].ToString();
                        c.Citta = reader["citta"].ToString();
                        c.Provincia = reader["provincia"].ToString();
                        c.Email = reader["email"].ToString();
                        c.Telefono = reader["telefono"].ToString();
                        c.Cellulare = reader["cellulare"].ToString();

                        Camere camera = new Camere();
                        camera.Numero = reader["numero"].ToString();
                        camera.Descrizione = reader["descrizione"].ToString();
                        camera.Tipologia = reader["tipologia"].ToString();

                        Prenotazioni p = new Prenotazioni();
                        p.cliente = c;
                        p.camera = camera;
                        p.IdPrenotazione = Convert.ToInt16(reader["idPrenotazione"].ToString());
                        p.IdCamera = Convert.ToInt32(reader["idCamera"]);
                        p.DataPrenotazione = Convert.ToDateTime(reader["data"]);
                        p.Anno = reader["anno"].ToString();
                        p.PeriodoDal = Convert.ToDateTime(reader["periodoDal"]);
                        p.PeriodoAl = Convert.ToDateTime(reader["periodoAl"]);
                        p.Caparra = Convert.ToDouble(reader["caparra"]);
                        p.Tariffa = Convert.ToDouble(reader["tariffa"]);
                        p.MezzaPensione = Convert.ToBoolean(reader["mezzaPensione"]);
                        p.PensioneCompleta = Convert.ToBoolean(reader["pensioneCompleta"]);
                        p.PrimaColazione = Convert.ToBoolean(reader["primaColazione"]);

                        reservations.Add(p);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return reservations;
        }

        public static List<Prenotazioni> GetReservationsByCodiFisc(string cod)
        {
            List<Prenotazioni> reservations = new List<Prenotazioni>();
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select * from Prenotazioni as P INNER JOIN Clienti as D ON P.idCliente = D.idCliente INNER JOIN Camere as C ON P.idCamera = C.idCamera where D.codiceFiscale = @codiceFiscale", conn);
                cmd.Parameters.AddWithValue("codiceFiscale", cod);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Clienti c = new Clienti();
                        c.Id = Convert.ToInt16(reader["idCliente"].ToString());
                        c.CodiceFiscale = reader["codiceFiscale"].ToString();
                        c.Cognome = reader["cognome"].ToString();
                        c.Nome = reader["nome"].ToString();
                        c.Citta = reader["citta"].ToString();
                        c.Provincia = reader["provincia"].ToString();
                        c.Email = reader["email"].ToString();
                        c.Telefono = reader["telefono"].ToString();
                        c.Cellulare = reader["cellulare"].ToString();

                        Camere camera = new Camere();
                        camera.Numero = reader["numero"].ToString();
                        camera.Descrizione = reader["descrizione"].ToString();
                        camera.Tipologia = reader["tipologia"].ToString();

                        Prenotazioni p = new Prenotazioni();
                        p.cliente = c;
                        p.camera = camera;
                        p.IdPrenotazione = Convert.ToInt16(reader["idPrenotazione"].ToString());
                        p.IdCamera = Convert.ToInt32(reader["idCamera"]);
                        p.DataPrenotazione = Convert.ToDateTime(reader["data"]);
                        p.Anno = reader["anno"].ToString();
                        p.PeriodoDal = Convert.ToDateTime(reader["periodoDal"]);
                        p.PeriodoAl = Convert.ToDateTime(reader["periodoAl"]);
                        p.Caparra = Convert.ToDouble(reader["caparra"]);
                        p.Tariffa = Convert.ToDouble(reader["tariffa"]);
                        p.MezzaPensione = Convert.ToBoolean(reader["mezzaPensione"]);
                        p.PensioneCompleta = Convert.ToBoolean(reader["pensioneCompleta"]);
                        p.PrimaColazione = Convert.ToBoolean(reader["primaColazione"]);

                        reservations.Add(p);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return reservations;
        }
    }
}