<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="row overflow-hidden">
      <h2>Danh sách xe đã tham gia</h2>
      <div class="col-12">
        <ul v-if="pupilBus.length > 0" class="timeline timeline-center mt-12">
          <li
            v-for="bus in pupilBus"
            :key="bus.id"
            class="timeline-item"
            style="padding-bottom: 5px"
            @click="handleBusClick(bus.busId, bus.semesterId)"
          >
            <span
              class="timeline-indicator timeline-indicator-success aos-init aos-animate"
              data-aos="zoom-in"
              data-aos-delay="200"
            >
              <i class="bx bx-bus"></i>
            </span>
            <div class="timeline-event card p-0 aos-init aos-animate" data-aos="fade-right">
              <h6 class="card-header" style="padding: 10px">{{ bus.busName }}</h6>
              <div class="card-body" style="padding: 10px">
                <ul class="list-unstyled">
                  <li class="d-flex justify-content-start align-items-center text-success mb-2">
                    <i class="bx bx-calendar bx-sm me-4"></i>
                    <div class="ps-4 border-start">
                      <small class="text-muted mb-1">Kỳ</small>
                      <h5 class="mb-0">
                        {{ bus.semesterName }}, Thời gian: {{ formatDate(bus.startDate) }} -
                        {{ formatDate(bus.endDate) }}
                      </h5>
                    </div>
                  </li>
                  <li class="d-flex justify-content-start align-items-center text-info mb-2">
                    <i class="bx bx-calendar-alt bx-sm me-4"></i>
                    <div class="ps-4 border-start">
                      <small class="text-muted mb-1">Năm học</small>
                      <h5 class="mb-0">{{ bus.schoolYearName }}</h5>
                    </div>
                  </li>
                  <li class="d-flex justify-content-start align-items-center text-info mb-2">
                    <i class="bx bx-trip bx-sm me-4"></i>
                    <div class="ps-4 border-start">
                      <small class="text-muted mb-1">Điểm dừng</small>
                      <h5 class="mb-0">{{ bus.busStopName }}</h5>
                    </div>
                  </li>
                  <li class="d-flex justify-content-start align-items-center text-info mb-2">
                    <i class="bx bx-time bx-sm me-4"></i>
                    <div class="ps-4 border-start">
                      <small class="text-muted mb-1">Thời gian đón, trả ước tính</small>
                      <h5 class="mb-0">Đón: {{ bus.pickUpTime }} - Trả: {{ bus.returnTime }}</h5>
                    </div>
                  </li>
                </ul>
              </div>
              <div class="timeline-event-time">
                {{ getBusStatus(bus) }}
              </div>
            </div>
          </li>
        </ul>
        <div v-else class="text-center mt-4">
          <p>Không tìm thấy dữ liệu xe tuyến</p>
        </div>
      </div>
    </div>
    <ModalViewBusDetail :showModal="showModal" :busDetail="busDetail" @update-showModal="showModal = $event" />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useBusDetailComposable } from '@/composables/bus.ts';
import ModalViewBusDetail from '@/components/modals/bus/BusDetailModal.vue';
import { formatDate } from '@/helpers/formatDate.ts';

export default defineComponent({
  name: 'BusDetail',
  components: { ModalViewBusDetail },
  metaInfo: {
    title: 'Chi tiết xe buýt',
  },
  setup() {
    const { busDetail, pupilBus, fetchPupilBus, fetchBusDetailOfPupil } = useBusDetailComposable();
    const showModal = ref(false);

    onMounted(async () => {
      await fetchPupilBus();
    });

    const handleBusClick = async (busId: number, semesterId: number) => {
      await fetchBusDetailOfPupil(busId, semesterId);
      showModal.value = true;
    };

    const getBusStatus = (bus: any) => {
      const currentDate = new Date();
      const startDate = new Date(bus.startDate);
      const endDate = new Date(bus.endDate);

      if (endDate < currentDate) {
        return 'Đã kết thúc';
      } else if (startDate > currentDate) {
        return 'Sắp tới';
      } else {
        return 'Đang hoạt động';
      }
    };

    return {
      pupilBus,
      showModal,
      busDetail,
      handleBusClick,
      formatDate,
      getBusStatus,
    };
  },
});
</script>