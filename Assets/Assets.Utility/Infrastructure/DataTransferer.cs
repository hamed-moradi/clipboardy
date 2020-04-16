using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Utility.Infrastructure {
    public static class DataTransferer {
        #region General
        public static IServiceResult Ok(object data = null) { return new ServiceResult(200, nameof(Ok), data); }
        public static IServiceResult BadRequest(object data = null) { return new ServiceResult(400, nameof(BadRequest), data); }
        public static IServiceResult InternalServerError(object data = null) { return new ServiceResult(500, nameof(InternalServerError), data); }
        public static IServiceResult SomethingWentWrong(object data = null) { return new ServiceResult(0, nameof(SomethingWentWrong), data); }
        public static IServiceResult PhoneIsNotVerified(object data = null) { return new ServiceResult(0, nameof(PhoneIsNotVerified), data); }
        public static IServiceResult ConnectionError(object data = null) { return new ServiceResult(0, nameof(ConnectionError), data); }
        public static IServiceResult DuplicatedValueFound(object data = null) { return new ServiceResult(0, nameof(DuplicatedValueFound), data); }
        public static IServiceResult NothingFound(object data = null) { return new ServiceResult(0, nameof(NothingFound), data); }
        public static IServiceResult DefectiveEntry(object data = null) { return new ServiceResult(405, nameof(DefectiveEntry), data); }
        public static IServiceResult RetrieveLimit(object data = null) { return new ServiceResult(0, nameof(RetrieveLimit), data); }
        public static IServiceResult DangerousRequest(object data = null) { return new ServiceResult(0, nameof(DangerousRequest), data); }
        public static IServiceResult UnsupportedLanguage(object data = null) { return new ServiceResult(0, nameof(UnsupportedLanguage), data); }
        public static IServiceResult UnsupportedTimeZone(object data = null) { return new ServiceResult(0, nameof(UnsupportedTimeZone), data); }
        public static IServiceResult AccessDenied(object data = null) { return new ServiceResult(0, nameof(AccessDenied), data); }
        public static IServiceResult UnofficialRequest(object data = null) { return new ServiceResult(0, nameof(UnofficialRequest), data); }
        #endregion

        #region Account

        #region authentication
        public static IServiceResult AuthenticationFailed(object data = null) { return new ServiceResult(0, nameof(AuthenticationFailed), data); }
        public static IServiceResult TokenNotFound(object data = null) { return new ServiceResult(0, nameof(TokenNotFound), data); }
        public static IServiceResult DeviceIdNotFound(object data = null) { return new ServiceResult(401, nameof(DeviceIdNotFound), data); }
        public static IServiceResult DeviceIsNotActive(object data = null) { return new ServiceResult(0, nameof(DeviceIsNotActive), data); }
        public static IServiceResult RequestForVerificationCodeFirst(object data = null) { return new ServiceResult(0, nameof(RequestForVerificationCodeFirst), data); }
        public static IServiceResult VerificationCodeHasBeenExpired(object data = null) { return new ServiceResult(0, nameof(VerificationCodeHasBeenExpired), data); }
        public static IServiceResult WrongVerificationCode(object data = null) { return new ServiceResult(0, nameof(WrongVerificationCode), data); }
        public static IServiceResult UserNotFound(object data = null) { return new ServiceResult(404, nameof(UserNotFound), data); }
        public static IServiceResult UserIsNotActive(object data = null) { return new ServiceResult(420, nameof(UserIsNotActive), data); }
        public static IServiceResult PhoneNotFound(object data = null) { return new ServiceResult(0, nameof(PhoneNotFound), data); }
        public static IServiceResult EmailNotFound(object data = null) { return new ServiceResult(0, nameof(EmailNotFound), data); }
        public static IServiceResult BadEmailOrCellphone(object data = null) { return new ServiceResult(0, nameof(BadEmailOrCellphone), data); }
        public static IServiceResult PasswordChanged(object data = null) { return new ServiceResult(0, nameof(PasswordChanged), data); }
        public static IServiceResult ChangingPasswordWithoutToken(object data = null) { return new ServiceResult(0, nameof(ChangingPasswordWithoutToken), data); }
        public static IServiceResult ChangingPasswordWithWrongToken(object data = null) { return new ServiceResult(0, nameof(ChangingPasswordWithWrongToken), data); }
        #endregion

        #region SignUp
        public static IServiceResult DefectiveCellPhone(object data = null) { return new ServiceResult(0, nameof(DefectiveCellPhone), data); }
        public static IServiceResult DefectiveEmail(object data = null) { return new ServiceResult(0, nameof(DefectiveEmail), data); }
        public static IServiceResult DefectiveEmailOrCellPhone(object data = null) { return new ServiceResult(0, nameof(DefectiveEmailOrCellPhone), data); }
        public static IServiceResult InvalidEmailOrCellPhone(object data = null) { return new ServiceResult(0, nameof(InvalidEmailOrCellPhone), data); }
        public static IServiceResult CellPhoneAlreadyExists(object data = null) { return new ServiceResult(0, nameof(CellPhoneAlreadyExists), data); }
        public static IServiceResult EmailAlreadyExists(object data = null) { return new ServiceResult(0, nameof(EmailAlreadyExists), data); }
        #endregion

        #region SignIn
        public static IServiceResult DefectiveUsernameOrPassword(object data = null) { return new ServiceResult(0, nameof(DefectiveUsernameOrPassword), data); }
        public static IServiceResult DefectivePassword(object data = null) { return new ServiceResult(0, nameof(DefectivePassword), data); }
        public static IServiceResult PasswordsMissmatch(object data = null) { return new ServiceResult(0, nameof(PasswordsMissmatch), data); }
        public static IServiceResult WrongPassword(object data = null) { return new ServiceResult(0, nameof(WrongPassword), data); }
        public static IServiceResult InvalidSigninAttempt(object data = null) { return new ServiceResult(0, nameof(InvalidSigninAttempt), data); }
        public static IServiceResult GoToStepTwo(object data = null) { return new ServiceResult(0, nameof(GoToStepTwo), data); }
        #endregion

        #region external authentication
        public static IServiceResult ExternalAuthenticationFailed(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationFailed), data); }
        public static IServiceResult ExternalAuthenticationUserError(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationUserError), data); }
        public static IServiceResult ExternalAuthenticationEmailError(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationEmailError), data); }
        public static IServiceResult ExternalAuthenticationWithUnknownProvider(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationWithUnknownProvider), data); }
        #endregion

        #endregion

        #region Clipboard
        #endregion
    }
}
