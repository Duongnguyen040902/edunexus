<template>
  <div class="container-xxl">
    <div class="authentication-wrapper authentication-basic container-p-y">
      <div class="authentication-inner">
        <div class="card px-sm-6 px-0">
          <div class="card-body">
            <div class="app-brand justify-content-center">
              <img src="../../assets/images/logo/edunexus_logo.png" class="mb-2 mx-2" width="200"/>
            </div>
            <!-- /Logo -->
            <h4 class="mb-1">Đổi mật khẩu? 🔒</h4>
            <p class="mb-6">Nhập mật khẩu cũ và mật khẩu mới của bạn để đổi mật khẩu</p>
            <form class="mb-6 fv-plugins-bootstrap5 fv-plugins-framework" @submit.prevent="handleSubmitChangePassword">
              <div class="mb-6 fv-plugins-icon-container">
                <label for="oldPassword" class="form-label">Mật khẩu cũ <span class="required">*</span></label>
                <input type="password" class="form-control" id="oldPassword" v-model="changePasswordData.oldPassword">
                <div v-if="ErrorChangePassword.OldPassword" class="text-danger">{{ ErrorChangePassword.OldPassword[0] }}</div>
              </div>
              <div class="mb-6 fv-plugins-icon-container">
                <label for="newPassword" class="form-label">Mật khẩu mới <span class="required">*</span></label>
                <input type="password" class="form-control" id="newPassword" v-model="changePasswordData.newPassword">
                <div v-if="ErrorChangePassword.NewPassword" class="text-danger">{{ ErrorChangePassword.NewPassword[0] }}</div>
              </div>
              <button class="btn btn-primary d-grid w-100">Đổi mật khẩu</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue';
import { useChangePasswordComposable } from '@/composables/change-password.ts';

export default defineComponent({
  name: 'ChangePassword',
  components: {},
  setup() {
    const { changePassword, ErrorChangePassword } = useChangePasswordComposable();
    const changePasswordData = reactive({
      oldPassword: '',
      newPassword: ''
    });

    const handleSubmitChangePassword = async () => {
      await changePassword(changePasswordData);
    };

    return {
      changePasswordData,
      ErrorChangePassword,
      handleSubmitChangePassword
    };
  }
});
</script>