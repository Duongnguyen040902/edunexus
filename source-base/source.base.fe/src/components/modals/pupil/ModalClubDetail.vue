<template>
    <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
      <div class="modal-body">
        <div class="text-center mb-6">
          <h4 class="mb-2">Thông tin câu lạc bộ</h4>
        </div>
        <form class="row g-6">
          <div class="col-12 col-md-6">
            <label class="form-label" for="clubName">Tên câu lạc bộ</label>
            <div class="d-flex justify-content-start align-items-center user-name">
                    <div class="d-flex flex-column">
                      <span class="fw-medium">{{ club.name ?? club.clubName }}</span>
                    </div>
                  </div>
          </div>
          <div class="col-12 col-md-6">
            <label class="form-label" for="clubName">Mô tả</label>
            <div class="d-flex justify-content-start align-items-center user-name">
                    <div class="d-flex flex-column">
                      <span class="fw-medium">{{ club.description ?? club.clubDescription }}</span>
                    </div>
                  </div>
          </div>
            <div v-if="club.teacher" class="col-12 col-md-6">
            <label class="form-label" for="teacherName">Giáo viên</label>
            <div class="d-flex justify-content-start align-items-center user-name">
                    <div class="d-flex flex-column">
                      <span class="fw-medium">
                        {{ club.teacher && club.teacher.firstName && club.teacher.lastName ? `${club.teacher.firstName}
                        ${club.teacher.lastName}` : 'Kì này chưa có giáo viên' }}
                      </span>
                    </div>
                  </div>
          </div>
          <div v-if="club.teacher" class="col-12 col-md-6">
            <label class="form-label" for="clubName">Số điện thoại</label>
            <div class="d-flex justify-content-start align-items-center user-name">
                    <div class="d-flex flex-column">
                      <span class="fw-medium">{{ club.teacher.phoneNumber }}</span>
                    </div>
                  </div>
          </div>
          <div v-if="club.teacher" class="col-12 col-md-6">
            <label class="form-label" for="clubName">Địa chỉ</label>
            <div class="d-flex justify-content-start align-items-center user-name">
                    <div class="d-flex flex-column">
                      <span class="fw-medium">{{ club.teacher.address }}</span>
                    </div>
                  </div>
          </div>
          <div v-else>
            Câu lạc bộ kì này chưa có giáo viên
          </div>
        </form>
      </div>
      <template #footer>
        <div class="modal-footer">
          <div class="col-12 text-center">
            <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
          </div>
        </div>
      </template>
    </ModalComponent>
  </template>
  
  <script lang="ts">
  import { defineComponent, toRefs } from 'vue';
  import ModalComponent from '@/components/common/Modal.vue';
  
  export default defineComponent({
    name: 'ModalClubDetail',
    components: {
      ModalComponent,
    },
    props: {
      isShowModal: {
        type: Boolean,
        default: false,
      },
      club: {
        type: Object,
        required: true,
      }
    },
    emits: ['closeModal'],
    setup(props, { emit }) {
  
      const close = () => {
        emit('closeModal');
      };
  
      return {
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