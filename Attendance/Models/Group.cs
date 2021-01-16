using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class Group
    {
        public int Id { set; get; }
        public string Name { set; get; }
        //
        public List<Student> Students { set; get; }
    }

}
