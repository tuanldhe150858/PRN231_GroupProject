using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

namespace PRN_Final_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly final_projectContext context;
        public SubjectController(final_projectContext context)
        {
            this.context = context;
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
                    c.Class,
                    FileDetails = c.FileDetails.Select(s => new
                    {
                        s.FileId,
                        s.FileName,
                        s.FilePath
                    }).ToList()
                }).ToList();
            return Ok(subjects);
        }
        [HttpGet("GetSubjectByName")]
        public async Task<IActionResult> GetSubjectByName(string subjectName)
        {
            var subject = context.Subjects
                .Where(s => s.SubjectName.Equals(subjectName))
                .Select(c => new
                {
                    c.SubjectId,
                    c.SubjectName,
                    c.ClassId,
                    c.Class,
                    FileDetails = c.FileDetails.Select(s => new
                    {
                        s.FileId,
                        s.FileName,
                        s.FilePath
                    }).ToList()
                }).FirstOrDefault();
            if (subject != null)
            {
                return Ok(subject);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetSubjectById")]
        public IActionResult GetSubjectById(int subjectId)
        {
            var subject = context.Subjects
                .Where(s => s.SubjectId == subjectId)
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
            if (subject != null)
            {
                return Ok(subject);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetSubjectsByClassId")]
        public IActionResult GetSubjectOfClass(int classId)
        {
            var subjects = context.Subjects
                .Where(s => s.ClassId == classId)
                .Select(c => new
                {
                    c.SubjectId,
                    c.SubjectName,
                    c.ClassId,
                    c.Class,
                    FileDetails = c.FileDetails.Select(s => new
                    {
                        s.FileId,
                        s.FileName,
                        s.FilePath
                    }).ToList()
                }).ToList();
            if (subjects != null)
            {
                return Ok(subjects);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateSubject")]
        public async Task<bool> UpdateSubject(SubjectDTO subjectDTO)
        {
            var classS = context.Classes.Where(t => t.ClassId == subjectDTO.ClassId).FirstOrDefault();
            var subject = context.Subjects.Where(s => s.SubjectId == subjectDTO.SubjectId).FirstOrDefault();
            if (subject != null)
            {
                subject.SubjectName = subjectDTO.SubjectName;
                subject.ClassId = subjectDTO.ClassId;
                subject.Class = classS;
                context.Subjects.Update(subject);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("DeleteSubject")]
        public async Task<bool> DeleteSubject(int subjectId)
        {
            var subject1 = context.Subjects
            .Where(s => s.SubjectId == subjectId)
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

            Subject subject2 = context.Subjects.Where(t => t.SubjectId == subjectId).FirstOrDefault();
            context.Subjects.Remove(subject2);
            if (context.SaveChanges() <= 0) return false;
            return true;
        }
    }
}
