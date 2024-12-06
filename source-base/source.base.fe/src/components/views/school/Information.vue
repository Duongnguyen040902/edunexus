<template>
  <div class="col-xl-4 col-lg-5 col-md-5 order-1 order-md-0">
    <div class="card mb-6">
      <div class="card-body pt-12">
        <div class="customer-avatar-section">
          <div class="d-flex align-items-center flex-column">
            <img class="img-fluid rounded mb-4" :src="imageUrl" height="120" width="120" alt="User avatar" />
            <div class="customer-info text-center mb-6">
              <h5 class="mb-0">{{ schoolDetail.name }}</h5>
              <span>Mã chuẩn: {{ schoolDetail.standardCode }}</span>
            </div>
          </div>
        </div>
        <div class="info-container">
          <h5 class="pb-4 border-bottom text-capitalize mt-6 mb-4">Chi tiết</h5>
          <ul class="list-unstyled mb-6">
            <li class="mb-2">
              <span class="h6 me-1">Tên trường:</span>
              <span>{{ schoolDetail.name }}</span>
            </li>
            <li class="mb-2">
              <span class="h6 me-1">Email:</span>
              <span>{{ schoolDetail.email }}</span>
            </li>
            <li class="mb-2">
              <span class="h6 me-1">Liên lạc:</span>
              <span>{{ schoolDetail.phoneNumber }}</span>
            </li>
            <li class="mb-2">
              <span class="h6 me-1">Website:</span>
              <span>{{ schoolDetail.websiteLink }}</span>
            </li>
          </ul>
          <div class="d-flex justify-content-center">
            <a
              href="javascript:;"
              class="btn btn-primary w-100"
              data-bs-target="#editUser"
              @click="isShow()"
              data-bs-toggle="modal"
            >
              Chỉnh sửa
            </a>
          </div>
        </div>
      </div>
    </div>
    <FormInfo
      :showModal="isShowModal"
      :schoolDetail="schoolDetail"
      :provinces="provinces"
      :wards="wards"
      :districts="districts"
      :errorsUpdateSchool="errorsUpdateSchool"
      :imageUrl="imageUrl"
      @update:showModal="isShowModal = $event"
      @notSave="notSave"
      @onProvinceChange="onProvinceChange"
      @onDistrictChange="onDistrictChange"
      @handleFileChange="handleFileChange"
      @confirm="saveChanges"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import FormInfo from '@/components/modals/school/FormInfo.vue';
import { useSchoolDetailComposable } from '@/composables/school';

export default defineComponent({
  name: 'Information',
  components: {
    FormInfo,
  },
  setup() {
    const {
      isShowModal,
      schoolDetail,
      imageUrl,
      imageFile,
      imagePath,
      errorsUpdateSchool,
      provinces,
      districts,
      wards,
      isEditable,
      isShow,
      onProvinceChange,
      onDistrictChange,
      notSave,
      saveChanges,
      fetchSchoolDetail,
      fetchData,
      handleFileChange,
      toggleEditMode,
      fetchProvinces,
      fetchDistricts,
      fetchWards,
      fetchUpdateSchool,
    } = useSchoolDetailComposable();

    onMounted(async () => {
      await fetchSchoolDetail();
      await fetchData();
    });

    return {
      isShowModal,
      schoolDetail,
      imageUrl,
      imageFile,
      imagePath,
      errorsUpdateSchool,
      provinces,
      districts,
      wards,
      isEditable,
      isShow,
      onProvinceChange,
      onDistrictChange,
      notSave,
      saveChanges,
      fetchSchoolDetail,
      fetchData,
      handleFileChange,
      toggleEditMode,
      fetchProvinces,
      fetchDistricts,
      fetchWards,
      fetchUpdateSchool,
    };
  },
});
</script>
