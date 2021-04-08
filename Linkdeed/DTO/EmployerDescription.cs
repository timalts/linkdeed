using Linkdeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.DTO
{
    public class EmployerDescription
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Description { get; set; }
        public int IsPrenium { get; set; }
    }
}