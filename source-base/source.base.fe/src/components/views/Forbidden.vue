<template>
  <div class="container-xxl container-p-y">
    <div class="misc-wrapper">
      <h1 class="mb-2 mx-2" style="line-height: 6rem; font-size: 6rem">403</h1>
      <h4 class="mb-2 mx-2">Bạn không được phép truy cập! 🔐</h4>
      <p class="mb-6 mx-2">Bạn không có quyền truy cập vào trang này. Quay về đăng nhập!</p>
      <router-link :to="'/authen/login'" class="btn btn-primary">Bấm vào đây để đăng nhập</router-link>
      <div class="mt-6">
        <img
          alt="page-misc-not-authorized-light"
          class="img-fluid"
          data-app-dark-img="illustrations/girl-with-laptop-dark.png"
          data-app-light-img="illustrations/girl-with-laptop-light.png"
          src="@/assets/images/error/403.png"
          width="500"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'ForbiddenComponent',
  components: {},
  props: {},
  emits: [],
  setup(props, { emit }) {
    return {};
  },
});
</script>

<style scoped>
.misc-wrapper {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 1.5rem * 2);
  text-align: center;
}
</style>
