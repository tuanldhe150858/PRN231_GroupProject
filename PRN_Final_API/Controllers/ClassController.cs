using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace PRN_Final_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly final_projectContext context;
        public ClassController(final_projectContext context)
        {
            this.context = context;
        }

        [HttpPost("createClass")]
        public async Task<IActionResult> CreateClass(ClassDTO classDTO)
        {
            /*int teacherId = int.Parse(collection["teacherId"]);
            var teacher = context.Teachers.Where(t => t.TeacherId== teacherId).FirstOrDefault();
            if(teacher == null)
            {
                return BadRequest();
            }
            else
            {
                Class newClass = new Class
                {
                    ClassName = collection["className"],
                    TeacherId = teacherId,
                    Teacher = teacher
                };
                context.Classes.Add(newClass);
                context.SaveChanges();
                return Ok();
            }*/
            var teacher = context.Teachers.Where(t => t.TeacherId == classDTO.TeacherId).FirstOrDefault();
            if (teacher != null)
            {
                Class newClass = new Class
                {
                    ClassName = classDTO.ClassName,
                    TeacherId = classDTO.TeacherId,
                    Teacher = teacher
                };
                context.Classes.Add(newClass);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllClass")]
        public async Task<IActionResult> GetAllClass()
        {
            var classes = context.Classes
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
                }).ToList();
            return Ok(classes);
        }
        [HttpGet("GetClassByClassName")]
        public IActionResult GetClassByName(string className)
        {
            var classS = context.Classes
                .Where(c => c.ClassName.Equals(className))
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
            return Ok(classS);
        }
        [HttpGet("GetClassById")]
        public IActionResult GetClassById(int classId)
        {
            var classS = context.Classes
                .Where(c => c.ClassId == classId)
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
            return Ok(classS);
        }
        [HttpPut("UpdateClass")]
        public async Task<bool> UpdateClass(ClassDTO classDTO)
        {
            var teacher = context.Teachers.Where(t => t.TeacherId == classDTO.TeacherId).FirstOrDefault();
            var classS = context.Classes.Where(t => t.ClassId == classDTO.ClassId).FirstOrDefault();
            if (classS == null)
            {
                return false;
            }
            else
            {
                classS.ClassName = classDTO.ClassName;
                classS.TeacherId = classDTO.TeacherId;
                classS.Teacher = teacher;
                context.Classes.Update(classS);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
        }
        [HttpDelete("DeleteClass")]
        public async Task<bool> DeleteClass(int classId)
        {
            var class1 = context.Classes
                .Where(c => c.ClassId == classId)
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

            Class class2 = context.Classes.Where(t => t.ClassId == classId).FirstOrDefault();
            context.Classes.Remove(class2);
            if (context.SaveChanges() <= 0) return false;
            return true;
        }
        [HttpGet("GetClassesByTeacherId")]
        public IActionResult GetClassByTeacherId(int teacherId)
        {
            var classes = context.Classes
                .Where(c => c.TeacherId == teacherId)
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
                }).ToList();
            return Ok(classes);
        }
    }
}
