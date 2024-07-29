using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EventManagement.Repository
{
    public class UserRepository
    {
        private SqlConnection connection;

        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }
        public IEnumerable<EmployeeResponse> GetAllResponses()
        {
            Connection();
            List<EmployeeResponse> responses = new List<EmployeeResponse>();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM EmployeeResponses", connection))
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EmployeeResponse response = new EmployeeResponse
                    {
                        ResponseID = Convert.ToInt32(reader["ResponseID"]),
                        EventID = Convert.ToInt32(reader["EventID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        ResponseDate = Convert.ToDateTime(reader["ResponseDate"]),
                        Status = reader["Status"].ToString()
                    };
                    responses.Add(response);
                }

                connection.Close();
            }

            return responses;
        }


        public EmployeeResponse GetResponseForEvent(int eventId, int userId)
        {
            Connection();
            connection.Open();
            EmployeeResponse response = null;
            using (SqlCommand command = new SqlCommand("SELECT * FROM EmployeeResponses WHERE EventID = @EventID AND UserID = @UserID", connection))
            {
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    response = new EmployeeResponse
                    {
                        ResponseID = Convert.ToInt32(reader["ResponseID"]),
                        EventID = Convert.ToInt32(reader["EventID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            connection.Close();
            return response;
        }

        public void AddResponse(EmployeeResponse response)
        {
            Connection();
            connection.Open();
            using (SqlCommand command = new SqlCommand("INSERT INTO EmployeeResponses (EventID, UserID, Status) VALUES (@EventID, @UserID, @Status)", connection))
            {
                command.Parameters.AddWithValue("@EventID", response.EventID);
                command.Parameters.AddWithValue("@UserID", response.UserID);
                command.Parameters.AddWithValue("@Status", response.Status);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public IEnumerable<EmployeeResponse> GetResponsesByUser(int userId)
        {
            List<EmployeeResponse> responses = new List<EmployeeResponse>();
            Connection();
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT * FROM EmployeeResponses WHERE UserID = @UserID", connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    responses.Add(new EmployeeResponse
                    {
                        ResponseID = Convert.ToInt32(reader["ResponseID"]),
                        EventID = Convert.ToInt32(reader["EventID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            connection.Close();
            return responses;
        }
        public bool ApproveResponse(int responseId)
        {
            Connection();
            connection.Open();
            using (SqlCommand command = new SqlCommand("UPDATE EmployeeResponses SET Status = 'Approved' WHERE ResponseID = @ResponseID", connection))
            {
                command.Parameters.AddWithValue("@ResponseID", responseId);
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                return rowsAffected > 0;
            }
        }


    }

}