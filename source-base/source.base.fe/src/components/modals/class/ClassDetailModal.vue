<template>
  <ModalComponent
    :modelValue="showModal"
    :title="`Chi tiết lớp`"
    @update:modelValue="$emit('update-showModal', $event)"
    @before-close="handleClose()"
  >
    <div v-if="classDetail" class="d-flex flex-column align-items-start mt-3">
      <div class="class-info mb-4 w-100">
        <h6 class="mb-3">Thông tin lớp</h6>
        <div class="row">
          <div class="col-md-6">
            <p><strong>Tên lớp:</strong> {{ classDetail?.className }}</p>
            <p><strong>Kỳ:</strong> {{ classDetail?.semesterName }}</p>
          </div>
          <div class="col-md-6">
            <p><strong>Năm học:</strong> {{ classDetail?.schoolYearName }}</p>
            <p><strong>Sĩ số:</strong> {{ classDetail?.pupils.length }} học sinh</p>
          </div>
        </div>
      </div>
      <div class="teacher-info mb-4 w-100">
        <h6 class="mb-3">Thông tin giáo viên chủ nhiệm</h6>
        <div v-if="classDetail.homeroomTeacher" class="row">
          <div class="col-md-6">
            <p><strong>Họ tên:</strong> {{ classDetail.homeroomTeacher?.firstName }} {{ classDetail.homeroomTeacher?.lastName }}</p>
            <p><strong>Ngày sinh:</strong> {{ classDetail.homeroomTeacher?.dateOfBirth }}</p>
          </div>
          <div class="col-md-6">
            <p><strong>Giới tính:</strong> {{ classDetail.homeroomTeacher?.gender ? 'Nam' : 'Nữ' }}</p>
            <p><strong>Địa chỉ:</strong> {{ classDetail.homeroomTeacher?.address }}</p>
            <p><strong>Số điện thoại:</strong> {{ classDetail.homeroomTeacher?.phoneNumber }}</p>
          </div>
        </div>
        <div v-else>
          <p>Không tìm thấy thông tin giáo viên</p>
        </div>
      </div>
    </div>

    <template #footer>
      <div class="dialog-footer">
        <button class="btn btn-outline-secondary" style="margin-right: 10px" @click="handleClose()">ĐÓNG</button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ResponseGetClassDetailInterface } from '@/types/model/class.ts';

export default defineComponent({
  name: 'ModalViewClassDetail',
  components: { ModalComponent },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    classDetail: {
      type: Object as PropType<ResponseGetClassDetailInterface | null>,
    },
  },
  emits: ['update-showModal'],
  setup(props, { emit }) {
    const handleClose = () => {
      emit('update-showModal', false);
    };

    return { handleClose };
  },
});
</script>

<style scoped>
.d-flex {
  display: flex;
}
.me-4 {
  margin-right: 1.5rem;
}
.w-100 {
  width: 100%;
}
.mb-3 {
  margin-bottom: 1rem;
}
.mb-4 {
  margin-bottom: 1.5rem;
}
</style>