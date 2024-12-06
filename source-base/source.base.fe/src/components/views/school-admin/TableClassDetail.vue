<template>
  <div class="col-xl-12 col-lg-12 col-md-12 order-0 order-md-1" v-if="dataMember">
    <!-- Go Back Button -->
    <button class="btn btn-sm mb-3" @click="gotoClassManage">Quay lại</button>

    <!-- Overview Cards Row -->
    <div class="row text-nowrap mb-4">
      <div class="col-md-6 mb-4">
        <div class="card h-100 shadow-sm">
          <div class="card-body text-center">
            <div class="card-icon mb-3 mx-auto">
              <div class="avatar">
                <div class="avatar-initial rounded-circle bg-label-primary">
                  <i class="bx bx-group bx-lg"></i>
                </div>
              </div>
            </div>
            <h5 class="card-title mb-0">
              Thông tin lớp học
              <span class="text-primary"></span>
            </h5>
          </div>
        </div>
      </div>
      <div class="col-md-6 mb-4">
        <div class="card h-100 shadow-sm">
          <div class="card-body text-center">
            <div class="card-icon mb-3 mx-auto">
              <div class="avatar">
                <div class="avatar-initial rounded-circle bg-label-success">
                  <i class="bx bx-calendar-event bx-lg"></i>
                </div>
              </div>
            </div>
            <h5 class="card-title mb-0">Sự kiện gần đây</h5>
          </div>
        </div>
      </div>
    </div>

    <!-- Action Buttons -->
    <div class="mb-8">
      <button class="btn btn-primary" @click="openModalPupil(classId)" :disabled="isSemesterDisabled">
        Thêm học sinh
      </button>
    </div>
    <button class="btn btn-primary" @click="openUpLevelModal()" v-if="checkUpClass && !checkGraduate">Lên lớp</button>
    <button class="btn btn-primary" @click="openGraduteModal()" v-if="checkUpClass && checkGraduate">
      Kết thúc khóa học
    </button>
    <div v-if="checkHasTeacher && !isSemesterDisabled" class="supervisor-section d-flex">
      <p>
        Lớp chưa có giáo viên, hãy
        <a class="text-danger mb-0 me-2" @click="openModalAssignTeacher(classId)" href="javascript:void(0);">
          thêm ngay!
        </a>
      </p>
    </div>
    <!-- Student List Table -->
    <div class="card shadow-sm" v-if="dataMember.value.data.data">
      <div class="card-header d-flex flex-column flex-md-row justify-content-between align-items-center">
        <h5 class="card-title mb-2 mb-md-0">Danh sách học sinh</h5>
        <div class="d-flex align-items-center flex-wrap gap-2">
          <label for="semesterSelect">Chọn kỳ học:</label>
          <select
            id="semesterSelect"
            v-model="selectedSemester"
            @change="filterChangeSemester"
            class="form-select w-auto"
          >
            <option v-for="semester in semesterData" :key="semester.id" :value="semester.id">
              {{ semester.semesterName }} - {{ semester.schoolYearName }}
            </option>
          </select>
          <input type="search" class="form-control w-auto" placeholder="Tìm kiếm câu lạc bộ" @change="searchMember" />
          <button @click="searchMember" class="btn btn-primary">Tìm kiếm</button>
        </div>
      </div>
      <div class="table-responsive">
        <div v-if="checkConditionCopy">
          <label>Sao chép dữ liệu học sinh sang kỳ tiếp theo</label>
          <a @click="openUpLevelModal" href="javascript:void(0);"> Thực hiện </a>
        </div>
        <table class="table table-striped">
          <thead class="table-light">
            <tr>
              <th>#</th>
              <th>Ảnh</th>
              <th>Họ và tên</th>
              <th>Chức vụ</th>
              <th>Mã số</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="dataMember.value.data.data.length === 0">
              <td colspan="6" class="text-center">Không có dữ liệu</td>
            </tr>
            <tr v-else v-for="(pupil, index) in dataMember.value.data.data" :key="pupil.id">
              <td>{{ index + 1 }}</td>
              <td>
                <img :src="`${apiUrl}${pupil.image}`" alt="image" class="img-thumbnail" width="30" height="30" />
              </td>
              <td>{{ pupil.teacherCode ? pupil.teacherName : pupil.pupilName }}</td>
              <td>{{ pupil.teacherCode ? 'Giáo viên' : 'Học sinh' }}</td>
              <td>{{ pupil.teacherCode ? pupil.teacherCode : pupil.pupilCode }}</td>
              <td>
                <div class="d-inline-block">
                  <a
                    href="javascript:;"
                    class="btn btn-icon dropdown-toggle hide-arrow me-1"
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                  >
                    <i class="bx bx-dots-vertical-rounded bx-md"></i>
                  </a>
                  <ul class="dropdown-menu dropdown-menu-end m-0">
                    <div class="dropdown-divider"></div>
                    <li>
                      <a
                        href="javascript:;"
                        class="dropdown-item text-danger delete-record"
                        @click="openModalDeleteMemberInClass(pupil.id)"
                      >
                        Xóa
                      </a>
                    </li>
                  </ul>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination -->
      <div
        v-if="dataMember.value.data.totalPages > 1"
        class="card-footer d-flex justify-content-between align-items-center"
      >
        <span> Danh sách {{ dataMember.value.data.totalRecords }} học sinh </span>
        <Pagination
          :total-records="dataMember.value.data.totalRecords"
          :page-size="dataMember.value.data.pageSize"
          :current-page="dataMember.value.data.pageNumber"
          @update:currentPage="currentPage = $event"
          @page-changed="handlePageChange"
        />
      </div>
    </div>

    <!-- Modals -->
    <AssignTeacher
      :modelValue="showAssignTeacherModal"
      :isUpdate="isUpdate"
      :apiUrl="apiUrl"
      :teacherList="teachers"
      :classId="classId"
      :semesterId="selectedSemester"
      @assignTeacher="handleModalConfirmAssignTeacher"
      @update:showModal="showAssignTeacherModal = $event"
    />
    <AssignPupil
      :showAssignPupilModal="showAssignPupilModal"
      :pupils="pupils"
      :classId="classId"
      :apiUrl="apiUrl"
      :semesterId="selectedSemester"
      @confirm="handleModalConfirmAssignPupils"
      @update:showModal="showAssignPupilModal = $event"
    />
    <DeleteMember
      :isShowModalDelete="showModalDeleteMember"
      :memberId="selectedMemberId"
      @confirm="handleTeacherRemove"
      @update:isShowModalDelete="showModalDeleteMember = $event"
    />
    <ConfirmUpLevel
      :showUpLevel="showUpLevel"
      :pupilInClass="dataMember.value.data.data"
      :pupilInClassNextSemester="dataMemberInNextSemester.value"
      :classId="classId"
      :checkUpClass="checkUpClass"
      :listClass="filteredClasses"
      :semesterId="nextSemester ? nextSemester : semesterNextYear"
      :apiUrl="apiUrl"
      @confirm="handleUpLevel"
      @update:showModal="showUpLevel = $event"
    />
    <ConfirmGraduate
      :showUpLevel="showModalGradute"
      :pupilInClass="dataMember.value.data.data"
      :apiUrl="apiUrl"
      @confirm="handleConfirmPupilsGradute"
      @update:showModal="showModalGradute = $event"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, PropType, ref, watch } from 'vue';
import { ResponseGetClassDetailInterface } from '@/types/model/class';
import { Gender } from '@/constants/enums/gender';
import { useAdminSchoolComposable } from '@/composables/admin-school';
import { useRoute } from 'vue-router';
import DeleteMember from '@/components/modals/class-enrollment/DeleteMember.vue';
import AssignPupil from '@/components/modals/school-admin/AssignPupil.vue';
import AssignTeacher from '@/components/modals/school-admin/AssignTeacher.vue';
import Pagination from '@/components/common/Pagination.vue';
import ConfirmUpLevel from '@/components/modals/class-enrollment/ConfirmUpLevel.vue';
import ConfirmGraduate from '@/components/modals/class-enrollment/ConfirmGraduate.vue';
import { RequestDeleteTeacherInterface, RequestUpdateAssignTeacherInterface } from '@/types/model/class-enrollment';
import { AnyARecord } from 'node:dns';
export default defineComponent({
  name: 'TableClassDetailComponent',
  components: {
    Pagination,
    DeleteMember,
    AssignPupil,
    AssignTeacher,
    ConfirmUpLevel,
    ConfirmGraduate,
  },
  props: {},
  setup(props, { emit }) {
    const apiUrl = import.meta.env.VITE_APP_API_URL;
    const gender = Gender;
    const {
      handleGetListMember,
      dataMember,
      handleFetchSemester,
      currentPage,
      selectedMemberId,
      selectedSemester,
      semesterData,
      handlePageChange,
      checkFinish,
      checkSemesterStatus,
      showAssignPupilModal,
      closeModalPupil,
      openGraduteModal,
      showModalGradute,
      showUpLevel,
      showAssignTeacherModal,
      isUpdate,
      teachers,
      filteredClasses,
      filterChangeSemester,
      handleFetchAssignPupils,
      searchMember,
      checkHasTeacher,
      hanldeUpdatessignTeacher,
      handleFetchAssignTeacherToClass,
      selectedClassEnrollment,
      handleFetchClass,
      requestGetClass,
      gotoClassManage,
      pupils,
      openUpLevelModal,
      handleRedirectToDetail,
      handleRemoveMemberFromClass,
      openModalDeleteMemberInClass,
      handleConfirmPupilsGradute,
      handleFetchRemoveTeacherFromClass,
      openModalAssignTeacher,
      openModalPupil,
      refeshPage,
      checkGraduate,
      showModalDeleteMember,
      requestRemoveTeacher,
      isSemesterDisabled,
      currentSemester,
      nextSemester,
      checkConditionCopy,
      checkUpClass,
      semesterNextYear,
      showTeacherSwapModal,
      checkFinishBlock,
      dataMemberInNextSemester,
    } = useAdminSchoolComposable();

    const route = useRoute();
    const classId: number = parseInt(route.query.classId as string);
    watch(refeshPage, async newValue => {
      if (newValue) {
        refeshPage.value = false;
        await handleGetListMember(classId);
        await checkFinishBlock(classId);
      }
    });

    watch(
      selectedSemester,
      newValue => {
        if (currentSemester.value) {
          checkFinishBlock(classId);
          if(nextSemester.value){
            isSemesterDisabled.value =
            Number(selectedSemester.value) !== currentSemester.value &&
            Number(selectedSemester.value) !== nextSemester.value;
          }else{
            isSemesterDisabled.value =
            Number(selectedSemester.value) !== currentSemester.value &&
            Number(selectedSemester.value) !== semesterNextYear.value;
          }
          
          checkConditionCopy.value =
            Number(selectedSemester.value) === currentSemester.value && nextSemester.value != null;
        } else {
          isSemesterDisabled.value = false;
        }
      },
      { immediate: true },
    );

    onMounted(async () => {
      await handleFetchSemester();
      await handleGetListMember(classId);
      await checkFinishBlock(classId);
      checkConditionCopy.value = Number(selectedSemester.value) === currentSemester.value && nextSemester.value != null;
    });

    const handleTeacherRemove = async (request: number) => {
      await handleRemoveMemberFromClass(request);
      await handleFetchClass(requestGetClass);
    };

    const handleUpLevel = async (teacher: any, pupil: any[]) => {
      if (teacher.length > 0) {
        await handleFetchAssignTeacherToClass(teacher);
      }
      if (pupil.length > 0) {
        await handleFetchAssignPupils(pupil);
      }
      showUpLevel.value = false;
    };

    const handleModalConfirmAssignTeacher = async (request: {
      classId: number;
      teacherId: number;
      semesterId: number;
    }) => {
      try {
        if (isUpdate.value) {
          const requestUpdate: RequestUpdateAssignTeacherInterface = {
            classEnrollmentId: selectedClassEnrollment.value,
            classId: request.classId,
            teacherId: request.teacherId,
            semesterId: request.semesterId,
          };
          await hanldeUpdatessignTeacher(requestUpdate);
        } else {
          await handleFetchAssignTeacherToClass(request);
        }
      } catch (error) {
        console.error('Error in assigning teacher:', error);
      }
    };

    const handleModalConfirmAssignPupils = async (request: any[]) => {
      try {
        await handleFetchAssignPupils(request);
        closeModalPupil();
      } catch (error) {
        console.error('Error in assigning teacher:', error);
      }
    };

    return {
      apiUrl,
      gender,
      showModalGradute,
      currentPage,
      dataMember,
      selectedSemester,
      semesterData,
      semesterNextYear,
      selectedMemberId,
      showUpLevel,
      searchMember,
      dataMemberInNextSemester,
      handleConfirmPupilsGradute,
      checkConditionCopy,
      checkGraduate,
      checkUpClass,
      checkFinishBlock,
      checkFinish,
      handlePageChange,
      handleTeacherRemove,
      showModalDeleteMember,
      isSemesterDisabled,
      showTeacherSwapModal,
      filteredClasses,
      handleUpLevel,
      openGraduteModal,
      openUpLevelModal,
      openModalDeleteMemberInClass,
      openModalAssignTeacher,
      openModalPupil,
      filterChangeSemester,
      handleModalConfirmAssignPupils,
      handleModalConfirmAssignTeacher,
      gotoClassManage,
      nextSemester,
      checkHasTeacher,
      pupils,
      classId,
      showAssignPupilModal,
      showAssignTeacherModal,
      isUpdate,
      teachers,
    };
  },
});
</script>

<style scoped></style>
