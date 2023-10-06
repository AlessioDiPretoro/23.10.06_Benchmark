using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Albergo.Models
{
    public class Clienti
    {
        public int Id { get; set; }

        [Required]
        public string CodiceFiscale { get; set; }

        [Required]
        public string Cognome { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Citta { get; set; }

        [Required]
        public string Provincia { get; set; }

        [Required]
        public string Email { get; set; }

        public string Telefono { get; set; }

        [Required]
        public string Cellulare { get; set; }

        public static List<Clienti> GetAllClients()
        {
            List<Clienti> clients = new List<Clienti>();

            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * FROM Clienti", conn);
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
                        clients.Add(c);
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return clients;
        }

        public static Clienti GetClientId(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            Clienti c = new Clienti();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select * FROM Clienti WHERE idCliente ={id}", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        c.Id = Convert.ToInt16(reader["idCliente"].ToString());
                        c.CodiceFiscale = reader["codiceFiscale"].ToString();
                        c.Cognome = reader["cognome"].ToString();
                        c.Nome = reader["nome"].ToString();
                        c.Citta = reader["citta"].ToString();
                        c.Provincia = reader["provincia"].ToString();
                        c.Email = reader["email"].ToString();
                        c.Telefono = reader["telefono"].ToString();
                        c.Cellulare = reader["cellulare"].ToString();
                    }
                }
            }
            catch { Exception ex; }
            finally { conn.Close(); }

            return c;
        }


        public static void DeleteClient(int id)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

            SqlConnection conn = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM Clienti WHERE idCliente=@id";
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