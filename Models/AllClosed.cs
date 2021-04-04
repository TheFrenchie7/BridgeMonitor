using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BridgeMonitor.Models
{
    public class AllClosed
    {
        public List<Boat> BoatsBefore { get; set; }
        public List<Boat> BoatsAfter { get; set; }
    }
}