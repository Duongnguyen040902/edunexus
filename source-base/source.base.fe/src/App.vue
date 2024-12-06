<template>
  <component v-if="layout" :is="layout">
    <router-view></router-view>
  </component>
  <router-view v-else></router-view>
</template>

<script lang="ts">
import { defineComponent, markRaw, ref, watch } from 'vue';
import { useRoute } from 'vue-router';

export default defineComponent({
  setup() {
    const layout = ref();
    const route = useRoute();

    watch(
      () => route.meta.layout as string,
      async (metaLayout: string) => {
        let component;
        if (metaLayout) {
          document.querySelector('#app')?.classList.add(`${metaLayout}Default`.toLowerCase());
          try {
            component = metaLayout && (await import(`@/layouts/${metaLayout}Default.vue`));
          } catch (error) {
            console.error(`Failed to load layout: ${metaLayout}`, error);
          }
        }
        layout.value = component?.default ? markRaw(component.default) : null;
      },
      { immediate: true },
    );

    return { layout };
  },
});
</script>
