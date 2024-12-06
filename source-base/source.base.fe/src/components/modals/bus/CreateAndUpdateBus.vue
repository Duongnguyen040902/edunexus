<template>
  <ModalComponent
    :modelValue="showModalAdd"
    :title="isUpdateMode ? 'Cập nhật xe' : 'Thêm xe'"
    @update:modelValue="$emit('update:showModal', $event)"
    :beforeClose="handleClose"
  >
    <el-form>
      <!-- Tên xe -->
      <el-form-item>
        <label class="form-label"> Tên xe <span class="text-danger">*</span> </label>
        <el-input v-model="localBus.name" placeholder="Nhập tên xe" />
        <div v-if="errorsBus.Name" class="text-danger">{{ errorsBus.Name[0] }}</div>
      </el-form-item>

      <!-- Tên tài xế -->
      <el-form-item>
        <label class="form-label"> Tên tài xế <span class="text-danger">*</span> </label>
        <el-input v-model="localBus.driverName" placeholder="Nhập tên tài xế" />
        <div v-if="errorsBus.DriverName" class="text-danger">{{ errorsBus.DriverName[0] }}</div>
      </el-form-item>

      <!-- Số điện thoại tài xế -->
      <el-form-item>
        <label class="form-label"> Số điện thoại tài xế <span class="text-danger">*</span> </label>
        <el-input v-model="localBus.driverPhone" placeholder="Nhập số điện thoại tài xế" />
        <div v-if="errorsBus.DriverPhone" class="text-danger">{{ errorsBus.DriverPhone[0] }}</div>
      </el-form-item>

      <!-- Biển số xe -->
      <el-form-item>
        <label class="form-label"> Biển số xe <span class="text-danger">*</span> </label>
        <el-input v-model="localBus.licensePlate" placeholder="29C1-99999" />
        <div v-if="errorsBus.LicensePlate" class="text-danger">{{ errorsBus.LicensePlate[0] }}</div>
      </el-form-item>

      <!-- Số ghế -->
      <el-form-item>
        <label class="form-label"> Số ghế <span class="text-danger">*</span> </label>
        <el-input v-model="localBus.seatNumber" type="number" placeholder="Nhập số ghế" />
        <div v-if="errorsBus.SeatNumber" class="text-danger">{{ errorsBus.SeatNumber[0] }}</div>
      </el-form-item>

      <!-- Trạng thái -->
      <el-form-item>
        <label class="form-label"> Trạng thái <span class="text-danger">*</span> </label>
        <el-select v-model="localBus.status" placeholder="Chọn trạng thái">
          <el-option label="Hoạt động" :value="1" />
          <el-option label="Không hoạt động" :value="0" />
        </el-select>
        <div v-if="errorsBus.Status" class="text-danger">{{ errorsBus.Status[0] }}</div>
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
import { CreateBus } from '@/types/model/bus';

export default defineComponent({
  name: 'CreateAndUpdateBus',
  components: {
    ModalComponent,
  },
  props: {
    showModalAdd: {
      type: Boolean,
      required: true,
    },
    dataBusDetail: {
      type: Object as PropType<CreateBus>,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    errorsBus: {
      type: Object,
      required: true,
    },
  },
  emits: ['update:showModal', 'update:bus', 'confirm', 'closeUpdateModal'],
  setup(props, { emit }) {
    const localBus = ref<CreateBus>({
      name: '',
      driverName: '',
      driverPhone: '',
      licensePlate: '',
      seatNumber: 0,
      status: 1,
    });

    watch(
      () => props.isUpdateMode,
      newValue => {
        if (newValue && props.dataBusDetail.value.data) {
          localBus.value = { ...props.dataBusDetail.value.data };
        } else {
          localBus.value = {
            name: '',
            driverName: '',
            driverPhone: '',
            licensePlate: '',
            seatNumber: 0,
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
          localBus.value = {
            name: '',
            driverName: '',
            driverPhone: '',
            licensePlate: '',
            seatNumber: 0,
            status: 1,
          };
          handleResetForm();
        }
      },
    );

    const handleModalConfirm = () => {
      handleResetForm();
      emit('confirm', localBus.value);
    };

    const handleClose = () => {
      localBus.value = {
        name: '',
        driverName: '',
        driverPhone: '',
        licensePlate: '',
        seatNumber: 0,
        status: 1,
      };
      emit('update:showModal', false);
      emit('closeUpdateModal', false);
    };

    const handleResetForm = () => {
      props.errorsBus.Name = [];
      props.errorsBus.DriverName = [];
      props.errorsBus.DriverPhone = [];
      props.errorsBus.LicensePlate = [];
      props.errorsBus.SeatNumber = [];
      props.errorsBus.Status = [];
    };

    return {
      localBus,
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
