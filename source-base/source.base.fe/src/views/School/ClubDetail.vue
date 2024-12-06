<template>
    <div class="container-xxl flex-grow-1 container-p-y">
        <div>
            <detail-club />
        </div>
    </div>

</template>

<script lang="ts">
import { defineComponent, onMounted } from 'vue';
import DetailClub from '@/components/views/club/DetailClub.vue';
import { useClubDetailComposable } from '@/composables/club';
export default defineComponent({
    name: "ClubManagement",
    components: {
        DetailClub
    },
    setup() {
        const { handleGetAllClubs } = useClubDetailComposable();
        onMounted(async () => {
        });

        return {

        };
    },
});
</script>