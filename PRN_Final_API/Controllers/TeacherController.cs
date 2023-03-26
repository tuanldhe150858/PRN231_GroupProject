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
    public class TeacherController : ControllerBase
    {
        private readonly final_projectContext context;
        public TeacherController(final_projectContext context)
        {
            this.context = context;
        }
        [HttpPost("createTeacher")]
        public async Task<bool> CreateTeacher(TeacherDTO teacherDTO)
        {
            Teacher teacher = new Teacher
            {
                Mail = teacherDTO.Mail,
                Password = teacherDTO.Password,
                Role = "teacher"
            };
            context.Teachers.Add(teacher);
            if (context.SaveChanges() <= 0) return false;
            return true;
        }
        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeacher()
        {
            var teachers = context.Teachers
                .Select(t => new
                {
                    t.TeacherId,
                    t.Mail,
                    t.Password,
                    t.Role,
                    Classes = t.Classes.Select(c => new
                    {
                        c.ClassId,
                        c.ClassName
                    }).ToList()
                }).ToList();
            return Ok(teachers);
        }
        [HttpGet("getTeacherById")]
        public async Task<IActionResult> GetTeacherById(int teacherId)
        {
            var teacher = context.Teachers
                .Where(t => t.TeacherId == teacherId)
                .Select(t => new
                {
                    t.TeacherId,
                    t.Mail,
                    t.Password,
                    t.Role,
                    Classes = t.Classes.Select(c => new
                    {
                        c.ClassId,
                        c.ClassName
                    }).ToList()
                }).FirstOrDefault();
            if (teacher == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(teacher);
            }
        }
        [HttpPut("UpdateTeacher")]
        public async Task<bool> UpdateTeacher(TeacherDTO teacherDTO)
        {
            var teacher = context.Teachers.Where(t => t.TeacherId == teacherDTO.TeacherId).FirstOrDefault();
            if (teacher == null)
            {
                return false;
            }
            else
            {
                teacher.Mail = teacherDTO.Mail;
                teacher.Password = teacherDTO.Password;
                context.Teachers.Update(teacher);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
        }
        [HttpDelete("DeleteTeacher")]
        public async Task<bool> DeleteTeacher(int teacherId)
        {
            var teacher1 = context.Teachers
                .Where(t => t.TeacherId == teacherId)
                .Select(t => new
                {
                    t.TeacherId,
                    t.Mail,
                    t.Password,
                    t.Role,
                    Classes = t.Classes.Select(c => new
                    {
                        c.ClassId,
                        c.ClassName
                    }).ToList()
                }).FirstOrDefault();
            foreach (var i in teacher1.Classes)
            {
                var class1 = context.Classes
                .Where(c => c.ClassId == i.ClassId)
                .Select(c => new
                {
                    c.ClassId,
                    c.ClassName,
                    c.TeacherId,
                    c.Teacher,
                    Students = c.Students.Select(s => new
                    {
                        s.StudentId,
                        s.Mail
                    }).ToList(),
                    Subjects = c.Subjects.Select(s => new
                    {
                        s.SubjectId,
                        s.SubjectName
                    }).ToList()
                }).FirstOrDefault();
                foreach (var j in class1.Students)
                {
                    Student student = context.Students.Where(s => s.StudentId == j.StudentId).FirstOrDefault();
                    context.Students.Remove(student);
                }
                foreach (var k in class1.Subjects)
                {
                    var subject1 = context.Subjects
                .Where(s => s.SubjectId == k.SubjectId)
                .Select(s => new
                {
                    s.SubjectId,
                    s.SubjectName,
                    s.ClassId,
                    s.Class,
                    FileDetails = s.FileDetails.Select(f => new
                    {
                        f.FileId,
                        f.FileName,
                        f.FilePath
                    }).ToList()
                }).FirstOrDefault();
                    foreach (var l in subject1.FileDetails)
                    {
                        FileDetail file = context.FileDetails.Where(f => f.FileId == l.FileId).FirstOrDefault();
                        context.FileDetails.Remove(file);
                    }
                    Subject subject2 = context.Subjects.Where(s => s.SubjectId == k.SubjectId).FirstOrDefault();
                    context.Subjects.Remove(subject2);
                }

                Class class2 = context.Classes.Where(c => c.ClassId == i.ClassId).FirstOrDefault();
                context.Classes.Remove(class2);
            }
            Teacher teacher2 = context.Teachers.Where(t => t.TeacherId == teacherId).FirstOrDefault();
            context.Teachers.Remove(teacher2);
            if (context.SaveChanges() <= 0) return false;
            return true;
        }
    }
}