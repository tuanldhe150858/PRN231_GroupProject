using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN_Final_Client.Controllers
{
    public class TeacherController : Controller
    {
        private readonly HttpClient client = null;
        private string TeacherApiUrl = "";
        public TeacherController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            TeacherApiUrl = "http://localhost:5043/api/teacher";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(TeacherApiUrl + "/GetAllTeacher");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Teacher> teachersList = JsonSerializer.Deserialize<List<Teacher>>(strData, options);
            ViewData["TeachersList"] = teachersList;
            return View();
        }
        public async Task<IActionResult> GoToAddPage()
        {
            return View("./Create");
        }
        public async Task<IActionResult> Create(TeacherDTO teacherDTO)
        {
            if (teacherDTO == null) return View("./Create");
            HttpResponseMessage response = await client.PostAsJsonAsync(TeacherApiUrl + "/createTeacher", teacherDTO);
            if(response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not add new teacher";
                return View();
            }
        }
        public async Task<IActionResult> GoToUpdatePage(int teacherId)
        {
            HttpResponseMessage response = await client.GetAsync(TeacherApiUrl + "/getTeacherById?teacherId=" + teacherId);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Teacher teacher = JsonSerializer.Deserialize<Teacher>(strData, options);
            ViewBag.Teacher = teacher;
            return View("./Update");
        }
        public async Task<IActionResult> Update(TeacherDTO teacherDTO)
        {
            if (teacherDTO == null) return View("./Update");
            HttpResponseMessage response = await client.PutAsJsonAsync(TeacherApiUrl + "/UpdateTeacher", teacherDTO);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not update teacher";
                return View();
            }
        }
        public async Task<IActionResult> Delete(int teacherId)
        {
            HttpResponseMessage response = await client.DeleteAsync(TeacherApiUrl + "/DeleteTeacher?teacherId=" + teacherId);
            return Redirect("/Teacher/Index");
        }
        public async Task<IActionResult> Search(string TeacherId)
        {
            HttpResponseMessage response = null;
            if (TeacherId == null || TeacherId.Equals(""))
            {
                return Redirect("./Index");
            }
            else
            {
                int teacherId = int.Parse(TeacherId);
                response = await client.GetAsync(TeacherApiUrl + "/getTeacherById?teacherId=" + teacherId);
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Teacher teacher = JsonSerializer.Deserialize<Teacher>(strData, options);
                ViewBag.Search = teacher;
                return View("./Index");
            }
        }
    }
}
