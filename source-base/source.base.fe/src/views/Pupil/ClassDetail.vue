<template>
    <div class="container-fluid flex-grow-1 container-p-y">

        <ClassInfoComponent :classDetail="classDetail" :current-semester="currentSemester" />
        <TableClassEnrollmentComponent :classDetail="classDetail" />
    </div>
</template>
<script lang="ts">

import { defineComponent, onMounted, ref } from 'vue';
import ClassInfoComponent from '@/components/views/Class/ClassInfo.vue';
import TableClassEnrollmentComponent from '@/components/views/Class/TableClassEnrollment.vue';
import ClassSidebarComponent from '@/components/views/Class/ClassSiderbar.vue';
import { useClassDetailComposable } from '@/composables/class';
export default defineComponent({
    name: 'ClassDetail',
    components: {
        ClassInfoComponent,
        TableClassEnrollmentComponent,
        ClassSidebarComponent
    },
    setup() {
        const { fetchClassDetail, classDetail, classId, currentSemester , fetchCurrentSemester } = useClassDetailComposable();

        onMounted(async () => { 
            await fetchCurrentSemester();
            await fetchClassDetail(classId.value, currentSemester.id);
        });

        return {
            currentSemester,
            classDetail,
            classId,
            fetchCurrentSemester
        };
    }
});
</script>