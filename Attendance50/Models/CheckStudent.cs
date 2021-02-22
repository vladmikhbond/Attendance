using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance50.Models
{
    public class CheckStudent
    {
        public int CheckId { set; get; }
        public int StudentId { set; get; }
        [Required]
        public bool IsPresent { set; get; }
        //
        public Check Check { set; get; }
        public Student Student { set; get; }
    }

}
