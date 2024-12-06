<template>
  <div class="container-fluid club-detail-page">
    <div class="row">
      <!-- Thông tin chi tiết -->
      <div class="col-md-12 mb-4">
        <div class="card h-100 shadow-sm rounded-lg">
          <div class="card-body" v-if="dataClubDetail.value">
            <div class="d-flex align-items-center justify-content-between mb-3">
              <div class="d-flex align-items-center">
                <div class="avatar">
                  <div
                    class="avatar-initial rounded-circle bg-primary d-flex align-items-center justify-content-center"
                  >
                    <i class="bx bx-car bx-lg text-white"></i>
                  </div>
                </div>
                <h5 class="ms-3 mb-0 text-primary">Thông tin câu lạc bộ</h5>
              </div>
              <button class="btn btn-outline-primary" @click="openModalEditClub(dataClubDetail.value.id)">
                Chỉnh sửa
              </button>
            </div>
            <div class="info-details">
              <p class="mb-2"><strong>Tên câu lạc bộ:</strong> {{ dataClubDetail.value.name }}</p>
              <p class="mb-2"><strong>Mô tả:</strong> {{ dataClubDetail.value.description }}</p>
              <p class="mb-0">
                <strong>Trạng thái:</strong>
                <span :class="dataClubDetail.value.status === 1 ? 'text-success' : 'text-danger'">
                  {{ dataClubDetail.value.status === 1 ? 'Hoạt động' : 'Không hoạt động' }}
                </span>
              </p>
            </div>
          </div>
        </div>
      </div>

      <!-- Nút quay lại -->
      <div class="col-12 mb-3">
        <button @click="handleRedirectToManageClub" class="btn btn-info btn-sm">Quay lại</button>
      </div>

      <!-- Tìm kiếm -->
      <div class="col-12 mb-3 d-flex justify-content-between align-items-center">
        <div class="semester-selector me-3">
          <select class="form-select" v-model="selectedSemester" @change="handleSemesterChange">
            <option v-for="semester in semesterData" :key="semester.id" :value="semester.id">
              {{ semester.semesterName + ' ' + semester.schoolYearName }}
            </option>
          </select>
        </div>
        <!-- Phần tìm kiếm nằm ở góc trái -->
        <div class="col-6 search-bar d-flex align-items-center">
          <input
            type="search"
            class="form-control me-2"
            placeholder="Tìm kiếm thành viên"
            v-model="clubEnrollmentSearchKey"
            @keyup.enter="handleSearchClubEnrollment"
          />
          <button @click="handleSearchClubEnrollment" class="btn btn-primary">Tìm kiếm</button>
        </div>

        <!-- Hai button nằm ở góc phải -->
        <div class="col-6 d-flex justify-content-end">
          <div class="action-buttons me-2">
            <button class="btn btn-primary" @click="openAddClubEnrollmentModal" :disabled="isSemesterDisabled">
              Thêm học sinh
            </button>
          </div>
          <div v-if="checkHasTeacher" class="action-buttons">
            <button class="btn btn-primary" @click="openAddAssignTeacherModal" :disabled="isSemesterDisabled">
              Phân công giáo viên
            </button>
          </div>
          <div class="action-buttons">
            <button class="btn btn-primary" @click="openConfirmRegisterPupilModal" :disabled="isSemesterDisabled">
              Duyệt đơn đăng kí
            </button>
          </div>
        </div>
      </div>

      <!-- Bảng thành viên -->
      <div class="col-12">
        <div class="card">
          <h5 class="card-header">Danh sách thành viên</h5>
          <div v-if="checkConditionCopy">
            <label>Sao chép dữ liệu học sinh sang kỳ tiếp theo</label>
          <a
            @click="openUpSemesterModal"
            href="javascript:void(0);" 
          >
            Thực hiện
          </a>
          </div>  
          <div class="table-responsive text-nowrap">
            <table class="table">
              <thead class="table-primary">
                <tr>
                  <th>#</th>
                  <th>Tên thành viên</th>
                  <th>Mã thành viên</th>
                  <th>Vai trò</th>
                  <th>Ngày tham gia</th>
                  <th>Hành động</th>
                </tr>
              </thead>
              <tbody
                class="table-border-bottom-0"
                v-if="dataClubEnrollment.value.data.data && dataClubEnrollment.value.data.data.length"
              >
                <tr
                  v-for="(member, index) in dataClubEnrollment.value.data.data"
                  :key="member.id"
                  :class="{ 'highlight-row': member.teacherId }"
                >
                  <td>{{ index + 1 }}</td>
                  <td>
                    {{ member.teacherId ? member.teacherName : member.pupilName }}
                  </td>
                  <td>
                    {{ member.teacherId ? member.teacherUsername : member.pupilUsername }}
                  </td>
                  <td>{{ member.teacherId ? 'Người quản lý' : 'Thành viên' }}</td>
                  <td>{{ formatDate(member.createdDate) }}</td>
                  <td>
                    <a
                      class="dropdown-item text-danger"
                      href="javascript:void(0);"
                      @click="openDeleteClubEnrollmentModal(member.id)"
                    >
                      <i class="bx bx-trash me-2"></i>
                    </a>
                  </td>
                </tr>
              </tbody>
              <tbody v-else>
                <tr>
                  <td colspan="6" class="text-center text-muted py-3">
                    <i class="bx bx-info-circle me-1"></i> Câu lạc bộ chưa có thành viên tham gia
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div v-if="dataClubEnrollment.value.data.totalPages > 1" class="pagination-container d-flex justify-content-end">
          <pagination
            :total-records="dataClubEnrollment.value.data.totalRecords"
            :page-size="dataClubEnrollment.value.data.pageSize"
            :current-page="dataClubEnrollment.value.data.pageNumber"
            @update:currentPage="currentPage = $event"
            @page-changed="handlePageChange"
          />
        </div>
      </div>
    </div>
    <CreateAndUpdateClub
      :showModalAdd="isAddClubModalVisible"
      :clubErrors="clubErrors"
      :isUpdateMode="isEditClubMode"
      :dataClubDetail="dataClubDetail"
      @confirm="handleConfirm"
      @update:showModal="isAddClubModalVisible = $event"
      @closeUpdateModal="isEditClubMode = $event"
    />
    <AssignPupilToClub
      :showAssignPupilModal="isAddClubEnrollmentModalVisible"
      :pupils="dataPupilsNotInClub"
      :clubId="selectedClubId"
      :semesterId="selectedSemester"
      :apiUrl="apiUrl"
      @confirm="handleCreateClubEnrollment"
      @update:showModal="isAddClubEnrollmentModalVisible = $event"
    />
    <AssignTeacherToClub
      :showAssignTeacherModal="isAssignTeacherModalVisible"
      :teachers="dataTeachersNotInClub"
      :clubId="selectedClubId"
      :semesterId="selectedSemester"
      :apiUrl="apiUrl"
      @confirm="handleCreateClubEnrollment"
      @update:showModal="isAssignTeacherModalVisible = $event"
    />
    <DeleteMemberClub
      :showDeleteModal="isDeleteClubEnrollmentModalVisible"
      @update:showDeleteModal="isDeleteClubEnrollmentModalVisible = $event"
      @confirm="handleDeleteClubEnrollment"
    />
    <ConfirmPupilToClub
      :showConfirmPupilToClub="showConfirmPupilToClub"
      :pupilRegisterClub="dataPupilRegisterClub"
      :clubId="selectedClubId"
      :semesterId="selectedSemester"
      :apiUrl="apiUrl"
      @confirm="handleUpdateClubEnrollment"
      @update:showModal="showConfirmPupilToClub = $event"
    />    
    <UpSemester
      :showUpSemester="showUpSemester"
      :pupilInClub="dataClubEnrollment.value.data.data"
      :pupilInClubNextSemester="dataCopyClub.value"
      :clubId="selectedClubId"
      :semesterId="nextSemester?nextSemester:semesterNextYear"
      :apiUrl="apiUrl"
      @confirm="handleCreateClubEnrollment"
      @update:showModal="showUpSemester = $event"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, watch } from 'vue';
import Pagination from '@/components/common/Pagination.vue';
import AssignPupilToClub from '@/components/modals/club-enrollment/AssignPupilToClub.vue';
import DeleteMemberClub from '@/components/modals/club-enrollment/DeleteMemberClub.vue';
import AssignTeacherToClub from '@/components/modals/club-enrollment/AssignTeacherToClub.vue';
import { useClubEnrollmentComposable } from '@/composables/club-enrollment';
import UpSemester from '@/components/modals/club-enrollment/UpSemester.vue';
import CreateAndUpdateClub from '@/components/modals/club/CreateAndUpdateClub.vue';
import ConfirmPupilToClub from '@/components/modals/club-enrollment/ConfirmPupilToClub.vue';
import { useRoute } from 'vue-router';
import { useClubDetailComposable } from '@/composables/club';

export default defineComponent({
  name: 'ClubEnrollment',
  components: {
    Pagination,
    AssignPupilToClub,
    DeleteMemberClub,
    AssignTeacherToClub,
    CreateAndUpdateClub,
    ConfirmPupilToClub,
    UpSemester,
  },
  setup() {
    const {
      dataClubEnrollment,
      dataTeachersNotInClub,
      dataPupilsNotInClub,
      isAddClubEnrollmentModalVisible,
      isEditClubEnrollmentMode,
      isDeleteClubEnrollmentModalVisible,
      apiUrl,
      currentPage,
      selectedSemester,
      clubEnrollmentSearchKey,
      semesterData,
      selectedClubId,
      resetPageClubEnrollment,
      isAssignTeacherModalVisible,
      currentSemester,
      nextSemester,
      isSemesterDisabled,
      showConfirmPupilToClub,
      dataPupilRegisterClub,
      showUpSemester,
      checkConditionCopy,
      dataCopyClub,
      openUpSemesterModal,
      openConfirmRegisterPupilModal,
      checkSemesterStatus,
      openAddAssignTeacherModal,
      handleFetchSemester,
      handleSemesterChange,
      checkFinish,
      handleRedirectToManageClub,
      handleGetClubEnrollments,
      handleGetTeachersNotInClub,
      handleGetPupilsNotInClub,
      handleCreateClubEnrollment,
      handleUpdateClubEnrollment,
      handleDeleteClubEnrollment,
      openAddClubEnrollmentModal,
      openEditClubEnrollmentModal,
      openDeleteClubEnrollmentModal,
      handleSearchClubEnrollment,
      handlePageChange,
      resetModal,
      clubEnrollmentErrors,
      checkHasTeacher,
      semesterNextYear
    } = useClubEnrollmentComposable();
    const {
      handleGetClubDetail,
      dataClubDetail,
      isAddClubModalVisible,
      resetPage,
      clubErrors,
      isEditClubMode,
      openModalEditClub,
      handleConfirm,
    } = useClubDetailComposable();
    const route = useRoute();
    const clubId: number = parseInt(route.query.clubId as string);

    watch(resetPageClubEnrollment, async newValue => {
      if (newValue) {
        await handleGetClubEnrollments(clubId);
        resetPageClubEnrollment.value = false;
      }
    });

    watch(resetPage, async newValue => {
      if (newValue) {
        await handleGetClubDetail(clubId);
        resetPage.value = false;
      }
    });

    onMounted(async () => {
      await handleFetchSemester();
      await handleGetClubDetail(clubId);
      await handleGetClubEnrollments(clubId);
    });

    const formatDate = (dateString: string) => {
      return new Date(dateString).toLocaleDateString();
    };

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
      semesterData,
      dataClubEnrollment,
      apiUrl,
      dataClubDetail,
      currentPage,
      isAddClubModalVisible,
      clubErrors,
      clubEnrollmentSearchKey,
      isAddClubEnrollmentModalVisible,
      isDeleteClubEnrollmentModalVisible,
      isEditClubMode,
      selectedSemester,
      selectedClubId,
      dataPupilsNotInClub,
      isAssignTeacherModalVisible,
      dataTeachersNotInClub,
      currentSemester,
      nextSemester,
      isSemesterDisabled,
      showConfirmPupilToClub,
      dataPupilRegisterClub,
      showUpSemester,
      checkConditionCopy,
      checkHasTeacher,
      dataCopyClub,
      semesterNextYear,
      openUpSemesterModal,
      checkFinish,
      handleUpdateClubEnrollment,
      openConfirmRegisterPupilModal,
      checkSemesterStatus,
      openAddAssignTeacherModal,
      handleSearchClubEnrollment,
      openAddClubEnrollmentModal,
      handleDeleteClubEnrollment,
      handleCreateClubEnrollment,
      openDeleteClubEnrollmentModal,
      handleSemesterChange,
      handleConfirm,
      formatDate,
      openModalEditClub,
      handlePageChange,
      handleRedirectToManageClub,
    };
  },
});
</script>

<style scoped>
.highlight-row {
  background-color: #f5f5f5;
  font-weight: bold;
}

.club-detail-page {
  font-family: 'Arial', sans-serif;
}

.info-details {
  background-color: #f9f9f9;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 16px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.info-details p {
  margin-bottom: 8px;
  line-height: 1.5;
}

.search-bar input {
  flex: 1;
}

.table-striped thead {
  background-color: #f1f8ff;
}

.highlight-row {
  background-color: #eef5ff !important;
}

.dropdown-menu .delete-record {
  color: #dc3545;
}

.search-bar {
  max-width: 400px;
}

.action-buttons button {
  margin-left: 10px;
}
</style>
