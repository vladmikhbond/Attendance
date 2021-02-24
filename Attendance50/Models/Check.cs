using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance50.Models
{
    public class Check
    {
        public int Id { set; get; }
        public int FlowId { set; get; }  
        public DateTime When { set; get; }
        //
        public List<CheckStudent> CheckStudents { set; get; }
        public Flow Flow { set; get; }
    }

}
