<template>
  <div>
    <table class="table">
      <thead>
      <tr>
        <th>Tiết</th>
        <th>Thứ 2</th>
        <th>Thứ 3</th>
        <th>Thứ 4</th>
        <th>Thứ 5</th>
        <th>Thứ 6</th>
        <th>Thứ 7</th>
        <th>Chủ nhật</th>
      </tr>
      </thead>
      <tbody class="table-border-bottom-0">
      <tr v-for="(slot, index) in time_slot" :key="index">
        <td class="text-nowrap">
          {{ slot.name }} <br />
          {{ slot.startTime }} - {{ slot.endTime }}
        </td>
        <td v-for="dayIndex in 7" :key="dayIndex">
          <div v-if="getSubject(slot.id, dayIndex)">
            <span>{{ getSubject(slot.id, dayIndex)?.subjectName }}</span>
          </div>
          <div v-else>

          </div>
        </td>
      </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, onMounted } from 'vue';
import { useTimetableComposable } from '@/composables/timetable';
import { ResponseGetTimetableInterface } from '@/types/model/timetable';
import { ResponseGetTimeSlotInterface } from '@/types/model/timeslot';

export default defineComponent({
  name: 'TableTimeTableOfPupilComponent',
  props: {
    timetables: {
      type: Array as PropType<ResponseGetTimetableInterface[]>,
      default: () => [],
    },
    time_slot: {
      type: Array as PropType<ResponseGetTimeSlotInterface[]>,
      default: () => [],
    },
    classId: {
      type: Number,
      required: true,
    },
    semesterId: {
      type: Number,
      required: true,
    },
    schoolId: {
      type: Number,
      required: true,
    },
  },
  setup(props) {
    const { handleFetchSubjects, subjectData } = useTimetableComposable();

    const getSubject = (timeSlotId: number, dayOfWeek: number) => {
      return props.timetables.find(t => t.timeSlotId === timeSlotId && t.dayOfWeek === dayOfWeek) || null;
    };

    onMounted(async () => {
      try {
        await handleFetchSubjects();
      } catch (error) {
        console.error('Error during onMounted:', error);
      }
    });

    return {
      getSubject,
      subjects: subjectData,
    };
  },
});
</script>

<style scoped>
.dialog-footer {
  text-align: right;
}

.custom-confirm-button {
  background-color: #696cff !important;
  border-color: #696cff !important;
}
</style>