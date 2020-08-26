using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SchlManSysAPI.Models;

// Ado.Net technology used for to create web api project
namespace SchlManSysAPI.Controllers
{
    [RoutePrefix("students")]
    public class StudentsController : ApiController
    {
        [Route("")]
        public IHttpActionResult GetAllStudents()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("select * from dbo.Students", conn);
            DataSet dataset = new DataSet("Students");
            adapter.Fill(dataset);
            List<StudentEntity> studentList = new List<StudentEntity>();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                studentList.Add(new StudentEntity
                {
                    StdId = Convert.ToInt32(dr["StdId"]),
                    FamId = Convert.ToInt32(dr["FamId"]),
                    TutId = Convert.ToInt32(dr["TutId"]),
                    StdMob = Convert.ToInt64(dr["StdMob"]),
                    StdName = Convert.ToString(dr["StdName"]),
                    StdDob = Convert.ToString(Convert.ToDateTime(dr["StdDob"]).ToShortDateString()),
                    StdAge = Convert.ToInt32(dr["StdAge"]),
                });
            }
            if (studentList.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(studentList);
            }
            
        }

        [Route("{id:int}")]
        public IHttpActionResult GetAStudents(int id)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("select * from dbo.Students", conn);
            DataSet dataset = new DataSet("Students");
            adapter.Fill(dataset);
            List<StudentEntity> studentList = new List<StudentEntity>();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                studentList.Add(new StudentEntity
                {
                    StdId = Convert.ToInt32(dr["StdId"]),
                    FamId = Convert.ToInt32(dr["FamId"]),
                    TutId = Convert.ToInt32(dr["TutId"]),
                    StdMob = Convert.ToInt64(dr["StdMob"]),
                    StdName = Convert.ToString(dr["StdName"]),
                    StdDob = Convert.ToString(Convert.ToDateTime(dr["StdDob"]).ToShortDateString()),
                    StdAge = Convert.ToInt32(dr["StdAge"]),
                });
            }
            StudentEntity student = studentList.SingleOrDefault(x => x.StdId == id);
            if (student == null)
            {
                //return NotFound();
                return Ok(new { Message = "The data is not found in our system", Status = "Ok" });
            }
            else
            {
                return Ok(student);
            }
        }

        [Route("")]
        public IHttpActionResult PostAStudent( StudentEntity student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Inavalid data format");
            }
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            SqlCommand cmd = new SqlCommand("exec dbo.AddStudentDetails @sname, @smob, @famid, @tutid, @stddob", conn);
            cmd.Parameters.Add(new SqlParameter("@sname", student.StdName));
            cmd.Parameters.Add(new SqlParameter("@smob", student.StdMob));
            cmd.Parameters.Add(new SqlParameter("@famid", student.FamId));
            cmd.Parameters.Add(new SqlParameter("@tutid", student.TutId));
            cmd.Parameters.Add(new SqlParameter("@stddob", student.StdDob));
            using (conn)
            {
                conn.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {   
                    return Ok(new { Message = "The data is added successfully", Status = "Ok" });
                }
            }
            return Ok(new { Message = "Try again after sometimes", Status = "Not Ok" });
        }

        [Route("")]
        public IHttpActionResult PutAStudent(StudentEntity student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Inavalid data format");
            }
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            SqlCommand cmd1 = new SqlCommand("select count(*) from dbo.Students where StdId=@stdid", conn);
            cmd1.Parameters.Add(new SqlParameter("@stdid", student.StdId));
            using (conn)
            {
                conn.Open();
                int isValid = (int)cmd1.ExecuteScalar();
                if (isValid > 0)
                {
                    SqlCommand cmd2 = new SqlCommand("update dbo.Students set StdName=@sname, StdMob=@smob, FamId=@famid, TutId=@tutid, StdDob=@stddob where StdId=@stdid", conn);
                    cmd2.Parameters.Add(new SqlParameter("@sname", student.StdName));
                    cmd2.Parameters.Add(new SqlParameter("@smob", student.StdMob));
                    cmd2.Parameters.Add(new SqlParameter("@famid", student.FamId));
                    cmd2.Parameters.Add(new SqlParameter("@tutid", student.TutId));
                    cmd2.Parameters.Add(new SqlParameter("@stddob", student.StdDob));
                    cmd2.Parameters.Add(new SqlParameter("@stdid", student.StdId));
                    int rowAffected = cmd2.ExecuteNonQuery();
                    if (rowAffected>0)
                    {
                        return Ok(new { Message = "The data is updated successfully", Status = "Updated" });
                    }
                    else
                    {
                        return Ok(new { Message = "The data is no updated", Status = "Not Updated" });
                    }
                }
                else
                {
                    return Ok(new { Message = "The data is not available", Status = "Not Available" });
                }
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult DeleteAStudents(int id)
        {
            if (id<0)
            {
                return BadRequest("Inavalid data format");
            }
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            SqlCommand cmd = new SqlCommand("delete from dbo.Students where StdId=@stdid", conn);
            cmd.Parameters.Add(new SqlParameter("@stdid", id));
            using (conn)
            {
                conn.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    return Ok(new { Message = "The data deleted from our system", Status = "Ok" });
                }
            }
            return Ok(new { Message = "The data is not found", Status = "Not Deleted" });
        }
    }
}
