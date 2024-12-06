<template>
  <div class="authentication-wrapper authentication-basic px-4">
    <div class="authentication-inner">
      <div class="card px-sm-6 px-0">
        <div class="card-body">
          <div class="app-brand justify-content-center mb-6">
            <img src="/src/assets/images/logo/edunexus_logo.png" class="mb-2 mx-2" width="200"/>
          </div>
          <h4 class="mb-1">Nhập mã xác nhận của bạn 💬</h4>
          <p class="text-start mb-6">
            chúng tôi vừa gửi cho bạn 1 mã xác nhận đến
            <span class="fw-medium d-block mt-1 text-heading">****gmail.com</span>
          </p>
          <p class="mb-0">Nhập mã nhận gồm 6 chữ số</p>
        <form @submit.prevent="handleVerifyEmail">
          <div class="mb-6 fv-plugins-icon-container">
            <div class="auth-input-wrapper d-flex align-items-center justify-content-between numeral-mask-wrapper">
              <input
                v-for="(code, index) in verifyEmailRequest.verificationCode"
                :key="index"
                type="tel"
                class="form-control auth-input h-px-50 text-center numeral-mask mx-sm-1 mt-2"
                maxlength="1"
                v-model="verifyEmailRequest.verificationCode[index]"
              />
            </div>
            <div v-if="errorVerify.verifyCode" class="text-danger">{{ errorVerify.verifyCode[0] }}</div>
          </div>
          <button class="btn btn-primary d-grid w-100 mb-6" type="submit">Xác nhận</button>
          <div class="text-center">
            Không nhận được mã?
            <a href="javascript:void(0);"> Gửi lại mã </a>
          </div>
        </form>
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
  name: 'verify-page',
  components: {},
  setup(props, { emit }) {
    const authComposable = useAuthComposable();
    const { verifyEmailRequest, errorVerify, handleVerifyEmail } = authComposable;
    const token = Cookie.get('token-verify-email') ?? '';
    verifyEmailRequest.token = token;


    return {
      verifyEmailRequest,
      errorVerify,
      handleVerifyEmail,
    };
  },
});
</script>

