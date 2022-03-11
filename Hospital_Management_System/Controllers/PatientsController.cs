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

namespace Hospital_Management_System.Controllers
{
    public class PatientsController : Controller
    {
        DBHandle dbhandle = new DBHandle();
        // GET: User
        public ActionResult Index()
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
                System.IO.File.WriteAllBytes(@"G:\project-ust-05-12-2021\Ajay\Hospital_Management_System\Hospital_Management_System\PdfViewer\report.pdf", pdf);
            }
            
            return PartialView("_Print");

        }
    }
}
