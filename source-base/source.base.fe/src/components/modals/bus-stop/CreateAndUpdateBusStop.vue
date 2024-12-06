<template>
  <ModalComponent
    :modelValue="showModalAdd"
    :title="isUpdateMode ? 'Cập nhật điểm dừng xe' : 'Thêm điểm dừng xe'"
    @update:modelValue="$emit('update:showModal', $event)"
    :beforeClose="handleClose"
  >
    <el-form label-position="top">
      <!-- Tên điểm dừng xe -->
      <el-form-item label="Tên điểm dừng xe *">
        <el-input v-model="localBusStop.name" placeholder="Nhập tên điểm dừng xe" />
        <div v-if="errorsBusStop.Name" class="text-danger">{{ errorsBusStop.Name[0] }}</div>
      </el-form-item>

      <!-- Thời gian đón -->
      <el-form-item label="Thời gian đón *">
        <el-input
          v-model="localBusStop.pickUpTime"
          type="time"
          placeholder="Nhập thời gian ước tính (00:00:00)"
          maxlength="8"
        /><br />
        <div v-if="errorsBusStop.PickUpTime" class="text-danger">{{ errorsBusStop.PickUpTime[0] }}</div>
      </el-form-item>

      <!-- Thời gian đưa về -->
      <el-form-item label="Thời gian đưa về *">
        <el-input
          v-model="localBusStop.returnTime"
          type="time"
          placeholder="Nhập thời gian ước tính (00:00:00)"
          maxlength="8"
        />
        <br />
        <div v-if="errorsBusStop.ReturnTime" class="text-danger">{{ errorsBusStop.ReturnTime[0] }}</div>
      </el-form-item>

      <!-- Địa chỉ điểm dừng -->
      <el-form-item label="Địa chỉ *">
        <el-input v-model="localBusStop.address" placeholder="Nhập địa chỉ điểm dừng" />
        <div v-if="errorsBusStop.Address" class="text-danger">{{ errorsBusStop.Address[0] }}</div>
      </el-form-item>

      <!-- Trạng thái -->
      <el-form-item label="Trạng thái *">
        <el-select v-model="localBusStop.status" placeholder="Chọn trạng thái">
          <el-option label="Hoạt động" :value="1" />
          <el-option label="Không hoạt động" :value="0" />
        </el-select>
        <div v-if="errorsBusStop.Status" class="text-danger">{{ errorsBusStop.Status[0] }}</div>
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
import { CreateBusStop } from '@/interfaces/BusStop';

export default defineComponent({
  name: 'CreateAndUpdateBusStop',
  components: {
    ModalComponent,
  },
  props: {
    showModalAdd: {
      type: Boolean,
      required: true,
    },
    dataBusStopDetail: {
      type: Object as PropType<CreateBusStop>,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    errorsBusStop: {
      type: Object,
      required: true,
    },
    busRoutes: {
      type: Array,
      required: true,
    },
  },
  emits: ['update:showModal', 'update:busStop', 'confirm', 'closeUpdateModal'],
  setup(props, { emit }) {
    const localBusStop = ref<CreateBusStop>({
      name: '',
      pickUpTime: '00:00',
      returnTime: '00:00',
      address: '',
      busRouteId: 0,
      status: 1,
    });

    watch(
      () => props.isUpdateMode,
      newValue => {
        if (newValue && props.dataBusStopDetail.value.data) {
          localBusStop.value = {
            ...props.dataBusStopDetail.value.data,
            pickUpTime: props.dataBusStopDetail.value.data.pickUpTime.slice(0, 5), 
            returnTime: props.dataBusStopDetail.value.data.returnTime.slice(0, 5),
          };
        } else {
          localBusStop.value = {
            name: '',
            pickUpTime: '00:00',
            returnTime: '00:00',
            address: '',
            busRouteId: 0,
            status: 1,
          };
        }
      },
      { immediate: true },
    );
    watch(
      () => props.showModalAdd,
      newValue => {
        if (!newValue) {
          localBusStop.value = {
            name: '',
            pickUpTime: '00:00',
            returnTime: '00:00',
            address: '',
            busRouteId: 0,
            status: 1,
          };
          handleResetForm();
        }
      },
    );

    const handleModalConfirm = () => {
      handleResetForm();
      const originalPickUpTime = localBusStop.value.pickUpTime;
      const originalReturnTime = localBusStop.value.returnTime;
      localBusStop.value.pickUpTime += ':00';
      localBusStop.value.returnTime += ':00';
      emit('confirm', localBusStop.value);
      localBusStop.value.pickUpTime = originalPickUpTime;
      localBusStop.value.returnTime = originalReturnTime;
    };

    const handleResetForm = () => {
        props.errorsBusStop.Name = [];
        props.errorsBusStop.PickUpTime = [];
        props.errorsBusStop.ReturnTime = [];
        props.errorsBusStop.Address = [];
        props.errorsBusStop.Status = [];
        props.errorsBusStop.BusRouteId  = [];    
        };

    const handleClose = () => {
      localBusStop.value = {
        name: '',
        pickUpTime: '00:00',
        returnTime: '00:00',
        address: '',
        busRouteId: 0,
        status: 1,
      };
      emit('update:showModal', false);
      emit('closeUpdateModal', false);
    };

    return {
      localBusStop,
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
