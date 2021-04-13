using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Models
{
    public class UserBase
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string AddressMail { get; set; }
    }
}