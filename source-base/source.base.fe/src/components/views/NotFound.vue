<template>
  <div class="container-xxl container-p-y">
    <div class="misc-wrapper">
      <h1 class="mb-2 mx-2" style="line-height: 6rem; font-size: 6rem">404</h1>
      <h4 class="mb-2 mx-2">Không tìm thấy ⚠️</h4>
      <router-link :to="'/authen/login'" class="btn btn-primary">Bấm vào đây để đăng nhập</router-link>
      <div class="mt-6">
        <img
          alt="page-misc-error-light"
          class="img-fluid"
          data-app-dark-img="illustrations/page-misc-error-dark.png"
          data-app-light-img="illustrations/page-misc-error-light.png"
          src="@/assets/images/avatars/page-misc-error-light.png"
          width="500"
        />
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'NotFoundComponent',
  components: {},
  setup() {
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
