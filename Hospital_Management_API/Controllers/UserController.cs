using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hospital_Management_API.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        DBHandle dbhandle = new DBHandle();
        [HttpGet]
        [Route("GetEmployee")]
        public string GetEmployeeDetails(int PageNumber, int RowsInPage, string SearchValue)
        {
            try
            {
                List<RegisterEmployee> employeeList = dbhandle.GetEmployee(PageNumber, RowsInPage, SearchValue);
                int totalCount = dbhandle.GetCounts(SearchValue);
                return JsonConvert.SerializeObject(employeeList);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
       
        [HttpPost]
        [Route("RegisterEmployee")]
        public String Register(RegisterEmployee register)
        {
            try
            {
                var Username = User.Identity.Name;
                RegisterEmployee user = dbhandle.GetUser(Username);
                register.CreatedBy = user.Id;
                register.CreatedDate = DateTime.Now.ToString();
                register.ModifiedBy = user.Id;
                register.ModifiedDate = DateTime.Now.ToString();
                if (dbhandle.RegisterEmployee(register))
                {
                    return "Employee saved successful";
                }
                else
                {
                    return "Employee Not Saved";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        [Route("Getuserbyid")]
        public string Getuserbyid(int id)
        {
            try
            {
                RegisterEmployee register = new RegisterEmployee();
                var data = dbhandle.GetemployeebyId(id);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPut]
        [Route("EditEmployee")]
        public string Edituser(RegisterEmployee register)
        {
            try
            {
                var Username = User.Identity.Name;
                RegisterEmployee user = dbhandle.GetUser(Username);
                register.ModifiedBy = user.Id;
                register.ModifiedDate = DateTime.Now.ToString();
                if (dbhandle.Updateemployee(register))
                {
                    return "Employee Updated successfully";
                }
                else
                {
                    return "Employee Not Updated";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        

        [HttpPost]
        [Route("DeleteEmployee")]
        public string Deleteuser(RegisterEmployee register)
        {
            try
            {
                int id = register.Id;
                int r = dbhandle.Deleteemployee(id);
                if (r > 0)
                {
                    return "Employee Deleted successfully";
                }
                else
                {
                    return "Employee Not Deleted";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
