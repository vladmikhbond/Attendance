using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance50.Models
{
    public class Check
    {
        public int Id { set; get; }
        public int Flow { set; get; }  // owner
        public DateTime When { set; get; }
        public string Comment { set; get; }
        //
        public List<CheckStudent> MeetStudents { set; get; }
    }

}
