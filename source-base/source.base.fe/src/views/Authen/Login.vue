<template>
  <div class="container-xxl">
    <div class="authentication-wrapper authentication-basic container-p-y">
      <div class="authentication-inner">
        <div class="card px-sm-6 px-0">
          <div class="card-body">
            <div class="app-brand justify-content-center">
              <img src="@assets/images/logo/edunexus_logo.png" class="mb-2 mx-2" width="200"/>
            </div>
            <h4 class="mb-1">Xin chào đến EduNexus 👋</h4>
            <p class="mb-6">Vui lòng nhập email hoặc tên đăng nhập và mật khẩu của bạn</p>

            <form @submit.prevent="handleSubmitLoginByMode">
              <div class="mb-6 fv-plugins-icon-container">
                <label class="form-label" for="email">Email <span class="required">*</span></label>
                <input
                  id="email"
                  v-model="loginRequest.email"
                  class="form-control"
                  name="email-username"
                  placeholder="Nhập email của bạn"
                  type="text"
                />
                <div v-if="errorResponseLogin.Email" class="text-danger">{{ errorResponseLogin.Email[0] }}</div>
              </div>
              <div class="mb-6 form-password-toggle fv-plugins-icon-container">
                <label class="form-label" for="password">Mật khẩu <span class="required">*</span></label>
                <div class="input-group input-group-merge has-validation">
                  <input
                    id="password"
                    v-model="loginRequest.password"
                    aria-describedby="password"
                    class="form-control"
                    name="password"
                    placeholder="············"
                    type="password"
                  />
                  <span class="input-group-text cursor-pointer"><i class="bx bx-hide"></i></span>
                </div>
                <div v-if="errorResponseLogin.Password" class="text-danger">{{ errorResponseLogin.Password[0] }}</div>
              </div>
              <div class="mb-6 form-password-toggle fv-plugins-icon-container">
                <select id="UserPlan" class="form-select text-capitalize" v-model="loginRequest.mode">
                  <option value="-1">Chọn vai trò</option>
                  <option v-for="(mode, index) in modesLogin" :value="mode.value">
                    {{ mode.label }}
                  </option>
                </select>
                <div v-if="errorResponseLogin.Mode" class="text-danger">{{ errorResponseLogin.Mode[0] }}</div>
              </div>
              <div class="mb-8">
                <div class="d-flex justify-content-between mt-8">
                  <a href="/authen/forgot-password"> <!--TODO -->
                    <span>Quên mật khẩu?</span>
                  </a>
                </div>
              </div>
              <div class="mb-6">
                <button class="btn btn-primary d-grid w-100">Đăng nhập</button>
              </div>
            </form>

            <p class="text-center">
              <span>Bạn chưa có tài khoản? </span>
                 <a href="/authen/register"> <!--TODO -->
                <span>Tạo mới tài khoản</span>
              </a>
            </p>

            <div class="divider my-6">
              <div class="divider-text">or</div>
            </div>

            <div class="d-flex justify-content-center">
              <a class="btn btn-sm btn-icon rounded-circle btn-text-google-plus" href="javascript:">
                <i class="tf-icons bx bxl-google"></i>
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useAuthComposable } from '@/composables/auth';

export default defineComponent({
  name: 'LoginPage',
  components: {},
  setup(props, { emit }) {
    const authComposable = useAuthComposable();
    const {
      loginRequest,
      modesLogin,
      errorResponseLogin,
      handleSubmitLoginByMode,
      handleRedirectPageForgotPassword,
      handleRedirectPageRegister,
    } = authComposable;

    return {
      modesLogin,
      loginRequest,
      errorResponseLogin,
      handleSubmitLoginByMode,
      handleRedirectPageForgotPassword,
      handleRedirectPageRegister,
    };
  },
});
</script>
