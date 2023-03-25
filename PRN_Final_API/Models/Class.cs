using System;
using System.Collections.Generic;

namespace PRN_Final_API.Models
{
    public partial class Class
    {
        public Class()
        {
            Students = new HashSet<Student>();
            Subjects = new HashSet<Subject>();
        }

        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
