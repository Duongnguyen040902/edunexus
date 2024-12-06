<template>
  <div class="card mb-6">
    <div class="card-body pt-12">
      <div class="user-avatar-section">
        <div class="d-flex align-items-center flex-column">
          <img
            :src="userAvatar"
            alt="User avatar"
            class="img-fluid rounded mb-4"
            height="120"
            width="120"
          />
          <div class="user-info text-center">
            <h5>{{ school.schoolName }}</h5>
          </div>
        </div>
      </div>
      <h5 class="pb-4 border-bottom mb-4">Chi tiết</h5>
      <div class="info-container">
        <ul class="list-unstyled mb-6">
          <li class="mb-2">
            <span class="h6">Tài khoaản: </span>
            <span>{{ school.email }}</span>
          </li>
          <li class="mb-2">
            <span class="h6">Email: </span>
            <span>{{ school.email }}</span>
          </li>
          <li class="mb-2">
            <span class="h6">Địa chỉ: </span>
            <span>{{ school.address }}</span>
          </li>
          <li class="mb-2">
            <span class="h6">Website: </span>
            <span>{{ school.websiteLink }}</span>
          </li>
          <li class="mb-2">
            <span class="h6">Số điện thoại: </span>
            <span>{{ school.phoneNumber }}</span>
          </li>
          <li class="mb-2">
            <span class="h6">Trạng thái: </span>
            <span>{{ school.statusName }}</span>
          </li>
        </ul>
        <div class="d-flex justify-content-center">
          <a
            class="btn btn-primary me-4"
            data-bs-target="#editUser"
            data-bs-toggle="modal"
            href="#"
            @click="handleOpenModalEdit"
            >Chinh sửa</a
          >
        </div>
      </div>
    </div>
    <ModalEditAdminSchoolComponent
      :isShowModal="isShowModalEdit"
      @closeModal="handleCloseModalEdit"
      @confirmAction="handleConfirmEdit"
    />
  </div>
</template>

<script lang="ts">
import {computed, defineComponent, onMounted, ref} from 'vue';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import { useRouter, useRoute } from 'vue-router';
import { SchoolInterface } from '@/types/model/admin-school.ts';
import ModalEditAdminSchoolComponent from '@/components/modals/ModalEditAdminSchool.vue';

export default defineComponent({
  name: 'ProfileAdminSchoolComponent',
  components: {
    ModalEditAdminSchoolComponent,
  },
  props: {
    school: {
      type: Object as () => SchoolInterface,
      required: true,
    },
  },
  setup(props, { emit }) {
    const adminSchoolComposable = useAdminSchoolComposable();
    const { isShowModalEdit, handleOpenModalEdit, handleCloseModalEdit, handleGetSchoolDetail, handleConfirmEdit } =
      adminSchoolComposable;
    const router = useRouter();
    const route = useRoute();

    onMounted(async () => {});
    const apiUrl = import.meta.env.VITE_APP_API_URL;

    const userAvatar = computed(() => {
      return props.school.image ? `${apiUrl}${props.school.image}` : '@/assets/images/avatars/1.png';
    });

    return {
      userAvatar,
      apiUrl,
      isShowModalEdit,
      handleGetSchoolDetail,
      handleOpenModalEdit,
      handleCloseModalEdit,
      handleConfirmEdit,
    };
  },
});
</script>
