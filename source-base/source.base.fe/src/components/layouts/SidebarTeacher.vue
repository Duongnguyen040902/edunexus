<template>
  <aside id="layout-menu" class="layout-menu menu-vertical menu bg-menu-theme">
    <div class="menu-inner-shadow"></div>
    <MenuItemTeacherComponent />
  </aside>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import MenuItemTeacherComponent from '@/components/views/sidebar/MenuItemTeacher.vue';

export default defineComponent({
  name: 'SidebarTeacherComponent',
  components: {
    MenuItemTeacherComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>

<style lang="css" scoped></style>
