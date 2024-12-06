<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
    <div class="modal-body">
      <div class="text-center mb-6">
        <h4 class="mb-2">{{ isCreateSemester ? 'Thêm học kỳ' : 'Chỉnh sửa học kỳ' }}</h4>
      </div>
      <form class="row g-6">
        <div class="col-12 col-md-6">
          <label class="form-label" for="semesterName">Tên học kỳ(*)</label>
          <input
            id="semesterName"
            class="form-control"
            name="semesterName"
            placeholder="Nhập tên học kỳ"
            type="text"
            v-model="requestSemesterUpdate.semesterName"
          />
          <div v-if="errorSemester.SemesterName" class="text-danger">{{ errorSemester.SemesterName[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="semesterCode">Mã học kỳ(*)</label>
          <input
            id="semesterCode"
            class="form-control"
            name="semesterCode"
            placeholder="Nhập mã học kỳ"
            type="text"
            v-model="requestSemesterUpdate.semesterCode"
          />
          <div v-if="errorSemester.SemesterCode" class="text-danger">{{ errorSemester.SemesterCode[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="startDate">Ngày bắt đầu </label>
          <el-date-picker
            id="startDate"
            name="startDate"
            placeholder="Nhập ngày bắt đầu"
            type="date" format="DD/MM/YYYY"
            v-model="requestSemesterUpdate.startDate"
          />
          <div v-if="errorSemester.StartDate" class="text-danger">{{ errorSemester.StartDate[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="endDate">Ngày kết thúc</label>
          <el-date-picker
            id="endDate"
            name="endDate"
            placeholder="Nhập ngày kết thúc"
            type="date" format="DD/MM/YYYY"
            v-model="requestSemesterUpdate.endDate"
          />
          <div v-if="errorSemester.EndDate" class="text-danger">{{ errorSemester.EndDate[0] }}</div>
        </div>
      </form>
    </div>
    <template #footer>
      <div class="modal-footer">
        <div class="col-12 text-center">
          <button v-if="isCreateSemester" class="btn btn-primary me-3" type="submit" @click="confirmCreate">Thêm</button>
          <button v-else class="btn btn-primary me-3" type="submit" @click="confirmAction">Lưu</button>
          <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, toRefs } from 'vue';
import { useSemesterComposable } from '@/composables/semester';
import ModalComponent from '@/components/common/Modal.vue';

export default defineComponent({
  name: 'ModalEditSemesterComponent',
  components: {
    ModalComponent,
  },
  props: {
    isShowModal: {
      type: Boolean,
      default: false,
    },
    isCreateSemester: {
      type: Boolean,
      default: false,
    },
  },
  emits: ['closeModal', 'confirmAction', 'confirmCreate'],
  setup(props, { emit }) {
    const semesterComposable = useSemesterComposable();
    const { requestSemesterUpdate, errorSemester } = semesterComposable;
    const { } = toRefs(props);

    const close = () => {
      emit('closeModal');
    };

    const confirmAction = async () => {
      emit('confirmAction');
    };

    const confirmCreate = async () => {
      emit('confirmCreate');
    };

    return {
      errorSemester,
      confirmCreate,
      requestSemesterUpdate,
      close,
      confirmAction,
    };
  },
});
</script>

<style scoped>
.el-dialog__body {
  padding: 1.5rem !important;
}

.modal-body {
  padding: 1.5rem;
}
</style>