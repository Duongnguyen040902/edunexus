export interface ChangePasswordDTO {
  oldPassword?: string;
  newPassword?: string;
}

export interface ErrorChangePasswordDTO {
  NewPassword?: string[];
  OldPassword?: string[];
}