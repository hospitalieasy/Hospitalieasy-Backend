using HospitalEasyDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace HospitalEasyDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            string query = @"
                            select PatientId,Name,Surname,Birthdate,Email,Password,Telno from
                            Tbl_Patient
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalEasyApp");

            SqlDataReader myReader;


            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        
        [HttpPut("{id}")]
        public string Put(Patient patient,int id)
        {
            try
            {
                DataTable table = new DataTable();

                string query = @"
                    update Tbl_Patient set Name ='" + patient.Name + @"',Surname ='" + patient.Surname + @"'
                        ,Birthdate ='" + patient.BirthDate + @"',Email = '"+patient.Email+@"'
                        ,Password = '"+patient.Password+@"',Telno = '"+patient.Telno+@"'
                        where PatientId = @p1

                ";

                string sqlDataSource = _configuration.GetConnectionString("HospitalEasyApp");

                using (var con = new SqlConnection(sqlDataSource))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@p1", id);
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
                return "Updated Successfuly";
            }
            catch (System.Exception)
            {

                return "Failed to update";
            }
        }

        [HttpPost]
        public string Post(Patient patient)
        {
            try
            {
                DataTable table = new DataTable();

                string query = @"
                        insert into Tbl_Patient (Name,Surname,Birthdate,Email,Password,Telno)
                        Values(
                                '"+patient.Name+@"'
                                ,'"+patient.Surname+ @"'
                                ,'"+patient.BirthDate+@"'
                                ,'"+patient.Email+@"'
                                ,'"+patient.Password+@"'
                                ,'"+patient.Telno+@"'
                                )
                    
                ";

                string sqlDataSource = _configuration.GetConnectionString("HospitalEasyApp");

                using (var con = new SqlConnection(sqlDataSource))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
                return "Added Successfuly";
            }
            catch (System.Exception)
            {

                return "Failed to add";
            }
        }
     
        
        
    }
}
