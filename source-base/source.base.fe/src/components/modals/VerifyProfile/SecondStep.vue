<template>
  <div>
    <div>
      <label class="formbold-form-label" for="verify-code">Mã xác nhận</label>
      <textarea
        id="verify-code"
        class="formbold-form-input"
        name="verify-code"
        placeholder="Nhập mã xác nhận"
        rows="6"
      ></textarea>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'SecondStep',
  components: {},
  props: {},
  emits: [],
  setup(props, { emit }) {
    return {};
  },
});
</script>

<style scoped></style>
