<template>
  <ModalComponent
    :modelValue="showModal"
    :title="'Chi tiết hồ sơ điểm danh'"
    :width="'80%'"
    @update:modelValue="$emit('update:showModal', $event)"
    :before-close="handleClose"
  >
    <div class="modal-body">
      <div class="card">
        <div class="table-responsive text-nowrap">
          <table class="table">
            <thead>
              <tr>
                <th>Họ và Tên</th>
                <th>Ảnh</th>
                <th>Điểm danh</th>
                <th>Ghi chú</th>
              </tr>
            </thead>
            <tbody class="table-border-bottom-0">
              <tr v-for="pupil in attendanceRecord" :key="pupil.pupilId">
                <td>
                  <strong>{{ pupil.pupilName }}</strong>
                </td>
                <td>
                  <ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">
                    <li
                      data-bs-toggle="tooltip"
                      data-popup="tooltip-custom"
                      data-bs-placement="top"
                      class="avatar avatar-xl pull-up"
                      :title="pupil.pupilName"
                    >
                      <img :src="`${apiUrl}${pupil.image}`" alt="Avatar" class="rounded-circle" />
                    </li>
                  </ul>
                </td>
                <td>
                  <div class="form-check form-switch mb-2">
                    <input class="form-check-input" type="checkbox" v-model="pupil.isAttend" disabled />
                    <label class="form-check-label">Có mặt</label>
                  </div>
                </td>
                <td>
                  <div class="form-input mb-2">
                    <input class="form-control" type="text" v-model="pupil.feedback" disabled />
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button class="btn btn-outline-secondary" style="margin-right: 10px" @click="handleClose">ĐÓNG</button>
        <button class="btn btn-primary" @click="handleEdit">CHỈNH SỬA</button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ResponseGetAttendanceRecord } from '@/types/model/class-attendance.ts';

export default defineComponent({
  name: 'ModalViewAttendanceDetail',
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    attendanceRecord: {
      type: Array as PropType<ResponseGetAttendanceRecord[]>,
    },
  },
  emits: ['update:showModal', 'edit'],
  setup(props, { emit }) {
    const apiUrl = import.meta.env.VITE_APP_API_URL;

    const handleClose = () => {
      emit('update:showModal', false);
    };

    const handleEdit = () => {
      emit('update:showModal', false);
      emit('edit');
    };

    return {
      handleClose,
      handleEdit,
      apiUrl,
    };
  },
});
</script>

<style scoped>
.table td {
  padding: 0.4rem; /* Adjust the padding as needed */
}

.table tr {
  margin: 0; /* Remove margin */
}

.dialog-footer {
  text-align: right;
}
</style>
