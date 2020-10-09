
using System;

namespace Assets.Model.Common {
    public class HttpDeviceHeader {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }

    public class HttpAccountHeader {
        public int? Id { get; set; }
        public string Username { get; set; }
        public DateTime? LastSignedinAt { get; set; }
        public string DeviceId { get; set; }
    }
}
