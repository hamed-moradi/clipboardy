using Assets.Model.Base;

namespace Assets.Model.Binding
{

    public class SignupBindingModel : IBaseBindingModel
    {
        public string AccountKey { get; set; }
        public AccountProfileTypes AccountType { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string DeviceKey { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }

    public class SigninBindingModel : IBaseBindingModel
    {
        public string AccountKey { get; set; }
        public string Password { get; set; }

        public string DeviceKey { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }

    public class ForgotPasswordBindingModel : IBaseBindingModel
    {
        public string Username { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangeForgotenPasswordBindingModel : IBaseBindingModel
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }

    public class ActivateAccountBindingModel : IBaseBindingModel
    {
        public string Username { get; set; }
        public string Code { get; set; }
    }

    public class ForgotResetPasswordBindingModel : IBaseBindingModel
    {
        public string Username { get; set;}
    }

    public class ResetPasswordBindingModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; } 
    }
}

