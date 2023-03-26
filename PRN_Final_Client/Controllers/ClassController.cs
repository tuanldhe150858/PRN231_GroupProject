using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN_Final_Client.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class ClassController : Controller
    {
        private readonly HttpClient client = null;
        private string ClassApiUrl = "";
        public ClassController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ClassApiUrl = "http://localhost:5043/api/class";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ClassApiUrl + "/GetAllClass");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Class> classList = JsonSerializer.Deserialize<List<Class>>(strData, options);
            ViewData["Classlist"] = classList;
            return View();
        }
        public async Task<IActionResult> GoToAddPage()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5043/api/teacher/GetAllTeacher");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Teacher> teachersList = JsonSerializer.Deserialize<List<Teacher>>(strData, options);
            ViewData["TeachersList"] = teachersList;
            return View("./Create");
        }
        public async Task<IActionResult> Create(ClassDTO classDTO)
        {
            if (classDTO == null) return View("./Create");
            HttpResponseMessage response = await client.PostAsJsonAsync(ClassApiUrl + "/createClass", classDTO);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not add new class";
                return View();
            }
        }
        public async Task<IActionResult> GoToUpdatePage(int classId)
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5043/api/teacher/GetAllTeacher");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Teacher> teachersList = JsonSerializer.Deserialize<List<Teacher>>(strData, options);
            ViewData["TeachersList"] = teachersList;

            response = await client.GetAsync(ClassApiUrl + "/GetClassById?classId=" + classId);
            strData = await response.Content.ReadAsStringAsync();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Class classS = JsonSerializer.Deserialize<Class>(strData, options);
            ViewBag.Class = classS;
            return View("./Update");
        }
        public async Task<IActionResult> Update(ClassDTO classDTO)
        {
            if (classDTO == null) return View("./Update");
            HttpResponseMessage response = await client.PutAsJsonAsync(ClassApiUrl + "/UpdateClass", classDTO);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not update class";
                return View();
            }
        }
        public async Task<IActionResult> Delete(int classId)
        {
            HttpResponseMessage response = await client.DeleteAsync(ClassApiUrl + "/DeleteClass?classId=" + classId);
            return Redirect("/Class/Index");
        }
        public async Task<IActionResult> Search(string ClassId)
        {
            HttpResponseMessage response = null;
            if (ClassId == null || ClassId.Equals(""))
            {
                return Redirect("./Index");
            }
            else
            {
                int classId = int.Parse(ClassId);
                response = await client.GetAsync(ClassApiUrl + "/GetClassById?classId=" + classId);
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Class classS = JsonSerializer.Deserialize<Class>(strData, options);
                ViewBag.Search = classS;
                return View("./Index");
            }
        }
    }
}

