<template>
    <div class="container-fluid flex-grow-1 container-p-y">
        <BusSidebarComponent />
    <div class="row g-6 mt-3">
      <div class="col-12">
        <div class="row">
          <div class="col-sm-6 col-xl-4 d-flex mb-3">
                <div class="card flex-grow-1">
                    <div class="card-body">
                        <div class="d-flex align-items-start justify-content-between">
                            <div class="content-left">
                                <span class="text-heading">Thông tin xe</span>
                                <div class="d-flex align-items-center my-1">
                      <h4 class="mb-0 me-2">{{ busDetail?.name ?? ' ' }}</h4>
                                </div>
                    <small class="mb-0"
                      >Lái xe: {{ busDetail?.driverName ?? ' ' }} - Biển số:
                      {{ busDetail?.licensePlate ?? 'Chưa cập nhật' }}</small
                    >
                            </div>
                            <div class="avatar">
                    <span class="avatar-initial rounded bg-label-success"><i class="bx bx-windows"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
          <div class="col-sm-6 col-xl-4 d-flex mb-3">
                <div class="card flex-grow-1">
                    <div class="card-body">
                        <div class="d-flex align-items-start justify-content-between">
                            <div class="content-left">
                                <span class="text-heading">Người giám sát</span>
                                <div class="d-flex align-items-center my-1">
                                    <h4 class="mb-0 me-2" v-if="busDetail?.busSupervisor">
                        {{ busDetail.busSupervisor?.firstName ?? '' }} {{ busDetail.busSupervisor?.lastName ?? ' ' }}
                                    </h4>
                                    <h4 class="mb-0 me-2" v-else>Hiện chưa có người giám sát</h4>
                                </div>
                    <small class="mb-0" v-if="busDetail?.busSupervisor"
                      >Số điện thoại: {{ busDetail.busSupervisor?.phoneNumber ?? 'Chưa cập nhật' }}</small
                    >
                            </div>
                            <div class="avatar">
                    <span class="avatar-initial rounded bg-label-danger"><i class="bx bxs-user-detail"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
          <div class="col-sm-6 col-xl-4 d-flex mb-3">
                <div class="card flex-grow-1">
                    <div class="card-body">
                        <div class="d-flex align-items-start justify-content-between">
                            <div class="content-left">
                                <span class="text-heading">Học Kỳ</span>
                                <div class="d-flex align-items-center my-1">
                                    <h4 class="mb-0 me-2">
                        {{ currentSemester.semesterName || 'Hiện không có kỳ học nào hoạt động' }}
                                    </h4>
                                </div>
                    <small class="mb-0">{{ busDetail?.busRouteName ?? '' }}</small>
                            </div>
                            <div class="avatar">
                    <span class="avatar-initial rounded bg-label-primary"><i class="bx bxs-calendar"></i></span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-12">
        <div class="card h-100">
          <div class="card-header d-flex align-items-center justify-content-between">
            <div class="card-title mb-0">
              <h5 class="m-0 me-2">Điểm dừng của {{ busDetail?.busRouteName ?? '' }}</h5>
            </div>
          </div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="table card-table table-border-top-0">
                <tbody class="table-border-bottom-0">
                  <tr v-for="stop in busDetail?.busStops" :key="stop.id">
                    <td class="w-50 ps-0">
                      <div class="d-flex justify-content-start align-items-center">
                        <div class="me-2">
                          <i class="bx bx-map bx-lg text-heading"></i>
                        </div>
                        <h6 class="mb-0 fw-normal">{{ stop.name }}</h6>
                            </div>
                    </td>
                    <td class="text-end pe-0 text-nowrap">
                      <h6 class="mb-0">{{ stop.estimatedTime }}</h6>
                    </td>
                    <td class="text-end pe-0">
                      <span>{{ stop.address }}</span>
                    </td>
                  </tr>
                </tbody>
              </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <TableBusEnrollmentComponent v-if="busDetail" :busDetail="busDetail" />
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted } from 'vue';
import { useBusDetailComposable } from '@/composables/bus';
import BusSidebarComponent from '@/components/views/bus/BusSidebar.vue';
import TableBusEnrollmentComponent from '@/components/views/bus/TableBusEnrollment.vue';

export default defineComponent({
  name: 'BusDetail',
    components: {
        BusSidebarComponent,
        TableBusEnrollmentComponent,
    },
    setup() {
        const { busDetail, fetchBusDetail, busId, currentSemester, fetchCurrentSemester } = useBusDetailComposable();

        onMounted(async () => {
            if (busId.value) {
                await fetchCurrentSemester();
                await fetchBusDetail(busId.value);
            }
        });

        return {
            busDetail,
      currentSemester,
        };
    },
});
</script>

<style scoped>
.card {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}
</style>