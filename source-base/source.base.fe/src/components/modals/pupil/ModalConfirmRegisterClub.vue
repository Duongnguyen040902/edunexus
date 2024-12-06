<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
    <div class="delete-confirm-body">
      <IconWarningComponent width="48" height="48" fill="#f44336" />
      <h4 class="text-center mt-4">Xác nhận</h4>
      <h6 v-if="isRegisterClub">Bạn chắc chắn muốn đăng kí {{ club.name }}</h6>
      <h6 v-else-if="isUnRegisterClub">Bạn chắc chắn muốn hủy đăng kí {{ club.name }}</h6>
      <h6 v-else> Bạn chắc chắn muốn đăng kí lại {{ club.name }}</h6>
    </div>
    <template #footer>
      <div class="modal-footer">

        <div class="col-12 text-center">
          <button v-if="isRegisterClub" class="btn btn-label-secondary" @click="register(club.id)">Đăng kí</button>
          <button v-else-if="isUnRegisterClub" class="btn btn-label-secondary"
            @click="unRegister(club.id, StatusClubEnrollment.CANCEL)">Hủy đăng kí</button>
          <button v-else class="btn btn-label-secondary"
            @click="reRegister(club.id, StatusClubEnrollment.REGISTER)">Đăng kí lại</button>
          <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { StatusClubEnrollment } from '@/constants/enums/statuses.ts';
export default defineComponent({
  name: 'ModalConfirmRegisterClub',
  components: {
    ModalComponent,
  },
  props: {
    isShowModal: {
      type: Boolean,
      default: false,
    },
    isRegisterClub: {
      type: Boolean,
      required: true,
    },
    isUnRegisterClub: {
      type: Boolean,
      required: true,
    },
    club: {
      type: Object,
      required: true,
    }
  },
  emits: ['closeModal', 'register', 'unRegister', 'reRegister'],
  setup(props, { emit }) {

    const register = (clubId: number) => {
      emit('register', clubId);
    };
    const unRegister = (clubId: number, status: number) => {
      emit('unRegister', clubId, status);
    };
    const reRegister = (clubId: number, status: number) => {
      emit('reRegister', clubId, status);
    };
    const close = () => {
      emit('closeModal');
    };

    return {
      StatusClubEnrollment,
      register,
      unRegister,
      reRegister,
      close,
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

.btn {
  margin: 10px 5px;
  padding: 10px 20px;
  font-size: 16px;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s, color 0.3s;
}

.btn-label-secondary {
  background-color: #f0f0f0;
  color: #333;
  border: 1px solid #ccc;
}

.btn-label-secondary:hover {
  background-color: #e0e0e0;
  color: #000;
}

.btn-primary {
  background-color: #007bff;
  color: #fff;
  border: 1px solid #007bff;
}

.btn-primary:hover {
  background-color: #0056b3;
  color: #fff;
}

.btn-danger {
  background-color: #dc3545;
  color: #fff;
  border: 1px solid #dc3545;
}

.btn-danger:hover {
  background-color: #c82333;
  color: #fff;
}

.btn-success {
  background-color: #28a745;
  color: #fff;
  border: 1px solid #28a745;
}

.btn-success:hover {
  background-color: #218838;
  color: #fff;
}
</style>