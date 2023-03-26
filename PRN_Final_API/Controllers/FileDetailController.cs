using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PRN_Final_API.DTO;
using PRN_Final_API.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

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
    }
}
