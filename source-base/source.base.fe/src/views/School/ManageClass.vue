<template>
  <div>
    <ClassTable :classDataProp="classData"  @refresh="updateData"/>
  </div>
</template>

<script lang="ts">
import ClassTable from '@/components/views/school-admin/ClassTable.vue';
import { useAdminSchoolComposable } from '@/composables/admin-school';
import { defineComponent, onMounted } from 'vue';

export default defineComponent({
  name: "ManageCLass",
  components: {
    ClassTable
  },
  setup() {
    const { 
      classData,
      requestGetClass, 
      handleFetchClass 
    } = useAdminSchoolComposable();

    onMounted(async () => {
    });

    const updateData = (newData: any) => {
      classData.value = [];
      classData.value.push(...newData.value);
    };
    
    return {
        requestGetClass,
        classData,
        updateData
    };
  },
});
</script>
