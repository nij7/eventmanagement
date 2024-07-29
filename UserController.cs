using EventManagement.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using EventManagement.Models;

namespace EventManagement.Controllers
{
    public class UserController : Controller
    {
        private SqlConnection connection;
        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }
        AdminRepository data = new AdminRepository();
        UserRepository user = new UserRepository();

            public ActionResult EmployeResponse()
            {
              
                return View();
            }
            
            
        



            // GET: User
            public ActionResult UserDashboard()
        {
            return View();
        }
        public void GetId(string username)
        {
            Connection();
            connection.Open();
            using (SqlCommand command = new SqlCommand("sp_GetUserId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);
                

                object result = command.ExecuteScalar();

                int userId = Convert.ToInt32(result);

                Session["user"] = userId;

            }
        }
        public ActionResult ProfileInfo()
        {
            try
            {
                int employee = (int)Session["UserID"];
                var courses = data.GetUserByID(employee).FirstOrDefault();
                if (courses == null)
                {
                    return RedirectToAction("ProfileInfo");
                }
                return View(courses);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public ActionResult UpcomingEvents()
        {
            try
            {
                var UserList = data.AllEvent();
                return View(UserList);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public ActionResult EventResponse(int eventId)
        {
            int userId = (int)Session["UserID"];
            var existingResponse = user.GetResponseForEvent(eventId, userId);
            if (existingResponse != null)
            {
                TempData["ErrorMessage"] = "You have already applied for this event.";
                return RedirectToAction("UpcomingEvents");
            }
            else
            {
                var response = new EmployeeResponse
                {
                    EventID = eventId,
                    UserID = userId,
                    Status = "Pending"
                };

                user.AddResponse(response);
                TempData["SuccessMessage"] = "Your response has been recorded.";
                return RedirectToAction("UpcomingEvents");
            }
        }
        public ActionResult UserResponses()
        {
            int userId = (int)Session["UserID"];
            var responses = user.GetResponsesByUser(userId);
            return View(responses);
        }



        public ActionResult EditProfile()
        {
            int employee = (int)Session["UserID"];
            var mark = data.GetUserByID(employee).FirstOrDefault();
            if(mark ==null)
            {
                
                return RedirectToAction("ProfileInfo");
            }
            return View(mark);
        }
        [HttpPost, ActionName("EditProfile")]
        public ActionResult EditProfile(Signup mark)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsUpdated = data.UpdateUser(mark);
                    if (IsUpdated)
                    {
                        TempData["SuccessMessage"] = "Details updated successfully";

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to update the details";

                    }
                }
                
                return RedirectToAction("UserDashboard");

                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]= ex.Message;
                return View();
               
            }
        }
        public ActionResult ChangePasswordUser()
        {
            ViewBag.Message = "Change Password User";
            return View();
        }

        [HttpPost]
        public ActionResult ChangePasswordUser(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = (int)Session["UserID"];
                    var user = data.GetUserByID(userId).FirstOrDefault();

                    if (user == null)
                    {
                        TempData["ErrorMessage"] = "User not found.";
                        return RedirectToAction("UserDashboard");
                    }

                    if (model.NewPassword != model.ConfirmNewPassword)
                    {
                        TempData["ErrorMessage"] = "Passwords do not match.";
                        return View(model);
                    }

                    user.Password = model.NewPassword; // Ideally, hash the password before saving
                    bool isUpdated = data.UpdateUserPassword(user);

                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "Password updated successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to update the password.";
                    }

                    return RedirectToAction("UserDashboard");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return View(model);
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Home");

        }
    }
}