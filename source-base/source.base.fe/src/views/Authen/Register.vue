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
            <h4 class="mb-6">Đăng ký tài khoản mới 🚀</h4>
            <form @submit.prevent="handleSubmitRegister" class="mb-6">
              <div class="mb-3 fv-plugins-icon-container">
                <label for="username" class="form-label">Tên đăng nhập <span class="required">*</span></label>
                <input
                  type="text"
                  class="form-control"
                  id="username"
                  name="username"
                  placeholder="Nhập tên đăng nhập"
                  v-model="registerRequest.username"
                />
                <div v-if="errorsRegister.Username" class="text-danger">{{ errorsRegister.Username[0] }}</div>
              </div>
              <div class="mb-3 fv-plugins-icon-container">
                <label for="email" class="form-label">Email <span class="required">*</span></label>
                <input
                  type="text"
                  class="form-control"
                  id="email"
                  name="email"
                  placeholder="Nhập email"
                  v-model="registerRequest.email"
                />
                <div v-if="errorsRegister.Email" class="text-danger">{{ errorsRegister.Email[0] }}</div>
              </div>
              <div class="mb-3 fv-plugins-icon-container">
                <label for="name-school" class="form-label">Tên trường <span class="required">*</span></label>
                <input
                  type="text"
                  class="form-control"
                  id="name-school"
                  name="name-school"
                  placeholder="Nhập tên trường"
                  v-model="registerRequest.name"
                />
                <div v-if="errorsRegister.Name" class="text-danger">{{ errorsRegister.Name[0] }}</div>
              </div>
              <div class="mb-3 fv-plugins-icon-container">
                <label for="address" class="form-label">Địa chỉ <span class="required">*</span></label>
                <input
                  type="text"
                  class="form-control"
                  id="address"
                  name="address"
                  placeholder="Nhập địa chỉ"
                  v-model="registerRequest.address"
                />
                <div v-if="errorsRegister.Address" class="text-danger">{{ errorsRegister.Address[0] }}</div>
              </div>
              <div class="mb-3 form-password-toggle fv-plugins-icon-container">
                <label class="form-label" for="password">Mật khẩu <span class="required">*</span></label>
                <div class="input-group input-group-merge has-validation">
                  <input
                    type="password"
                    id="password"
                    class="form-control"
                    name="password"
                    placeholder="············"
                    aria-describedby="password"
                    v-model="registerRequest.password"
                  />
                  <span class="input-group-text cursor-pointer"><i class="bx bx-hide"></i></span>
                </div>
                <div v-if="errorsRegister.Password" class="text-danger">{{ errorsRegister.Password[0] }}</div>
              </div>
              <div class="mb-3 form-password-toggle fv-plugins-icon-container">
                <label class="form-label" for="re-password">Xác nhận mật khẩu <span class="required">*</span></label>
                <div class="input-group input-group-merge has-validation">
                  <input
                    type="password"
                    id="re-password"
                    class="form-control"
                    name="re-password"
                    placeholder="············"
                    aria-describedby="re-password"
                    v-model="registerRequest.rePassword"
                  />
                  <span class="input-group-text cursor-pointer"><i class="bx bx-hide"></i></span>
                </div>
                <div v-if="errorsRegister.RePassword" class="text-danger">{{ errorsRegister.RePassword[0] }}</div>
              </div>
              <button class="btn btn-primary d-grid w-100">Đăng ký</button>
            </form>
            <p class="text-center">
              <span>Bạn đã có tài khoản ? </span>
              <a href="/authen/login"> <!--TODO -->
                <span>Đăng nhập ở đây</span>
              </a>
            </p>

            <div class="divider my-6">
              <div class="divider-text">or</div>
            </div>

            <div class="d-flex justify-content-center">
              <a href="javascript:;" class="btn btn-sm btn-icon rounded-circle btn-text-google-plus">
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
    const { registerRequest, errorsRegister, handleSubmitRegister, handleRedirectLoginPage } = authComposable;
    return {
      registerRequest,
      errorsRegister,
      handleSubmitRegister,
      handleRedirectLoginPage,
    };
  },
});
</script>
