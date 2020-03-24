﻿using Assets.Model.Base;

namespace Assets.Model.Common {
    public class AccountHeader: IBaseBindingModel {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int ProfileId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; }
        public string Phone { get; set; }
        public bool ConfirmedPhone { get; set; }
        public AccountProvider ProviderId { get; set; }
    }
}
