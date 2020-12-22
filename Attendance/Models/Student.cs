using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class Student
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string Surname { set; get; }
        public string Patronim { set; get; }
        public string Email { set; get; }
        public string Group { set; get; }

        [NotMapped]
        public bool IsPresent { set; get; }
        [NotMapped]
        public string NameSurname { get => $"{Name} {Surname}"; }

    }

    public class StudentComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y) =>
            x.Name == y.Name && x.Surname == y.Surname;

        public int GetHashCode([DisallowNull] Student obj) =>
            obj.GetHashCode();
    }
}
