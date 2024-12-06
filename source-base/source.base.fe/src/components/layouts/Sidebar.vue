<template>
  <aside id="layout-menu" class="layout-menu menu-vertical menu bg-menu-theme">
    <div class="menu-inner-shadow"></div>
    <MenuItemComponent />
  </aside>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import MenuItemComponent from '@/components/views/sidebar/MenuItem.vue';

export default defineComponent({
  name: 'SidebarComponent',
  components: {
    MenuItemComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>

<style lang="css" scoped></style>
