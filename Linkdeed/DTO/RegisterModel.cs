using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.DTO
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string AddressMail { get; set; }
        [Required]
        public string AccesLevel { get; set; }
        [Required]
        public int AverageJobRate { get; set; }

    }
}
