using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EventManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Repository
{
    public class HomeRepository
    {
        private SqlConnection connection;
        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }
        


        ///inset user
        
        public bool InsertUser(Signup obj)
        {
            Connection();
            SqlCommand command = new SqlCommand("sp_InsertUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FirstName",obj.FirstName);
            command.Parameters.AddWithValue("@LastName", obj.LastName);
            command.Parameters.AddWithValue("DateOfBirth", obj.DateOfBirth);
            command.Parameters.AddWithValue("Gender", obj.Gender);
            command.Parameters.AddWithValue("@PhoneNumber",obj.PhoneNumber);
            command.Parameters.AddWithValue("@EmailAddress",obj.EmailAddress);
            command.Parameters.AddWithValue("@Address", obj.Address);
            command.Parameters.AddWithValue("@State", obj.State);
            command.Parameters.AddWithValue("@City", obj.City);
            command.Parameters.AddWithValue("@UserName", obj.Username);
            command.Parameters.AddWithValue("@Password", obj.Password);
            command.Parameters.AddWithValue("@ConfirmPassword", obj.ConfirmPassword);
            connection.Open();
            int id = command.ExecuteNonQuery();
            connection.Close();
            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        ///contactus
        
        public bool ContactUs(ContactUs contact)
        {
            Connection();
            SqlCommand command = new SqlCommand("SP_Contactus", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Name", contact.Name);
            command.Parameters.AddWithValue("@EmailAddress", contact.Email);
            command.Parameters.AddWithValue("@Subject",contact.Subject);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
    }
}