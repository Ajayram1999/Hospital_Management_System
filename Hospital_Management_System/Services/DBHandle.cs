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
        static string connectionstring = "Data Source=LAPTOP-5IQ1TLRU;Initial Catalog=Database;Integrated Security=True";
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
                    Deleted = Convert.ToInt32(dr["Deleted"])
                });

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
            return patient;
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
    }
}