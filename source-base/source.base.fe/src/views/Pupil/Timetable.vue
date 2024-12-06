<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <div class="card mt-3">
      <div class="card-header border-bottom d-flex align-items-center justify-content-between flex-column flex-md-row">
        <h5 class="card-title mb-0">Thời Khóa biểu</h5>
        <div class="filter-container mt-2 mt-md-0">
          <FilterTimetableComponent
            :semesters="semesterData"
            :semesterId="selectedSemesterId?.valueOf() ?? 0"
            @semesterSelected="handleSemesterSelected"
          />
        </div>
      </div>
      <div class="table-responsive text-nowrap">
        <TableTimeTableOfPupilComponent
          :timetables="timetableData"
          :time_slot="timeSlotData"
          :class-id="classId"
          :semester-id="selectedSemesterId ?? 0"
          :school-id="schoolId"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useTimetableComposable } from '@/composables/timetable';
import TableTimeTableOfPupilComponent from '@/components/views/pupil/TableTimeTable.vue';
import FilterTimetableComponent from '@/components/views/class/FilterTimeTable.vue';

export default defineComponent({
  name: 'TimeTable',
  components: {
    TableTimeTableOfPupilComponent,
    FilterTimetableComponent,
  },
  setup() {
    const {
      schoolId,
      classId,
      handleFetchTimetable,
      timetableData,
      handleFetchTimeSlots,
      timeSlotData,
      handleFetchSemester,
      semesterData,
      findLatestSemesterWithTimetable,
      selectedSemesterId,
      updateTimetables,
      handleSemesterSelected,
    } = useTimetableComposable();

    onMounted(async () => {
      await handleFetchSemester();
      selectedSemesterId.value = findLatestSemesterWithTimetable();
      if (selectedSemesterId.value !== null) {
        await handleSemesterSelected(selectedSemesterId.value, classId.value);
        await handleFetchTimetable(classId.value, selectedSemesterId.value);
      }
      await handleFetchTimeSlots(schoolId);
    });

    return {
      timetableData,
      timeSlotData,
      classId,
      selectedSemesterId,
      schoolId,
      updateTimetables,
      handleSemesterSelected: (semesterId: number) => handleSemesterSelected(semesterId, classId.value),
      semesterData,
    };
  },
});
</script>

<style scoped>
.table-responsive {
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
}

.table {
  width: 100%;
  max-width: 100%;
  margin-bottom: 1rem;
  background-color: transparent;
}

.table th,
.table td {
  white-space: nowrap;
}

.filter-container {
  flex-grow: 1;
  max-width: 500px;
}
</style>