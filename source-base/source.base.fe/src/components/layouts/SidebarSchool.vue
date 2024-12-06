<template>
  <aside id="layout-menu" class="layout-menu menu-vertical menu bg-menu-theme">
    <div class="menu-inner-shadow"></div>
    <MenuItemSchoolComponent />
  </aside>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import MenuItemSchoolComponent from '@/components/views/sidebar/MenuItemSchool.vue';

export default defineComponent({
  name: 'SidebarSchoolComponent',
  components: {
    MenuItemSchoolComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>

<style lang="css" scoped></style>
