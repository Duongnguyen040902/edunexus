<template>
  <div class="container-xxl">
    <div class="authentication-wrapper authentication-basic container-p-y">
      <div class="authentication-inner">
        <div class="card px-sm-6 px-0">
          <div class="card-body">
            <div class="app-brand justify-content-center">
              <img src="@assets/images/logo/edunexus_logo.png" class="mb-2 mx-2" width="200"/>
            </div>
            <!-- /Logo -->
            <h4 class="mb-1">Khôi phục lại mật khẩu 🔒</h4>
            <p class="mb-6">
              <span class="fw-medium">Mật khẩu mới của bạn phải khác với mật khẩu đã sử dụng trước đó</span>
            </p>
            <form class="fv-plugins-bootstrap5 fv-plugins-framework" @submit.prevent="handleSubmitResetPassword">
              <div class="mb-6 form-password-toggle fv-plugins-icon-container">
                <label class="form-label" for="password">Mật khẩu mới</label>
                <div class="input-group input-group-merge has-validation">
                  <input
                    type="password"
                    id="password"
                    class="form-control"
                    name="password"
                    placeholder="············"
                    aria-describedby="password"
                    v-model="requestResetPassword.password"
                  />
                  <span class="input-group-text cursor-pointer"><i class="bx bx-hide"></i></span>
                </div>
                <div v-if="errorResetPassword.Password" class="text-danger">{{ errorResetPassword.Password[0] }}</div>
              </div>
              <div class="mb-6 form-password-toggle fv-plugins-icon-container">
                <label class="form-label" for="confirm-password">Xác nhận lại mật khẩu</label>
                <div class="input-group input-group-merge has-validation">
                  <input
                    type="password"
                    id="confirm-password"
                    class="form-control"
                    name="confirm-password"
                    placeholder="············"
                    aria-describedby="password"
                    v-model="requestResetPassword.confirmPassword"
                  />
                  <span class="input-group-text cursor-pointer"><i class="bx bx-hide"></i></span>
                </div>
                <div v-if="errorResetPassword.ConfirmPassword" class="text-danger">{{ errorResetPassword.ConfirmPassword[0] }}</div>
              </div>
              <button class="btn btn-primary d-grid w-100 mb-6">Thay đổi mật khẩu mới</button>
              <div class="text-center">
                <a href="/authen/login">
                  <i class="bx bx-chevron-left scaleX-n1-rtl me-1 align-top"></i>
                  Quay lại trang đăng nhập
                </a>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useAuthComposable } from '@/composables/auth';
import { Cookie } from '@/classes/cookie';

export default defineComponent({
  name: 'LoginPage',
  components: {},
  setup(props, { emit }) {
    const authComposable = useAuthComposable();
    const {
        requestResetPassword,
        errorResetPassword,
        handleSubmitResetPassword,
    } = authComposable;
    const token = Cookie.get('token-confirm-code-reset-password') ?? '';
    requestResetPassword.token = token;
    return {
        requestResetPassword,
        errorResetPassword,
        handleSubmitResetPassword,
    };
  },
});
</script>
