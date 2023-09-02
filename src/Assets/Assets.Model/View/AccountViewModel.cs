using Assets.Model.Base;
using System;

namespace Assets.Model.View
{
    public class SigninViewModel : IBaseViewModel
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ChangeForgotenPasswordViewModel : IBaseViewModel
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
