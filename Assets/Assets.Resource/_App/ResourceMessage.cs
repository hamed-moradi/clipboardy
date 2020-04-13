using Assets.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Resource {
    public enum ResourceMessages {
        Ok = 200,
        BadRequest=400
    }

    public class ResourceMessage {
        #region General
        public const string Ok = nameof(Ok);
        public const string BadRequest = nameof(BadRequest);
        public const string InternalServerError = nameof(InternalServerError);
        public const string SomethingWentWrong = nameof(SomethingWentWrong);
        public const string ConnectionError = nameof(ConnectionError);
        public const string DuplicatedValueFound = nameof(DuplicatedValueFound);
        public const string NothingFound = nameof(NothingFound);
        public const string DefectiveEntry = nameof(DefectiveEntry);
        public const string RetrieveLimit = nameof(RetrieveLimit);
        public const string DangerousRequest = nameof(DangerousRequest);
        public const string UnsupportedLanguage = nameof(UnsupportedLanguage);
        public const string UnsupportedTimeZone = nameof(UnsupportedTimeZone);
        public const string AccessDenied = nameof(AccessDenied);
        public const string UnofficialRequest = nameof(UnofficialRequest);
        #endregion

        #region Account

        #region authentication
        public const string AuthenticationFailed = nameof(AuthenticationFailed);
        public const string TokenNotFound = nameof(TokenNotFound);
        public const string DeviceIdNotFound = nameof(DeviceIdNotFound);
        public const string DeviceIsNotActive = nameof(DeviceIsNotActive);
        public const string PhoneIsNotVerified = nameof(PhoneIsNotVerified);
        public const string EmptyHeader = nameof(EmptyHeader);//"You're trying to signing in through the wrong way! buddy."
        public const string UserNotFound = nameof(UserNotFound);
        public const string PhoneNotFound = nameof(PhoneNotFound);//"This Phone number is not found."
        public const string EmailNotFound = nameof(EmailNotFound);//"This Email is not found."
        public const string BadEmailOrCellphone = nameof(BadEmailOrCellphone);//"Please define a correct Email or CellPhone number."
        public const string UserIsNotActive = nameof(UserIsNotActive);
        public const string PasswordChanged = nameof(PasswordChanged);//"Password is changed successfully."
        public const string ChangingPasswordWithoutToken = nameof(ChangingPasswordWithoutToken);//"You should request forgoten password first."
        public const string ChangingPasswordWithWrongToken = nameof(ChangingPasswordWithWrongToken);//"Your request for changing the password is liked an abnormal activity and it's logged."
        #endregion

        #region SignUp
        public const string RequestForVerificationCodeFirst = nameof(RequestForVerificationCodeFirst);
        public const string VerificationCodeHasBeenExpired = nameof(VerificationCodeHasBeenExpired);
        public const string WrongVerificationCode = nameof(WrongVerificationCode);
        public const string DefectiveCellPhone = nameof(DefectiveCellPhone);
        public const string DefectiveEmail = nameof(DefectiveEmail);
        public const string DefectiveEmailOrCellPhone = nameof(DefectiveEmailOrCellPhone);//"Please define your Email or CellPhone number."
        public const string InvalidEmailOrCellPhone = nameof(InvalidEmailOrCellPhone);//"Please define a correct Email or CellPhone number."
        public const string CellPhoneAlreadyExists = nameof(CellPhoneAlreadyExists);//"This Phone number is already registered. If you forgot your Password try to use Forgat Password feature."
        public const string EmailAlreadyExists = nameof(EmailAlreadyExists);//"This Email is already registered. If you forgot your Password try to use Forgat Password feature."
        #endregion

        #region SignIn
        public const string DefectiveUsernameOrPassword = nameof(DefectiveUsernameOrPassword);//"Please define your Username and Password."
        public const string DefectivePassword = nameof(DefectivePassword);//"Please define a Password."
        public const string PasswordsMissmatch = nameof(PasswordsMissmatch);//"Defined Password is not match with repeated one."
        public const string WrongPassword = nameof(WrongPassword);
        public const string InvalidSigninAttempt = nameof(InvalidSigninAttempt);
        public const string GoToStepTwo = nameof(GoToStepTwo);
        #endregion

        #region external authentication
        public const string ExternalAuthenticationFailed = nameof(ExternalAuthenticationFailed);//"External authentication failed"
        public const string ExternalAuthenticationUserError = nameof(ExternalAuthenticationUserError);//"External authentication user principal error"
        public const string ExternalAuthenticationEmailError = nameof(ExternalAuthenticationEmailError);//"External authentication user email not found"
        public const string ExternalAuthenticationWithUnknownProvider = nameof(ExternalAuthenticationWithUnknownProvider);//"External signup with unknown ProviderId"
        #endregion

        #endregion

        #region Business
        public const string InvalidPoint = nameof(InvalidPoint);
        public const string BusinessNotFound = nameof(BusinessNotFound);
        public const string BusinessIsNotActive = nameof(BusinessIsNotActive);
        #endregion

        #region Product
        public const string ProductNotFound = nameof(ProductNotFound);
        public const string ProductIsNotActive = nameof(ProductIsNotActive);
        #endregion

        #region Comment
        public const string CommentNotFound = nameof(CommentNotFound);
        public const string CommentIsNotActive = nameof(CommentIsNotActive);
        #endregion
    }
}
