using Assets.Model.Base;
using Assets.Model.Common;

namespace Assets.Model.Binding {
    public class ExternalUserBindingModel: HttpDeviceHeader {
        public AccountProvider ProviderId { get; set; } = AccountProvider.Clipboard;
        public string Surname { get; set; }
        public string UserData { get; set; }
        public string NameIdentifier { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string GivenName { get; set; }
        public string Locality { get; set; }
    }
}
