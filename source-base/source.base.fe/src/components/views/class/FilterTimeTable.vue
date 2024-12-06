<template>
  <div class="d-flex justify-content-end align-items-center">
    <div class="col-md-6 user_role d-flex align-items-center">
      <h6 class="mb-0 me-2 nowrap-text">Chọn kỳ:</h6>
      <select class="form-select text-capitalize" v-model="selectedSemester" @change="handleSelect(selectedSemester)">
        <option v-for="semester in semesters" :key="semester.id" :value="semester.id">
          {{ semester.semesterName }} - {{ semester.schoolYearName }}
        </option>
      </select>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from 'vue';
import { ResponseGetSemesterInterface } from '@/types/model/semester';

export default defineComponent({
  name: 'FilterTimetableComponent',
  props: {
    semesters: {
      type: Array as PropType<ResponseGetSemesterInterface[]>,
      default: () => [],
    },
    semesterId: {
      type: Number,
      default: 0,
    },
  },

  emits: ['semesterSelected'],
  setup(props, { emit }) {
    const selectedSemester = ref<number | null>(null);

    const handleSelect = (semesterId: number) => {
      emit('semesterSelected', semesterId);
    };

    // Watch for changes in semesters or semesterId to set the default selected semester
    watch(
      () => [props.semesters, props.semesterId],
      ([newSemesters, newSemesterId]) => {
        const defaultSemester = newSemesters.find(semester => semester.id === newSemesterId) || newSemesters[0] || null;
        selectedSemester.value = defaultSemester ? defaultSemester.id : null;
      },
      { immediate: true },
    );

    return {
      selectedSemester,
      handleSelect,
    };
  },
});
</script>
<style scoped>
.nowrap-text {
  white-space: nowrap;
}
</style>
