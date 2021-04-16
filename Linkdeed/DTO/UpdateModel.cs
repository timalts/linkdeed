using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.DTO
{
    public class UpdateModel
    {
        public string AddressMail { get; set; }
        public int AverageJobRate { get; set; }
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
