using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Albergo.Models
{
    public class Servizi
    {
        public int Id { get; set; }
        public int IdPrenotazione { get; set; }
        public string Descrizione { get; set; }
        public double Prezzo { get; set; }
        public DateTime Data { get; set; }
        public int Quantita { get; set; }

        public static List<Servizi> GetServiceById(int id)
        {
            List<Servizi> s = new List<Servizi>();
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select * FROM Servizi WHERE idPrenotazione ={id}", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Servizi ser = new Servizi();
                        ser.Id = Convert.ToInt16(reader["idServizio"].ToString());
                        ser.Descrizione = reader["descrizioneServizio"].ToString();
                        ser.Prezzo = Convert.ToDouble(reader["prezzoServizio"]);
                        ser.Data = Convert.ToDateTime(reader["data"]);
                        ser.Quantita = Convert.ToInt16(reader["quantita"]);
                        s.Add(ser);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return s;
        }

        public static Servizi GetThisServiceById(int id)
        {
            Servizi ser = new Servizi();
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select * FROM Servizi WHERE idServizio ={id}", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ser.Id = Convert.ToInt16(reader["idServizio"].ToString());
                        ser.Descrizione = reader["descrizioneServizio"].ToString();
                        ser.Prezzo = Convert.ToDouble(reader["prezzoServizio"]);
                        ser.Data = Convert.ToDateTime(reader["data"]);
                        ser.Quantita = Convert.ToInt16(reader["quantita"]);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return ser;
        }

        public static void DeleteService(int id)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

            SqlConnection conn = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM Servizi WHERE idServizio=@id";
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
    }
}