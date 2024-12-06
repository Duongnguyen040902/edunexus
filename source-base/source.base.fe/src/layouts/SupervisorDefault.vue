<template>
    <div class="layout-wrapper layout-content-navbar">
        <div class="layout-container">
            <SidebarSupervisorComponent />
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
import SidebarSupervisorComponent from '@/components/layouts/SidebarSupervisor.vue';

export default defineComponent({
    name: 'supervisor-layout',
    components: {
        SidebarSupervisorComponent,
    },
    props: {},
    setup(props: {}, { emit }) {
        return {};
    },
});
</script>