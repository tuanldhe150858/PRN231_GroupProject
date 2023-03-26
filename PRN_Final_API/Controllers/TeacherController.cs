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

    }
}