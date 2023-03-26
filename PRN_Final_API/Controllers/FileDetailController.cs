using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace PRN_Final_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDetailController : ControllerBase
    {
        private readonly final_projectContext context;
        public FileDetailController(final_projectContext context)
        {
            this.context = context;
        }
        
        [HttpPost("CreateFileDetails")]
        public async Task<IActionResult> CreateFile(FileDetailDTO fileDetailDTO)
        {
            var subject = context.Subjects.Where(t => t.SubjectId == fileDetailDTO.SubjectId).FirstOrDefault();
            if (subject != null)
            {
                FileDetail newFile = new FileDetail
                {
                    FileName = fileDetailDTO.FileName,
                    FilePath = fileDetailDTO.FilePath,
                    SubjectId = fileDetailDTO.SubjectId,
                    Subject = subject
                };
                context.FileDetails.Add(newFile);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllFiles")]
        public async Task<IActionResult> GetAllFile()
        {
            var files = context.FileDetails.ToList();
            return Ok(files);
        }
        [HttpGet("GetFilesBySubjectId/{subjectId}")]
        public async Task<IActionResult> GetFilesBySubjectId(int subjectId)
        {
            var files = context.FileDetails.Where(f => f.SubjectId== subjectId).ToList();
            return Ok(files);
        }
        [HttpGet("GetFileById/{fileId}")]
        public async Task<IActionResult> GetFileById(int fileId)
        {
            var file = context.FileDetails.Where(f => f.FileId == fileId).FirstOrDefault();
            if(file != null)
            {
                return Ok(file);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut("updateFile")]
        public bool UpdateFile(int fileId, FileDetailDTO fileDetailDTO)
        {
            FileDetail fileUpdate = context.FileDetails.Where(f => f.FileId == fileId).FirstOrDefault();
            if(fileUpdate != null)
            {
                fileUpdate.FileName = fileDetailDTO.FileName;
                fileUpdate.FilePath = fileDetailDTO.FilePath;
                context.FileDetails.Update(fileUpdate);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("DeleteFileById")]
        public bool DeleteFile(int fileId)
        {
            var file = context.FileDetails.Where(f => f.FileId== fileId).FirstOrDefault();
            if(file != null)
            {
                context.FileDetails.Remove(file);
                if (context.SaveChanges() <= 0) return false;
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await WriteFile(file);
            FileDetail newFile = new FileDetail
            {
                FileName = "test2",
                FilePath = file.FileName,
                SubjectId = 3,
                Subject = null
            };
            context.FileDetails.Add(newFile);
            context.SaveChanges();
            return Ok(result);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {
                //var extension = "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                //filename = DateTime.Now.Ticks.ToString() + extension;
                filename = file.FileName;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var exactpath = Path.Combine(filePath,filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            } catch (Exception ex)
            {

            }
            return filename;
        }

        [HttpGet]
        [Route("downloadfile/{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", filename);

            var file = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(file, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}
