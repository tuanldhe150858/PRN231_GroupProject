using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace PRN_Final_Client.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private string ApiUrl = ""; 

        public LoginController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "http://localhost:5043/api";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountDTO teacher)
        {
            var url = ApiUrl + "/Login/Login/";
            HttpResponseMessage response = client
                .GetAsync(url + teacher.Email + "/" + teacher.Password)
                .GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string strData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                AccountDTO acc = JsonSerializer.Deserialize<AccountDTO>(strData, options);
                var claims = new[]
               {
                    new Claim(ClaimTypes.NameIdentifier, acc.Email),
                    new Claim(ClaimTypes.Role, acc.Role)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (acc.Role.Equals("0")) return RedirectToAction("Index", "Subjectnormal");
                else return RedirectToAction("Index", "Subject");
            }
            else
            {
                ViewData["msg"] = "Login fails";
                return View();
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> StudentLogin(AccountDTO student)
        //{
        //    var url = ApiUrl + "/Login/StudentLogin/";
        //    HttpResponseMessage response = client
        //        .GetAsync(url + student.Email + "/" + student.Password)
        //        .GetAwaiter().GetResult();
        //    AccountDTO t;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string strData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        //        var options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        };
        //        t = JsonSerializer.Deserialize<AccountDTO>(strData, options);

        //        HttpContext.Session.SetString("AccName", t.Email);

        //        return RedirectToAction("Index", "Subjectnormal");
        //    }
        //    else
        //    {
        //        ViewData["msg"] = "Login fails";
        //        return View();
        //    }
        //}
    }
}
