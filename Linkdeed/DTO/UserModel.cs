using Linkdeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.DTO
{
    public class UserModel
    {
        public int Id { get; set; }
        public string AddressMail { get; set; }
        public string AverageJobRate { get; set; }
        public string Username { get; set; }
    }
}