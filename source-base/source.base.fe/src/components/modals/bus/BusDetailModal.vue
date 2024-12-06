<template>
  <ModalComponent
    :modelValue="showModal"
    :title="`Chi tiết xe buýt`"
    @update:modelValue="$emit('update-showModal', $event)"
    @before-close="handleClose()"
  >
    <div v-if="busDetail" class="d-flex flex-column align-items-start mt-3">
      <div class="bus-info mb-4 w-100">
        <h6 class="mb-3">Thông tin xe buýt</h6>
        <div class="row">
          <div class="col-md-6">
            <p><strong>Tên xe:</strong> {{ busDetail?.name }}</p>
            <p><strong>Biển số:</strong> {{ busDetail?.licensePlate }}</p>
          </div>
          <div class="col-md-6">
            <p><strong>Số ghế:</strong> {{ busDetail?.seatNumber }}</p>
            <p><strong>Tuyến xe:</strong> {{ busDetail?.busRouteName }}</p>
          </div>
        </div>
      </div>
      <div class="supervisor-info mb-4 w-100">
        <h6 class="mb-3">Thông tin giám sát viên</h6>
        <div v-if="busDetail.busSupervisor" class="row">
          <div class="col-md-6">
            <p><strong>Họ tên:</strong> {{ busDetail.busSupervisor?.firstName }} {{ busDetail.busSupervisor?.lastName }}</p>
            <p><strong>Ngày sinh:</strong> {{ busDetail.busSupervisor?.dateOfBirth }}</p>
          </div>
          <div class="col-md-6">
            <p><strong>Giới tính:</strong> {{ busDetail.busSupervisor?.gender ? 'Nam' : 'Nữ' }}</p>
            <p><strong>Địa chỉ:</strong> {{ busDetail.busSupervisor?.address }}</p>
            <p><strong>Số điện thoại:</strong> {{ busDetail.busSupervisor?.phoneNumber }}</p>
          </div>
        </div>
        <div v-else>
          <p>Không tìm thấy thông tin giám sát viên</p>
        </div>
      </div>
      <div v-if="busDetail?.busStops?.length" class="bus-stops w-100">
        <h6 class="mb-3">Danh sách điểm dừng</h6>
        <ul class="list-group">
          <li v-for="stop in busDetail.busStops" :key="stop.id" class="list-group-item">
            <p><strong>Tên điểm dừng:</strong> {{ stop.name }}</p>
            <p><strong>Thời gian ước tính:</strong> {{ stop.estimatedTime }}</p>
            <p><strong>Địa chỉ:</strong> {{ stop.address }}</p>
          </li>
        </ul>
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button class="btn btn-outline-secondary" style="margin-right: 10px" @click="handleClose()">ĐÓNG</button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ResponseGetBusDetail } from '@/types/model/bus.ts';

export default defineComponent({
  name: 'ModalViewBusDetail',
  components: { ModalComponent },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    busDetail: {
      type: Object as PropType<ResponseGetBusDetail | null>,
    },
  },
  emits: ['update-showModal'],
  setup(props, { emit }) {
    const handleClose = () => {
      emit('update-showModal', false);
    };

    return { handleClose };
  },
});
</script>

<style scoped>
.d-flex {
  display: flex;
}
.me-4 {
  margin-right: 1.5rem;
}
.w-100 {
  width: 100%;
}
.mb-3 {
  margin-bottom: 1rem;
}
.mb-4 {
  margin-bottom: 1.5rem;
}
</style>