using Assets.Model.Base;

namespace Assets.Model.Binding {
    public class SigninBindingModel: IBaseBindingModel {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
