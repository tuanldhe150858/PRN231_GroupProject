using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PRN_Final_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly final_projectContext context;
        public StudentController(final_projectContext context)
        {
            this.context = context;
        }
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent(StudentDTO studentDTO)
        {
            var classS = context.Classes.Where(c => c.ClassId == studentDTO.ClassId).FirstOrDefault();
            if (classS == null)
            {
                return BadRequest();
            }
            else
            {
                Student student = new Student
                {
                    Mail = studentDTO.Mail,
                    Password = studentDTO.Password,
                    Role = "student",
                    ClassId = studentDTO.ClassId,
                    Class = classS
                };
                context.Students.Add(student);
                context.SaveChanges();
                return Ok();
            }

        }
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = context.Students
                .Select(t => new
                {
                    t.StudentId,
                    t.Mail,
                    t.Password,
                    t.Role,
                    t.ClassId
                }).ToList();
            return Ok(students);
        }
        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var student = context.Students
                .Where(s => s.StudentId == studentId)
                .Select(t => new
                {
                    t.StudentId,
                    t.Mail,
                    t.Password,
                    t.Role
                }).FirstOrDefault();
            return Ok(student);
        }
        [HttpPut("UpdateStudent")]
        public async Task<bool> UpdateStudent(StudentDTO studentDTO)
        {
            var classS = context.Classes.Where(c => c.ClassId == studentDTO.ClassId).FirstOrDefault();
            var student = context.Students.Where(t => t.StudentId == studentDTO.StudentId).FirstOrDefault();
            if (student == null)
            {
                return false;
            }
            else
            {
                student.Mail = studentDTO.Mail;
                student.Password = studentDTO.Password;
                student.ClassId = studentDTO.ClassId;
                student.Class = classS;
                context.Students.Update(student);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
        }
        [HttpDelete("DeleteStudent")]
        public async Task<bool> DeleteStudent(int studentId)
        {
            var student = context.Students.Where(t => t.StudentId == studentId).FirstOrDefault();
            context.Students.Remove(student);
            if (context.SaveChanges() <= 0) return false;
            return true;
        }
    }
}