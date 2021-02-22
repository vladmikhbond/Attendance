using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance50.Models
{
    public class Student
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string Surname { set; get; }
        public string Nick { set; get; }
        //
        public Group Group { set; get; }
        public List<CheckStudent> CheckStudents { set; get; }

        [NotMapped]
        public bool IsPresent { set; get; }
    }

}
