<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <BusSidebarComponent />
    <div class="d-flex justify-content-between align-items-center mb-3 mt-5 row">
      <div class="col-12 col-md-6 align-items-center">
        <h4 class="mb-1 mt-3">Hồ sơ điểm danh</h4>
        <p class="mb-6">Hồ sơ điểm danh của 7 ngày gần nhất. Vui lòng nhập ngày để tìm kiếm thêm hồ sơ</p>
      </div>
      <div class="col-12 col-md-6 d-flex flex-column align-items-md">
        <label for="attendance-date" class="form-label">Chọn ngày:</label>
        <input
          style="background-color: white"
          type="date"
          id="attendance-date"
          v-model="selectedDate"
          class="form-control"
          @change="fetchAttendanceByDate"
        />
      </div>
    </div>
    <div class="row g-6 mt-3">
      <div class="col-xl-4 col-lg-6 col-md-6">
        <div class="card h-100">
          <div class="row h-100">
            <div class="col-sm-5">
              <div class="d-flex align-items-end h-100 justify-content-center mt-sm-0 mt-4 ps-6">
                <img
                  src="@assets/images/avatars/lady-with-laptop-light.png"
                  class="img-fluid"
                  alt="Image"
                  width="120"
                  data-app-light-img="illustrations/lady-with-laptop-light.png"
                  data-app-dark-img="illustrations/lady-with-laptop-dark.png"
                />
              </div>
            </div>
            <div class="col-sm-7">
              <div class="card-body text-sm-end text-center ps-sm-0">
                <label for="attendance-section" class="form-label">Chọn buổi:</label>
                <select id="attendance-section" v-model="selectedSection" class="form-control mb-4">
                  <option :value="AttendanceSection.Morning">Sáng</option>
                  <option :value="AttendanceSection.Afternoon">Chiều</option>
                </select>
                <button
                  class="btn btn-sm btn-primary mb-4 text-nowrap add-new-role"
                  v-on:click="handleOpenModal(false, new Date())"
                >
                  Tạo hồ sơ điểm danh
                </button>
                <p class="mb-0">
                  Tạo hồ sơ điểm danh, <br />
                  nếu chưa tồn tại.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-xl-4 col-lg-6 col-md-6" v-for="attendance in attendanceList" :key="attendance.entityId">
        <div class="card">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-4">
              <h6 class="fw-normal mb-0 text-body">
                Sĩ số: {{ attendance.classize }} học sinh | Vắng mặt: {{ attendance.absentees }} học sinh
              </h6>
            </div>
            <div class="d-flex justify-content-between align-items-end">
              <div class="role-heading">
                <h5 class="mb-1">Hồ sơ điểm danh: {{ new Date(attendance.date).toLocaleDateString() }}</h5>
                <p class="mb-1">
                  Buổi: <span v-if="attendance.type === AttendanceSection.Morning">Sáng</span
                  ><span v-else-if="attendance.type === AttendanceSection.Afternoon">Chiều</span>
                </p>
                <a
                  href="javascript:;"
                  @click="handleOpenModal(true, attendance.date, attendance.type)"
                  class="role-edit-modal"
                  ><span>Chỉnh sửa</span></a
                >
              </div>
              <a @click="handleViewDetail(attendance.date, attendance.type)"
                ><i class="bx bx-copy bx-md text-muted"></i
              ></a>
            </div>
          </div>
        </div>
      </div>
    </div>
    <CreateAndUpdateAttendanceModal
      :show-modal="isShowModal"
      :isUpdateMode="isUpdate"
      :attendanceRecord="attendanceRecords"
      @update:show-modal="isShowModal = $event"
      @update:is-update-mode="isUpdate = $event"
      @refresh-list="fetchAttendanceByDate"
    />
    <ViewAttendanceDetailModal
      :show-modal="isShowDetailModal"
      :attendanceRecord="attendanceRecords"
      @update:show-modal="isShowDetailModal = $event"
      @edit="handleEditFromDetail"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import BusSidebarComponent from '@/components/views/bus/BusSidebar.vue';
import { useAttendanceComposable } from '@/composables/class-attendance';
import { AttendanceSection, AttendanceType } from '@/constants/enums/attendance';
import CreateAndUpdateAttendanceModal from '@/components/modals/attendance/CreateAndUpdateModalAttendance.vue';
import { getCurrentDateFormatted } from '@/helpers/formatDate.ts';
import ViewAttendanceDetailModal from '@/components/modals/attendance/AttendanceDetailModal.vue';

export default defineComponent({
  name: 'BusAttendance',
  components: {
    ViewAttendanceDetailModal,
    BusSidebarComponent,
    CreateAndUpdateAttendanceModal,
  },
  setup() {
    const {
      busId,
      updateAttendance,
      isShowModal,
      isUpdate,
      attendanceList,
      attendanceRecords,
      prepareCreateAttendance,
      fetchListAttendance,
      fetchAttendanceRecord,
      createAttendance,
      fetchCurrentSemester,
      currentSemester,
    } = useAttendanceComposable();

    const selectedDate = ref(getCurrentDateFormatted()); // Default to today's date
    const selectedSection = ref(AttendanceSection.Morning); // Default to Morning section
    const isShowDetailModal = ref(false);

    const handleOpenModal = async (isUpdateMode: boolean, date: Date, typeID?: number) => {
      if (isUpdateMode) {
        await fetchAttendanceRecord({
          entityId: busId.value,
          session: AttendanceType.Bus,
          type: typeID,
          semesterId: currentSemester.id,
          date: date,
        });
        isUpdate.value = true;
        isShowModal.value = true;
      } else {
        await prepareCreateAttendance({
          entityId: busId.value,
          session: AttendanceType.Bus,
          type: selectedSection.value,
          semesterId: currentSemester.id,
          date: date,
        });
        if (attendanceRecords.value.length > 0) {
          isUpdate.value = false;
          isShowModal.value = true;
        }
      }
    };

    const fetchAttendanceByDate = async () => {
      await fetchListAttendance({
        entityId: busId.value,
        session: AttendanceType.Bus,
        type: AttendanceSection.Morning,
        semesterId: currentSemester.id,
        date: new Date(selectedDate.value),
      });
    };

    const handleViewDetail = async (date: Date, typeID?: number) => {
      await fetchAttendanceRecord({
        entityId: busId.value,
        session: AttendanceType.Bus,
        type: typeID,
        semesterId: currentSemester.id,
        date: date,
      });
      isShowDetailModal.value = true;
    };

    const handleEditFromDetail = () => {
      isShowDetailModal.value = false;
      isUpdate.value = true;
      isShowModal.value = true;
    };

    onMounted(async () => {
      await fetchCurrentSemester();
      await fetchAttendanceByDate();
    });

    return {
      isShowDetailModal,
      handleEditFromDetail,
      handleViewDetail,
      updateAttendance,
      createAttendance,
      attendanceRecords,
      handleOpenModal,
      isShowModal,
      isUpdate,
      attendanceList,
      selectedDate,
      selectedSection,
      fetchAttendanceByDate,
      AttendanceSection,
    };
  },
});
</script>
