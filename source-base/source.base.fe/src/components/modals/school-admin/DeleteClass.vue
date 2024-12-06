<template>
  <ModalConfirmDelete
    :isShowModal="isShowModalDelete"
    @confirmAction="handleModalConfirm"
    @closeModal="handleCloseModal"
    :width="`30%`"
  />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ModalConfirmDelete from '@/components/common/ModalConfirmDelete.vue';

export default defineComponent({
  name: 'DeleteClassModal',
  components: {
    ModalConfirmDelete,
  },
  props: {
    isShowModalDelete: {
      type: Boolean,
      default: false,
    },
    classId: {
      type: [Number, String],
    },
  },
  emits: ['update:showModalDelete', 'confirm', 'closeModal'],
  setup(props, { emit }) {
    const handleModalConfirm = () => {
      emit('confirm', props.classId);
      emit('update:showModalDelete', false);
    };

    const handleCloseModal = () => {
      emit('update:showModalDelete', false);
    };

    return {
      handleModalConfirm,
      handleCloseModal,
    };
  },
});
</script>
