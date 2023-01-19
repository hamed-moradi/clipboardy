using System;

namespace Assets.Model.Header {
  public class AccountHeaderModel {
    public AccountHeaderModel() { }

    public AccountHeaderModel(int id, int deviceId, string username, DateTime? lastSignedinAt = null) {
      Id = id;
      DeviceId = deviceId;
      Username = username;
      LastSignedinAt = lastSignedinAt;
    }

    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string Username { get; set; }
    public DateTime? LastSignedinAt { get; set; }
  }
}
