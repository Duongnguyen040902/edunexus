<template>
    <aside id="layout-menu" class="layout-menu menu-vertical menu bg-menu-theme">
        <div class="menu-inner-shadow"></div>
        <MenuItemSupervisorComponent />
    </aside>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import MenuItemSupervisorComponent from '@/components/views/sidebar/MenuItemSupervisor.vue';

export default defineComponent({
    name: 'SidebarSupervisorComponent',
    components: {
        MenuItemSupervisorComponent,
    },
    props: {},
    setup(props: {}, { emit }) {
        return {};
    },
});
</script>

<style lang="css" scoped></style>