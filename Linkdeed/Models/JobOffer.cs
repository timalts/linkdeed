using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Models
{
    public class JobOffer
    {
        public int Id { get; set; }
        public Job Job { get; set; }
        public string OfferStatus { get; set; }
    }
}
