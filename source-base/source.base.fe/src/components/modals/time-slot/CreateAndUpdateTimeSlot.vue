<template>
  <ModalComponent
    :modelValue="showModalAdd"
    :title="isUpdateMode ? 'Cập nhật tiết học' : 'Thêm tiết học'"
    @update:modelValue="$emit('update:showModal', $event)"
    :beforeClose="handleClose"
  >
    <el-form labelPosition="top">
      <!-- Tên thời gian -->
      <el-form-item label="Tên thời gian *">
        <el-input v-model="localTimeSlot.name" placeholder="Nhập tên thời gian" />
        <div v-if="errorsTimeSlot.Name" class="text-danger">{{ errorsTimeSlot.Name[0] }}</div>
      </el-form-item>

      <!-- Thời gian bắt đầu -->
      <el-form-item label="Thời gian bắt đầu *">
        <el-input
          v-model="localTimeSlot.startTime"
          type="time"
          placeholder="Nhập thời gian bắt đầu (00:00:00)"
          maxlength="8"
        />
        <div v-if="errorsTimeSlot.StartTime" class="text-danger">{{ errorsTimeSlot.StartTime[0] }}</div>
      </el-form-item>

      <!-- Thời gian kết thúc -->
      <el-form-item label="Thời gian kết thúc *">
        <el-input
          v-model="localTimeSlot.endTime"
          type="time"
          placeholder="Nhập thời gian kết thúc (00:00:00)"
          maxlength="8"
        />
        <div v-if="errorsTimeSlot.EndTime" class="text-danger">{{ errorsTimeSlot.EndTime[0] }}</div>
      </el-form-item>

      <!-- Trạng thái -->
      <el-form-item label="Trạng thái *">
        <el-select v-model="localTimeSlot.isActive" placeholder="Chọn trạng thái">
          <el-option label="Hoạt động" :value="true" />
          <el-option label="Không hoạt động" :value="false" />
        </el-select>
        <div v-if="errorsTimeSlot.IsActive" class="text-danger">{{ errorsTimeSlot.IsActive[0] }}</div>
      </el-form-item>
    </el-form>

    <!-- Footer buttons -->
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="handleClose">HỦY</el-button>
        <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button">
          {{ isUpdateMode ? 'Cập nhật' : 'Thêm' }}
        </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, ref, watch, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { CreateTimeSlot } from '@/interfaces/TimeSlot';
import { ErrorResponseCreateTimeSlot, ResponseGetTimeSlotInterface } from '@/types/model/timeslot';

export default defineComponent({
  name: 'CreateAndUpdateTimeSlot',
  components: {
    ModalComponent,
  },
  props: {
    showModalAdd: {
      type: Boolean,
      required: true,
    },
    timeSlotDetail: {
      type: Object as PropType<ResponseGetTimeSlotInterface | null>,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    errorsTimeSlot: {
      type: Object as PropType<ErrorResponseCreateTimeSlot>,
      required: true,
    },
  },
  emits: ['update:showModal', 'update:timeSlot', 'confirm', 'closeUpdateModal'],
  setup(props, { emit }) {
    const localTimeSlot = ref<CreateTimeSlot>({
      name: '',
      startTime: '00:00',
      endTime: '00:00',
      isActive: true,
    });

    watch(
      () => props.showModalAdd,
      newValue => {
        localTimeSlot.value = {
          name: '',
          startTime: '00:00',
          endTime: '00:00',
          isActive: true,
        };
        handleResetForm();
      },
    );
    watch(
      () => props.isUpdateMode,
      newValue => {
        if (newValue && props.timeSlotDetail.value.data) {
          localTimeSlot.value = {
            ...props.timeSlotDetail.value.data,
            startTime: props.timeSlotDetail.value.data.startTime.slice(0, 5),
            endTime: props.timeSlotDetail.value.data.endTime.slice(0, 5),
          };
        } else {
          localTimeSlot.value = {
            name: '',
            startTime: '00:00',
            endTime: '00:00',
            isActive: true,
          };
        }
      },
      { immediate: true },
    );

    const handleModalConfirm = () => {
      handleResetForm();
      const originalStartTime = localTimeSlot.value.startTime;
      const originalEndTime = localTimeSlot.value.endTime;
      localTimeSlot.value.startTime += ':00';
      localTimeSlot.value.endTime += ':00';
      emit('confirm', localTimeSlot.value);
      localTimeSlot.value.startTime = originalStartTime;
      localTimeSlot.value.endTime = originalEndTime;
    };

    const handleClose = () => {
      localTimeSlot.value = {
        name: '',
        startTime: '00:00',
        endTime: '00:00',
        isActive: true,
      };
      emit('update:showModal', false);
      emit('closeUpdateModal', false);
    };

    const handleResetForm = () => {
      props.errorsTimeSlot.Name = [];
      props.errorsTimeSlot.StartTime = [];
      props.errorsTimeSlot.EndTime = [];
      props.errorsTimeSlot.IsActive = [];
    };
    return {
      localTimeSlot,
      handleClose,
      handleModalConfirm,
    };
  },
});
</script>

<style scoped>
.dialog-footer {
  display: flex;
  justify-content: flex-end;
  padding: 10px 0;
  border-top: 1px solid #e8e8e8;
  margin-top: 20px;
}

.dialog-footer .el-button {
  margin-left: 10px;
}

.custom-confirm-button {
  background-color: #409eff;
  border-color: #409eff;
  color: #fff;
}

.custom-confirm-button:hover {
  background-color: #66b1ff;
  border-color: #66b1ff;
  color: #fff;
}
</style>
