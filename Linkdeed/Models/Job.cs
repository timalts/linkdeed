using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Models
{
    public class Job
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string JobName { get; set; }
        public int JobPayment { get; set; }
        public string JobDescription { get; set; }
        public string JobStatus { get; set; }
        public int EmployeeRate { get; set; }
    }
}
