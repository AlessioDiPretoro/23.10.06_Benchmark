using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Albergo.Models
{
    public class Camere
    {
        public int Id { get; set; }

        [Required]
        public string Numero { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Required]
        public string Tipologia { get; set; }

        public static List<Camere> GetAllRooms()
        {
            List<Camere> rooms = new List<Camere>();

            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * FROM Camere", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Camere r = new Camere();

                        r.Id = Convert.ToInt16(reader["idCamera"].ToString());
                        r.Numero = reader["numero"].ToString();
                        r.Descrizione = reader["descrizione"].ToString();
                        r.Tipologia = reader["tipologia"].ToString();
                        rooms.Add(r);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return rooms;
        }

        public static Camere GetRoomById(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            Camere r = new Camere();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select * FROM Camere WHERE idCamera ={id}", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        r.Id = Convert.ToInt16(reader["idCamera"].ToString());
                        r.Numero = reader["numero"].ToString();
                        r.Descrizione = reader["descrizione"].ToString();
                        r.Tipologia = reader["tipologia"].ToString();
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return r;
        }
    }
}