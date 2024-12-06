<template>
  <div class="col-xl-12 col-lg-12 col-md-12 order-0 order-md-1">
    <!-- Club Overview Cards -->
    <div class="row text-nowrap">
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
              Tổng số câu lạc bộ:
              <span class="text-primary">{{ dataClub.value.data.totalRecords }}</span>
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
    <button class="btn btn-primary" @click="openModalAddClub">Thêm câu lạc bộ</button>
    <!-- Club List Table -->
    <div class="card shadow-sm" v-if="dataClub.value.data.data">
      <div class="card-header d-flex flex-column flex-md-row justify-content-between align-items-center">
        <h5 class="card-title mb-2 mb-md-0">Danh sách câu lạc bộ</h5>
        <div class="d-flex align-items-center flex-wrap gap-2">
          <input
            type="search"
            class="form-control w-auto"
            placeholder="Tìm kiếm câu lạc bộ"
            v-model="clubSearchKey"
            @keyup.enter="handleSearchClub"
          />
          <select class="form-select w-auto" @change="handleChangeStatus">
            <option value="">Tất cả</option>
            <option value="1">Hoạt động</option>
            <option value="0">Ngừng hoạt động</option>
          </select>
          <button @click="handleSearchClub" class="btn btn-primary">Tìm kiếm</button>
        </div>
      </div>
      <div class="table-responsive">
        <table class="table table-striped">
          <thead class="table-light">
            <tr>
              <th>#</th>
              <th>Tên câu lạc bộ</th>
              <th>Trạng thái</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="dataClub.value.data.data.length === 0">
              <td colspan="4" class="text-center">Không có dữ liệu</td>
            </tr>
            <tr v-else v-for="(club, index) in dataClub.value.data.data" :key="club.id">
              <td>{{ index + 1 }}</td>
              <td>{{ club.name }}</td>
              <td>
                <span :class="club.status === 1 ? 'text-success' : 'text-danger'">
                  {{ club.status === 1 ? 'Hoạt động' : 'Ngừng hoạt động' }}
                </span>
              </td>
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
                    <li>
                      <a href="javascript:;" class="dropdown-item" @click="handleRedirectToDetail(club.id)"> Xem </a>
                    </li>
                    <li>
                      <a href="javascript:;" class="dropdown-item" @click="openModalEditClub(club.id)"> Sửa </a>
                    </li>
                    <div class="dropdown-divider"></div>
                    <li>
                      <a
                        href="javascript:;"
                        class="dropdown-item text-danger delete-record"
                        @click="openModalDeleteClub(club.id)"
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
      <div class="card-footer d-flex justify-content-between align-items-center">
        <span>
          Hiển thị {{ dataClub.value.data.data.length }} / {{ dataClub.value.data.totalRecords }} câu lạc bộ
        </span>
        <div v-if="dataClub.value.data.totalPages > 1">
          <Pagination       
          :total-records="dataClub.value.data.totalRecords"
          :page-size="dataClub.value.data.pageSize"
          :current-page="dataClub.value.data.pageNumber"
          @update:currentPage="currentPage = $event"
          @page-changed="handlePageChange"
        />
        </div>
       
      </div>
    </div>

    <!-- Modals -->
    <CreateAndUpdateClub
      :showModalAdd="isAddClubModalVisible"
      :clubErrors="clubErrors"
      :isUpdateMode="isEditClubMode"
      :dataClubDetail="dataClubDetail"
      @confirm="handleConfirm"
      @update:showModal="isAddClubModalVisible = $event"
      @closeUpdateModal="isEditClubMode = $event"
    />
    <DeleteClub
      :showDeleteModal="isDeleteClubModalVisible"
      @update:showDeleteModal="isDeleteClubModalVisible = $event"
      @confirm="handleConfirmDelete"
    />
  </div>
</template>

<script lang="ts">
import Pagination from '@/components/common/Pagination.vue';
import { useClubDetailComposable } from '@/composables/club';
import { onMounted, watch } from 'vue';
import { defineComponent } from 'vue';
import CreateAndUpdateClub from '@/components/modals/club/CreateAndUpdateClub.vue';
import DeleteClub from '@/components/modals/club/DeleteClub.vue';
export default defineComponent({
  name: 'TableClub',
  components: {
    Pagination,
    CreateAndUpdateClub,
    DeleteClub,
  },
  setup(props, { emit }) {
    const {
      dataClub,
      isAddClubModalVisible,
      isEditClubMode,
      clubErrors,
      clubSearchKey,
      dataClubDetail,
      isDeleteClubModalVisible,
      resetPage,
      selectedClubStatus,
      currentPage,
      handleChangeStatus,
      handleRedirectToDetail,
      openModalDeleteClub,
      handleConfirmDelete,
      resetModal,
      handleUpdateClub,
      openModalEditClub,
      handleRefreshClub,
      handleCreateClub,
      handleGetClubDetail,
      handleConfirm,
      openModalAddClub,
      handleGetAllClubs,
      handleSearchClub,
      handlePageChange,
    } = useClubDetailComposable();

    watch(resetPage, async newValue => {
      if (newValue) {
        emit('refreshPage');
        await handleGetAllClubs();
        resetPage.value = false;
      }
    });

    onMounted(async () => {
      await handleGetAllClubs();
    });

    return {
      dataClub,
      isEditClubMode,
      isDeleteClubModalVisible,
      isAddClubModalVisible,
      clubSearchKey,
      clubErrors,
      dataClubDetail,
      currentPage,
      handleConfirm,
      openModalAddClub,
      handleSearchClub,
      openModalEditClub,
      handleRedirectToDetail,
      openModalDeleteClub,
      handlePageChange,
      handleConfirmDelete,
      handleChangeStatus,
    };
  },
});
</script>

<style scoped>
.card-icon .avatar-initial {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
}

.card {
  border-radius: 10px;
  overflow: hidden;
}

.card-icon .avatar-initial {
  width: 60px;
  height: 60px;
  font-size: 28px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.table-responsive {
  margin-top: 20px;
}

.card-header {
  background-color: #f8f9fa;
  border-bottom: 1px solid #dee2e6;
}

.table th,
.table td {
  vertical-align: middle;
  text-align: center;
}

.btn {
  padding: 6px 12px;
  font-size: 14px;
}
</style>
