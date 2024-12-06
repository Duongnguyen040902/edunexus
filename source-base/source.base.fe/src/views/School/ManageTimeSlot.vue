<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="layout-container">
      <div class="layout-page">
        <div class="content-wrapper">
          <div>
            <TableTimeSlot @refreshPage="refreshPage" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import TableTimeSlot from '@/components/views/time-slot/TableTimeSlot.vue';
import { useTimeSlotComposable } from '@/composables/time-slot';
import { defineComponent, onMounted } from 'vue';

export default defineComponent({
  name: 'ManageTimeSlot',
  components: {
    TableTimeSlot,
  },
  setup() {
    const { timeSlotData, handleGetAllTimeSlots } = useTimeSlotComposable();

    onMounted(async () => {});

    const refreshPage = async () => {
      await handleGetAllTimeSlots();
    };


    return {
      timeSlotData,
      refreshPage
    };
  },
});
</script>
