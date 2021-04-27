using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Models
{
    public class ForgotPassword
    {
        [Required]
        public string Username { get; set; }
    }
}
