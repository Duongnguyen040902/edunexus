<template>
  <div class="layout-wrapper layout-content-navbar">
    <div class="layout-container">
      <SidebarSchoolComponent />
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
import SidebarSchoolComponent from '@/components/layouts/SidebarSchool.vue';

export default defineComponent({
  name: 'school-layout',
  components: {
    SidebarSchoolComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>
