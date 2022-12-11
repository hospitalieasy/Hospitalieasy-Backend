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
        public JsonResult Get()
        {
            string query = @"
                            select PatientId,Email,Password from
                            dbo.Patient
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

        [HttpPut]
        public string Put(Patient patient)
        {
            try
            {
                DataTable table = new DataTable();

                string query = @"
                    update dbo.Patient set Email ='" + patient.Email + @"',Password ='" + patient.Password + @"' 
                        where PatientId = " + patient.PatientId + @"

                ";

                string sqlDataSource = _configuration.GetConnectionString("HospitalEasyApp");

                using (var con = new SqlConnection(sqlDataSource))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
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
     


    }
}
