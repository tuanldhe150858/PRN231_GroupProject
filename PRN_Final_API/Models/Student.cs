using System;
using System.Collections.Generic;

namespace PRN_Final_API.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string Mail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int ClassId { get; set; }

        public virtual Class Class { get; set; } = null!;
    }
}
