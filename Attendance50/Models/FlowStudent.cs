using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance50.Models
{
    public class FlowStudent
    {
        public int StudentId { set; get; }
        public int FlowId { set; get; }
        //
        public Flow Flow { set; get; }
        public Student Student { set; get; }
    }
}
