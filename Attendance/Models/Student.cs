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
        public string Nick { set; get; }
        public string Group { set; get; }
        //
        public List<MeetStudent> MeetStudents { set; get; }

        [NotMapped]
        public bool IsPresent { set; get; }

        public string ReverseName {
            get
            {
                var ss = Nick.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return ss.Length == 1 ? ss[0] : $"{ss[1]} {ss[0]}";
            }
        }       
    }

    public class StudentComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y) =>
            x.Nick == y.Nick && x.Group == y.Group;

        public int GetHashCode([DisallowNull] Student obj) =>
            obj.GetHashCode();
    }
}
