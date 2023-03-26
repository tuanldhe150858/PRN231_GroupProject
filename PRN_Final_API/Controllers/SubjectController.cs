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
                    FileDetail = c.FileDetails.Select(s => new
                    {
                        s.FileId,
                        s.FileName,
                        s.FilePath
                    }).ToList()
                }).ToList();
            return Ok(subjects);
        }
    }
}
