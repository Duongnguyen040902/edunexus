<template>
  <small class="text-light fw-semibold">Nội dung chi tiết</small>
  <div class="demo-inline-spacing mt-3">
    <div class="list-group">
      <div class="list-group-item list-group-item-action flex-column align-items-start">
        <div class="d-flex justify-content-between w-100 mb-2">
          <h6>{{ localClassApplication?.title }}</h6>
          <small>{{ formatDate(localClassApplication.createDate, 'dd/mm/yyyy') }}</small>
        </div>
        <div class="mb-2">
          <label class="fw-bold">Nội dung:</label>
          <p class="mb-1">{{ localClassApplication?.description }}</p>
        </div>
        <div class="mb-2">
          <label class="fw-bold">Học sinh:</label>
          <p class="mb-1">{{ localClassApplication?.firstName }} {{ localClassApplication?.lastName }}</p>
        </div>
        <div class="mb-2">
          <label class="fw-bold">Phụ huynh:</label>
          <p class="mb-1">{{ localClassApplication?.donorName }}</p>
        </div>
        <div class="mb-2">
          <label class="fw-bold">Trạng thái:</label>
          <p class="mb-1">{{ localClassApplication?.statusName }}</p>
        </div>
        <div class="mb-2">
          <label class="fw-bold">Phản hồi:</label>
          <p class="mb-1">
            {{ localClassApplication?.response || 'Chưa được phản hồi' }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { usePupilApplicationComposable } from '@/composables/pupil-application';
import { ResponseGetClassApplication } from '@/types/model/class-application';
import { defineComponent, PropType, ref, watch } from 'vue';
import { formatDate } from '@/helpers/formatDate.ts';

export default defineComponent({
  name: 'PupilApplicationDetailComponent',
  components: {},
  props: {
    classApplication: {
      type: Object as PropType<ResponseGetClassApplication>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const { isUpdate, listCategory, handleShowModal, isShowModal, updateListApplication } =
      usePupilApplicationComposable();

    const localClassApplication = ref(props.classApplication);

    watch(
      () => props.classApplication,
      newVal => {
        localClassApplication.value = newVal;
      },
    );

    return {
      isUpdate,
      isShowModal,
      localClassApplication,
      updateListApplication,
      handleShowModal,
      listCategory,
      formatDate,
    };
  },
});
</script>

<style scoped>
.list-group-item {
  background-color: #f8f9fa;
  border: 1px solid #dee2e6;
  border-radius: 0.25rem;
  text-align: left;
}

.list-group-item h6 {
  font-size: 1.25rem;
  font-weight: 500;
}

.list-group-item label {
  font-weight: 600;
}

.list-group-item p {
  margin-bottom: 0.5rem;
}
</style>
