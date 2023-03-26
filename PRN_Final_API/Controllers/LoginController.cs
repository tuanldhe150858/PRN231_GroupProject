using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.Models;

namespace PRN_Final_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly final_projectContext context;
        public LoginController(final_projectContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("{email}/{password}")]
        public IActionResult StudentLogin(string email, string password)
        {
            // Kiểm tra thông tin đăng nhập của student với database
            AccountDTO student = new AccountDTO
            {
                Email = email,
                Password = password
            };
            if (IsValidStudent(student))
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { message = "Invalid username or password." });
            }
        }

        [HttpGet]
        [Route("{email}/{password}")]
        public IActionResult TeacherLogin(string email, string password)
        {
            AccountDTO teacher = new AccountDTO
            {
                Email = email,
                Password = password
            };
            // Kiểm tra thông tin đăng nhập của student với database
            if (IsValidTeacher(teacher))
            {
                return Ok(teacher);
            }
            else
            {
                return BadRequest(new { message = "Invalid username or password." });
            }
        }

        private bool IsValidStudent(AccountDTO student)
        {
            if (student == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(student.Email) || string.IsNullOrEmpty(student.Password))
            {
                return false;
            }

            // Kiểm tra tính hợp lệ của email và password ở đây
            var studentLogin = context.Students
                .FirstOrDefault(s => s.Mail.Equals(student.Email) && s.Password.Equals(student.Password));
            if (studentLogin == null) return false;
            else return true;
        }

        private bool IsValidTeacher(AccountDTO teacher)
        {
            if (teacher == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(teacher.Email) || string.IsNullOrEmpty(teacher.Password))
            {
                return false;
            }

            // Kiểm tra tính hợp lệ của email và password ở đây
            var studentLogin = context.Teachers
                .FirstOrDefault(s => s.Mail.Equals(teacher.Email) && s.Password.Equals(teacher.Password));
            if (studentLogin == null) return false;
            else return true;
        }
    }
}
