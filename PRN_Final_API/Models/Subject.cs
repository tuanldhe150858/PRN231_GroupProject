using System;
using System.Collections.Generic;

namespace PRN_Final_API.Models
{
    public partial class Subject
    {
        public Subject()
        {
            FileDetails = new HashSet<FileDetail>();
        }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int ClassId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual ICollection<FileDetail> FileDetails { get; set; }
    }
}
