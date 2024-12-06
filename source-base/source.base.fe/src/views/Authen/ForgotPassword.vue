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
                        <h4 class="mb-1">Quên mật khẩu? 🔒</h4>
                        <p class="mb-6">Nhập email của bạn để chúng tôi gửi mã xác nhận khôi phục mật khẩu</p>
                        <form class="mb-6 fv-plugins-bootstrap5 fv-plugins-framework" @submit.prevent="handleSubmitForgotPassword">
                            <div class="mb-6 fv-plugins-icon-container">
                                <label for="email" class="form-label">Email <span class="required">*</span></label>
                                <input type="text" class="form-control" id="email" name="email" placeholder="Email của bạn" v-model="requestForgotPassword.email">
                                <div v-if="errorForgotPassword" class="text-danger">{{ errorForgotPassword }}</div>
                            </div>
                            <button class="btn btn-primary d-grid w-100">Gửi mã xác nhận</button>
                        </form>
                        <div class="text-center">
                            <a href="/authen/login" class="d-flex justify-content-center"> <!-- TODO -->
                                <i class="bx bx-chevron-left scaleX-n1-rtl me-1"></i>
                                Quay trở lại đăng nhập
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
    name: 'forgot-password',
    components: {},
    setup(props, { emit }) {
        const authComposable = useAuthComposable();
        const { errorForgotPassword, requestForgotPassword, handleSubmitForgotPassword, handleRedirectPageLogin } = authComposable;
        return {
            errorForgotPassword,
            requestForgotPassword,
            handleRedirectPageLogin,
            handleSubmitForgotPassword,
        };
    },
});
</script>