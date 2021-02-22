using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance50.Models
{
    public class Flow
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string UserName { set; get; }
    }
}
