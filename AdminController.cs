using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using EventManagement.Repository;
using EventManagement.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.SessionState;
using System.Linq.Expressions;

namespace EventManagement.Controllers
{
    public class AdminController : Controller
    {
        private SqlConnection connection;
        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }

        AdminRepository data = new AdminRepository();
        UserRepository user = new UserRepository();

        // GET: Admin
        public ActionResult AdminDashboard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EmployeeList()
        {
            try
            {
                var UserList = data.AllEmployeeList();
                return View(UserList);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            try
            {
                var User = data.GetUserByID(id).FirstOrDefault();
                if (User == null)
                {
                    TempData["InfoMessage"] = "Currently employee not available in the database.";
                    return RedirectToAction("Index");
                }
                return View(User);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        public ActionResult Edit(int id)
        {

            var user = data.GetUserByID(id).FirstOrDefault();
            if (user == null)
            {
                TempData["InfoMessage"] = "User not available with Id" + id.ToString();
                return RedirectToAction("EmployeeList");
            }

            return View();
        }

        // POST: Product/Edit/5
        [HttpPost, ActionName("Edit")]
        public ActionResult Update(Signup user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsUpdated = data.UpdateUser(user);
                    if (IsUpdated)
                    {
                        TempData["SuccessMessage"] = "User details updated successfully...!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User is already avilable/Unable to update the details";
                    }

                }
                return RedirectToAction("EmployeeList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(user);
            }
        }
        /// <summary>
        /// insert event(image)
        /// </summary>
        /// <returns></returns>
        public ActionResult AddEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEvent(Event obj)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    IsInserted = data.AddEvent(obj);
                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Event Added";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong";
                    }
                }
                return RedirectToAction("AddEvent");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }




        [HttpGet]
        public ActionResult UpdateEvent()
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
        public ActionResult DetailsEvent(int id)
        {
            try
            {
                var details = data.GetEventByID(id).FirstOrDefault();
                if (details == null)
                {
                    TempData["InfoMessage"] = "Currently Event not available in the database.";
                    return RedirectToAction("UpdateEvent");
                }
                return View(details);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        public ActionResult EditEvent(int id)
        {

            var details = data.GetEventByID(id).FirstOrDefault();
            if (details == null)
            {
                TempData["InfoMessage"] = "Event not available with Id" + id.ToString();
                return RedirectToAction("UpdateEvent");
            }

            return View();
        }

        // POST: Edit/5
        [HttpPost, ActionName("EditEvent")]
        public ActionResult EditEvent(Event details)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsUpdated = data.UpdateEventByID(details);
                    if (IsUpdated)
                    {
                        TempData["SuccessMessage"] = "Event details updated successfully...!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Event is unable to update the details";
                    }

                }
                return RedirectToAction("UpdateEvent");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        // GET: Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = data.GetEventByID(id).FirstOrDefault();
                if (obj == null)
                {
                    TempData["InfoMessage"] = "Event not available with Id " + id.ToString();
                    return RedirectToAction("UpdateEvent");
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                string result = data.DeleteEventByID(id);
                if (result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }

                return RedirectToAction("UpdateEvent");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }


        public ActionResult UserResponses()
        {
            var responses = user.GetAllResponses();
            return View(responses);
        }

        [HttpPost]
        public ActionResult ApproveResponse(int responseId)
        {
            bool isApproved = user.ApproveResponse(responseId);
            if (isApproved)
            {
                TempData["SuccessMessage"] = "Response approved successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to approve response.";
            }
            return RedirectToAction("UserResponses", "Admin");
        }


        [HttpGet]
        public ActionResult ToContact()
        {
            try
            {
                var ContactList = data.ContacList();
                return View(ContactList);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login","Home");

        }
    }
}