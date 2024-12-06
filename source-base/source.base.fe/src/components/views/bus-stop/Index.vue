<template>
  <div>
    <!-- Address accordion -->
    <div class="card card-action mb-6">
      <div class="card-header align-items-center py-6">
        <h5 class="card-action-title mb-0">Quản lý điểm dừng</h5>
        <div class="card-action-element">
          <button
            class="btn btn-sm btn-label-primary"
            type="button"
            data-bs-toggle="modal"
            data-bs-target="#addNewAddress"
            @click="OpenModalAddBusStop"
          >
            Thêm điểm dừng
          </button>
        </div>
      </div>
      <div class="card-body">
        <div class="accordion accordion-flush accordion-arrow-left" id="ecommerceBillingAccordionAddress">
          <div
            v-for="(busStop, index) in dataBusStop.value.data"
            :key="busStop.id"
            class="accordion-item border-bottom"
          >
            <div
              class="accordion-header d-flex justify-content-between align-items-center flex-wrap flex-sm-nowrap"
              :id="`heading${busStop.id}`"
            >
              <a
                class="accordion-button collapsed"
                data-bs-toggle="collapse"
                :data-bs-target="`#ecommerceBillingAddress${busStop.id}`"
                :aria-controls="`heading${busStop.id}`"
                role="button"
              >
                <span>
                  <span class="d-flex gap-2 align-items-baseline">
                    <span class="h6 mb-1">{{ busStop.name }}</span>
                    <span v-if="busStop.index == 1" class="badge bg-label-success">Điểm bắt đầu</span>
                    <span
                      v-if="busStop.index !== 1 && busStop.index !== dataBusStop.value.data.length"
                      class="badge bg-label-success"
                      >Điểm thứ
                      <span
                        ><b>{{ busStop.index }}</b></span
                      ></span
                    >
                    <span v-if="busStop.index == dataBusStop.value.data.length && busStop.index != 1" class="badge bg-label-success"
                      >Điểm kết thúc</span
                    >
                  </span>
                  <span class="mb-0 text-body">Thời gian đón: {{ busStop.pickUpTime }} (h:m:s)</span> <br />
                  <span class="mb-0 text-body">Thời gian đưa về: {{ busStop.returnTime }} (h:m:s)</span>
                </span>
              </a>
              <div class="d-flex gap-4 p-6 p-sm-0 pt-0 ms-1 ms-sm-0">
                <a href="javascript:void(0);" @click="OpenStudentList(busStop.id)">
                  <i class="bx bx-group text-body bx-md"></i>
                </a>
                <a href="javascript:void(0);" @click="OpenModalUpdateBusStop(busStop.id)"
                  ><i class="bx bx-edit text-body bx-md"></i
                ></a>
                <a href="javascript:void(0);" @click="OpenModalDeleteBusStop(busStop.id)"
                  ><i class="bx bx-trash text-body bx-md"></i
                ></a>
              </div>
            </div>
            <div
              :id="`ecommerceBillingAddress${busStop.id}`"
              class="accordion-collapse collapse"
              data-bs-parent="#ecommerceBillingAccordionAddress"
            >
              <div class="accordion-body ps-8 ms-1_5">
                <h6 class="mb-1">Địa chỉ: {{ busStop.address }}</h6>
                <p
                  class="mb-1"
                  :class="{
                    'text-success': busStop.status === 1,
                    'text-danger': busStop.status === 0,
                  }"
                >
                  {{ busStop.status === 1 ? 'Đang Hoạt động' : 'Không hoạt động' }}
                </p>
                <p class="mb-1">Số thứ tự: {{ busStop.index }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Address accordion -->
    <CreateAndUpdateBusStop
      :showModalAdd="showModalAdd"
      :errorsBusStop="errorsBusStop"
      :isUpdateMode="isUpdateMode"
      :dataBusStopDetail="dataBusStopDetail"
      @confirm="handleConfirm"
      @update:showModal="showModalAdd = $event"
      @closeUpdateModal="isUpdateMode = $event"
    />
    <ViewListPupils 
      :isShowPupilInBusStop="isShowPupilInBusStop"
      :pupils="dataPupilsInBusStop"
      :semesterId="selectedSemester"
      :apiUrl="apiUrl"
      @update:showModal="isShowPupilInBusStop = $event"
    />
    <DeleteBusStop
      :showDeleteModal="showModalDelete"
      @update:showDeleteModal="showModalDelete = $event"
      @confirm="handleConfirmDelete"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, onMounted, watch } from 'vue';
import CreateAndUpdateBusStop from '@/components/modals/bus-stop/CreateAndUpdateBusStop.vue';
import DeleteBusStop from '@/components/modals/bus-stop/DeleteBusStop.vue';
import { useBusStopComposable } from '@/composables/bus-stop';
import { useRoute, useRouter } from 'vue-router';
import { useBusEnrollmentComposable } from '@/composables/bus-enrollment';
import ViewListPupils from '@/components/modals/bus-stop/ViewListPupils.vue';
export default defineComponent({
  name: 'ListBusStopComponent',
  components: {
    CreateAndUpdateBusStop,
    DeleteBusStop,
    ViewListPupils
  },
  props: {},
  setup(props, { emit }) {
    const {
      dataBusStop,
      semester,
      showModalAdd,
      isUpdateMode,
      errorBusStopKeys,
      errorsBusStop,
      searchKey,
      dataBusStopDetail,
      showModalDelete,
      resetPage,
      selectedStatus,
      handleFilterByStatus,
      OpenModalDeleteBusStop,
      handleConfirmDelete,
      resetModal,
      handleUpdateBusStop,
      OpenModalUpdateBusStop,
      handleRefreshBusStop,
      handleCreateBusStop,
      handleGetBusStopDetail,
      handleConfirm,
      handleFetchSemester,
      OpenModalAddBusStop,
      handleGetAllBusStops,
      handleSearchBusStop,
      handlePageChange,
    } = useBusStopComposable();
    const {OpenStudentList,isShowPupilInBusStop,selectedSemester,apiUrl,dataPupilsInBusStop} = useBusEnrollmentComposable();
    watch(resetPage, newValue => {
      if (newValue) {
        emit('refreshPage');
        resetPage.value = false;
      }
    });
    const route = useRoute();
    const busRouteId = route.query.id;
    onMounted(async () => {
      if (busRouteId) {
        await handleGetAllBusStops(busRouteId);
      }
    });

    return {
      dataBusStop,
      semester,
      showModalAdd,
      isUpdateMode,
      errorBusStopKeys,
      errorsBusStop,
      searchKey,
      dataBusStopDetail,
      showModalDelete,
      resetPage,
      selectedStatus,
      isShowPupilInBusStop,
      selectedSemester,
      apiUrl,
      dataPupilsInBusStop,
      OpenStudentList,
      handleFilterByStatus,
      OpenModalDeleteBusStop,
      handleConfirmDelete,
      resetModal,
      handleUpdateBusStop,
      OpenModalUpdateBusStop,
      handleRefreshBusStop,
      handleCreateBusStop,
      handleGetBusStopDetail,
      handleConfirm,
      handleFetchSemester,
      OpenModalAddBusStop,
      handleGetAllBusStops,
      handleSearchBusStop,
      handlePageChange,
    };
  },
});
</script>

<style scoped>
.dialog-footer {
  text-align: right;
}

.custom-confirm-button {
  background-color: #696cff !important;
  border-color: #696cff !important;
}
</style>
