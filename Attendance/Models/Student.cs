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
        public int GroupId { set; get; }
        //
        public Group Group { set; get; }
        public List<MeetStudent> MeetStudents { set; get; }

        [NotMapped]
        public bool IsPresent { set; get; }
        [NotMapped]
        public string ReverseName
        {
            get
            {
                if (Nick == null) 
                    return null;
                var ss = Nick.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return ss.Length == 1 ? ss[0] : $"{ss[1]} {ss[0]}";
            }
        }
    }

}
