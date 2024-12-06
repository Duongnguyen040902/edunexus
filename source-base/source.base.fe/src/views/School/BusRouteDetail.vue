<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div v-if="!dataBusRouteDetail.value || !dataBusRouteDetail.value.data || !dataBusRouteDetail.value.data[0].name">
      <p>Không tìm kiếm được thông tin!</p>
      <button class="btn btn-secondary" @click="handleBackPage">Quay lại</button>
    </div>

    <div
      v-else
      class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between mb-6 text-center text-sm-start gap-2"
    >
      <div class="mb-2 mb-sm-0">
        <h4 class="mb-1">Thông tin chi tiết tuyến xe {{ dataBusRouteDetail.value.data[0].name }}</h4>
        <button class="btn btn-secondary" @click="handleBackPage">Quay lại</button>
      </div>
    </div>
    <div class="row">
      <div class="col-xl-4 col-lg-5 order-1 order-md-0">
        <div class="card mb-6">
          <div class="accordion-body pt-5 pb-0">
            <div class="d-flex align-items-center justify-content-between">
              <h6 class="fw-normal mb-1">Chi tiết điểm đi</h6>
            </div>

            <div
              v-if="
                !dataBusRouteDetail.value ||
                !dataBusRouteDetail.value.data ||
                !dataBusRouteDetail.value.data[0].busStops
              "
            >
              <p>Chưa có điểm đi nào.</p>
            </div>
            <!-- Hiển thị danh sách busStops -->
            <ul v-else class="timeline ps-4 mt-6 mb-2">
              <li
                v-for="(busStop, index) in dataBusRouteDetail.value.data[0].busStops"
                :key="index"
                class="timeline-item ps-6 pb-3 border-left-dashed"
              >
                <span class="timeline-indicator-advanced timeline-indicator-primary border-0 shadow-none">
                  <i class="bx bx-map mt-1"></i>
                </span>
                <div class="timeline-event ps-0 pb-0">
                  <div class="timeline-header">
                    <small class="text-success text-uppercase">{{ busStop.name }}</small>
                  </div>
                  <h6 class="my-50">Thời gian đón dự kiến</h6>
                  <small class="text-body">{{ busStop.pickUpTime }}</small>
                  <h6 class="my-50">Thời gian trả về dự kiến</h6>
                  <small class="text-body">{{ busStop.returnTime }}</small>
                </div>
              </li>
            </ul>
          </div>
        </div>

        <!-- /Customer-detail Card -->
      </div>
      <!--/ Customer Sidebar -->
      <div class="col-xl-8 col-lg-7 order-0 order-md-1">
        <div class="nav-align-top">
          <ul class="nav nav-pills flex-column flex-md-row mb-12">
            <li class="nav-item">
              <a
                class="nav-link"
                :class="{ active: currentView === 'tableBus' || currentView === 'detail' }"
                href="javascript:void(0);"
                @click="showTableBus()"
              >
                <i class="tf-icons bx bx-car"></i> Quản lý thông tin xe
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link"
                :class="{ active: currentView === 'index' }"
                href="javascript:void(0);"
                @click="handleRedirectToIndex"
              >
                <i class="bx bx-map bx-sm me-1_5"></i>Quản lý điểm dừng xe
              </a>
            </li>
          </ul>
          <Index v-if="currentView === 'index'" @refreshPage="refreshPage" />
          <TableBus v-if="currentView === 'tableBus'" :busId="selectedId" @refreshPage="refreshPage" />
          <Detail v-if="currentView === 'detail'" :busId="selectedId" @refreshPage="refreshPage" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, watch } from 'vue';
import TableBusRoute from '@/components/views/bus-route/TableBusRoute.vue';
import { useBusRouteComposable } from '@/composables/bus-route';
import Index from '@/components/views/bus-stop/Index.vue';
import TableBus from '@/components/views/bus-stop/TableBus.vue';
import Detail from '@/components/views/bus-stop/Detail.vue';
import { useBusStopComposable } from '@/composables/bus-stop';
import { useBusDetailComposable } from '@/composables/bus';
import { useBusEnrollmentComposable } from '@/composables/bus-enrollment';
import { useRoute, useRouter } from 'vue-router';
export default defineComponent({
  name: 'RouteManagement',
  components: {
    TableBusRoute,
    Index,
    TableBus,
    Detail,
  },
  setup() {
    const busRouteComposable = useBusRouteComposable();
    const router = useRouter();
    const { handleGetAllBusStops } = useBusStopComposable();
    const { handleGetAllBuses, handleGetBusDetail } = useBusDetailComposable();
    const { currentView, selectedId, showTableBus, showDetail, handleRedirectToIndex } = useBusEnrollmentComposable();
    const {
      dataBusRouteDetail,
      handleGetAllBusRoute,
      handleSearchBusRoute,
      handleGetBusRouteDetail,
      handlePageChange,
      handleBackPage,
    } = busRouteComposable;
    const route = useRoute();
    const busRouteId = route.query.id;

    const refreshPage = async () => {
      const busRId: number = parseInt(route.query.id as string);
      const busId: number = parseInt(route.query.bid as string);
      if (currentView.value === 'index') {
        await handleGetAllBusStops(busRId);
      } else if (currentView.value === 'tableBus') {
        await handleGetAllBuses(busRId);
      } else if (currentView.value === 'detail') {
        debugger;
        await handleGetBusDetail(busId);
      }
      await handleGetBusRouteDetail(busRId);
    };

    watch(
      () => currentView.value,
      async (newView, oldView) => {
        const busRId: number = parseInt(route.query.id as string);
        const busId: number = parseInt(route.query.bid as string);
        if (newView === 'index') {
          await handleGetAllBusStops(busRId);
        } else if (newView === 'tableBus') {
          await handleGetAllBuses(busRId);
        } else if (newView === 'detail') {
          await handleGetBusDetail(busId);
        }
        await handleGetBusRouteDetail(busRId);
      },
    );

    onMounted(async () => {
      if (busRouteId != null) {
        await handleGetBusRouteDetail(busRouteId);
      } else {
        await handleGetBusRouteDetail(selectedId.value);
      }
    });

    return {
      dataBusRouteDetail,
      handleGetAllBusRoute,
      handleSearchBusRoute,
      handlePageChange,
      refreshPage,
      currentView,
      handleBackPage,
      selectedId,
      showTableBus,
      showDetail,
      handleRedirectToIndex,
    };
  },
});
</script>

<style scoped>
.timeline {
  max-height: 700px;
  overflow-y: auto;
  padding-right: 10px;
  position: relative;
  padding-left: 30px;
}

.timeline-item {
  position: relative;
  padding-left: 30px;
  margin-bottom: 20px;
}

.timeline-item:before {
  content: '';
  position: absolute;
  top: 0;
  left: 10px;
  width: 2px;
  height: 100%;
  background-color: #bd3a3a;
  border-left: dashed 2px #bd3a3a;
}

.timeline-item .timeline-indicator-advanced {
  position: absolute;
  top: 0;
  left: -9px;
  background-color: #ffffff88;
  border-radius: 50%;
  border: 2px solid #bd3a3a;
  padding: 8px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}

.timeline-item .timeline-event {
  padding-left: 20px;
}

.timeline-item .timeline-header small {
  font-size: 14px;
  color: #5d5d5d;
}

.timeline-item .timeline-event h6 {
  font-size: 16px;
  font-weight: 600;
  margin-top: 8px;
}

.timeline-item .timeline-event small {
  font-size: 14px;
  color: #888;
}

.timeline-item .timeline-event p {
  font-size: 14px;
  color: #444;
}

.timeline-item .timeline-indicator-advanced i {
  font-size: 18px;
  color: #bd3a3a;
}
</style>
