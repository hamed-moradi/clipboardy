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
        public string DeviceType { get; set; }
    }

    public class SigninBindingModel: IBaseBindingModel {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }

    public class ForgotPasswordBindingModel: IBaseBindingModel {
        public string Username { get; set; }
    }

    public class ChangePasswordBindingModel: HeaderBindingModel {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangeForgotenPasswordBindingModel: IBaseBindingModel {
        public string Username { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
