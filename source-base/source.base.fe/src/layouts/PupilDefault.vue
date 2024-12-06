<template>
  <div class="layout-wrapper layout-content-navbar">
    <div class="layout-container">
      <SidebarPupilComponent />
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
import SidebarPupilComponent from '@/components/layouts/SidebarPupil.vue';

export default defineComponent({
  name: 'pupil-layout',
  components: {
    SidebarPupilComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>
