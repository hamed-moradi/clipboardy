using Assets.Model.Header;
using System;
using System.Security.Claims;

namespace Assets.Utility.Extension
{
    public static class ClaimExtension
    {

        public static ClaimsIdentity ToClaimsIdentity(this AccountHeaderModel account)
        {
            var result = new ClaimsIdentity(new[] {
                    new Claim("id", account.Id.ToString()),
                    new Claim("deviceId", !string.IsNullOrEmpty( account.DeviceId.ToString()) ? account.DeviceId.ToString()  : ""),
                    new Claim("username", account.Username),
                    new Claim("lastSignedinAt", account.LastSignedinAt?.ToString("yyyy-MM-dd hh:mm:ss tt"))
                });
            return result;
        }

        public static AccountHeaderModel ToAccount(this ClaimsPrincipal principal)
        {
            var result = new AccountHeaderModel();
            foreach (var claim in principal.Claims)
            {
                switch (claim.Type.ToLower())
                {
                    case "id":
                        result.Id = int.Parse(claim.Value);
                        break;

                    case "deviceid":
                        result.DeviceId = int.Parse(claim.Value);
                        break;

                    case "username":
                        result.Username = claim.Value;
                        break;

                    case "lastsignedinat":
                        DateTime.TryParse(claim.Value, out var lastSignedinAt);
                        result.LastSignedinAt = lastSignedinAt == DateTime.MinValue ? (DateTime?)null : lastSignedinAt;
                        break;
                }
            }
            return result;
        }

        public static void Key(this Claim claim)
        {
            //var result = new Dictionary<string, string> {
            //    ["Surname"] = ClaimTypes.Surname,
            //    ["UserData"] = ClaimTypes.UserData,
            //    ["NameIdentifier"] = ClaimTypes.NameIdentifier,
            //    ["Name"] = ClaimTypes.Name,
            //    ["MobilePhone"] = ClaimTypes.MobilePhone,
            //    ["Country"] = ClaimTypes.Country,
            //    ["DateOfBirth"] = ClaimTypes.DateOfBirth,
            //    ["Email"] = ClaimTypes.Email,
            //    ["Gender"] = ClaimTypes.Gender,
            //    ["GivenName"] = ClaimTypes.GivenName,
            //    ["Locality"] = ClaimTypes.Locality
            //};
            //return result;
        }
    }
}
