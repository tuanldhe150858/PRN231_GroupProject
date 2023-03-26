using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.Models;
using System.Security.Claims;

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

        //[HttpGet]
        //[Route("{email}/{password}")]
        //public IActionResult StudentLogin(string email, string password)
        //{
        //    // Kiểm tra thông tin đăng nhập của student với database
        //    AccountDTO student = new AccountDTO
        //    {
        //        Email = email,
        //        Password = password
        //    };
        //    if (IsValidStudent(student))
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest(new { message = "Invalid username or password." });
        //    }
        //}

        [HttpGet]
        [Route("{email}/{password}")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var studentLogin = context.Students
               .FirstOrDefault(s => s.Mail.Equals(email) && s.Password.Equals(password));

            var teacher = context.Teachers
                .FirstOrDefault(t => t.Mail.Equals(email) && t.Password.Equals(password));
            // Kiểm tra thông tin đăng nhập của student với database
            if (studentLogin != null)
            {
                AccountDTO account = new AccountDTO
                {
                    Email = email,
                    Password = password,
                    Role = studentLogin.Role
                };

                return Ok(account);
            }
            else if (teacher != null)
            {
                AccountDTO account = new AccountDTO
                {
                    Email = email,
                    Password = password,
                    Role = teacher.Role
                };

                return Ok(account);
            }
            else
            {
                return BadRequest(new { message = "Invalid username or password." });
            }
        }

        //private bool IsValidStudent(AccountDTO student)
        //{
        //    if (student == null)
        //    {
        //        return false;
        //    }

        //    if (string.IsNullOrEmpty(student.Email) || string.IsNullOrEmpty(student.Password))
        //    {
        //        return false;
        //    }

        //    // Kiểm tra tính hợp lệ của email và password ở đây
        //    var studentLogin = context.Students
        //        .FirstOrDefault(s => s.Mail.Equals(student.Email) && s.Password.Equals(student.Password));
        //    if (studentLogin == null) return false;
        //    else return true;
        //}

        //private bool IsValidTeacher(AccountDTO teacher)
        //{
        //    if (teacher == null)
        //    {
        //        return false;
        //    }

        //    if (string.IsNullOrEmpty(teacher.Email) || string.IsNullOrEmpty(teacher.Password))
        //    {
        //        return false;
        //    }

        //    // Kiểm tra tính hợp lệ của email và password ở đây
        //    var studentLogin = context.Teachers
        //        .FirstOrDefault(s => s.Mail.Equals(teacher.Email) && s.Password.Equals(teacher.Password));
        //    if (studentLogin == null) return false;
        //    else return true;
        //}
    }
}
