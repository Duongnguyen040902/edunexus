<template>
  <ForbiddenComponent />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ForbiddenComponent from '@/components/views/Forbidden.vue';

export default defineComponent({
  name: 'Forbidden',
  components: {
    ForbiddenComponent,
  },
  props: {},
  emits: [],
  setup(props, { emit }) {
    return {};
  },
});
</script>

<style scoped></style>
