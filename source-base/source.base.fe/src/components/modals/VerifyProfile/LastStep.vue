<template>
  <div class="formbold-form-confirm">
    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.</p>

    <div>
      <button class="formbold-confirm-btn active">Yes! I want it.</button>
      <button class="formbold-confirm-btn">No! I don’t want it.</button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'LastStep',
  components: {},
  props: {},
  emits: [],
  setup(props, { emit }) {
    return {};
  },
});
</script>

<style scoped></style>
