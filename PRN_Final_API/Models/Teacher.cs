using System;
using System.Collections.Generic;

namespace PRN_Final_API.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Classes = new HashSet<Class>();
        }

        public int TeacherId { get; set; }
        public string Mail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ICollection<Class> Classes { get; set; }
    }
}
