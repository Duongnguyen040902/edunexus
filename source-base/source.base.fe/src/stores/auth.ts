import {
  AuthResponseInterface,
  AuthResponseRegisterInterface,
  ErrorConfirmCodeVerifyFirstLogin,
  ErrorConfirmFirstLogin,
  InformationUser,
  LoginRequestInterface,
  RegisterRequestInterface,
  RequestConfirmCodeResetPasswordInterface,
  RequestForgotPasswordInterface,
  RequestResetPasswordInterface,
  ResponseConfirmCodeResetPasswordInterface,
  ResponseForgotPasswordInterface,
  VerifyEmailRequestInterface,
} from '@/types/model/auth';
import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { AuthRepository } from '@/repositories/repository-auth';
import { reactive, ref } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { Cookie } from '@/classes/cookie';
import router from '@/router';
import { ROUTER_PATHS } from '@/constants/api/router-paths';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state.ts';
import { ShortRoleName } from '@/constants/enums/mode';

export const useAuthStore = defineStore('auth', () => {
  const authFactory = RepositoryFactory.create('auth') as AuthRepository;
  const user = reactive<{ value: AuthResponseInterface }>({ value: { name: '', email: '', token: '' } });
  const informationUser = ref<InformationUser>({
    mode: 0,
    id: 0,
    email: '',
    isCompleteVerify: false,
  });
  const errorVerifyFirstLogin = reactive(<ErrorConfirmFirstLogin>{
    Email: [],
  });
  const errorVerifyFirstLoginKeys: (keyof ErrorConfirmFirstLogin)[] = ['Email'];
  const errorCodeConfirmFirstLogin = reactive(<ErrorConfirmCodeVerifyFirstLogin>{
    VerificationCode: [],
  });

  const errorCodeConfirmFirstLoginKeys: (keyof ErrorConfirmCodeVerifyFirstLogin)[] = ['VerificationCode'];
  const loginMode = async (
    params: LoginRequestInterface,
    success: (res: AuthResponseInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await authFactory.loginMode(
      params,
      res => {
        const authRes = res.data as AuthResponseInterface;
        const token = authRes.token;
        Cookie.set('token', token);
        Cookie.set('mode', params.mode.toString());
        if (
          authRes.role === ShortRoleName.Teacher ||
          authRes.role === ShortRoleName.Parent ||
          authRes.role === ShortRoleName.Bus ||
          authRes.role === ShortRoleName.SchoolAdmin
        ) {
          informationUser.value.id = authRes.user?.id || 0;
          informationUser.value.mode = params.mode || 0;
          informationUser.value.isCompleteVerify = authRes.user?.isCompleteVerify ?? false;
          informationUser.value.email = authRes.user?.email || '';
          Cookie.set('is_completed_verify_email', authRes.user?.isCompleteVerify?.valueOf().toString() || 'false');
        }

        user.value = authRes;
        return success(authRes);
      },
      err => {
        const errorResponse: ErrorResponse = {
          ...err,
          errors: err.errors,
        };
        return error(errorResponse);
      },
    );
  };

  const registerSchoolAdmin = async (
    params: RegisterRequestInterface,
    success: (res: AuthResponseRegisterInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await authFactory.register(
      params,
      res => {
        const resData = res.data as AuthResponseRegisterInterface;
        const token = resData.token;
        Cookie.set('token-verify-email', token);

        return success(resData);
      },
      err => {
        const errorResponse: ErrorResponse = {
          ...err,
          errors: err.errors,
        };
        return error(errorResponse);
      },
    );
  };

  const verifyEmail = async (
    params: VerifyEmailRequestInterface,
    success: (res: AuthResponseRegisterInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await authFactory.verifyEmail(
      params,
      res => {
        const resData = res.data as AuthResponseRegisterInterface;
        Cookie.remove('token-verify-email');

        return success(resData);
      },
      err => {
        error({
          ...err,
          errors: err.errors,
        });
      },
    );
  };

  const forgotPassword = async (
    params: RequestForgotPasswordInterface,
    success: (res: ResponseForgotPasswordInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await authFactory.forgotPassword(
      params,
      res => {
        const resData = res.data as ResponseForgotPasswordInterface;
        Cookie.set('token-forgot-password', resData.token);

        return success(resData);
      },
      err => {
        error({
          ...err,
          errors: err.errors,
        });
      },
    );
  };

  const confirmCodeResetPassword = async (
    params: RequestConfirmCodeResetPasswordInterface,
    success: (res: ResponseConfirmCodeResetPasswordInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await authFactory.confirmCodeResetPassword(
      params,
      res => {
        const resData = res.data as ResponseConfirmCodeResetPasswordInterface;
        Cookie.remove('token-forgot-password');
        Cookie.set('token-confirm-code-reset-password', resData.token);

        return success(resData);
      },
      err => {
        error({
          ...err,
          errors: err.errors,
        });
      },
    );
  };

  const resetPassword = async (
    params: RequestResetPasswordInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await authFactory.resetPassword(
      params,
      res => {
        Cookie.remove('token-confirm-code-reset-password');
        return success(res);
      },
      err => {
        error({
          ...err,
          errors: err.errors,
        });
      },
    );
  };

  const logout = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    const token = Cookie.get('token');
    await authFactory.logout(
      token,
      res => {
        Cookie.remove('token');
        Cookie.remove('is_completed_verify_email');
        Cookie.remove('mode');
        Cookie.remove('user');
        user.value = { name: '', email: '', token: '' };
        informationUser.value = {
          mode: 0,
          id: 0,
          firstName: '',
          lastName: '',
          address: '',
          email: '',
          dateOfBirth: '',
        };
        router.push({ path: ROUTER_PATHS.LOGIN });
        return success(res);
      },
      err => {
        error({
          ...err,
          errors: err.errors,
        });
      },
    );
  };

  const verifyFirstLogin = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    const mode = Cookie.get('mode');
    informationUser.value.mode = mode ? parseInt(mode) : 0;
    informationUser.value.id = parseInt(Cookie.get('id_verify') ?? '0');
    await authFactory.verifyFirstLogin(
      informationUser.value,
      res => {
        const response = res.data;
        Cookie.set('token-verify-first-login', response.token);
        clearErrorKeys(errorVerifyFirstLoginKeys, errorVerifyFirstLogin);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorConfirmFirstLogin;
        mapErrorKeys(errorVerifyFirstLoginKeys, errorVerifyFirstLogin, errorsResponse);
        error(err);
      },
    );
  };

  const confirmVerifyFirstLogin = async (
    verificationCode: string,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    const token = Cookie.get('token-verify-first-login');
    await authFactory.confirmVerifyFirstLogin(
      token,
      verificationCode,
      res => {
        Cookie.remove('token-verify-first-login');
        Cookie.remove('id_verify');

        clearErrorKeys(errorCodeConfirmFirstLoginKeys, errorCodeConfirmFirstLogin);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorConfirmCodeVerifyFirstLogin;
        mapErrorKeys(errorCodeConfirmFirstLoginKeys, errorCodeConfirmFirstLogin, errorsResponse);
        error(err);
      },
    );
  };

  return {
    errorVerifyFirstLogin,
    errorCodeConfirmFirstLogin,
    informationUser,
    user,
    loginMode,
    registerSchoolAdmin,
    verifyEmail,
    forgotPassword,
    confirmCodeResetPassword,
    resetPassword,
    logout,
    verifyFirstLogin,
    confirmVerifyFirstLogin,
  };
});
