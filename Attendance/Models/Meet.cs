using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class Meet
    {
        public int Id { set; get; }
        public string UserName { set; get; }
        public DateTime When { set; get; }
        public string Comment { set; get; }
        //
        public List<MeetStudent> MeetStudents { set; get; }
    }

}
