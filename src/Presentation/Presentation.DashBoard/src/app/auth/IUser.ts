export interface IUser {
  AccountKey?: string;
  Password?: string;
  ConfirmPassword?: string;
  RememberMe?: boolean;
  NewPassword?: string;
  resetPassToken?: string;
}
