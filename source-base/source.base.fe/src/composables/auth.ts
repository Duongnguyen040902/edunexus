import { useAuthStore } from '@/stores/auth';
import {
  ErrorResponseLogin,
  ErrorResponseRegister,
  ErrorResponseResetPassword,
  ErrorResponseVerifyEmail,
  LoginRequestInterface,
  RegisterRequestInterface,
  RequestResetPasswordInterface,
} from '@/types/model/auth';
import { computed, reactive, ref } from 'vue';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifySuccess, notifyError } from '@/helpers/notify';
import { mapErrorKeys, clearErrorKeys } from '@/helpers/state';
import router from '@/router';
import { ModeLogin, ShortRoleName } from '@/constants/enums/mode';
import { ROUTER_PATHS } from '@/constants/api/router-paths.ts';
import { Cookie } from '@/classes/cookie.ts';

export const useAuthComposable = () => {
  const authStore = useAuthStore();

  const {
    errorCodeConfirmFirstLogin,
    errorVerifyFirstLogin,
    informationUser,
    loginMode,
    registerSchoolAdmin,
    verifyEmail,
    forgotPassword,
    confirmCodeResetPassword,
    resetPassword,
    logout,
    verifyFirstLogin,
    confirmVerifyFirstLogin,
  } = authStore;
  const loginRequest = reactive<LoginRequestInterface>({ mode: 0, email: '', password: '' });
  const registerRequest = reactive<RegisterRequestInterface>({
    username: '',
    name: '',
    email: '',
    address: '',
    password: '',
    rePassword: '',
  });
  const verifyEmailRequest = reactive({ token: '', verificationCode: Array(6).fill('') });
  const requestForgotPassword = reactive({ mode: ModeLogin[1].value, email: '' });
  const errorsRegister = reactive(<ErrorResponseRegister>{
    Name: [],
    Email: [],
    Address: [],
    Password: [],
    Username: [],
    RePassword: [],
  });
  const errorVerify = reactive(<ErrorResponseVerifyEmail>{
    verifyCode: '',
  });
  const errorRegisterKeys: (keyof ErrorResponseRegister)[] = [
    'Name',
    'Email',
    'Password',
    'Address',
    'Username',
    'RePassword',
  ];
  const errorVerifyKey: (keyof ErrorResponseVerifyEmail)[] = ['verifyCode'];
  const errorForgotPassword = ref('');
  const requestResetPassword = reactive(<RequestResetPasswordInterface>{
    token: '',
    password: '',
    confirmPassword: '',
  });
  const errorResetPasswordKeys: (keyof ErrorResponseResetPassword)[] = ['Password', 'ConfirmPassword'];
  const errorResetPassword = reactive<ErrorResponseResetPassword>({
    Password: [],
    ConfirmPassword: [],
  });
  const errorResponseLogin = reactive<ErrorResponseLogin>({
    Email: [],
    Password: [],
    Mode: [],
  });
  const errorResponseLoginKeys: (keyof ErrorResponseLogin)[] = ['Email', 'Password', 'Mode'];
  const modesLogin = ref(ModeLogin);
  const handleSubmitLoginByMode = (): void => {
    startLoading();
    loginMode(
      loginRequest,
      response => {
        endLoading();
        notifySuccess('Đăng nhập thành công');
        const isCompletedVerifyEmail = response.user?.isCompleteVerify;
        console.log('isCompletedVerifyEmail', isCompletedVerifyEmail);
        if (!isCompletedVerifyEmail && isCompletedVerifyEmail !== undefined) {
          const user = response.user;
          Cookie.set('id_verify', JSON.stringify(user?.id));
          const path = savePathWhenVerify(response.role);
          Cookie.set('path_origin_with_role', path);
          router.push({ path: '/authen/verify-email' });
        } else {
          redirectWhenSuccessWithRole(response.role);
        }
      },
      err => {
        endLoading();
        notifyError('Đăng nhập thất bại', 'Thất bại');
        const errorsResponse = err.errors as ErrorResponseLogin;
        mapErrorKeys(errorResponseLoginKeys, errorResponseLogin, errorsResponse);
      },
    );
  };

  const handleSubmitRegister = (): void => {
    startLoading();
    registerSchoolAdmin(
      registerRequest,
      () => {
        endLoading();
        clearErrorKeys(errorRegisterKeys, errorsRegister);
        router.push('verify');
      },
      err => {
        handleErrors(err);
      },
    );
  };

  const combinedCode = computed(() => verifyEmailRequest.verificationCode.join(''));

  const handleVerifyEmail = (): void => {
    startLoading();
    verifyEmail(
      { ...verifyEmailRequest, verificationCode: combinedCode.value },
      () => {
        endLoading();
        notifySuccess('Tạo tài khoản thành công', 'Thành công');
        clearErrorKeys(errorVerifyKey, errorVerify);
        router.push({ path: '/authen/login' });
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseVerifyEmail;
        mapErrorKeys(errorVerifyKey, errorVerify, errorsResponse);
        endLoading();
        notifyError('Xác thực email lỗi');
      },
    );
  };

  const handleErrors = (err: any) => {
    endLoading();
    notifyError('Đăng ký lỗi');
    const errorsResponse = err.errors as ErrorResponseRegister;
    mapErrorKeys(errorRegisterKeys, errorsRegister, errorsResponse);
  };

  const handleRedirectPageForgotPassword = () => {
    router.push({ path: 'forgot-password' });
  };

  const handleRedirectPageLogin = () => {
    router.push({ path: '/authen/login' });
  };

  const handleRedirectPageRegister = () => {
    router.push({ path: '/authen/register' });
  };

  const handleSubmitForgotPassword = (): void => {
    startLoading();
    forgotPassword(
      requestForgotPassword,
      () => {
        endLoading();
        router.push({path: '/authen/verify-forgot-password'});
      },
      err => {
        if (err.code === 404) {
          errorForgotPassword.value = err.message;
        } else {
          errorForgotPassword.value = err.errors.Email[0];
        }
        endLoading();
        notifyError('Quên mật khẩu thất bại');
      },
    );
  };

  const handleSubmitConfirmCodeResetPassword = () => {
    startLoading();
    confirmCodeResetPassword(
      { ...verifyEmailRequest, verificationCode: combinedCode.value },
      () => {
        endLoading();
        router.push({path: '/authen/reset-password'});
      },
      () => {
        endLoading();
        notifyError('Xác thực email lỗi');
      },
    );
  };

  const handleSubmitResetPassword = async () => {
    startLoading();
    await resetPassword(
      requestResetPassword,
      () => {
        endLoading();
        notifySuccess('Khôi phục mật khẩu thành công');
        router.push({ path: '/authen/login' });
      },
      err => {
        endLoading();
        notifyError('Khôi phục mật khẩu thất bại');
        const errorsResponse = err.errors as ErrorResponseResetPassword;
        mapErrorKeys(errorResetPasswordKeys, errorResetPassword, errorsResponse);
      },
    );
  };

  const roleRoutes: { [key in ShortRoleName]: string } = {
    [ShortRoleName.Admin]: ROUTER_PATHS.ADMIN.DASHBOARD,
    [ShortRoleName.SchoolAdmin]: ROUTER_PATHS.SCHOOL_ADMIN.INDEX, // TODO: change to route
    [ShortRoleName.Parent]: ROUTER_PATHS.PUPIL.INDEX,
    [ShortRoleName.Teacher]: ROUTER_PATHS.TEACHER.INDEX,
    [ShortRoleName.Bus]: ROUTER_PATHS.SUPERVISOR.INDEX,
  };

  const redirectWhenSuccessWithRole = (role: string) => {
    const path = roleRoutes[role as ShortRoleName] || '/authen/login';
    router.push({ path });
  };

  const savePathWhenVerify = (role: string): string => {
    const path = roleRoutes[role as ShortRoleName] || '/authen/login';
    return path;
  };

  const handleLogout = () => {
    startLoading();
    logout(
      () => {
        endLoading();
        notifySuccess('Đăng xuất thành công');
      },
      () => {
        endLoading();
        notifyError('Đăng xuất thất bại');
      },
    );
  };

  const handleConfirmVerifyFirstLogin = async () => {
    startLoading();
    verifyFirstLogin(
      () => {
        router.push({ path: '/authen/verify-email-first-login' });
        endLoading();
      },
      () => {
        endLoading();
      },
    );
  };

  const handleConfirmCodeVerifyFirstLogin = async () => {
    startLoading();
    confirmVerifyFirstLogin(
      combinedCode.value,
      () => {
        notifySuccess('Xác thực thành công');
        const pathOrigin = Cookie.get('path_origin_with_role');
        router.push({ path: pathOrigin });
        Cookie.remove('path_origin_with_role');
        endLoading();
      },
      () => {
        notifyError('Xác thực thất bại');
        endLoading();
      },
    );
  };

  return {
    errorCodeConfirmFirstLogin,
    errorVerifyFirstLogin,
    informationUser,
    errorResponseLogin,
    loginRequest,
    registerRequest,
    errorsRegister,
    verifyEmailRequest,
    errorVerify,
    errorForgotPassword,
    requestForgotPassword,
    requestResetPassword,
    errorResetPassword,
    modesLogin,
    loginMode,
    handleSubmitLoginByMode,
    handleSubmitRegister,
    handleVerifyEmail,
    handleRedirectPageForgotPassword,
    handleRedirectPageLogin,
    handleRedirectPageRegister,
    handleSubmitForgotPassword,
    handleSubmitConfirmCodeResetPassword,
    handleSubmitResetPassword,
    handleLogout,
    handleConfirmVerifyFirstLogin,
    handleConfirmCodeVerifyFirstLogin,
  };
};
