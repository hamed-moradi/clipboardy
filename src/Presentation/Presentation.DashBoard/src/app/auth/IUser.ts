export interface IUser {
  AccountKey?: string;
  Password: string;
  ConfirmPassword?: string;
  RememberMe?: boolean;
  oldPassword?: string;
  resetPassToken?: string;
}
