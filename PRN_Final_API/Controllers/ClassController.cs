using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN_Final_API.Models;

namespace PRN_Final_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly final_projectContext context = new final_projectContext();
        
        [HttpGet]
        public IActionResult GetAllClass()
        {
            return Ok(context.Classes.ToList());
        }

        [HttpGet]
        [Route("{name}")]
        public IActionResult GetClassByName(string name)
        {
            var classes = context.Classes.Include(c => c.Subjects)
                .Where(s => s.ClassName.Equals(name)).ToList();
            if (classes.Any()) return Ok(classes);
            else return NotFound();
        }

        [HttpGet]
        [Route("{teacherId}")]
        public IActionResult GetClassByTeacherId(int teacherId)
        {
            var classOfTeacher = context.Classes.Include(c => c.Subjects)
                .Where(c => c.TeacherId == teacherId).ToList();
            if (classOfTeacher.Any()) return Ok(classOfTeacher);
            else return NotFound();
        }
    }
}
