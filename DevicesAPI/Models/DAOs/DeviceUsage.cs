using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesAPI.Models.DAOs
{
    public class DeviceUsage
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public DateTime Date { get; set; }
        public double Metric1 { get; set; }
        public double Metric2 { get; set; }
        public double Metric3 { get; set; }
    }
}
