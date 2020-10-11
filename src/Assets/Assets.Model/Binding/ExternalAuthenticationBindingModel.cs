using Assets.Model.Base;
using Assets.Model.Common;
using Assets.Model.Header;

namespace Assets.Model.Binding {
    public partial class ExternalUserBindingModel: Device {
        public AccountProvider ProviderId { get; set; } = AccountProvider.Clipboardy;
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
