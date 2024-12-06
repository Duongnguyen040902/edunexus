<template>
  <ModalComponent
    :model-value="showModal"
    :title="'Phản hồi đơn:' + localClassApplication.title"
    @update:modelValue="$emit('update-showModal', $event)"
  >
    <div class="modal-body">
      <div>
        <label class="form-label">Phản hồi:</label>
        <textarea
          class="form-control"
          v-model="localClassApplication.response"
          rows="5"
          style="height: 17px"
        ></textarea>
        <div v-if="errorsResponse.Response" class="text-danger">
          {{ errorsResponse.Response[0] }}
        </div>
      </div>

      <div class="col-md">
        <small class="text-light fw-semibold d-block">Trạng thái</small>
        <div class="form-check form-check-inline mt-3">
          <input
            class="form-check-input"
            type="radio"
            name="inlineRadioOptions"
            id="inlineRadio1"
            v-model="localClassApplication.status"
            :value="ApplicationStatus.APPROVED"
          />
          <label class="form-check-label" for="inlineRadio1">Chấp thuận</label>
        </div>
        <div class="form-check form-check-inline">
          <input
            class="form-check-input"
            type="radio"
            name="inlineRadioOptions"
            id="inlineRadio2"
            :value="ApplicationStatus.REJECTED"
            v-model="localClassApplication.status"
          />
          <label class="form-check-label" for="inlineRadio2">Từ chối</label>
        </div>
        <div v-if="errorsResponse.Status" class="text-danger">
          {{ errorsResponse.Status[0] }}
        </div>
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button type="button" class="btn btn-outline-secondary" @click="handleCancel">Hủy</button>
        <button style="margin-left: 10px" type="button" @click="handleModalConfirm" class="btn btn-primary ml-3">
          Lưu
        </button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ResponseGetClassApplication, ErrorResponseClassApplication } from '@/types/model/class-application';
import { useClassApplicationComposable } from '@/composables/class-application';
import { ApplicationStatus } from '@/types/model/class-application';

export default defineComponent({
  name: 'ResponseApplicationModal',
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    applicationDetail: {
      type: Object as PropType<ResponseGetClassApplication>,
      required: true,
    },
  },
  emits: ['update-showModal', 'responseApplication', 'success', 'update-errors'],
  setup(props, { emit }) {
    const { responsePayload, responseApplication, errorsResponse } = useClassApplicationComposable();
    const localClassApplication = ref(props.applicationDetail);
    const initialClassApplication = ref({ ...props.applicationDetail });

    watch(
      () => props.applicationDetail,
      newVal => {
        Object.assign(localClassApplication.value, newVal);
        Object.assign(initialClassApplication.value, newVal);

      },
    );

    const handleModalConfirm = () => {
      responsePayload.id = localClassApplication.value.id;
      responsePayload.status = localClassApplication.value.status;
      responsePayload.response = localClassApplication.value.response || '';
      responseApplication(emit, responsePayload);
    };
    const handleCancel = () => {
      Object.assign(localClassApplication.value, initialClassApplication.value);
      emit('update-showModal', false);
    };
    return {
      responsePayload,
      handleModalConfirm,
      localClassApplication,
      errorsResponse,
      ApplicationStatus,
      handleCancel,
    };
  },
});
</script>
