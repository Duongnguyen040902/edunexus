<template>
  <ModalComponent
    :modelValue="showModalAdd"
    :title="isUpdateMode ? 'Cập nhật tuyến xe' : 'Thêm tuyến xe'"
    @update:modelValue="$emit('update:showModal', $event)"
  >
    <el-form>
      <!-- Tên tuyến xe -->
      <el-form-item>
        <label class="form-label"> Tên tuyến xe <span class="text-danger">*</span> </label>
        <el-input v-model="localBusRoute.name" placeholder="Nhập tên tuyến xe" />
        <div v-if="errorsBusRoute.Name" class="text-danger">{{ errorsBusRoute.Name[0] }}</div>
      </el-form-item>

      <!-- Mô tả tuyến xe -->
      <el-form-item>
        <label class="form-label"> Mô tả tuyến xe <span class="text-danger">*</span> </label>
        <el-input type="textarea" v-model="localBusRoute.description" placeholder="Nhập mô tả tuyến xe" :rows="3" />
        <div v-if="errorsBusRoute.Description" class="text-danger">{{ errorsBusRoute.Description[0] }}</div>
      </el-form-item>

      <!-- Trạng thái -->
      <el-form-item>
        <label class="form-label"> Trạng thái <span class="text-danger">*</span> </label>
        <el-select v-model="localBusRoute.status" placeholder="Chọn trạng thái">
          <el-option label="Hoạt động" :value="1" />
          <el-option label="Chờ hoạt động" :value="0" />
        </el-select>
        <div v-if="errorsBusRoute.Status" class="text-danger">{{ errorsBusRoute.Status[0] }}</div>
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
import { defineComponent, onMounted, PropType, ref, watch } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import type { BusRoute } from '@/interfaces/BusRoute';
import { BusRouteDetail } from '@/types/model/bus-route';
import { useBusRouteComposable } from '@/composables/bus-route';

export default defineComponent({
  name: 'AddAndUpdateBusRoute',
  components: {
    ModalComponent,
  },
  props: {
    showModalAdd: {
      type: Boolean,
      required: true,
    },
    busRoute: {
      type: Object as PropType<BusRoute>,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    errorsBusRoute: {
      type: Object,
      required: true,
    },
    dataBusRouteDetail: {
      type: Object as PropType<BusRouteDetail>,
    },
  },
  emits: ['update:showModal', 'update:busRoute', 'confirm', 'closeUpdateModal'],
  setup(props, { emit }) {
    const busRouteComposable = useBusRouteComposable();
    //const { dataBusRouteDetail } = busRouteComposable;
    const localBusRoute = ref<BusRoute>({
      name: '',
      description: '',
      status: null,
    });

    watch(
      () => props.isUpdateMode,
      newValue => {
        if (newValue && props.dataBusRouteDetail && props.dataBusRouteDetail.value.data[0]) {
          localBusRoute.value = {
            id: props.dataBusRouteDetail.value.data[0].id || '',
            name: props.dataBusRouteDetail.value.data[0].name || '',
            description: props.dataBusRouteDetail.value.data[0].description || '',
            status: props.dataBusRouteDetail.value.data[0].status !== undefined ? props.dataBusRouteDetail.value.data[0].status : null,
          };
        } else {
          localBusRoute.value = { name: '', description: '', status: null };
        }
      },
      { immediate: true },
    );

    watch(
      () => props.showModalAdd,
      newValue => {
        if (!newValue) {
          localBusRoute.value = { name: '', description: '', status: null };
          handleResetForm();
        }
      },
    );

    const handleModalConfirm = async () => {
      handleResetForm();
      await emit('confirm', localBusRoute.value);
    };

    const handleClose = () => {
      localBusRoute.value = { name: '', description: '', status: null };
      emit('update:showModal', false);
      emit('closeUpdateModal', false);
    };
    const handleResetForm = () => {
      props.errorsBusRoute.Name = [];
      props.errorsBusRoute.Description = [];
      props.errorsBusRoute.Status = [];
    };

    return {
      localBusRoute,
      handleClose,
      handleModalConfirm,
    };
  },
});
</script>

<style scoped>
.modal-container {
  padding: 20px;
}

.el-input,
.el-select {
  width: 100%;
}
</style>
