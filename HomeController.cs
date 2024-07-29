using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using EventManagement.Repository;
using EventManagement.Controllers;
using EventManagement.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace EventManagement.Controllers
{
    public class HomeController : Controller
    {
        private SqlConnection connection;

        private void InitializeConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }

        HomeRepository homeData = new HomeRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Signin login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    InitializeConnection(); // Initialize
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @Username AND Password = @Password", connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", login.Username);
                        cmd.Parameters.AddWithValue("@Password", login.Password); 

                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Session["UserID"] = reader["UserID"];
                            Session["Username"] = reader["Username"];

                            if (login.Username.ToLower() == "admin")
                            {
                                return RedirectToAction("AdminDashboard", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("UserDashboard", "User");
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"]="Invalid login attempt.";
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(login);
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Signup user)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    IsInserted = homeData.InsertUser(user);
                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Registered successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Try different username";
                    }
                }
                return RedirectToAction("Signup");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactUs contact)
        {
            try
            {
                homeData.ContactUs(contact);
                TempData["SuccessMessage"] = "Message sent. We will reach out to you..!";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
