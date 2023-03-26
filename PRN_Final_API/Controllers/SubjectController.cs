using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN_Final_API.Models;

namespace PRN_Final_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly final_projectContext context = new final_projectContext();

        [HttpGet]
        public IActionResult GetAllSubject()
        {
            return Ok(context.Subjects.ToList());
        }

        [HttpGet]
        [Route("{name}")]
        public IActionResult GetSubjectByName(string name)
        {
            var subject = context.Subjects.Where(s => s.SubjectName.Contains(name)).ToList();
            if (subject.Any())
            {
                return Ok(subject);
            } else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSubjectById(int id)
        {
            var subject = context.Subjects.Include(s => s.FileDetails).FirstOrDefault(s => s.SubjectId == id);
            if (subject == null) return NotFound();
            else return Ok(subject);
        }
    }
}
