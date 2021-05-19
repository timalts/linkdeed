using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Models
{
    public class EmployerDescription
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Description { get; set; }

        public int IsPremium { get; set; }
    }
}
