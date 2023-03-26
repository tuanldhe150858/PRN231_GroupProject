using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
﻿using Microsoft.AspNetCore.Http;

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
        [HttpGet("GetClassByClassName/{className}")]
        public IActionResult GetClassByName(string className)
        {
            var classS = context.Classes.Where(c => c.ClassName.Equals(className)).FirstOrDefault();
            if(classS != null)
            {
                return Ok(classS);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetClassesByTeacherId/{teacherId}")]
        public IActionResult GetClassByTeacherId(int teacherId)
        {
            var classes = context.Classes.Where(c => c.TeacherId == teacherId).ToList();
            if(classes != null)
            {
                return Ok(classes);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
