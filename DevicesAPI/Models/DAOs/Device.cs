﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesAPI.Models.DAOs
{
    public class Device
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public double Temperature { get; set; }
        public Status Status { get; set; }
        public string Icon { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<DeviceUsage> DeviceUsages { get; set; }
    }

}
