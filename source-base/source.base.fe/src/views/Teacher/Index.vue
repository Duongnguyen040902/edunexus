<template>
  <div class="container-fluid container-p-y d-flex justify-content-center align-items-center vh-100">
    <div class="misc-wrapper text-center">
      <img src="/src/assets/images/logo/edunexus_logo.png" class="mb-2 mx-2"  width="200"/>
      <h4 class="mb-2 mx-2">Chào mừng đến với EduNexus</h4>
      <p class="mb-6 mx-2" style="max-width: 700px">Chúng tôi tự hào là cầu nối giữa nhà trường và phụ huynh, đồng hành cùng bạn trên hành trình phát triển giáo dục.</p>


      <div class="mt-6">
        <img
          alt="page-misc-error-light"
          class="img-fluid"
          data-app-dark-img="illustrations/page-misc-error-dark.png"
          data-app-light-img="illustrations/page-misc-error-light.png"
          src="/src/assets/images/logo/edu-icon.png"
          width="500"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'Home',
  setup() {
    return {};
  },
});
</script>

<style scoped>
.home-container {
  text-align: center;
  margin-top: 50px;
}

h1 {
  font-size: 2.5em;
  margin-bottom: 20px;
}

p {
  font-size: 1.2em;
  line-height: 1.5;
}
</style>