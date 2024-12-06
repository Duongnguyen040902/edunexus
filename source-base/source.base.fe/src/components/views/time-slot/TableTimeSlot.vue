<template>
  <div class="col-xl-12 col-lg-12 col-md-12 order-0 order-md-1">
    <!-- / Time Slot Cards -->
    <div class="row text-nowrap">
      <div class="col-md-6 mb-6">
        <div class="card h-100">
          <div class="card-body">
            <div class="card-icon mb-2">
              <div class="avatar">
                <div class="avatar-initial rounded bg-label-primary">
                  <i class="bx bx-time bx-lg"></i>
                </div>
              </div>
            </div>
            <div class="card-info">
              <h5 class="card-title mb-2">
                Tổng số tiết học: {{ timeSlotData.value ? timeSlotData.value.totalRecords : 0 }}
              </h5>
              <div class="d-flex align-items-baseline gap-1">
                <h5 class="text-primary mb-0"></h5>
              </div>
              <p class="mb-0 text-truncate"></p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Time Slot Table -->
    <div class="mb-3 mb-md-0 me-md-2">
      <button class="btn btn-primary" @click="OpenModalAddTimeSlot">Thêm tiết học</button>
    </div>
    <div class="card mb-6">
      <div class="table-responsive mb-4">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="card-header flex-column flex-md-row py-0 mt-6 mt-md-0">
            <div
              class="head-label text-center text-md-start pt-2 pt-md-0 flex-grow-1 d-flex align-items-center flex-wrap"
            >
              <div class="d-flex align-items-center mb-3 mb-md-0">
                <h5 class="card-title mb-0 text-nowrap me-3">Danh sách Thời gian tiết học</h5>
              </div>

              <div class="status-select-container mb-3 mb-md-0 w-100">
                <span class="status-label">Trạng thái:</span>
                <select v-model="selectedStatus" class="form-select" @change="handleFilterByStatus">
                  <option :label="'Tất cả'" :value="``" />
                  <option
                    v-for="status in StatusTimeSlotOptions"
                    :key="status.value"
                    :label="status.label"
                    :value="status.value"
                  />
                </select>
              </div>
            </div>
            <div id="DataTables_Table_0_filter" class="dataTables_filter d-flex align-items-center">
              <label class="mb-0 me-2">
                <input
                  type="search"
                  class="form-control"
                  placeholder="Tìm kiếm tiết học"
                  v-model="searchKey"
                  aria-controls="DataTables_Table_0"
                  @keyup.enter="handleSearchTimeSlot"
                />
              </label>
              <button @click="handleSearchTimeSlot" class="btn btn-primary">Tìm kiếm</button>
            </div>
          </div>
          <table
            class="table datatables-time-slot border-top dataTable no-footer dtr-column collapsed"
            id="DataTables_Table_0"
            aria-describedby="DataTables_Table_0_info"
          >
            <thead>
              <tr>
                <th class="control sorting_disabled" rowspan="1" colspan="1" style="width: 0px"></th>
                <th class="sorting sorting_desc" tabindex="0">Số thứ tự</th>
                <th class="sorting">Thời gian</th>
                <th class="sorting">Thời gian bắt đầu</th>
                <!-- Start Time Column -->
                <th class="sorting">Thời gian kết thúc</th>
                <!-- End Time Column -->
                <th class="sorting">Trạng thái</th>
                <th class="text-md-center sorting_disabled dtr-hidden">Hành động</th>
              </tr>
            </thead>
            <tbody v-if="timeSlotData.value?.data?.length > 0">
              <tr v-for="(slot, index) in timeSlotData.value?.data || []" :key="index">
                <td class="control"></td>
                <td>{{ index + 1 }}</td>
                <td>{{ slot.name }}</td>
                <td>{{ slot.startTime }}</td>
                <!-- Bind Start Time -->
                <td>{{ slot.endTime }}</td>
                <!-- Bind End Time -->
                <td>
                  <span
                    :class="{
                      'bg-label-success': slot.isActive === true,
                      'bg-label-danger': slot.isActive === false,
                    }"
                  >
                    {{ slot.isActive === true ? 'Đang hoạt động' : 'Không hoạt động' }}
                  </span>
                </td>
                <td>
                  <div class="text-xxl-center">
                    <button class="btn btn-icon dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                      <i class="bx bx-dots-vertical-rounded"></i>
                    </button>
                    <div class="dropdown-menu dropdown-menu-end m-0">
                      <a href="javascript:;" class="dropdown-item" @click="OpenModalUpdateTimeSlot(slot.id)"
                        >Chỉnh sửa</a
                      >
                      <a href="javascript:;" class="dropdown-item" @click="OpenModalDeleteTimeSlot(slot.id)">Xóa</a>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
            <tbody v-else>
              <tr>
                <td colspan="7" class="text-center">
                  <span>Không có dữ liệu hiển thị!</span>
                </td>
              </tr>
            </tbody>
          </table>

          <div class="row mx-6">
            <div class="col-md-12 col-xxl-6">
              <div
                v-if="timeSlotData.value?.totalPages > 1"
                class="dataTables_paginate paging_simple_numbers"
                id="DataTables_Table_0_paginate"
              >
                <Pagination
                  :total-records="timeSlotData.value?.totalRecords || 0"
                  :page-size="timeSlotData.value?.pageSize || 0"
                  :current-page="timeSlotData.value?.pageNumber || 0"
                  @update:currentPage="currentPage = $event"
                  @page-changed="handlePageChange"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- /table -->
    <CreateAndUpdateTimeSlot
      :showModalAdd="showModalAdd"
      :errorsTimeSlot="errorsTimeSlot"
      :isUpdateMode="isUpdateMode"
      :timeSlotDetail="timeSlotDetail"
      @confirm="handleConfirm"
      @update:showModal="showModalAdd = $event"
      @closeUpdateModal="isUpdateMode = $event"
    />
    <DeleteTimeSlot
      :showDeleteModal="showModalDelete"
      @update:showDeleteModal="showModalDelete = $event"
      @confirm="handleConfirmDelete"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, watch } from 'vue';
import CreateAndUpdateTimeSlot from '@/components/modals/time-slot/CreateAndUpdateTimeSlot.vue';
import DeleteTimeSlot from '@/components/modals/time-slot/DeleteTimeSlot.vue';
import { useTimeSlotComposable } from '@/composables/time-slot';
import Pagination from '@/components/common/Pagination.vue';
import { StatusTimeSlot } from '@/constants/enums/mode';
import { useRoute } from 'vue-router';

export default defineComponent({
  name: 'TimeSlotTableComponent',
  components: {
    CreateAndUpdateTimeSlot,
    DeleteTimeSlot,
    Pagination,
    StatusTimeSlot,
  },
  setup(props, { emit }) {
    const {
      timeSlotData,
      showModalAdd,
      isUpdateMode,
      errorsTimeSlot,
      timeSlotDetail,
      showModalDelete,
      currentPage,
      searchKey,
      resetPage,
      selectedStatus,
      handleFilterByStatus,
      handleSearchTimeSlot,
      handlePageChange,
      OpenModalAddTimeSlot,
      OpenModalUpdateTimeSlot,
      OpenModalDeleteTimeSlot,
      handleConfirmDelete,
      handleConfirm,
      handleGetAllTimeSlots,
    } = useTimeSlotComposable();

    const StatusTimeSlotOptions = StatusTimeSlot;
    watch(resetPage, newValue => {
      if (newValue) {
        emit('refreshPage');
        resetPage.value = false;
      }
    });

    onMounted(async () => {
      await handleGetAllTimeSlots();
      console.log('test', selectedStatus);
    });

    return {
      timeSlotData,
      showModalAdd,
      isUpdateMode,
      errorsTimeSlot,
      timeSlotDetail,
      showModalDelete,
      currentPage,
      handleFilterByStatus,
      searchKey,
      selectedStatus,
      StatusTimeSlotOptions,
      handleSearchTimeSlot,
      handlePageChange,
      OpenModalAddTimeSlot,
      OpenModalUpdateTimeSlot,
      OpenModalDeleteTimeSlot,
      handleConfirmDelete,
      handleConfirm,
      handleGetAllTimeSlots,
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
}

.status-select-container .form-select {
  width: auto;
  max-width: 200px;
  font-size: 0.875rem;
}
</style>
