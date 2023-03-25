using System;
using System.Collections.Generic;

namespace PRN_Final_API.Models
{
    public partial class FileDetail
    {
        public int FileId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; } = null!;
    }
}
