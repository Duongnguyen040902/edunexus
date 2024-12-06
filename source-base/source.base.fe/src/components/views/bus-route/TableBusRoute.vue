<template>
  <div class="col-12 order-5">
    <div class="card">
      <div class="card-header d-flex flex-column align-items-center align-items-md-start">
        <div class="d-flex justify-content-between w-100 pb-3 flex-wrap align-items-center">
          <h5 class="card-title mb-0 text-center text-md-start">Danh sách tuyến xe</h5>
          <div class="dt-action-buttons text-end">
            <div class="btn-group">
              <!-- <button
                class="btn buttons-collection dropdown-toggle btn-label-primary me-2"
                type="button"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <i class="bx bx-export bx-sm me-1"></i>
                <span class="d-none d-sm-inline-block">Xuất excel</span>
              </button> -->
              <button class="btn btn-secondary create-new btn-primary" type="button" @click="OpenModalAddBusRoute">
                <i class="bx bx-plus bx-sm me-1"></i>
                <span class="d-none d-sm-inline-block">Thêm tuyến xe</span>
              </button>
            </div>
          </div>
        </div>
        <div class="row w-100 mt-2">
          <div class="col-sm-12 col-md-6 d-flex justify-content-start mb-3 mb-md-0">
            <div class="button-group">
              <button @click="handleRefreshBusRoute" class="btn btn-secondary">Làm mới</button> <hr>
              <div class="filter-container">
                <label for="statusFilter" class="filter-label">Trạng thái:</label>
                <select id="statusFilter" v-model="selectedStatus" @change="handleFilterByStatus" class="status-select">
                  <option value="" >Tất cả</option>
                  <option value="1">Hoạt động</option>
                  <option value="0">Chờ hoạt động</option>
                </select>
              </div>
            </div>
          </div>
          <div class="col-sm-12 col-md-6 d-flex justify-content-end mt-3 mt-md-0">
            <div id="DataTables_Table_0_filter" class="dataTables_filter">
              <input
                type="search"
                class="form-control"
                placeholder="Tên tuyến xe, miêu tả,..."
                aria-controls="DataTables_Table_0"
                v-model="searchKey"
                @keyup.enter="handleSearchBusRoute"
              />
              <button @click="handleSearchBusRoute" class="btn btn-primary">Tìm kiếm</button>
            </div>
          </div>
        </div>
      </div>

      <div class="card-datatable">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div >
            <table
              class="dt-route-vehicles table table-sm dataTable no-footer dtr-column"
              id="DataTables_Table_0"
              aria-describedby="DataTables_Table_0_info"
            >
              <thead>
                <tr>
                  <th
                    class="control sorting_disabled dtr-hidden"
                    rowspan="1"
                    colspan="1"
                    aria-label=""
                    style="width: 0px; display: none"
                  ></th>
                  <th rowspan="1" colspan="1" data-col="1" aria-label="" style="width: 18px">
                    <span type="text">STT</span>
                  </th>
                  <th
                    class="sorting sorting_asc"
                    tabindex="0"
                    aria-controls="DataTables_Table_0"
                    rowspan="1"
                    colspan="1"
                    aria-label="location: activate to sort column descending"
                    aria-sort="ascending"
                    style="width: 143px"
                  >
                    Tên tuyến xe
                  </th>
                  <th
                    class="sorting"
                    tabindex="0"
                    aria-controls="DataTables_Table_0"
                    rowspan="1"
                    colspan="1"
                    aria-label="starting route: activate to sort column ascending"
                    style="width: 217px"
                  >
                    Mô tả
                  </th>
                  <th
                    class="w-20 sorting"
                    tabindex="0"
                    aria-controls="DataTables_Table_0"
                    rowspan="1"
                    colspan="1"
                    aria-label="progress: activate to sort column ascending"
                    style="width: 206px"
                  >
                    trạng thái
                  </th>
                  <th
                    class="w-20 sorting"
                    tabindex="0"
                    aria-controls="DataTables_Table_0"
                    rowspan="1"
                    colspan="1"
                    aria-label="progress: activate to sort column ascending"
                    style="width: 206px"
                  ></th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="(route, index) in dataBusRoute.value.data.data"
                  :key="route.id"
                  :class="index % 2 === 0 ? 'even' : 'odd'"
                >
                <td class="dt-checkboxes-cell"> {{ (dataBusRoute.value.data.pageNumber - 1) * dataBusRoute.value.data.pageSize + (index + 1) }}</td>
                  <td class="sorting_1">
                    <div class="d-flex justify-content-start align-items-center user-name">
                      <div class="avatar-wrapper">
                        <div class="avatar me-4">
                          <span class="avatar-initial rounded-circle bg-label-secondary">
                            <i class="bx bxs-truck bx-lg"></i>
                          </span>
                        </div>
                      </div>
                      <div class="d-flex flex-column">
                        <a class="text-heading fw-medium" href="app-logistics-fleet.html">{{ route.name }}</a>
                      </div>
                    </div>
                  </td>
                  <td>
                    <div class="text-body">{{ route.description }}</div>
                  </td>
                  <td>
                    <span
                      class="badge rounded"
                      :class="{
                        'bg-label-success': route.status === 1,
                        'bg-label-warning': route.status === 0,
                      }"
                    >
                      {{ route.status === 1 ? 'Hoạt động' : 'Chờ hoạt động' }}
                    </span>
                  </td>
                  <td class="" style="">
                    <div class="text-xxl-center">
                      <button class="btn btn-icon dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                        <i class="bx bx-dots-vertical-rounded"></i>
                      </button>
                      <div class="dropdown-menu dropdown-menu-end m-0">
                        <a href="javascript:;" class="dropdown-item" @click="handleRedirectToDetail(route.id)">Xem chi tiết</a>
                        <a href="javascript:;" class="dropdown-item" @click="OpenModalUpdateBusRoute(route.id)"
                          >Chỉnh sửa thông tin</a
                        ><a
                          href="javascript:;"
                          class="dropdown-item delete-record"
                          @click="OpenModalDeleteBusRoute(route.id)"
                          >Xóa tuyến xe</a
                        >
                      </div>
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
            <div style="width: 1%"></div>
          </div>
          <div class="row d-flex align-items-center">
            <div class="col-sm-12 col-md-6">
              <div class="dataTables_info" id="DataTables_Table_0_info" role="status" aria-live="polite"></div>
            </div>
            <div class="col-sm-12 col-md-6" v-if="dataBusRoute.value.data.totalPages >1">
              <Pagination
                :total="dataBusRoute.value.data.totalRecords"
                :pageSize="dataBusRoute.value.data.pageSize"
                :currentPage="dataBusRoute.value.data.pageNumber"
                @update:currentPage="handlePageChange"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
    <AddAndUpdateBusRoute
      :showModalAdd="showModalAdd"
      :errorsBusRoute="errorsBusRoute"
      :isUpdateMode="isUpdateMode"
      :dataBusRouteDetail="dataBusRouteDetail"
      @confirm="handleConfirm"
      @update:showModal="showModalAdd = $event"
      @closeUpdateModal="isUpdateMode = $event"
    />
    <DeleteBusRoute
      :showDeleteModal="showModalDelete"
      @update:showDeleteModal="showModalDelete = $event"
      @confirm="handleConfirmDelete"
    />
  </div>
</template>

<script lang="ts">
import { useBusRouteComposable } from '@/composables/bus-route';
import { onMounted, ref, watch } from 'vue';
import { defineComponent } from 'vue';
import AddAndUpdateBusRoute from '@/components/modals/bus-route/AddAndUpdateBusRoute.vue';
import Pagination from '@/components/common/Pagination.vue';
import DeleteBusRoute from '@/components/modals/bus-route/DeleteBusRoute.vue';
export default defineComponent({
  name: 'TableBusRoute',
  components: {
    AddAndUpdateBusRoute,
    Pagination,
    DeleteBusRoute,
  },
  props: {},
  setup(props, { emit }) {
    const selectedSemester = ref<string | number>('');
    const busRouteComposable = useBusRouteComposable();
    const {
      dataBusRoute,
      handleGetAllBusRoute,
      showModalAdd,
      semester,
      errorBusRouteKeys,
      errorsBusRoute,
      fetchSemester,
      searchKey,
      isUpdateMode,
      dataBusRouteDetail,
      showModalDelete,
      resetPage,
      selectedStatus,
      handleRedirectToDetail,
      handleFilterByStatus,
      OpenModalDeleteBusRoute,
      handleConfirmDelete,
      resetModal,
      handleGetBusRouteDetail,
      OpenModalUpdateBusRoute,
      handleFetchSemester,
      handleRefreshBusRoute,
      OpenModalAddBusRoute,
      handleCreateBusRoute,
      handleSearchBusRoute,
      handlePageChange,
      handleConfirm,
    } = busRouteComposable;

    watch(resetPage, newValue => {
      if (newValue) {
        emit('refreshPage');
        resetPage.value = false;
      }
    });

    onMounted(async () => {
      handleGetAllBusRoute;
    });

    return {
      selectedSemester,
      showModalAdd,
      errorBusRouteKeys,
      errorsBusRoute,
      semester,
      dataBusRoute,
      isUpdateMode,
      searchKey,
      dataBusRouteDetail,
      showModalDelete,
      selectedStatus,
      handleRedirectToDetail,
      handleFilterByStatus,
      OpenModalDeleteBusRoute,
      handleConfirmDelete,
      resetModal,
      handleConfirm,
      handleGetBusRouteDetail,
      OpenModalUpdateBusRoute,
      handleRefreshBusRoute,
      handleSearchBusRoute,
      handlePageChange,
      handleCreateBusRoute,
      OpenModalAddBusRoute,
    };
  },
});
</script>

<style scoped>
.custom-button {
  background-color: rgb(113, 113, 218);
  color: white;
  border: none;
  border-radius: 5px;
  padding: 10px 20px;
  font-size: 16px;
  cursor: pointer;
  transition:
    background-color 0.3s,
    transform 0.2s;
}

.custom-button:hover {
  background-color: #c0aead;
  transform: translateY(-2px);
}

.custom-button:active {
  transform: translateY(1px);
}
.table {
  width: 100%;
}

.table th,
.table td {
  text-align: left;
  vertical-align: middle;
}

.table th {
  background-color: #f8f9fa;
}

.custom-button {
  padding: 5px 10px;
  font-size: 14px;
  transition:
    background-color 0.2s,
    transform 0.2s;
}

.custom-button:hover {
  background-color: #e65c50;
  transform: translateY(-1px);
}
.table th:nth-child(3),
.table td:nth-child(3) {
  width: 20%;
}

.table th:nth-child(4),
.table td:nth-child(4) {
  width: 20%;
}
.dataTables_filter {
  display: flex;
  align-items: center;
  gap: 10px;
}

.dataTables_filter input[type='search'] {
  width: 250px;
  padding: 8px;
  border-radius: 4px;
  border: 1px solid #ccc;
}

.dataTables_filter .btn-primary {
  padding: 8px 12px;
  font-size: 14px;
  border-radius: 4px;
}
.filter-container {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.filter-label {
  font-size: 16px;
  font-weight: 500;
  margin-right: 10px;
  color: #333;
}

.status-select {
  padding: 10px 15px;
  font-size: 14px;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: #f9f9f9;
  transition: all 0.3s ease;
  outline: none;
}

.status-select:focus {
  border-color: #007bff;
  background-color: #e7f3fe;
}

.status-select:hover {
  border-color: #0056b3;
  background-color: #f1f1f1;
}

.status-select option {
  padding: 8px;
}
</style>
