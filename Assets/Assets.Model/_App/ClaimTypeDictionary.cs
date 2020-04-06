using System.Collections.Generic;

namespace Assets.Model {
    public class CustomClaimTypes {
        public static readonly IDictionary<string, string> List = new Dictionary<string, string> {
            ["Surname"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname",
            ["UserData"] = "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata",
            ["NameIdentifier"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            ["Name"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
            ["MobilePhone"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone",
            ["Country"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country",
            ["DateOfBirth"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth",
            ["Email"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
            ["Gender"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender",
            ["GivenName"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname",
            ["Locality"] = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality"
        };
    }
}
