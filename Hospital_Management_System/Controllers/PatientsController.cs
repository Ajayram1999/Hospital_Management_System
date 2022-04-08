using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Hospital_Library.Enum;
using Hospital_Library;
using System.Data;
using ArrayToPdf;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Web.Security;

namespace Hospital_Management_System.Controllers
{
    public class PatientsController : Controller
    {
        DBHandle dbhandle = new DBHandle();

        [HttpGet]
        
        public ActionResult Login(string returnUrl)
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            ViewBag.ReturnUrl = returnUrl ??
                Url.Action("Dashboard");
            return View();
        }
        //Post:When user click on the submit button then this method will call
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login LoginView)
        {
                if (dbhandle.IsValidUser(LoginView.Email, LoginView.Password))
                {
                    FormsAuthentication.SetAuthCookie(LoginView.Email, false);
                    return RedirectToAction("Index");
                }
                else if (dbhandle.IsValidAdmin(LoginView.Email, LoginView.Password))
                {
                    FormsAuthentication.SetAuthCookie(LoginView.Email, false);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Your Email and password is incorrect");
                }
            return View(LoginView);
        }

        public ActionResult password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult password(string Email, string ContactNo, string Password)
        {
            try
            {
                if (dbhandle.IsValid(Email, ContactNo))
                {
                    if (Password!="")
                    {
                        dbhandle.ResetPassword(Password, Email);
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter New password");
                        return View();
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Your Email and password is incorrect");
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Register(string operation)
        {
            try
            {
                RegisterEmployee register = new RegisterEmployee();
                if (operation == Operations.Add.ToString())
                {
                    register.ActionType = Operations.Add;
                }
                return PartialView("_Register", register);
            }
            catch
            {
                return PartialView();
            }
        }
        [HttpPost]
        public ActionResult Register(RegisterEmployee register)
        {
            try
            {
                bool authenticated = Request.IsAuthenticated;
                var Username = User.Identity.Name;
                RegisterEmployee user = dbhandle.GetUser(Username);
                register.CreatedBy = user.Id;
                register.CreatedDate = DateTime.Now.ToString();
                register.ModifiedBy = user.Id;
                register.ModifiedDate = DateTime.Now.ToString();
                if (dbhandle.RegisterEmployee(register))
                {
                    TempData["InsertMsg"] = "<script>alert('Employee saved successful..')</script>";
                    return RedirectToAction("AdminIndex");
                }
                else
                {
                    TempData["InsertErrorMsg"] = "<script>alert('Data not saved..')</script>";
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return PartialView();
            }
        }

        public ActionResult Edituser(string operation, int id)
        {
            try
            {
                DBHandle dbhandle = new DBHandle();
                RegisterEmployee register = new RegisterEmployee();
                if (operation == Operations.Edit.ToString())
                {
                    register.ActionType = Operations.Edit;
                }
                var data = dbhandle.GetemployeebyId(id);
                return PartialView("_Register", data);
            }
            catch
            {
                return PartialView();
            }
        }

        [HttpPost]

        public ActionResult Edituser(RegisterEmployee register)
        {
            try
            {
                bool authenticated = Request.IsAuthenticated;
                var Username = User.Identity.Name;
                RegisterEmployee user = dbhandle.GetUser(Username);
                register.ModifiedBy = user.Id;
                register.ModifiedDate = DateTime.Now.ToString();
                if (dbhandle.Updateemployee(register))
                {
                    TempData["UpdateMsg"] = "<script>alert('Employee updated successfully')</script>";
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["UpdateErrorMsg"] = "<script>alert('Data not updated..')</script>";
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return PartialView();
            }
        }

        [HttpGet]
        public ActionResult Deleteuser(int id)
        {
            try
            {
                DBHandle dbhandle = new DBHandle();
                var register = dbhandle.GetemployeebyId(id);
                return PartialView("_Deleteuser", register);
            }
            catch
            {
                return PartialView();
            }
        }

        [HttpPost]

        public ActionResult Deleteuser(RegisterEmployee register)
        {
            try
            {
                int id = register.Id;
                int r = dbhandle.Deleteemployee(id);
                if (r > 0)
                {
                    TempData["DeleteMsg"] = "<script>alert('Patient deleted successful..')</script>";
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["DeleteErrorMsg"] = "<script>alert('Data not deleted..')</script>";
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return PartialView();
            }
        }

        [Authorize]
        public ActionResult AdminIndex()
        {
            return View();
        }
        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult UserInfo()
        {
            return View();
        }


        public ActionResult Create(string operation)
        {
            try
            {
                Patient patient = new Patient();
                if (operation == Operations.Add.ToString())
                {
                    patient.ActionType = Operations.Add;
                }
                return PartialView("_Add_Edit", patient);
            }
            catch
            {
                return PartialView();
            }
        }

        [HttpPost]

        public ActionResult Create(Patient patient)
        {
            try
            {
                bool authenticated = Request.IsAuthenticated;
                var Username = User.Identity.Name;
                RegisterEmployee user = dbhandle.GetUser(Username);
                patient.CreatedBy = user.Id;
                patient.CreatedDate = DateTime.Now.ToString();
                patient.ModifiedBy = user.Id;
                patient.ModifiedDate = DateTime.Now.ToString();
                if (dbhandle.InsertPatient(patient))
                {
                    TempData["InsertMsg"] = "<script>alert('Patient saved successful..')</script>";
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["InsertErrorMsg"] = "<script>alert('Data not saved..')</script>";
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return PartialView();
            }
        }

        public ActionResult Edit(string operation, int id)
        {
            try
            {
                DBHandle dbhandle = new DBHandle();
                Patient patient = new Patient();
                if (operation == Operations.Edit.ToString())
                {
                    patient.ActionType = Operations.Edit;
                }
                var data = dbhandle.GetPatientbyId(id);
                return PartialView("_Add_Edit", data);
            }
            catch
            {
                return PartialView();
            }
        }

        [HttpPost]

        public ActionResult Edit(Patient patient)
        {
            try
            {
                bool authenticated = Request.IsAuthenticated;
                var Username = User.Identity.Name;
                RegisterEmployee user = dbhandle.GetUser(Username);
                patient.ModifiedBy = user.Id;
                patient.ModifiedDate = DateTime.Now.ToString();
                if (dbhandle.UpdatePatient(patient))
                {
                    TempData["UpdateMsg"] = "<script>alert('Patient updated successful..')</script>";
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["UpdateErrorMsg"] = "<script>alert('Data not updated..')</script>";
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return PartialView();
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                DBHandle dbhandle = new DBHandle();
                var patient = dbhandle.GetPatientbyId(id);
                return PartialView("_Delete", patient);
            }
            catch
            {
                return PartialView();
            }
        }

        [HttpPost]

        public ActionResult Delete(Patient patient)
        {
            try
            {
                int id = patient.Id;
                int r = dbhandle.DeletePatient(id);
                if (r > 0)
                {
                    TempData["DeleteMsg"] = "<script>alert('Patient deleted successful..')</script>";
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["DeleteErrorMsg"] = "<script>alert('Data not deleted..')</script>";
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return PartialView();
            }
        }
        public ActionResult GetPat()
        {
            try
            {
                DBHandle dbhandle = new DBHandle();
                var PageNumber = Convert.ToInt32(Request.Form["start"]);
                var RowsInPage = Convert.ToInt32(Request.Form["length"]);
                var SearchValue = Convert.ToString(Request.Form["search[value]"]);
                string sortColumnName = Request.Form["columns[" + Request["order[0][column]"] + "][data]"];
                string sortDirection = Request["order[0][dir]"];

                List<Patient> patientList = dbhandle.GetPatient(PageNumber, RowsInPage, SearchValue);
                patientList = patientList.OrderBy(sortColumnName + " " + sortDirection).ToList<Patient>();

                int totalCount = dbhandle.GetCount(SearchValue);
                
                return Json(new
                {
                    data = patientList,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount
                },
                JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetEmployeeDetails()
        {
            try
            {
                DBHandle dbhandle = new DBHandle();
                var PageNumber = Convert.ToInt32(Request.Form["start"]);
                var RowsInPage = Convert.ToInt32(Request.Form["length"]);
                var SearchValue = Convert.ToString(Request.Form["search[value]"]);
                string sortColumnName = Request.Form["columns[" + Request["order[0][column]"] + "][data]"];
                string sortDirection = Request["order[0][dir]"];

                List<RegisterEmployee> employeeList = dbhandle.GetEmployee(PageNumber, RowsInPage, SearchValue);
                employeeList = employeeList.OrderBy(sortColumnName + " " + sortDirection).ToList<RegisterEmployee>();

                int totalCount = dbhandle.GetCounts(SearchValue);

                return Json(new
                {
                    data = employeeList,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount
                },
                JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Print(string format)
        {
            DBHandle objdbhandle = new DBHandle();

            List<Patient> patientList = objdbhandle.PrintAll();

            if (format == "pdf")
            {

                var table = new DataTable("Patients Report");
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Age", typeof(int));
                table.Columns.Add("Gender", typeof(string));
                table.Columns.Add("Department", typeof(string));
                table.Columns.Add("Covid Result", typeof(string));
                table.Columns.Add("Contact", typeof(string));
                table.Columns.Add("In-Patient", typeof(bool));


                foreach (Patient patient in patientList)
                    table.Rows.Add(patient.Name, patient.Age, patient.Gender, patient.Department, patient.CovidTestResult, patient.Contact, patient.InPatient);

                var pdf = table.ToPdf();
                System.IO.File.WriteAllBytes(@"D:\Assingnment\Ajay\Hospital_Management_System\Hospital_Management_System\PdfViewer\report.pdf", pdf);
            }
            
            return PartialView("_Print");

        }

        [HttpGet]
        public ActionResult Prints()
        {
            DBHandle objdbhandle = new DBHandle();

            List<RegisterEmployee> employeeList = objdbhandle.PrintsAll();
                var table = new DataTable("Employee Report");
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Designation", typeof(string));
                table.Columns.Add("Email", typeof(string));
                table.Columns.Add("ContactNo", typeof(string));
                table.Columns.Add("CreatedBy", typeof(string));
                table.Columns.Add("CreatedDate", typeof(string));
                table.Columns.Add("ModifiedBy", typeof(string));
                table.Columns.Add("ModifiedDate", typeof(string));


                foreach (RegisterEmployee employee in employeeList)
                    table.Rows.Add(employee.Name, employee.Designation, employee.Email, employee.ContactNo, employee.CreatedBy, employee.CreatedDate, employee.ModifiedBy, employee.ModifiedDate);

                var pdf = table.ToPdf();
                System.IO.File.WriteAllBytes(@"D:\Assingnment\Ajay\Hospital_Management_System\Hospital_Management_System\PdfViewer\employee.pdf", pdf);
            return PartialView("_Prints");

        }
    }
}
