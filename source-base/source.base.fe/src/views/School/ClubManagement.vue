<template>
    <div class="container-xxl flex-grow-1 container-p-y">
        <div>
            <table-club />
        </div>
    </div>

</template>

<script lang="ts">
import { defineComponent } from 'vue';
import TableClub from '@/components/views/club/TableClub.vue';
export default defineComponent({
    name: "ClubManagement",
    components: {
        TableClub,
    },
    setup() {
    },
});
</script>