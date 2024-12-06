import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import {
  InformationUser,
  RequestConfirmCodeResetPasswordInterface,
  RequestForgotPasswordInterface,
  RequestResetPasswordInterface,
} from '@/types/model/auth';

export class AuthRepository extends BaseRepository {
  public async loginMode(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.post(API_URL.AUTH.LOGIN, data, success, error);
  }

  public async register(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.post(API_URL.AUTH.REGISTER, data, success, error);
  }

  public async verifyEmail(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.post(API_URL.AUTH.VERIFY_EMAIL, data, success, error);
  }

  public async forgotPassword(
    data: RequestForgotPasswordInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.AUTH.FORGOT_PASSWORD, data, success, error);
  }

  public async confirmCodeResetPassword(
    data: RequestConfirmCodeResetPasswordInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.AUTH.CONFIRM_CODE_RESET_PASSWORD, data, success, error);
  }

  public async resetPassword(
    data: RequestResetPasswordInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(API_URL.AUTH.RESET_PASSWORD, data, success, error);
  }

  public async logout(
    accessToken: string,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.AUTH.LOGOUT, accessToken, success, error);
  }

  public async verifyFirstLogin(
    data: InformationUser,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.AUTH.VERIFY_FIRST_LOGIN, data, success, error);
  }

  public async confirmVerifyFirstLogin(
    token: unknown,
    verificationCode: string,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.AUTH.CONFIRM_VERIFY_FIRST_LOGIN, { token, verificationCode }, success, error);
  }
}
