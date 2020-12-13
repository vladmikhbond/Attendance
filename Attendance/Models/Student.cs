using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.Models
{
    [Table("Students")]
    public class Student
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string Surname { set; get; }
        public string Patronim { set; get; }
        public string Email { set; get; }
        public string GroupName { set; get; }
        [NotMapped]
        public bool IsPresent { set; get; }

    }
}
