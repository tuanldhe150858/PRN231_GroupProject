using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN_Final_Client.Controllers
{
    [Authorize(Policy = "Teacher")]
    public class SubjectController : Controller
    {
        private readonly HttpClient client = null;
        private string SubjectApiUrl = "";
        public SubjectController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            SubjectApiUrl = "http://localhost:5043/api/subject";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(SubjectApiUrl + "/GetAllSubjects");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Subject> subjectsList = JsonSerializer.Deserialize<List<Subject>>(strData, options);
            ViewData["SubjectsList"] = subjectsList;
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
        public async Task<IActionResult> Create(SubjectDTO SubjectDTO)
        {
            if (SubjectDTO == null) return View("./Create");
            HttpResponseMessage response = await client.PostAsJsonAsync(SubjectApiUrl + "/createSubject", SubjectDTO);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not add new subject";
                return View();
            }
        }
        public async Task<IActionResult> GoToUpdatePage(int subjectId)
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:5043/api/class/GetAllClass");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Class> classesList = JsonSerializer.Deserialize<List<Class>>(strData, options);
            ViewData["ClassesList"] = classesList;

            response = await client.GetAsync(SubjectApiUrl + "/GetSubjectById?subjectId=" + subjectId);
            strData = await response.Content.ReadAsStringAsync();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Subject subject = JsonSerializer.Deserialize<Subject>(strData, options);
            ViewBag.Subject = subject;
            return View("./Update");
        }
        public async Task<IActionResult> Update(SubjectDTO SubjectDTO)
        {
            if (SubjectDTO == null) return View("./Update");
            HttpResponseMessage response = await client.PutAsJsonAsync(SubjectApiUrl + "/UpdateSubject", SubjectDTO);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("./Index");
            }
            else
            {
                ViewBag.Message = "Can not update subject";
                return View();
            }
        }
        public async Task<IActionResult> Delete(int subjectId)
        {
            HttpResponseMessage response = await client.DeleteAsync(SubjectApiUrl + "/DeleteSubject?subjectId=" + subjectId);
            return Redirect("/Subject/Index");
        }
        public async Task<IActionResult> Search(string SubjectId)
        {
            HttpResponseMessage response = null;
            if (SubjectId == null || SubjectId.Equals(""))
            {
                return Redirect("./Index");
            }
            else
            {
                int subjectId = int.Parse(SubjectId);
                response = await client.GetAsync(SubjectApiUrl + "/GetSubjectById?subjectId=" + subjectId);
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Subject subject = JsonSerializer.Deserialize<Subject>(strData, options);
                ViewBag.Search = subject;
                return View("./Index");
            }
        }
    }
}