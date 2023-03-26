using Microsoft.AspNetCore.Mvc;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN_Final_Client.Controllers
{
    public class SubjectController : Controller
    {
        private readonly HttpClient client = null;
        private string SubjectApiUrl = "";
        private string ApiUrl = "";
        public SubjectController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            SubjectApiUrl = "http://localhost:5043/api/subject";
            ApiUrl = "http://localhost:5043/api";
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
                return View(subjects);
            }
            else
            {
                return NoContent();
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
                return files;
            }
            else { return null; }
        }

        public async Task<IActionResult> Download(string filename)
        {
            var url = ApiUrl + "/filedetail/downloadfile/";
            HttpResponseMessage response = client
                .GetAsync(url + filename)
                .GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var filestream = await response.Content.ReadAsStreamAsync();
                return File(filestream, "application/octet-stream", filename);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var formContent = new MultipartFormDataContent();
            formContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
            var url = ApiUrl + "/filedetail/uploadfile";
            HttpResponseMessage response = client
                .PostAsync(url, formContent)
                .GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                //var filestream = await response.Content.ReadAsStreamAsync();
                //return File(filestream, "application/octet-stream", filename);
                return RedirectToAction("Index", "Subject");
            }
            else
            {
                return NoContent();
            }
        }

    }
}