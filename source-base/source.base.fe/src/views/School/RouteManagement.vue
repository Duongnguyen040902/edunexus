<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="row g-6">
      <!-- Card Border Shadow -->
      <div class="col-lg-6 col-sm-6">
        <div class="card card-border-shadow-primary h-100">
          <div class="card-body">
            <div class="d-flex align-items-center mb-2">
              <div class="avatar me-4">
                <span class="avatar-initial rounded bg-label-primary"><i class="bx bxs-truck bx-lg"></i></span>
              </div>
              <h4 class="mb-0">{{ totalActiveRoutes }}</h4>
            </div>
            <p class="mb-2">Tuyến xe đang hoạt động</p>
            <p class="mb-0">
              <span class="text-heading fw-medium me-2"></span>
            </p>
          </div>
        </div>
      </div>
      <div class="col-lg-6 col-sm-6">
        <div class="card card-border-shadow-warning h-100">
          <div class="card-body">
            <div class="d-flex align-items-center mb-2">
              <div class="avatar me-4">
                <span class="avatar-initial rounded bg-label-warning"><i class="bx bx-error bx-lg"></i></span>
              </div>
              <h4 class="mb-0">{{ totalInactiveRoutes }}</h4>
            </div>
            <p class="mb-2">Tuyến xe đang chờ hoạt động</p>
            <p class="mb-0">
              <span class="text-heading fw-medium me-2"></span>
            </p>
          </div>
        </div>
      </div>
      <!--/ Card Border Shadow -->
      <!-- On route vehicles Table -->
      <TableBusRoute 
      @refreshPage="refreshPage"/>
      <!-- On route vehicles Table -->
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import TableBusRoute from '@/components/views/bus-route/TableBusRoute.vue';
import { useBusRouteComposable } from '@/composables/bus-route';
export default defineComponent({
  name: 'RouteManagement',
  components: {
    TableBusRoute,
  },
  setup() {
    const busRouteComposable = useBusRouteComposable();
    const {
      dataBusRoute,
      totalActiveRoutes,
      totalInactiveRoutes,
      handleGetAllBusRoute,
      getOverviewRouteForView,
      handleSearchBusRoute,
      handlePageChange,
      handleRedirectToDetail,
    } = busRouteComposable;

     const refreshPage = async () => {
      await getOverviewRouteForView();
      await handleGetAllBusRoute();
      console.log('Trang đã được refresh!');
    };
    
    onMounted(async () => {
      await getOverviewRouteForView();
      await handleGetAllBusRoute();
    });

    return {
      refreshPage,
      dataBusRoute,
      totalActiveRoutes,
      totalInactiveRoutes,
      handleGetAllBusRoute,
      handleSearchBusRoute,
      handlePageChange,
      handleRedirectToDetail,
    };
  },
});
</script>
