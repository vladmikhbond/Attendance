using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class MeetStudent
    {
        public int MeetId { set; get; }
        public int StudentId { set; get; }
        [Required]
        public bool IsPresent { set; get; }
        //
        public Meet Meet { set; get; }
        public Student Student { set; get; }
    }

    public class MeetStudentViewModel
    {
        public int MeetId { set; get; }
        public int StudentId { set; get; }
        public string IsPresent { set; get; }
    }

}
