using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Hospital_Management_System.Models;
using System.Data.SqlClient;
using System.Data;

namespace Hospital_Management_System.Services
{
    public class DBHandle
    {
        static string connectionstring = "Data Source=DESKTOP-46PCH8R;Initial Catalog=Patientsdata;Integrated Security=True";
        // static string connectionstring = "Data Source=LAPTOP-5IQ1TLRU;Initial Catalog=Database;Integrated Security=True";
        SqlConnection con = new SqlConnection(connectionstring);
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        public List<Patient> GetPatient(int pagenumber, int pagesize, string search)
        {
            cmd = new SqlCommand("sp_select", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pagenumber", pagenumber);
            cmd.Parameters.AddWithValue("@pagesize", pagesize);
            cmd.Parameters.AddWithValue("@search", search);

            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();

            List<Patient> list = new List<Patient>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Patient
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Name = dr["Name"].ToString(),
                    Age = Convert.ToInt32(dr["Age"]),
                    Gender = dr["Gender"].ToString(),
                    Department = dr["Department"].ToString(),
                    CovidTestResult = dr["CovidTestResult"].ToString(),
                    Contact = dr["Contact"].ToString(),
                    InPatient = Convert.ToBoolean(dr["InPatient"]),
                    Deleted = Convert.ToInt32(dr["Deleted"]),
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                    ModifiedDate = dr["ModifiedDate"].ToString(),
                });

            }
            
            foreach (var n in list)
            {
                var Createduser = GetemployeebyId(n.CreatedBy);
                var Modifieduser = GetemployeebyId(n.ModifiedBy);
                n.CreatedName = Createduser.Email;
                n.ModifiedName = Modifieduser.Email;
            }
            return list;
        }

        public List<RegisterEmployee> GetEmployee(int pagenumber, int pagesize, string search)
        {
            cmd = new SqlCommand("emp_select", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pagenumber", pagenumber);
            cmd.Parameters.AddWithValue("@pagesize", pagesize);
            cmd.Parameters.AddWithValue("@search", search);

            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();

            List<RegisterEmployee> list = new List<RegisterEmployee>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new RegisterEmployee
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Name = dr["Name"].ToString(),
                    Designation = dr["Designation"].ToString(),
                    Email = dr["Email"].ToString(),
                    ContactNo = dr["ContactNo"].ToString(),
                    CreatedBy =Convert.ToInt32(dr["CreatedBy"]),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                    ModifiedDate = dr["ModifiedDate"].ToString(),
                });

            }
            foreach (var n in list)
            {
                var Createduser = GetemployeebyId(n.CreatedBy);
                var Modifieduser = GetemployeebyId(n.ModifiedBy);
                n.CreatedName = Createduser.Email;
                n.ModifiedName = Modifieduser.Email;
            }
            return list;
        }

        public Patient GetPatientbyId(int Id)
        {
            cmd = new SqlCommand("GETBYID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);

            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            Patient patient = new Patient();
            patient.Name = dt.Rows[0]["Name"].ToString();
            patient.Age = Convert.ToInt32(dt.Rows[0]["Age"]);
            patient.Gender = dt.Rows[0]["Gender"].ToString();
            patient.Department = dt.Rows[0]["Department"].ToString();
            patient.CovidTestResult = dt.Rows[0]["CovidTestResult"].ToString();
            patient.Contact = dt.Rows[0]["Contact"].ToString();
            patient.InPatient = Convert.ToBoolean(dt.Rows[0]["InPatient"]);
            patient.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
            patient.ModifiedDate = dt.Rows[0]["ModifiedDate"].ToString();
            return patient;
        }

        public RegisterEmployee GetemployeebyId(int Id)
        {
            cmd = new SqlCommand("GETemployeeBYID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);

            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            RegisterEmployee register = new RegisterEmployee();
            register.Name = dt.Rows[0]["Name"].ToString();
            register.Designation = dt.Rows[0]["Designation"].ToString();
            register.Email = dt.Rows[0]["Email"].ToString();
            register.Password = dt.Rows[0]["Password"].ToString();
            register.ContactNo = dt.Rows[0]["ContactNo"].ToString();
            register.Isadmin = Convert.ToBoolean(dt.Rows[0]["Isadmin"]);
            register.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
            register.ModifiedDate = dt.Rows[0]["ModifiedDate"].ToString();
            return register;
        }

        public RegisterEmployee GetUser(string Email)
        {
            cmd = new SqlCommand("GETUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", Email);

            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            RegisterEmployee register = new RegisterEmployee();
            register.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
            register.Name = dt.Rows[0]["Name"].ToString();
            register.Designation = dt.Rows[0]["Designation"].ToString();
            register.Email = dt.Rows[0]["Email"].ToString();
            register.Password = dt.Rows[0]["Password"].ToString();
            register.ContactNo = dt.Rows[0]["ContactNo"].ToString();
            register.Isadmin = Convert.ToBoolean(dt.Rows[0]["Isadmin"]);
            register.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
            register.ModifiedDate = dt.Rows[0]["ModifiedDate"].ToString();
            return register;
        }

        public int GetCount(string search)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("GetCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@search", search);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                count = Convert.ToInt32(row["Column1"]);
            }
            return count;
        }

        public int GetCounts(string search)
        {
            int counts = 0;
            SqlCommand cmd = new SqlCommand("Getcounts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@search", search);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                counts = Convert.ToInt32(row["Column1"]);
            }
            return counts;
        }

        public bool InsertPatient(Patient patient)
        {
            try
            {
                cmd = new SqlCommand("sp_insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", patient.Name);
                cmd.Parameters.AddWithValue("@age", patient.Age);
                cmd.Parameters.AddWithValue("@gender", patient.Gender);
                cmd.Parameters.AddWithValue("@department", patient.Department);
                cmd.Parameters.AddWithValue("@covidtestresult", patient.CovidTestResult);
                cmd.Parameters.AddWithValue("@contact", patient.Contact);
                cmd.Parameters.AddWithValue("@inpatient", patient.InPatient);
                cmd.Parameters.AddWithValue("@deleted", 1);
                cmd.Parameters.AddWithValue("@CreatedBy", patient.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate", patient.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedBy", patient.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", patient.ModifiedDate);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public bool RegisterEmployee(RegisterEmployee register)
        {
            try
            {
                cmd = new SqlCommand("EmployeeRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", register.Name);
                cmd.Parameters.AddWithValue("@Designation", register.Designation);
                cmd.Parameters.AddWithValue("@Email", register.Email);
                cmd.Parameters.AddWithValue("@Password", register.Password);
                cmd.Parameters.AddWithValue("@ContactNo", register.ContactNo);
                cmd.Parameters.AddWithValue("@Isadmin", register.Isadmin);
                cmd.Parameters.AddWithValue("@CreatedBy", register.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate", register.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedBy", register.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", register.ModifiedDate);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool UpdatePatient(Patient patient)
        {
            try
            {
                cmd = new SqlCommand("sp_update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", patient.Name);
                cmd.Parameters.AddWithValue("@age", patient.Age);
                cmd.Parameters.AddWithValue("@gender", patient.Gender);
                cmd.Parameters.AddWithValue("@department", patient.Department);
                cmd.Parameters.AddWithValue("@covidtestresult", patient.CovidTestResult);
                cmd.Parameters.AddWithValue("@contact", patient.Contact);
                cmd.Parameters.AddWithValue("@inpatient", patient.InPatient);
                cmd.Parameters.AddWithValue("@deleted", 1);
                cmd.Parameters.AddWithValue("@ModifiedBy", patient.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", patient.ModifiedDate);
                cmd.Parameters.AddWithValue("@id", patient.Id);

                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool Updateemployee(RegisterEmployee register)
        {
            try
            {
                cmd = new SqlCommand("emp_update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", register.Name);
                cmd.Parameters.AddWithValue("@Designation", register.Designation);
                cmd.Parameters.AddWithValue("@Email", register.Email);
                cmd.Parameters.AddWithValue("@ContactNo", register.ContactNo);
                cmd.Parameters.AddWithValue("@Isadmin", register.Isadmin);
                cmd.Parameters.AddWithValue("@ModifiedBy", register.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", register.ModifiedDate);
                cmd.Parameters.AddWithValue("@Id", register.Id);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {

                string emp = e.Message;
                throw;
            }

        }

        public int DeletePatient(int id)
        {
            try
            {
                cmd = new SqlCommand("sp_delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
               throw;
            }
        }

        public int Deleteemployee(int id)
        {
            try
            {
                cmd = new SqlCommand("emp_delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Patient> PrintAll()
        {
            cmd = new SqlCommand("PrintAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();

            List<Patient> list = new List<Patient>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Patient
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Name = dr["Name"].ToString(),
                    Age = Convert.ToInt32(dr["Age"]),
                    Gender = dr["Gender"].ToString(),
                    Department = dr["Department"].ToString(),
                    CovidTestResult = dr["CovidTestResult"].ToString(),
                    Contact = dr["Contact"].ToString(),
                    InPatient = Convert.ToBoolean(dr["InPatient"])
                });

            }
            return list;
        }

        public List<RegisterEmployee> PrintsAll()
        {
            cmd = new SqlCommand("PrintsAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();
            List<RegisterEmployee> list = new List<RegisterEmployee>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new RegisterEmployee
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Name = dr["Name"].ToString(),
                    Designation = dr["Designation"].ToString(),
                    Email = dr["Email"].ToString(),
                    ContactNo = dr["ContactNo"].ToString(),
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                    ModifiedDate = dr["ModifiedDate"].ToString(),
                });

            }
            return list;
        }

        public bool IsValidUser(string email, string password)
        {
            bool IsValid = false;
            string query = "select * from EmployeeTable where Email=@Email and Password=@Password and Isadmin=0";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }
        public bool IsValidAdmin(string email, string password)
        {
            bool IsValid = false;
            string query = "select * from EmployeeTable where Email=@Email and Password=@Password and Isadmin=1";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }

        public bool IsValid(string Email, string ContactNo)
        {
            bool IsValid = false;
            string query = "select * from EmployeeTable where Email=@Email and ContactNo=@ContactNo";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@ContactNo", ContactNo);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }
        public bool ResetPassword(string Password, string Email)
        {
                cmd = new SqlCommand("passwordreset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@Email", Email);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
            if (i >= 0)
                return true;
            else 
                return false;
        }
    }
}