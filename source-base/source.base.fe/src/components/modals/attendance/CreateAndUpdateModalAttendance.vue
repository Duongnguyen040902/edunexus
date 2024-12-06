<template>
  <ModalComponent
    :modelValue="showModal"
    :title="isUpdateMode ? 'Cập nhật hồ sơ điểm danh' : 'Tạo hồ sơ điểm danh'"
    :width="'80%'"
    @update:modelValue="$emit('update:showModal', $event)"
    :before-close="handleClose"
  >
    <p>
      <div v-if="errorResponseAttendance.feedback" class="text-danger">
        {{ errorResponseAttendance.feedback[0] }}
      </div>
      <div v-if="errorResponseAttendance.createdDate" class="text-danger">
        {{ errorResponseAttendance.createdDate[0] }}
      </div>
    </p>
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
                <input class="form-check-input" type="checkbox" v-model="pupil.isAttend" />
                <label class="form-check-label">Có mặt</label>
              </div>
            </td>
            <td>
              <div class="form-input mb-2">
                <input class="form-control" type="text" v-model="pupil.feedback" />
              </div>
            </td>
          </tr>
          </tbody>
        </table>
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button class="btn btn-secondary" style="margin-right: 10px" @click="handleClose">HỦY</button>
        <button class="btn btn-primary" @click="handleModalConfirm">
          {{ isUpdateMode ? "Cập nhật" : "Thêm" }}
        </button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import ModalComponent from "@/components/common/Modal.vue";
import { ResponseGetAttendanceRecord } from "@/types/model/class-attendance";
import { useAttendanceComposable } from "@/composables/class-attendance";

export default defineComponent({
  name: "CreateAndUpdateAttendanceModalComponent",
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    attendanceRecord: {
      type: Array as PropType<ResponseGetAttendanceRecord[]>,
    },
  },
  emits: ["update:showModal", "confirmUpdate", "confirm", "update:isUpdateMode", "refreshList"],
  setup(props, { emit }) {
    const apiUrl = import.meta.env.VITE_APP_API_URL;

    const { createAttendance, updateAttendance, errorResponseAttendance, handleCloseModal } = useAttendanceComposable();

    const handleModalConfirm = async () => {
      if (props.isUpdateMode) {
        await updateAttendance(props.attendanceRecord as ResponseGetAttendanceRecord[], emit);
      } else {
        await createAttendance(props.attendanceRecord as ResponseGetAttendanceRecord[], emit);
      }
    };

    const handleClose = () => {
      handleCloseModal(emit);
    };

    return {
      handleClose,
      apiUrl,
      errorResponseAttendance,
      handleModalConfirm,
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