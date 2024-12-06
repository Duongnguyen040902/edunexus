<template>
  <ModalConfirmDelete
    :isShowModal="isShowModalTeacherDelete"
    :message="message"
    @confirmAction="handleModalRemoveConfirm"
    @closeModal="handleCloseRemoveModal"
    :width="`30%`"
  />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ModalConfirmDelete from '@/components/common/ModalConfirmDelete.vue';

export default defineComponent({
  name: 'RemoveTeacherModal',
  components: {
    ModalConfirmDelete,
  },
  props: {
    isShowModalTeacherDelete: {
      type: Boolean,
      default: false,
    },
    teacherId: {
      type: [Number, String],
    },
    classId: {
      type: Number,
    },
    semesterId: {
      type: Number,
    },
  },
  emits: ['update:showModalTeacherDelete', 'confirm', 'close'],
  setup(props, { emit }) {
    const message = "Nếu xóa sẽ mất dữ liệu giáo viên này liên quan đến lớp học! Bạn chắc chứ?";
    const handleModalRemoveConfirm = () => {
      emit('confirm', {
        classId: props.classId,
        semesterId: props.semesterId,
        teacherId: props.teacherId,
      });
      emit('update:showModalTeacherDelete', false);
    };

    const handleCloseRemoveModal = (value: boolean) => {
      emit('update:showModalTeacherDelete', false);
    };

    return {
      message,
      handleModalRemoveConfirm,
      handleCloseRemoveModal,
    };
  },
});
</script>
