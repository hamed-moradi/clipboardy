using Assets.Model.Base;

namespace Assets.Model.View {
    public class AccountViewModel: IBaseViewModel {
        public string Username { get; set; }
    }

    public class ChangeForgotenPasswordViewModel: IBaseViewModel {
        public string Username { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
