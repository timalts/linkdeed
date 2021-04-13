using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Models
{
    public class User : UserBase
    {
        public string AccesLevel { get; set; }
        public int AverageJobRate { get; set; }
    }
}
