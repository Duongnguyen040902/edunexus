<template>
  <div class="layout-wrapper layout-content-navbar">
    <div class="layout-container">
      <SidebarComponent />
      <div class="layout-page">
        <div class="content-wrapper">
          <slot></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import SidebarComponent from '@/components/layouts/Sidebar.vue';

export default defineComponent({
  name: 'default-layout',
  components: {
    SidebarComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>
