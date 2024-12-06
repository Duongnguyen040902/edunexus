<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="card">
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="card-header border-bottom">
            <h4 v-if="nextSemester.id===0" class="card-title mb-0">
              Chưa có kỳ học tiếp theo nên chưa thể đăng ký câu lạc bộ 
            </h4>
            <h4 v-else class="card-title mb-0">
              Đăng kí câu lạc bộ cho kỳ: 
              <span >
                  {{ nextSemester.semesterName }} - {{ nextSemester.semesterCode }}
              </span>
            </h4>
          </div>
          <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed"
            id="DataTables_Table_0" aria-describedby="DataTables_Table_0_info" style="width: 1391px">
            <thead>
              <tr>
                <th class="control sorting_disabled" rowspan="1" colspan="1" style="width: 0px" aria-label=""></th>
                <th class="sorting sorting_desc" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                  style="width: 334px" aria-label="Club: activate to sort column ascending" aria-sort="descending">
                  Tên câu lạc bộ
                </th>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                  style="width: 149px" aria-label="Description: activate to sort column ascending">
                  Mô tả
                </th>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                  style="width: 149px" aria-label="Description: activate to sort column ascending">
                  Giáo viên
                </th>
                <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                  style="width: 107px" aria-label="Status: activate to sort column ascending">
                  Trạng thái
                </th>
                <th class="sorting_disabled" rowspan="1" colspan="1" style="width: 175px" aria-label="Actions">
                  Hoạt động
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(club, index) in dataClubIndex.value.data" :class="index % 2 === 0 ? 'even' : 'odd'"
                :key="club.id" @click="OpenModalDetail(club)">
                <td class="control" tabindex="0" style=""></td>
                <td class="sorting_1">
                  <div class="d-flex justify-content-start align-items-center user-name">
                    <div class="d-flex flex-column">
                      <span class="fw-medium">{{ club.name }}</span>
                    </div>
                  </div>
                </td>

                <td>
                  <span class="text-heading">{{ truncatedDescription(club.description) }}</span>
                </td>
                <td class="sorting_1">
                  <div class="d-flex justify-content-start align-items-center user-name">
                    <div class="d-flex flex-column">
                      <span class="fw-medium">
                        {{ club.teacher && club.teacher.firstName && club.teacher.lastName ? `${club.teacher.firstName}
                        ${club.teacher.lastName}` : 'Kì này chưa có giáo viên' }}
                      </span>
                    </div>
                  </div>
                </td>
                <td>
                  <span class="badge" :class="getStatusClass(club.id)" text-capitalized="">
                    {{ getStatusText(club.id) }}
                  </span>
                </td>
                <td>
                  <div class="d-flex" v-if="nextSemester.id !==0">
                    <div v-if="dataClubEnrollment.value.data.some(enrollment => enrollment.clubId === club.id
                      && (enrollment.status === StatusClubEnrollment.REGISTER || enrollment.status === StatusClubEnrollment.APPROVED))"
                      @click.stop="OpenModalUnRegisterClub(club)">
                      <i class="bx bx-x-circle" title="Hủy đăng kí"></i>
                    </div>
                    <div
                      v-else-if="dataClubEnrollment.value.data.some(enrollment => enrollment.clubId === club.id 
                      && (enrollment.status === StatusClubEnrollment.REJECTED || enrollment.status === StatusClubEnrollment.CANCEL))"
                      @click.stop="OpenModalConfirm(club)">
                      <i class="bx bx-plus" title="Đăng kí lại"></i>
                    </div>
                    <div v-else @click.stop="OpenModalRegisterClub(club)">
                      <i class="bx bx-plus" title="Đăng kí"></i>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-if="dataClubIndex.value.totalRecords / dataClubIndex.value.pageSize >1">
          <PaginationComponent :total-records="dataClubIndex.value.totalRecords" :page-size="dataClubIndex.value.pageSize"
          :current-page="dataClubIndex.value.pageNumber" 
          @page-changed="handlePageChange" />
        </div>
        
        <ModalClubDetail :isShowModal="isShowModalDetail" :club="selectedClub" @closeModal="handleCloseModalDetail" />

        <ModalConfirmRegisterClub :isShowModal="isShowModalConfirm" :club="selectedClub" :isRegisterClub="isRegisterClub"
          :isUnRegisterClub="isUnRegisterClub" @closeModal="handleCloseModalConfirm" @register="handleCreateClubEnrollment"
          @unRegister="handleUpdateClubEnrollment" @reRegister="handleUpdateClubEnrollment" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useClubComposable } from '@/composables/pupil-club';
import ClubTableComponent from '@/components/views/pupil/ClubTable.vue';
import PaginationComponent from '@/components/common/Pagination.vue';
import ModalClubDetail from '@/components/modals/pupil/ModalClubDetail.vue';
import ModalConfirmRegisterClub from '@/components/modals/pupil/ModalConfirmRegisterClub.vue';
import {StatusClubEnrollment} from '@/constants/enums/statuses.ts';
export default defineComponent({
  name: 'RegisterClub',
  components: {
    ClubTableComponent,
    PaginationComponent,
    ModalClubDetail,
    ModalConfirmRegisterClub,
  },
  setup() {
    const clubComposable = useClubComposable();
    const {
      dataClubIndex,
      requestCreateAndUpdateClubEnrollment,
      handleGetClubsBySemesterActive,
      handleCreateClubEnrollment,
      handleUpdateClubEnrollment,
      handlePageChange,
      handleGetNextSemester,
      nextSemester,
      dataClubEnrollment,
      isShowModalDetail,
      handleGetClubEnrollmentByPupilId,
      isShowModalConfirm,
      isRegisterClub,
      isUnRegisterClub,
      handleOpenModalConfirm,
      handleCloseModalConfirm,
      handleCloseModalDetail,
      handleOpenModalDetail,
    } = clubComposable;

    const selectedClub = ref<any>({});

    const OpenModalDetail = (club: any) => {
      selectedClub.value = club;
      handleOpenModalDetail();
    };


    const OpenModalUnRegisterClub = (club: any) => {
      selectedClub.value = club;
      isUnRegisterClub.value = true;
      handleOpenModalConfirm();
    };

    const OpenModalConfirm = (club: any) => {
      selectedClub.value = club;
      handleOpenModalConfirm();
    };
    const OpenModalRegisterClub = (club: any) => {
      selectedClub.value = club;
      isRegisterClub.value = true;
      handleOpenModalConfirm();
    };
    const truncatedDescription = (description: string) => {
      if (description.length > 25) {
        return description.substring(0, 25) + '...';
      }
      return description;
    };
    onMounted(async () => {
      await handleGetClubsBySemesterActive();
      await handleGetNextSemester();
      if (nextSemester) {
        await handleGetClubEnrollmentByPupilId(nextSemester.id);
      }
    });

    const getStatusText = (clubId: number) => {
      const enrollment = dataClubEnrollment.value.data.find(enrollment => enrollment.clubId === clubId);
      if (enrollment) {
        switch (enrollment.status) {
          case 1:
            return 'Đã đăng kí';
          case 2:
            return 'Đã tham gia';
          case 3:
            return 'Bị từ chối';
          case 4:
            return 'Đã hủy';
          default:
            return '';
        }
      }
      return 'Chưa đăng kí';
    };

    const getStatusClass = (clubId: number) => {
      const enrollment = dataClubEnrollment.value.data.find(enrollment => enrollment.clubId === clubId);
      if (enrollment) {
        switch (enrollment.status) {
          case 1:
            return 'bg-label-success';
          case 2:
            return 'bg-label-info';
          case 3:
            return 'bg-label-warning';
          case 4:
            return 'bg-label-danger';
          default:
            return '';
        }
      }
      return 'bg-label-success';
    };

    return {
      StatusClubEnrollment,
      dataClubIndex,
      selectedClub,
      nextSemester,
      OpenModalConfirm,
      OpenModalDetail,
      handleCloseModalDetail,
      dataClubEnrollment,
      requestCreateAndUpdateClubEnrollment,
      handleGetClubsBySemesterActive,
      handleCreateClubEnrollment,
      handleUpdateClubEnrollment,
      handlePageChange,
      getStatusText,
      truncatedDescription,
      isShowModalDetail,
      getStatusClass,
      isShowModalConfirm,
      isRegisterClub,
      isUnRegisterClub,
      OpenModalUnRegisterClub,
      OpenModalRegisterClub,
      handleOpenModalConfirm,
      handleCloseModalConfirm,
    };
  },
});
</script>

<style scoped>
.btn {
  margin: 5px 0;
  width: 30%;
  text-align: left;
  border-radius: 5px;
  padding: 10px;
  font-size: 14px;
}

.btn-soft-danger {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.btn-soft-warning {
  background-color: #fff3cd;
  color: #856404;
  border: 1px solid #ffeeba;
}

.btn-soft-success {
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
}

.btn-icon {
  margin-right: 10px;
}

.dropdown-menu .dropdown-item {
  padding: 0;
}

.dropdown-menu .btn {
  width: 100%;
  border: none;
  text-align: left;
}

.modal {
  display: block;
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgb(0, 0, 0);
  background-color: rgba(0, 0, 0, 0.4);
}

.modal-content {
  background-color: #fefefe;
  margin: 15% auto;
  padding: 20px;
  border: 1px solid #888;
  width: 80%;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}
</style>