using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;
using EventManagement.Models;
using System.Web;
using System.IO;

namespace EventManagement.Repository
{
    public class AdminRepository
    {
        
        string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();


            public List<Signup> AllEmployeeList()
            {
                List<Signup> SignupList = new List<Signup>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_GetUsers";
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    connection.Open();
                    adapter.Fill(dataTable);
                    connection.Close();
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        SignupList.Add(new Signup
                        {
                            UserID = Convert.ToInt32(dataRow["UserID"]),
                            FirstName = dataRow["FirstName"].ToString(),
                            LastName = dataRow["LastName"].ToString(),
                            DateOfBirth = dataRow["DateOfBirth"].ToString(),
                            Gender = dataRow["Gender"].ToString(),
                            PhoneNumber = dataRow["PhoneNumber"].ToString(),
                            EmailAddress = dataRow["EmailAddress"].ToString(),
                            Address = dataRow["Address"].ToString(),
                            State = dataRow["State"].ToString(),
                            City = dataRow["City"].ToString(),
                            Username = dataRow["Username"].ToString(),
                            Password = dataRow["Password"].ToString(),
                            ConfirmPassword = dataRow["ConfirmPassword"].ToString()


                        });
                    }
                }
                    return SignupList;
                
            }
        public List<Signup> GetUserByID(int UserID)
        {
            List<Signup> SignupList = new List<Signup>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetUserById";
                command.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dataUsers = new DataTable();

                connection.Open();
                sqlDA.Fill(dataUsers);
                connection.Close();

                foreach (DataRow dr in dataUsers.Rows)
                {
                    SignupList.Add(new Signup
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        DateOfBirth = dr["DateOfBirth"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        PhoneNumber = dr["PhoneNumber"].ToString(),
                        EmailAddress = dr["EmailAddress"].ToString(),
                        Address = dr["Address"].ToString(),
                        State = dr["State"].ToString(),
                        City = dr["City"].ToString(),
                        Username = dr["Username"].ToString(),
                        Password = dr["Password"].ToString(),
                        ConfirmPassword = dr["ConfirmPassword"].ToString()
                    });
                }
            }

            return SignupList;
        }

        public bool UpdateUser(Signup user)
        {
            int i= 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", user.UserID);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", user.Gender);
                command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                command.Parameters.AddWithValue("@Address", user.Address);
                command.Parameters.AddWithValue("@State", user.State);
                command.Parameters.AddWithValue("@City", user.City);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                
                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();

            }
            if (i > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool AddEvent(Event obj)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand("sp_AddEvent", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EventName", obj.EventName);
                command.Parameters.AddWithValue("@DateOfEvent", obj.DateOfEvent);
                command.Parameters.AddWithValue("@Description", obj.Description);
                command.Parameters.AddWithValue("@Subevent1", obj.Subevent1);
                command.Parameters.AddWithValue("@Subdesc1", obj.Subdesc1);
                command.Parameters.AddWithValue("@Subevent2", obj.Subevent2);
                command.Parameters.AddWithValue("@Subdesc2", obj.Subdesc2);
                command.Parameters.AddWithValue("@Subevent3", obj.Subevent3);
                command.Parameters.AddWithValue("@Subdesc3", obj.Subdesc3);
                command.Parameters.AddWithValue("@Subevent4", obj.Subevent4);
                command.Parameters.AddWithValue("@Subdesc4", obj.Subdesc4);
                command.Parameters.AddWithValue("@Subevent5", obj.Subevent5);
                command.Parameters.AddWithValue("@Subdesc5", obj.Subdesc5);
                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
                
            }
            else
            {
                return false;
            }

        }
        public List<Event> AllEvent()
        {
            List<Event> AllEventList = new List<Event>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_AllEvents";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AllEventList.Add(new Event
                    {
                        EventID = Convert.ToInt32(dataRow["EventID"]),
                        EventName = dataRow["EventName"].ToString(),
                        DateOfEvent = dataRow["DateOfEvent"].ToString(),
                        Description = dataRow["Description"].ToString(),
                        Subevent1= dataRow["Subevent1"].ToString(),
                        Subdesc1 = dataRow["Subdesc1"].ToString(),
                        Subevent2 = dataRow["Subevent2"].ToString(),
                        Subdesc2 = dataRow["Subdesc2"].ToString(),
                        Subevent3 = dataRow["Subevent3"].ToString(),
                        Subdesc3 = dataRow["Subdesc3"].ToString(),
                        Subevent4 = dataRow["Subevent4"].ToString(),
                        Subdesc4 = dataRow["Subdesc4"].ToString(),
                        Subevent5 = dataRow["Subevent5"].ToString(),
                        Subdesc5 = dataRow["Subdesc5"].ToString(),


                    });
                }
            }
            return AllEventList;

        }
        public List<Event> GetEventByID(int EventID)
        {
            List<Event> AllEventList = new List<Event>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetEventById";
                command.Parameters.AddWithValue("@EventID", EventID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dataUsers = new DataTable();

                connection.Open();
                sqlDA.Fill(dataUsers);
                connection.Close();

                foreach (DataRow dr in dataUsers.Rows)
                {
                    AllEventList.Add(new Event
                    {
                        EventID = Convert.ToInt32(dr["eventID"]),
                        EventName = dr["EventName"].ToString(),
                        DateOfEvent = dr["DateOfEvent"].ToString(),
                        Description = dr["Description"].ToString(),
                        Subevent1 = dr["Subevent1"].ToString(),
                        Subdesc1 = dr["Subdesc1"].ToString(),
                        Subevent2 = dr["Subevent2"].ToString(),
                        Subdesc2 = dr["Subdesc2"].ToString(),
                        Subevent3 = dr["Subevent3"].ToString(),
                        Subdesc3 = dr["Subdesc3"].ToString(),
                        Subevent4 = dr["Subevent4"].ToString(),
                        Subdesc4 = dr["Subdesc4"].ToString(),
                        Subevent5 = dr["Subevent5"].ToString(),
                        Subdesc5 = dr["Subdesc5"].ToString(),
                    });
                }
            }

            return AllEventList;
        }
        public bool UpdateEventByID(Event obj)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateEvent", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EventID", obj.EventID);
                command.Parameters.AddWithValue("@EventName", obj.EventName);
                command.Parameters.AddWithValue("@DateOfEvent", obj.DateOfEvent);
                command.Parameters.AddWithValue("@Description", obj.Description);
                command.Parameters.AddWithValue("@Subevent1", obj.Subevent1);
                command.Parameters.AddWithValue("@Subdesc1", obj.Subdesc1);
                command.Parameters.AddWithValue("@Subevent2", obj.Subevent2);
                command.Parameters.AddWithValue("@Subdesc2", obj.Subdesc2);
                command.Parameters.AddWithValue("@Subevent3", obj.Subevent3);
                command.Parameters.AddWithValue("@Subdesc3", obj.Subdesc3);
                command.Parameters.AddWithValue("@Subevent4", obj.Subevent4);
                command.Parameters.AddWithValue("@Subdesc4", obj.Subdesc4);
                command.Parameters.AddWithValue("@Subevent5", obj.Subevent5);
                command.Parameters.AddWithValue("@Subdesc5", obj.Subdesc5);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();

            }
            if (i > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public String DeleteEventByID(int EventID)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteEvent", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EventID", EventID);
                // Correct parameter name to match stored procedure definition
                command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                // Correct parameter name to match stored procedure definition
                result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
            }
            return result;
        }




        public List<ContactUs> ContacList()
        {
            List<ContactUs> ContactList = new List<ContactUs>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetContact";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();
            
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ContactList.Add(new ContactUs
                    {
                        ContactId = Convert.ToInt32(dataRow["ContactID"]),
                        Name = dataRow["Name"].ToString(),
                        Email = dataRow["EmailAddress"].ToString(),
                        Subject = dataRow["Subject"].ToString(),
                    });
                }
            }
            return ContactList;

        }
        public bool UpdateUserPassword(Signup user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Users SET Password = @Password WHERE UserID = @UserID", connection);
                cmd.Parameters.AddWithValue("@Password", user.Password); // Ideally, hash the password before saving
                cmd.Parameters.AddWithValue("@UserID", user.UserID);

                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connection.Close();


                return rowsAffected > 0;
            }
        }





    }
}
