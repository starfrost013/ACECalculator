using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACECalculator
{
    public class StormIntensityNode
    {
        public DateTime DateTime { get; set; }
        public double Intensity { get; set; }
        public double ACE { get; set; }
        public double Total { get; set; }
    }
}
