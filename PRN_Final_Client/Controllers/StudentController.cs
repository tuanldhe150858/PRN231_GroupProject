using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN_Final_Client.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class StudentController : Controller
    {
        private readonly HttpClient client = null;
        private string StudentApiUrl = "";
        public StudentController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            StudentApiUrl = "http://localhost:5043/api/student";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(StudentApiUrl + "/GetAllStudents");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Student> studentsList = JsonSerializer.Deserialize<List<Student>>(strData, options);
            ViewData["StudentsList"] = studentsList;
            return View();
        }
        public async Task<IActionResult> GoToAddPage()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5043/api/class/GetAllClass");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Class> classesList = JsonSerializer.Deserialize<List<Class>>(strData, options);
            ViewData["ClassesList"] = classesList;
            return View("./Create");
        }
        public async Task<IActionResult> Create(StudentDTO studentDTO)
        {
            if (studentDTO == null) return View("./Create");
            HttpResponseMessage response = await client.PostAsJsonAsync(StudentApiUrl + "/CreateStudent", studentDTO);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not add new student";
                return View();
            }
        }
        public async Task<IActionResult> GoToUpdatePage(int studentId)
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5043/api/class/GetAllClass");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Class> classesList = JsonSerializer.Deserialize<List<Class>>(strData, options);
            ViewData["ClassesList"] = classesList;

            response = await client.GetAsync(StudentApiUrl + "/GetStudentById?studentId=" + studentId);
            strData = await response.Content.ReadAsStringAsync();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Student student = JsonSerializer.Deserialize<Student>(strData, options);
            ViewBag.Student = student;
            return View("./Update");
        }
        public async Task<IActionResult> Update(StudentDTO studentDTO)
        {
            if (studentDTO == null) return View("./Update");
            HttpResponseMessage response = await client.PutAsJsonAsync(StudentApiUrl + "/UpdateStudent", studentDTO);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not update student";
                return View();
            }
        }
        public async Task<IActionResult> Delete(int studentId)
        {
            HttpResponseMessage response = await client.DeleteAsync(StudentApiUrl + "/DeleteStudent?studentId=" + studentId);
            return Redirect("/Student/Index");
        }
        public async Task<IActionResult> Search(string StudentId)
        {
            HttpResponseMessage response = null;
            if (StudentId == null || StudentId.Equals(""))
            {
                return Redirect("./Index");
            }
            else
            {
                int studentId = int.Parse(StudentId);
                response = await client.GetAsync(StudentApiUrl + "/GetStudentById?studentId=" + studentId);
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Student student = JsonSerializer.Deserialize<Student>(strData, options);
                ViewBag.Search = student;
                return View("./Index");
            }
        }
    }
}

