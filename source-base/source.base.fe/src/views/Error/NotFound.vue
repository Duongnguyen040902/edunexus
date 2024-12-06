<template>
  <NotFoundComponent />
</template>
<script lang="ts">
import { defineComponent } from 'vue';
import NotFoundComponent from '@/components/views/NotFound.vue';
export default defineComponent({
  name: 'Index',
  components: {
    NotFoundComponent,
  },
  setup() {
    return {};
  },
});
</script>
