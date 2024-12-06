<template>
  <div>
    <!-- / Customer cards -->
    <div class="row text-nowrap">
      <div class="col-md-12 mb-6">
        <div class="card h-100">
          <div class="card-body">
            <div class="card-icon mb-2">
              <div class="avatar">
                <div class="avatar-initial rounded bg-label-primary">
                  <i class="bx bx-car bx-lg"></i>
                </div>
              </div>
            </div>
            <div class="card-info">
              <h5 class="card-title mb-2">Tổng số xe bus trên tuyến</h5>
              <div class="d-flex align-items-baseline gap-1">
                <h5 class="text-primary mb-0">{{ dataBus.value.totalRecords }}</h5>
              </div>
              <p class="mb-0 text-truncate"></p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- / customer cards -->

    <!-- Invoice table -->
    <div class="card mb-6">
      <div class="table-responsive mb-4">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="card-header flex-column flex-md-row py-0 mt-6 mt-md-0">
            <div class="head-label text-center text-md-start pt-2 pt-md-0 flex-grow-1 d-flex align-items-center">
              <h5 class="card-title mb-0 text-nowrap me-3">Danh sách xe</h5>
              <!-- Nút Thêm xe -->
              <div class="mb-3 mb-md-0 me-md-2">
                <button class="btn btn-primary" @click="openModalAddBus">Thêm xe</button>
              </div>
            </div>
            <div id="DataTables_Table_0_filter" class="dataTables_filter d-flex align-items-center">
              <label class="mb-0 me-2">
                <input
                  type="search"
                  class="form-control"
                  placeholder="Tìm kiếm xe"
                  aria-controls="DataTables_Table_0"
                  @input="handleSearchBus"
                />
              </label>
            </div>
          </div>
          <div class="status-select-container mb-3 mb-md-0">
            <span class="status-label">Trạng thái:</span>
            <select class="form-select" @change="handleChangeStatus">
              <option value="">Tất cả</option>
              <option value="1">Hoạt động</option>
              <option value="0">Không hoạt động</option>
            </select>
          </div>
          <table
            class="table datatables-customer-order border-top dataTable no-footer dtr-column collapsed"
            id="DataTables_Table_0"
            aria-describedby="DataTables_Table_0_info"
            style="width: 798px"
          >
            <thead>
              <tr>
                <th class="control sorting_disabled" rowspan="1" colspan="1" style="width: 0px" aria-label=""></th>
                <th
                  class="sorting sorting_desc"
                  tabindex="0"
                  aria-controls="DataTables_Table_0"
                  rowspan="1"
                  colspan="1"
                  style="width: 81px"
                  aria-label="Số thứ tự: kích hoạt để sắp xếp cột theo thứ tự tăng dần"
                  aria-sort="descending"
                >
                  Số thứ tự
                </th>
                <th
                  class="sorting"
                  tabindex="0"
                  aria-controls="DataTables_Table_0"
                  rowspan="1"
                  colspan="1"
                  style="width: 136px"
                  aria-label="Tên xe: kích hoạt để sắp xếp cột theo thứ tự tăng dần"
                >
                  Tên xe
                </th>
                <th
                  class="sorting"
                  tabindex="0"
                  aria-controls="DataTables_Table_0"
                  rowspan="1"
                  colspan="1"
                  style="width: 172px"
                  aria-label="Trạng thái: kích hoạt để sắp xếp cột theo thứ tự tăng dần"
                >
                  Trạng thái
                </th>
                <th
                  class="text-md-center sorting_disabled dtr-hidden"
                  rowspan="1"
                  colspan="1"
                  style="width: 97px"
                  aria-label="Hành động"
                >
                  Hành động
                </th>
              </tr>
            </thead>
            <tbody>
              <template v-if="dataBus.value.data && dataBus.value.data.length > 0">
              <tr
                v-for="(bus, index) in dataBus.value.data"
                :key="index"
                :class="{ odd: index % 2 === 0, even: index % 2 !== 0 }"
              >
                <td class="control" tabindex="0"></td>
                <td class="sorting_1">
                  <a @click="showDetail(busRouteId,bus.id)">
                    <span>{{ index + 1 }}</span>
                  </a>
                </td>
                <td>
                  <span class="text-nowrap" @click="showDetail(busRouteId,bus.id)">{{ bus.name }}</span>
                </td>
                <td>
                  <span
                    :class="{
                      'bg-label-success': bus.status === 1,
                      'bg-label-danger': bus.status === 0,
                      'bg-label-info': bus.status !== 0 && bus.status !== 1,
                    }"
                  >
                    {{ bus.status === 1 ? 'Đang hoạt động' : 'Không hoạt động' }}
                  </span>
                </td>
                <td class="dtr-hidden">
                  <div class="text-xxl-center">
                    <button class="btn btn-icon dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                      <i class="bx bx-dots-vertical-rounded"></i>
                    </button>
                    <div class="dropdown-menu dropdown-menu-end m-0">
                      <a href="javascript:;" class="dropdown-item" @click="showDetail(busRouteId,bus.id)">Xem</a>
                      <a href="javascript:;" class="dropdown-item" @click="openModalEditBus(bus.id)"
                        >Chỉnh sửa thông tin</a
                      >
                      <a href="javascript:;" class="dropdown-item delete-record" @click="openModalDeleteBus(bus.id)"
                        >Xóa</a
                      >
                    </div>
                  </div>
                </td>
              </tr>
            </template>
              <tr v-else>
                <td colspan="5" class="text-center">Không có dữ liệu hiển thị!</td>
              </tr>
            </tbody>
          </table>
          <div class="row mx-6">
            <div class="col-md-12 col-xxl-6 text-center text-xl-start pb-2 pb-xxl-0 pe-0"></div>
            <div class="col-md-12 col-xxl-6" v-if="dataBus.value.totalPages > 1">
              <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_0_paginate">
                <Pagination
                  :total-records="dataBus.value.totalRecords"
                  :page-size="dataBus.value.pageSize"
                  :current-page="dataBus.value.pageNumber"
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
    <CreateAndUpdateBus
      :showModalAdd="isAddBusModalVisible"
      :errorsBus="busErrors"
      :isUpdateMode="isEditBusMode"
      :dataBusDetail="dataBusDetail"
      @confirm="handleConfirm"
      @update:showModal="isAddBusModalVisible = $event"
      @closeUpdateModal="isEditBusMode = $event"
    />
    <DeleteBus
      :showDeleteModal="isDeleteBusModalVisible"
      @update:showDeleteModal="isDeleteBusModalVisible = $event"
      @confirm="handleConfirmDelete"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, onMounted, watch } from 'vue';
import CreateAndUpdateBus from '@/components/modals/bus/CreateAndUpdateBus.vue';
import DeleteBus from '@/components/modals/bus/DeleteBus.vue';
import { useBusDetailComposable } from '@/composables/bus';
import Pagination from '@/components/common/Pagination.vue';
import { useRoute } from 'vue-router';
import { useBusEnrollmentComposable } from '@/composables/bus-enrollment';
export default defineComponent({
  name: 'TableBusComponent',
  components: {
    CreateAndUpdateBus,
    DeleteBus,
    Pagination,
  },
  props: {},
  setup(props, { emit }) {
    const {
      dataBus,
      isAddBusModalVisible,
      isEditBusMode,
      busErrorKeys,
      busErrors,
      busSearchKey,
      dataBusDetail,
      isDeleteBusModalVisible,
      resetPage,
      currentPage,
      selectedBusStatus,
      handleChangeStatus,
      openModalDeleteBus,
      handleConfirmDelete,
      resetModal,
      handleUpdateBus,
      openModalEditBus,
      handleRefreshBus,
      handleCreateBus,
      handleGetBusDetail,
      handleConfirm,
      openModalAddBus,
      handleGetAllBuses,
      handleSearchBus,
      handlePageChange,
    } = useBusDetailComposable();
    const { showDetail, resetPageForRedirect } = useBusEnrollmentComposable();

    watch(resetPage, newValue => {
      if (newValue) {     
        emit('refreshPage');
        resetPage.value = false;
      }
    });

    const route = useRoute();
    const busRouteId: number = parseInt(route.query.id as string);
    const p = ref();
    onMounted(async () => {
      if (busRouteId) {
        await handleGetAllBuses(busRouteId);
      }
    });

    return {
      isAddBusModalVisible,
      isDeleteBusModalVisible,
      dataBus,
      busErrors,
      dataBusDetail,
      handleConfirm,
      isEditBusMode,
      currentPage,
      busSearchKey,
      busRouteId,
      showDetail,
      handleChangeStatus,
      openModalDeleteBus,
      openModalEditBus,
      handleConfirmDelete,
      openModalAddBus,
      handleSearchBus,
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
.status-select-container {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 20px;
}

.status-label {
  font-weight: bold;
  margin-right: 8px;
}

.form-select {
  width: auto;
  min-width: 150px; /* Adjust as needed */
  padding: 5px 10px;
  font-size: 1rem;
}
</style>
