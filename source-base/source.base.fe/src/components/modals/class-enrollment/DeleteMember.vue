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
    memberId: {
      type: [Number, String],
    },
  },
  emits: ['update:isShowModalDelete', 'confirm', 'closeModal'],
  setup(props, { emit }) {
    const handleModalConfirm = () => {
      emit('confirm', props.memberId);
      emit('update:isShowModalDelete', false);
    };

    const handleCloseModal = () => {
      emit('update:isShowModalDelete', false);
    };

    return {
      handleModalConfirm,
      handleCloseModal,
    };
  },
});
</script>
