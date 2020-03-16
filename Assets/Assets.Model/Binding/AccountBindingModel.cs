using Assets.Model.Base;

namespace Assets.Model.Binding {

    public class SignupBindingModel: IBaseBindingModel {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string IMEI { get; set; }
        public string OS { get; set; }
    }

    public class SigninBindingModel: IBaseBindingModel {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
