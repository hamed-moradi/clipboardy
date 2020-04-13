using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Utility.Infrastructure {
    public static class ServiceResultHandler {
        #region General
        public static IServiceResult Ok(object data = null) { return new ServiceResult(200, nameof(Ok), data); }
        public static IServiceResult BadRequest(object data = null) { return new ServiceResult(400, nameof(BadRequest), data); }
        public static IServiceResult InternalServerError(object data = null) { return new ServiceResult(500, nameof(InternalServerError), data); }
        public static IServiceResult SomethingWentWrong(object data = null) { return new ServiceResult(0, nameof(SomethingWentWrong), data); }
        public static IServiceResult AuthenticationFailed(object data = null) { return new ServiceResult(0, nameof(AuthenticationFailed), data); }
        public static IServiceResult TokenNotFound(object data = null) { return new ServiceResult(0, nameof(TokenNotFound), data); }
        public static IServiceResult DeviceIdNotFound(object data = null) { return new ServiceResult(401, nameof(DeviceIdNotFound), data); }
        public static IServiceResult DeviceIsNotActive(object data = null) { return new ServiceResult(0, nameof(DeviceIsNotActive), data); }
        public static IServiceResult PhoneIsNotVerified(object data = null) { return new ServiceResult(0, nameof(PhoneIsNotVerified), data); }
        public static IServiceResult ConnectionError(object data = null) { return new ServiceResult(0, nameof(ConnectionError), data); }
        public static IServiceResult UnexpectedError(object data = null) { return new ServiceResult(0, nameof(UnexpectedError), data); }
        public static IServiceResult NothingFound(object data = null) { return new ServiceResult(0, nameof(NothingFound), data); }
        public static IServiceResult DefectiveEntry(object data = null) { return new ServiceResult(405, nameof(DefectiveEntry), data); }
        public static IServiceResult RetrieveLimit(object data = null) { return new ServiceResult(0, nameof(RetrieveLimit), data); }
        public static IServiceResult DangerousRequest(object data = null) { return new ServiceResult(0, nameof(DangerousRequest), data); }
        public static IServiceResult UnsupportedLanguage(object data = null) { return new ServiceResult(0, nameof(UnsupportedLanguage), data); }
        public static IServiceResult UnsupportedTimeZone(object data = null) { return new ServiceResult(0, nameof(UnsupportedTimeZone), data); }
        public static IServiceResult RequestForVerificationCodeFirst(object data = null) { return new ServiceResult(0, nameof(RequestForVerificationCodeFirst), data); }
        public static IServiceResult VerificationCodeHasBeenExpired(object data = null) { return new ServiceResult(0, nameof(VerificationCodeHasBeenExpired), data); }
        public static IServiceResult WrongVerificationCode(object data = null) { return new ServiceResult(0, nameof(WrongVerificationCode), data); }
        public static IServiceResult AccessDenied(object data = null) { return new ServiceResult(0, nameof(AccessDenied), data); }
        public static IServiceResult UnofficialRequest(object data = null) { return new ServiceResult(0, nameof(UnofficialRequest), data); }
        public static IServiceResult EmptyHeader(object data = null) { return new ServiceResult(0, nameof(EmptyHeader), data); }//"You're trying to signing in through the wrong way! buddy."
        #endregion

        #region Account

        #region authentication
        public static IServiceResult UserNotFound(object data = null) { return new ServiceResult(410, nameof(UserNotFound), data); }
        public static IServiceResult PhoneNotFound(object data = null) { return new ServiceResult(0, nameof(PhoneNotFound), data); }//"This Phone number is not found."
        public static IServiceResult EmailNotFound(object data = null) { return new ServiceResult(0, nameof(EmailNotFound), data); }//"This Email is not found."
        public static IServiceResult BadEmailOrCellphone(object data = null) { return new ServiceResult(0, nameof(BadEmailOrCellphone), data); }//"Please define a correct Email or CellPhone number."
        public static IServiceResult UserIsNotActive(object data = null) { return new ServiceResult(420, nameof(UserIsNotActive), data); }
        public static IServiceResult PasswordChanged(object data = null) { return new ServiceResult(0, nameof(PasswordChanged), data); }//"Password is changed successfully."
        public static IServiceResult ChangingPasswordWithoutToken(object data = null) { return new ServiceResult(0, nameof(ChangingPasswordWithoutToken), data); }//"You should request forgoten password first."
        public static IServiceResult ChangingPasswordWithWrongToken(object data = null) { return new ServiceResult(0, nameof(ChangingPasswordWithWrongToken), data); }//"Your request for changing the password is liked an abnormal activity and it's logged."
        #endregion

        #region SignUp
        public static IServiceResult DefectiveCellPhone(object data = null) { return new ServiceResult(0, nameof(DefectiveCellPhone), data); }
        public static IServiceResult DefectiveEmail(object data = null) { return new ServiceResult(0, nameof(DefectiveEmail), data); }
        public static IServiceResult DefectiveEmailOrCellPhone(object data = null) { return new ServiceResult(0, nameof(DefectiveEmailOrCellPhone), data); }//"Please define your Email or CellPhone number."
        public static IServiceResult InvalidEmailOrCellPhone(object data = null) { return new ServiceResult(0, nameof(InvalidEmailOrCellPhone), data); }//"Please define a correct Email or CellPhone number."
        public static IServiceResult CellPhoneAlreadyExists(object data = null) { return new ServiceResult(0, nameof(CellPhoneAlreadyExists), data); }//"This Phone number is already registered. If you forgot your Password try to use Forgat Password feature."
        public static IServiceResult EmailAlreadyExists(object data = null) { return new ServiceResult(0, nameof(EmailAlreadyExists), data); }//"This Email is already registered. If you forgot your Password try to use Forgat Password feature."
        #endregion

        #region SignIn
        public static IServiceResult DefectiveUsernameOrPassword(object data = null) { return new ServiceResult(0, nameof(DefectiveUsernameOrPassword), data); }//"Please define your Username and Password."
        public static IServiceResult DefectivePassword(object data = null) { return new ServiceResult(0, nameof(DefectivePassword), data); }//"Please define a Password."
        public static IServiceResult PasswordsMissmatch(object data = null) { return new ServiceResult(0, nameof(PasswordsMissmatch), data); }//"Defined Password is not match with repeated one."
        public static IServiceResult WrongPassword(object data = null) { return new ServiceResult(0, nameof(WrongPassword), data); }
        public static IServiceResult InvalidSigninAttempt(object data = null) { return new ServiceResult(0, nameof(InvalidSigninAttempt), data); }
        public static IServiceResult GoToStepTwo(object data = null) { return new ServiceResult(0, nameof(GoToStepTwo), data); }
        #endregion

        #region external authentication
        public static IServiceResult ExternalAuthenticationFailed(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationFailed), data); }//"External authentication failed"
        public static IServiceResult ExternalAuthenticationUserError(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationUserError), data); }//"External authentication user principal error"
        public static IServiceResult ExternalAuthenticationEmailError(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationEmailError), data); }//"External authentication user email not found"
        public static IServiceResult ExternalAuthenticationWithUnknownProvider(object data = null) { return new ServiceResult(0, nameof(ExternalAuthenticationWithUnknownProvider), data); }//"External signup with unknown ProviderId"
        #endregion

        #endregion

        #region Business
        public static IServiceResult InvalidPoint(object data = null) { return new ServiceResult(0, nameof(InvalidPoint), data); }
        public static IServiceResult BusinessNotFound(object data = null) { return new ServiceResult(0, nameof(BusinessNotFound), data); }
        public static IServiceResult BusinessIsNotActive(object data = null) { return new ServiceResult(0, nameof(BusinessIsNotActive), data); }
        #endregion

        #region Product
        public static IServiceResult ProductNotFound(object data = null) { return new ServiceResult(0, nameof(ProductNotFound), data); }
        public static IServiceResult ProductIsNotActive(object data = null) { return new ServiceResult(0, nameof(ProductIsNotActive), data); }
        #endregion

        #region Comment
        public static IServiceResult CommentNotFound(object data = null) { return new ServiceResult(0, nameof(CommentNotFound), data); }
        public static IServiceResult CommentIsNotActive(object data = null) { return new ServiceResult(0, nameof(CommentIsNotActive), data); }
        #endregion
    }
}
