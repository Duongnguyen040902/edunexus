<template>
  <div>
    <div class="col-md-12 mb-12 bus-detail-container">
      <div class="card h-100 shadow-sm rounded-lg">
        <div class="card-body">
          <div class="icon-container mb-3 d-flex align-items-center">
            <div class="avatar">
              <div class="avatar-initial rounded-circle bg-primary d-flex align-items-center justify-content-center">
                <i class="bx bx-car bx-lg text-white"></i>
              </div>
            </div>
            <h5 class="card-title ms-3 mb-0 text-primary">Thông tin chi tiết:</h5>
          </div>
          <button class="btn btn-outline-primary edit-button" @click="openModalEditBus(dataBusDetail.value.data.id)">
            Chỉnh sửa
          </button>
          <div class="card-info mt-3" v-if="dataBusDetail.value && dataBusDetail.value.data">
            <h5 class="text-primary mb-2">{{ dataBusDetail.value.data.name }}</h5>
            <div class="info-details">
              <p class="mb-1"><strong>Tên tài xế:</strong> {{ dataBusDetail.value.data.driverName }}</p>
              <p class="mb-1"><strong>Điện thoại tài xế:</strong> {{ dataBusDetail.value.data.driverPhone }}</p>
              <p class="mb-1"><strong>Biển số xe:</strong> {{ dataBusDetail.value.data.licensePlate }}</p>
              <p class="mb-1"><strong>Số ghế:</strong> {{ dataBusDetail.value.data.seatNumber }}</p>
              <p class="mb-1">
                <strong>Sĩ số:</strong> {{ dataBusEnrollment.value.data.totalRecords }}/{{
                  dataBusDetail.value.data.seatNumber
                }}
              </p>
              <p class="mb-0">
                <strong>Trạng thái:</strong>
                <span :class="dataBusDetail.value.data.status === 1 ? 'text-success' : 'text-danger'">
                  {{ dataBusDetail.value.data.status === 1 ? 'Hoạt động' : 'Không hoạt động' }}
                </span>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="action-buttons">
      <button class="btn btn-primary" @click="openModalAddBusEnrollment" :disabled="isSemesterDisabled">
        Thêm học sinh
      </button>
    </div>
    <!-- Invoice table -->
    <div class="card mb-6">
      <div class="table-responsive mb-4">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="card-header flex-column flex-md-row py-0 mt-6 mt-md-0">
            <div class="header-container p-3">
              <div class="title-section mb-3">
                <h5 class="card-title mb-0 text-nowrap">Danh sách học sinh đi xe bus</h5>
              </div>
              <div class="bottom-section d-flex align-items-center justify-content-between">
                <!-- Semester Selector -->
                <div class="semester-selector me-3">
                  <select class="form-select" v-model="selectedSemester" @change="handleSemesterChange">
                    <option v-for="semester in semesterData" :key="semester.id" :value="semester.id">
                      {{ semester.semesterName + ' ' + semester.schoolYearName }}
                    </option>
                  </select>
                </div>
                <div v-if="(checkHasBusSupervisor) && (!isSemesterDisabled)" class="supervisor-section d-flex">
                  <p>
                    Lớp chưa có người giám sát, hãy
                    <a
                      class="text-danger mb-0 me-2"
                      @click="openModalAssignBusSupervisor"
                      href="javascript:void(0);"
                    >
                      thêm ngay!
                    </a>
                  </p>
                </div>
              </div>
            </div>

            <div id="DataTables_Table_0_filter" class="dataTables_filter d-flex align-items-center">
              <label class="mb-0 me-2">
                <input
                  type="search"
                  class="form-control"
                  placeholder="Tìm kiếm học sinh"
                  v-model="pupilSearchKey"
                  aria-controls="DataTables_Table_0"
                  @keyup.enter="handleSearchPupil"
                />
              </label>
              <button @click="handleSearchPupil" class="btn btn-primary">Tìm kiếm</button>
            </div>
          </div>
          <div v-if="checkConditionCopy">
            <label>Sao chép dữ liệu học sinh sang kỳ tiếp theo</label>
          <a
            @click="openUpSemesterBus"
            href="javascript:void(0);" 
          >
            Thực hiện
          </a>
          </div>  
          <table
            class="table datatables-customer-order border-top dataTable no-footer dtr-column collapsed"
            id="DataTables_Table_0"
            aria-describedby="DataTables_Table_0_info"
            style="width: 798px"
          >
            <thead>
              <tr>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1">STT</th>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1">
                  Họ và tên
                </th>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1">Mã số</th>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1">Kì học</th>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1">
                  Điểm dừng
                </th>
                <th class="text-md-center sorting_disabled" rowspan="1" colspan="1">Hành động</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="(entry, index) in dataBusEnrollment.value.data.data"
                :key="index"
                :class="[
                  { odd: index % 2 === 0, even: index % 2 !== 0 },
                  entry.pupilName === null ? 'supervisor-row' : '',
                ]"
              >
                <td>
                  <span>{{ index + 1 }}</span>
                </td>
                <td>
                  <span class="text-nowrap">
                    {{ entry.pupilName || entry.busSupervisorName }}
                    <span v-if="entry.pupilName === null" class="supervisor-star">*</span>
                  </span>
                </td>
                <td>
                  <span class="text-nowrap">
                    {{ entry.pupilCode || entry.busSupervisorCode }}
                  </span>
                </td>
                <td>
                  <span>{{ entry.semesterName }}</span>
                </td>
                <td>
                  <span>{{ entry.busStopName }}</span>
                </td>
                <td class="dtr-hidden">
                  <button class="btn btn-icon" @click="openModalDeleteBusEnrollment(entry.id)">
                    <i class="bx bx-trash me-2"></i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
          <div class="row mx-6">
            <div v-if="!checkHasBusSupervisor" class="col-md-12 col-xxl-6 text-center text-xl-start pb-2 pb-xxl-0 pe-0">
              (*) Người giám sát học sinh
            </div>
            <div class="col-md-12 col-xxl-6 text-center text-xl-start pb-2 pb-xxl-0 pe-0"></div>
            <div class="col-md-12 col-xxl-6" v-if="dataBusEnrollment.value.data.totalPages > 1">
              <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_0_paginate">
                <Pagination
                  :total-records="dataBusEnrollment.value.data.totalRecords"
                  :page-size="dataBusEnrollment.value.data.pageSize"
                  :current-page="dataBusEnrollment.value.data.pageNumber"
                  @update:currentPage="currentPage = $event"
                  @page-changed="handlePageChange"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <AssignPupilToBus
      :showAssignPupilModal="isAddBusEnrollmentModalVisible"
      :pupils="dataPupils"
      :busId="selectedBusId"
      :dataBusStop="dataBusStop"
      :semesterId="selectedSemester"
      :apiUrl="apiUrl"
      @confirm="handleConfirmBusEnrollment"
      @update:showModal="isAddBusEnrollmentModalVisible = $event"
    />
    <AssignBusSupervisorToBus
      :showAssignSupervisorModal="isAssignBusSupervisor"
      :supervisors="dataBusSupervisor"
      :busId="selectedBusId"
      :dataBusStop="dataBusStop"
      :semesterId="selectedSemester"
      :apiUrl="apiUrl"
      @confirm="handleConfirmBusEnrollment"
      @update:showModal="isAssignBusSupervisor = $event"
    />
    <DeletePupilFromBus
      :showDeleteModal="isDeleteBusEnrollmentModalVisible"
      @update:showDeleteModal="isDeleteBusEnrollmentModalVisible = $event"
      @confirm="handleConfirmDelete"
    />
    <CreateAndUpdateBus
      :showModalAdd="isAddBusModalVisible"
      :errorsBus="busErrors"
      :isUpdateMode="isEditBusMode"
      :dataBusDetail="dataBusDetail"
      @confirm="handleConfirm"
      @update:showModal="isAddBusModalVisible = $event"
      @closeUpdateModal="isEditBusMode = $event"
    />
    <UpSemesterBus
      :showUpSemester="showUpSemesterBus"
      :pupilInBus="dataBusEnrollment.value.data.data"
      :busId="selectedBusId"
      :semesterId="nextSemester"
      :apiUrl="apiUrl"
      @confirm="handleConfirmBusEnrollment"
      @update:showModal="showUpSemesterBus = $event"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, onMounted, watch, computed } from 'vue';
import CreateAndUpdateBus from '@/components/modals/bus/CreateAndUpdateBus.vue';
import DeleteBus from '@/components/modals/bus/DeleteBus.vue';
import { useBusDetailComposable } from '@/composables/bus';
import Pagination from '@/components/common/Pagination.vue';
import { useRoute } from 'vue-router';
import { useBusEnrollmentComposable } from '@/composables/bus-enrollment';
import AssignPupilToBus from '@/components/modals/bus-enrollment/AssignPupilToBus.vue';
import DeletePupilFromBus from '@/components/modals/bus-enrollment/DeletePupilFromBus.vue';
import AssignBusSupervisorToBus from '@/components/modals/bus-enrollment/AssignBusSupervisorToBus.vue';
import UpSemesterBus from '@/components/modals/bus-enrollment/UpSemesterBus.vue';
import { useBusStopComposable } from '@/composables/bus-stop';

export default defineComponent({
  name: 'BusDetailComponent',
  components: {
    CreateAndUpdateBus,
    DeleteBus,
    Pagination,
    AssignPupilToBus,
    DeletePupilFromBus,
    AssignBusSupervisorToBus,
    UpSemesterBus,
  },
  props: {},
  setup(props, { emit }) {
    const { dataBusDetail, handleGetBusDetail } = useBusDetailComposable();
    const {
      dataBusEnrollment,
      isAddBusEnrollmentModalVisible,
      isDeleteBusEnrollmentModalVisible,
      resetPage,
      apiUrl,
      showUpSemesterBus,
      checkHasBusSupervisor,
      currentPage,
      dataPupils,
      semesterData,
      selectedBusId,
      selectedSemester,
      isAssignBusSupervisor,
      dataBusSupervisor,
      resetPageForRedirect,
      dataBusStop,
      currentSemester,
      nextSemester,
      isSemesterDisabled,
      checkConditionCopy,
      checkAllowCopy,
      openUpSemesterBus,
      showDetail,
      checkFinish,
      handleSemesterChange,
      openModalDeleteBusEnrollment,
      openModalAssignBusSupervisor,
      handleConfirmDelete,
      handleConfirmBusEnrollment,
      openModalAddBusEnrollment,
      handleGetAllBusEnrollments,
      handlePageChange,
      handleFetchSemester,
      checkSemesterStatus,
      semesterNextYear,
    } = useBusEnrollmentComposable();
    const { isAddBusModalVisible, busErrors, isEditBusMode, handleConfirm, openModalEditBus,resetPageDetail } =
      useBusDetailComposable();
   
    watch(resetPage, newValue =>  {
      if (newValue) {
        handleGetAllBusEnrollments(busId);       
        emit('refreshPage');
        resetPage.value = false;
      }
    });

    watch(resetPageDetail, newValue =>  {
      if (newValue) {   
        emit('refreshPage');
        resetPageDetail.value = false;
      }
    });
    const route = useRoute();
    const busId: number = parseInt(route.query.bid as string);
    onMounted(async () => {
      await handleGetBusDetail(busId);
      await handleFetchSemester();
      await handleGetAllBusEnrollments(busId);
    });

    watch(
      selectedSemester,
      newValue => {
        
        if (currentSemester.value) {
          if(nextSemester.value){
            isSemesterDisabled.value =
            Number(selectedSemester.value) !== currentSemester.value &&
            Number(selectedSemester.value) !== nextSemester.value;
          }else{
            isSemesterDisabled.value =
            Number(selectedSemester.value) !== currentSemester.value &&
            Number(selectedSemester.value) !== semesterNextYear.value;
          }
          if(semesterNextYear.value){
            checkConditionCopy.value = false;
          }else{
            checkConditionCopy.value = Number(selectedSemester.value) === currentSemester.value;
          }           
        } else {
          isSemesterDisabled.value = false;
        }
      },
      { immediate: true },
    );
    return {
      currentPage,
      dataBusEnrollment,
      dataBusDetail,
      dataPupils,
      selectedBusId,
      apiUrl,
      semesterData,
      isAssignBusSupervisor,
      isAddBusEnrollmentModalVisible,
      selectedSemester,
      isDeleteBusEnrollmentModalVisible,
      dataBusSupervisor,
      checkHasBusSupervisor,
      isEditBusMode,
      isAddBusModalVisible,
      busErrors,
      dataBusStop,
      currentSemester,
      showUpSemesterBus,
      nextSemester,
      isSemesterDisabled,
      checkConditionCopy,
      checkAllowCopy,
      checkSemesterStatus,
      openUpSemesterBus,
      checkFinish,
      handleSemesterChange,
      showDetail,
      handleConfirmBusEnrollment,
      openModalEditBus,
      handlePageChange,
      handleConfirm,
      handleConfirmDelete,
      openModalDeleteBusEnrollment,
      openModalAddBusEnrollment,
      openModalAssignBusSupervisor,
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
.card-info {
  background-color: #f8f9fa;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.card-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #495057;
}

.d-flex {
  display: flex;
  align-items: center;
}

.gap-1 {
  gap: 8px;
}

.text-primary {
  color: #007bff;
}

.mb-0 {
  margin-bottom: 0;
}

.text-truncate {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

p {
  margin-top: 10px;
}

strong {
  font-weight: 600;
  color: #343a40;
}

span.text-success {
  color: #28a745;
}

span.text-danger {
  color: #dc3545;
}

.card-info p {
  font-size: 0.9rem;
  line-height: 1.6;
}

.card-info .avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: #007bff;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 10px;
}

.card-info .avatar-initial {
  font-size: 1.25rem;
  color: #fff;
}

.supervisor-row {
  background-color: #f9f9f9;
  font-weight: bold;
}

.supervisor-star {
  color: red;
  font-size: 16px;
  margin-left: 5px;
}

.bottom-section {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  padding-top: 15px;
}

.semester-selector select {
  width: 200px;
  padding: 8px;
  font-size: 1rem;
  border-radius: 5px;
  transition: border-color 0.3s;
}

.semester-selector select:focus {
  border-color: #007bff;
}

.action-buttons button {
  padding: 8px 16px;
  font-size: 1rem;
  font-weight: bold;
}

.supervisor-section p {
  color: #dc3545;
  font-size: 0.9rem;
  font-weight: 500;
  margin-right: 8px;
}

.supervisor-section button {
  font-size: 0.9rem;
  font-weight: bold;
}
.bus-detail-container {
  margin: auto;
}

.card {
  border: none;
  border-radius: 8px;
  background-color: #ffffff;
}

.shadow-sm {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.icon-container {
  display: flex;
}

.avatar-initial {
  width: 50px;
  height: 50px;
}

.card-title {
  font-weight: bold;
}

.edit-button {
  float: right;
  font-size: 0.9rem;
  padding: 6px 12px;
}

.card-info {
  margin-top: 20px;
}

.info-details p {
  margin-bottom: 0.5rem;
  color: #555;
}

.text-success {
  color: #28a745;
}

.text-danger {
  color: #dc3545;
}

.text-primary {
  color: #007bff;
}
.action-buttons {
  display: flex;
  gap: 10px;
  justify-content: flex-start;
  margin-top: 20px;
}

.isDisabled {
  pointer-events: none;
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
