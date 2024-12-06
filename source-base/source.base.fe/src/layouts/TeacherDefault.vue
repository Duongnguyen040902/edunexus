<template>
  <div class="layout-wrapper layout-content-navbar">
    <div class="layout-container">
      <SidebarTeacherComponent />
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
import SidebarTeacherComponent from '@/components/layouts/SidebarTeacher.vue';

export default defineComponent({
  name: 'teacher-layout',
  components: {
    SidebarTeacherComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>
