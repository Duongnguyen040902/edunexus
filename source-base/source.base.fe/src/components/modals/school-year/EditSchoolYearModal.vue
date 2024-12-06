<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
    <div class="modal-body">
      <div class="mb-6">
        <h4 class="mb-2">{{ isCreateSchoolYear ? 'Thêm năm học' : 'Chỉnh sửa năm học' }}</h4>
      </div>
      <form class="row g-6">
        <!-- <div class="col-12">
          <label class="form-label" for="name">Tên năm học</label>
          <input
            id="name"
            class="form-control"
            name="name"
            placeholder="Nhập tên năm học"
            type="text"
            v-model="requestSchoolYearUpdate.name"
          />
          <div v-if="errorSchoolYear.Name" class="text-danger">{{ errorSchoolYear.Name[0] }}</div>
        </div> -->
        <div class="col-12 col-md-6">
          <label class="form-label" for="startDate">Ngày bắt đầu</label>
          <el-date-picker
            id="startDate"
            name="startDate"
            placeholder="Nhập ngày bắt đầu"
            type="date"  format="DD/MM/YYYY"
            v-model="requestSchoolYearUpdate.startDate"
          />
          <div v-if="errorSchoolYear.StartDate" class="text-danger">{{ errorSchoolYear.StartDate[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="endDate">Ngày kết thúc</label>
          <el-date-picker
            id="endDate"
            class=""
            name="endDate"
            placeholder="Nhập ngày kết thúc"
            type="date" format="DD/MM/YYYY"
            v-model="requestSchoolYearUpdate.endDate"
          />
          <div v-if="errorSchoolYear.EndDate" class="text-danger">{{ errorSchoolYear.EndDate[0] }}</div>
        </div>
      </form>
    </div>
    <template #footer>
      <div class="modal-footer">
        <div class="col-12">
          <button v-if="isCreateSchoolYear" class="btn btn-primary me-3" type="submit" @click="confirmCreate">Thêm</button>
          <button v-else class="btn btn-primary me-3" type="submit" @click="confirmAction">Lưu</button>
          <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, toRefs } from 'vue';
import { useSchoolYearComposable } from '@/composables/school-year.ts';
import ModalComponent from '@/components/common/Modal.vue';

export default defineComponent({
  name: 'ModalEditSchoolYearComponent',
  components: {
    ModalComponent,
  },
  props: {
    isShowModal: {
      type: Boolean,
      default: false,
    },
    isCreateSchoolYear: {
      type: Boolean,
      default: false,
    },
  },
  emits: ['closeModal', 'confirmAction','confirmCreate'],
  setup(props, { emit }) {
    const schoolYearComposable = useSchoolYearComposable();
    const { requestSchoolYearUpdate, errorSchoolYear } = schoolYearComposable;
    const {  } = toRefs(props);

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
      errorSchoolYear,
      confirmCreate,
      requestSchoolYearUpdate,
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