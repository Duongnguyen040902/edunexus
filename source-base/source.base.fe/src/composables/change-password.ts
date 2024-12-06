import { useSchoolStore } from '@/stores/school.ts';
import { ChangePasswordDTO, ErrorChangePasswordDTO } from '@/types/model/change-password.ts';
import { reactive } from 'vue';
import { mapErrorKeys } from '@/helpers/state.ts';
import { notifyError, notifySuccess } from '@/helpers/notify.ts';
import { useAuthStore } from '@/stores/auth.ts';

export const useChangePasswordComposable = () => {
  const schoolStore = useSchoolStore();
  const authStore = useAuthStore();
  const ErrorChangePassword = reactive<ErrorChangePasswordDTO>({
    OldPassword: [],
    NewPassword: [],
  });
  const ErrorChangePasswordKeys: (keyof ErrorChangePasswordDTO)[] = ['OldPassword', 'NewPassword'];

  const changePassword = async (data: ChangePasswordDTO) => {
    try {
      await schoolStore.ChangePassword(
        data,
        res => {
          if (res.data === true) {
            notifySuccess('Đổi mật khẩu thành công');
            setTimeout(() => {
              authStore.logout();
            }, 1000);
          }else{
            ErrorChangePassword.OldPassword = ['Mật khẩu cũ không đúng'];
          }
        },
        err => {
          console.log('error', err);
          handleErrors(err);
        },
      );
    } catch (err) {
      console.error('Unexpected error changing password:', err);
    }
  };

  const handleErrors = (err: any) => {
    const errors = err.errors as ErrorChangePasswordDTO;
    mapErrorKeys(ErrorChangePasswordKeys, ErrorChangePassword, errors);
  };

  return {
    changePassword,
    ErrorChangePassword,
    handleErrors,
  };
};