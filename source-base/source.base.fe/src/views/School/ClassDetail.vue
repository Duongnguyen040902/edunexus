<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <TableClassDetail  />

  </div>
</template>
<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import TableClassDetail from '@/components/views/school-admin/TableClassDetail.vue';
import { useClassDetailComposable } from '@/composables/class';
export default defineComponent({
  name: 'ClassDetail',
  components: {
    TableClassDetail,
  },
  setup() {
    const { fetchClassDetail, classDetail, classId, currentSemester, fetchCurrentSemester } =
      useClassDetailComposable();

    onMounted(async () => {
    });

    return {
      currentSemester,
      classDetail,
      classId,
      fetchCurrentSemester,
    };
  },
});
</script>
