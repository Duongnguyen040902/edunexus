export interface LoginRequestInterface {
  mode: number;
  email: string;
  password: string;
}

export interface AuthResponseInterface {
  name: string;
  email: string;
  token: string;
  user?: InformationUser;
  role?: string;
}

export interface AuthResponseRegisterInterface {
  message: string;
  token: string;
}

export interface RegisterRequestInterface {
  username: string;
  name: string;
  email: string;
  address: string;
  password: string;
  rePassword: string;
}

export interface VerifyEmailRequestInterface {
  token: string;
  verificationCode: string;
}

export interface ErrorResponseRegister {
  Name?: string[];
  Email?: string[];
  Address?: string[];
  Password?: string[];
  Username?: string[];
  RePassword?: string[];
}

export interface ErrorResponseVerifyEmail {
  verifyCode?: string;
}

export interface RequestForgotPasswordInterface {
  mode: number;
  email: string;
}

export interface ResponseForgotPasswordInterface {
  token: string;
}

export interface RequestConfirmCodeResetPasswordInterface {
  token: string;
  verificationCode: string;
}

export interface ErrorResponseConfirmCodeResetPassword {
  verifyCode?: string;
}

export interface ResponseConfirmCodeResetPasswordInterface {
  token: string;
}

export interface RequestResetPasswordInterface {
  token: string;
  password: string;
  confirmPassword: string;
}

export interface ErrorResponseResetPassword {
  Password?: string[];
  ConfirmPassword?: string[];
}

export interface ErrorResponseLogin {
  Email?: string[];
  Password?: string[];
  Mode?: string[];
}

export interface InformationUser {
  mode: number;
  id: number;
  email: string;
  isCompleteVerify?: boolean;
}

export interface ErrorConfirmFirstLogin {
  Email?: string[];
}

export interface ErrorConfirmCodeVerifyFirstLogin {
  VerificationCode?: string[];
}

