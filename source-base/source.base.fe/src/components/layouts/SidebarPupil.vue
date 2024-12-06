<template>
  <aside id="layout-menu" class="layout-menu menu-vertical menu bg-menu-theme">
    <div class="menu-inner-shadow"></div>
    <MenuItemPupilComponent />
  </aside>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import MenuItemPupilComponent from "@/components/views/sidebar/MenuItemPupil.vue";

export default defineComponent({
  name: "SidebarTeacherComponent",
  components: {
    MenuItemPupilComponent,
  },
  props: {},
  setup(props: {}, { emit }) {
    return {};
  },
});
</script>

<style lang="css" scoped></style>
