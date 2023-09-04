using System;

namespace Assets.Model.Header
{
    public class AccountHeaderModel
    {
        public AccountHeaderModel() { }

        public AccountHeaderModel(int id, int deviceId, string username, DateTime? lastSignedinAt = null, bool rememberMe = false)
        {
            Id = id;
            DeviceId = deviceId;
            Username = username;
            LastSignedinAt = lastSignedinAt;
            RememberMe = rememberMe;
        }

        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string Username { get; set; }
        public DateTime? LastSignedinAt { get; set; }
        public bool RememberMe { get; set; }
    }
}
