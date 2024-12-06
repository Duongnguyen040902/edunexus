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
            <div
              v-if="getSubject(slot.id, dayIndex)"
              class="dropdown d-flex align-items-center justify-content-between"
            >
              <span style="margin-right: 5px">{{ getSubject(slot.id, dayIndex)?.subjectName }}</span>
              <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                <i class="bx bx-dots-vertical-rounded"></i>
              </button>
              <div class="dropdown-menu">
                <a
                  class="dropdown-item"
                  href="javascript:void(0);"
                  @click="openModal(dayIndex, slot.id, getSubject(slot.id, dayIndex)?.subjectId ?? null)"
                  ><i class="bx bx-edit-alt me-1"></i> Sửa</a
                >
                <a
                  class="dropdown-item"
                  href="javascript:void(0);"
                  @click="
                    openDeleteModal(dayIndex, slot.id, getSubject(slot.id, dayIndex)?.subjectId ?? null, semesterId)
                  "
                  ><i class="bx bx-trash me-1"></i> Xóa</a
                >
              </div>
            </div>
            <button v-else type="button" class="btn btn-outline-primary" @click="openModal(dayIndex, slot.id)" plain>
              Thêm
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <CreateAndUpdateTimeTableModalComponent
      :showModal="showModal"
      :isUpdateMode="isUpdateMode"
      :selectedSubjectId="selectedSubjectId"
      :subjects="subjects"
      @update:showModal="showModal = $event"
      @update:selectedSubjectId="selectedSubjectId = $event"
      @confirm="handleModalConfirm"
    />
    <DeleteTimeTableModalComponent
      :showDeleteModal="showDeleteModal"
      @update:showDeleteModal="showDeleteModal = $event"
      @confirm="handleDeleteTimetableConfirm"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, onMounted } from 'vue';

import ModalComponent from '@/components/common/Modal.vue';
import { useTimetableComposable } from '@/composables/timetable';
import CreateAndUpdateTimeTableModalComponent from '@/components/modals/timetable/CreateAndUpdateTimeTableModal.vue';
import DeleteTimeTableModalComponent from '@/components/modals/timetable/DeleteTimeTable.vue';
import { ResponseGetTimetableInterface } from '@/types/model/timetable';
import { ResponseGetTimeSlotInterface } from '@/types/model/timeslot';
export default defineComponent({
  name: 'TableTimeTableComponent',
  components: {
    ModalComponent,
    CreateAndUpdateTimeTableModalComponent,
    DeleteTimeTableModalComponent,
  },
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
  setup(props, { emit }) {
    const {
      updateTimetables,
      showDeleteModal,
      openDeleteModal,
      selectedSemesterId,
      timetableData,
      subjectData,
      handleFetchTimetable,
      handleModalConfirm,
      handleFetchSubjects,
      openModal,
      handleDeleteTimetableConfirm,
      showModal,
      isUpdateMode,
      selectedTimeSlotId,
      selectedDayOfWeek,
      selectedSubjectId,
    } = useTimetableComposable();

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
      selectedSemesterId,
      showModal,
      isUpdateMode,
      selectedTimeSlotId,
      selectedDayOfWeek,
      selectedSubjectId,
      subjects: subjectData,
      showDeleteModal,
      openModal,
      handleModalConfirm: () => handleModalConfirm(emit, props.semesterId),
      getSubject,
      handleDeleteTimetableConfirm: () => handleDeleteTimetableConfirm(emit, props.semesterId),
      openDeleteModal,
      updateTimetables,
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
