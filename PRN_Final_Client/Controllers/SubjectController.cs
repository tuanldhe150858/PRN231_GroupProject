using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN_Final_Client.Controllers
{
    public class SubjectController : Controller
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";

        public SubjectController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "http://localhost:5043/api";
        }
        public async Task<IActionResult> Index()
        {
            var url = ApiUrl + "/subject/GetAllSubjects";
            HttpResponseMessage response = await client
                .GetAsync(url);
            //     .GetAwaiter().GetResult();
            List<Subject> subjects = new List<Subject>();
            if (response.IsSuccessStatusCode)
            {
                string strData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                subjects = JsonSerializer.Deserialize<List<Subject>>(strData, options);
                return View(subjects);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchSubjectByName(string search)
        {
            var url = ApiUrl + "/subject/GetSubjectByName/";
            HttpResponseMessage response = client
                .GetAsync(url + search)
                .GetAwaiter().GetResult();
            List<Subject> subjects = new List<Subject>();

            if (response.IsSuccessStatusCode)
            {
                string strData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                subjects = JsonSerializer.Deserialize<List<Subject>>(strData, options);
                if (subjects.Any()) return View(subjects);
                else return View();
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var url = ApiUrl + "/subject/GetSubjectById/";
            HttpResponseMessage response = client
                .GetAsync(url + id)
                .GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                string strData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Subject subject = JsonSerializer.Deserialize<Subject>(strData, options);
                ViewData["file"] = GetFileBySybjectID(id);
                return View(subject);
            }
            else
            {
                return View();
            }
        }

        public List<FileDetail> GetFileBySybjectID(int id)
        {
            var url = ApiUrl + "/filedetail/GetFilesBySubjectId/";
            HttpResponseMessage response = client
                .GetAsync(url + id)
                .GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string strData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<FileDetail> files = JsonSerializer.Deserialize<List<FileDetail>>(strData, options);
                //ViewData["file"] = subject.FileDetails;
                return files;
            }
            else { return null; }
        }

    }
}
