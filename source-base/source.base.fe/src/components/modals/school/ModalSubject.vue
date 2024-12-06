<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
    <div class="modal-body">
      <div class="text-center mb-6">
        <h4 class="mb-2">Thêm môn học</h4>
      </div>
      <form class="row g-6">
        <div class="col-12 col-md-6">
          <label class="form-label" for="name">Tên môn học(*)</label>
          <input
            id="name"
            v-model="requestCreateSubject.name"
            class="form-control"
            name="name"
            placeholder="Nhập tên môn học"
            type="text"
          />
          <div v-if="errorSubject.Name" class="text-danger">{{ errorSubject.Name[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="website">Tên viết tắt(*)</label>
          <input
            id="website"
            v-model="requestCreateSubject.code"
            class="form-control"
            name="website"
            placeholder="Nhập tên viết tắt"
            type="text"
          />
          <div v-if="errorSubject.Code" class="text-danger">{{ errorSubject.Code[0] }}</div>
        </div>
      </form>
    </div>
    <template #footer>
      <div class="modal-footer">
        <div class="col-12 text-center">
          <button class="btn btn-primary me-3" type="submit" @click="confirmAction">
            {{ isEditSubject ? 'Cập nhật' : 'Tạo' }}
          </button>
          <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { useSubjectComposable } from '@/composables/subject.ts';

export default defineComponent({
  name: 'ModalSubjectComponent',
  components: {
    ModalComponent,
  },
  props: {
    isShowModal: {
      type: Boolean,
      default: false,
    },
  },
  emits: ['closeModal', 'confirmAction'],
  setup(props, { emit }) {
    const subject = useSubjectComposable();
    const { requestCreateSubject, errorSubject, isEditSubject } = subject;
    const { isShowModal } = toRefs(props);
    const close = (value: boolean) => {
      emit('closeModal', value);
    };

    const confirmAction = () => {
      emit('confirmAction', true);
    };
    return {
      isShowModal,
      requestCreateSubject,
      errorSubject,
      isEditSubject,
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
