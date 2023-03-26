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
        public async Task<IActionResult> CreateTeacher(Teacher teacher)
        {
            teacher.Role = "teacher";
            context.Teachers.Add(teacher);
            context.SaveChanges();
            return Ok();
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
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var teacher = context.Teachers
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
            return Ok(teacher);
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
        [HttpGet("getAllClass")]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = context.Classes
                .Select(c => new
                {
                    c.ClassId,
                    c.ClassName,
                    c.TeacherId,
                    Subject = c.Subjects.Select(s => new
                    {
                        s.SubjectId,
                        s.SubjectName
                    }).ToList(),
                    Student = c.Students.Select(s => new
                    {
                        s.StudentId,
                        s.Mail
                    })
                }).ToList();
            return Ok(classes);
        }
        [HttpPost("createSubject")]
        public async Task<IActionResult> CreateSubject(SubjectDTO subjectDTO)
        {
            var classS = context.Classes.Where(t => t.ClassId == subjectDTO.ClassId).FirstOrDefault();
            if (classS != null)
            {
                Subject newSubject = new Subject
                {
                    SubjectName = subjectDTO.SubjectName,
                    ClassId = subjectDTO.ClassId,
                    Class = classS
                };
                context.Subjects.Add(newSubject);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = context.Subjects
                .Select(c => new
                {
                    c.SubjectId,
                    c.SubjectName,
                    c.ClassId,
                    FileDetail = c.FileDetails.Select(s => new
                    {
                        s.FileId,
                        s.FileName,
                        s.FilePath
                    }).ToList()
                }).ToList();
            return Ok(subjects);
        }
        [HttpPost("CreateFileDetails")]
        public async Task<IActionResult> CreateFile(FileDetailDTO fileDetailDTO)
        {
            var subject = context.Subjects.Where(t => t.SubjectId == fileDetailDTO.SubjectId).FirstOrDefault();
            if (subject != null)
            {
                FileDetail newFile = new FileDetail
                {
                    FileName = fileDetailDTO.FileName,
                    FilePath = fileDetailDTO.FilePath,
                    SubjectId = fileDetailDTO.SubjectId,
                    Subject = subject
                };
                context.FileDetails.Add(newFile);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllFiles")]
        public async Task<IActionResult> GetAllFile()
        {
            var files = context.FileDetails.ToList();
            return Ok(files);
        }
        [HttpGet("GetFilesBySubjectId/{subjectId}")]
        public async Task<IActionResult> GetFilesBySubjectId(int subjectId)
        {
            var files = context.FileDetails.Where(f => f.SubjectId== subjectId).ToList();
            return Ok(files);
        }
        [HttpGet("GetFileById/{fileId}")]
        public async Task<IActionResult> GetFileById(int fileId)
        {
            var file = context.FileDetails.Where(f => f.FileId == fileId).FirstOrDefault();
            if(file != null)
            {
                return Ok(file);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut("updateFile")]
        public bool UpdateFile(int fileId, FileDetailDTO fileDetailDTO)
        {
            FileDetail fileUpdate = context.FileDetails.Where(f => f.FileId == fileId).FirstOrDefault();
            if(fileUpdate != null)
            {
                fileUpdate.FileName = fileDetailDTO.FileName;
                fileUpdate.FilePath = fileDetailDTO.FilePath;
                context.FileDetails.Update(fileUpdate);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("DeleteFileById")]
        public bool DeleteFile(int fileId)
        {
            var file = context.FileDetails.Where(f => f.FileId== fileId).FirstOrDefault();
            if(file != null)
            {
                context.FileDetails.Remove(file);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
