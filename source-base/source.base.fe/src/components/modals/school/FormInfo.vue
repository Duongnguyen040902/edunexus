<template>
  <ModalComponent
    :modelValue="showModal"
    :width="`70%`"
    @update:modelValue="$emit('update:showModal', $event)"
    :title="'Chỉnh sửa thông tin nhà trường'"
  >
    <div v-if="schoolDetail" class="container-xxl flex-grow-1 container-p-y">
      <div class="row">
        <div class="col-md-12">
          <div class="card mb-4">
            <h5 class="card-header">Thông tin nhà trường</h5>
            <div class="card-body">
              <div class="d-flex">
              </div>
              <div class="d-flex align-items-start align-items-sm-center gap-4">
                <div class="button-wrapper">
                  <img
                    :src="imageUrl"
                    alt="user-avatar"
                    class="d-block rounded"
                    height="100"
                    width="100"
                    id="uploadedAvatar"
                  />
                  <div class="button-wrapper">
                    <label for="upload" class="btn btn-primary me-2 mb-4" tabindex="0">
                      <span class="d-none d-sm-block">Tải ảnh lên</span>
                      <i class="bx bx-upload d-block d-sm-none"></i>
                      <input
                        type="file"
                        id="upload"
                        class="account-file-input"
                        hidden
                        accept="image/png, image/jpeg, image/gif,image/jpg"
                        @change="$emit('handleFileChange', $event)"
                      />
                    </label>
                    <div v-if="errorsUpdateSchool.ImageFile" class="text-danger">
                      {{ errorsUpdateSchool.ImageFile[0] }}
                    </div>
                    <p class="text-muted mb-0">CHO PHÉP JPG, GIF, PNG or JPEG.</p>
                  </div>
                </div>
              </div>
            </div>
            <hr class="my-0" />
            <div class="card-body">
              <div class="row">
                <div class="mb-3 col-md-6">
                  <label for="StandardCode" class="form-label">Mã chuẩn(*)</label>
                  <input
                    class="form-control"
                    type="text"
                    id="StandardCode"
                    name="StandardCode"
                    v-model="schoolDetail.standardCode"
                    autofocus
                  />
                  <div v-if="errorsUpdateSchool.StandardCode" class="text-danger">
                    {{ errorsUpdateSchool.StandardCode[0] }}
                  </div>
                </div>

                <div class="mb-3 col-md-6">
                  <label for="schoolName" class="form-label">Tên trường(*)</label>
                  <input
                    class="form-control"
                    type="text"
                    id="schoolName"
                    name="schoolName"
                    v-model="schoolDetail.name"
                  />
                  <div v-if="errorsUpdateSchool.Name" class="text-danger">{{ errorsUpdateSchool.Name[0] }}</div>
                </div>

                <div class="mb-3 col-md-6">
                  <label for="principalName" class="form-label">Tên hiệu trưởng(*)</label>
                  <input
                    class="form-control"
                    type="text"
                    id="principalName"
                    name="principalName"
                    v-model="schoolDetail.principalName"
                  />
                  <div v-if="errorsUpdateSchool.PrincipalName" class="text-danger">
                    {{ errorsUpdateSchool.PrincipalName[0] }}
                  </div>
                </div>

                <div class="mb-3 col-md-6">
                  <label for="establishedDate" class="form-label">Ngày thành lập(*)</label>
                  <input
                    class="form-control"
                    type="datetime-local"
                    id="establishedDate"
                    name="establishedDate"
                    v-model="schoolDetail.dateOfEstablishment"
                  />
                  <div v-if="errorsUpdateSchool.DateOfEstablishment" class="text-danger">
                    {{ errorsUpdateSchool.DateOfEstablishment[0] }}
                  </div>
                </div>
                <div class="mb-3 col-md-6">
                  <label for="phoneNumber" class="form-label">Điện thoại(*)</label>
                  <input
                    class="form-control"
                    type="text"
                    id="phoneNumber"
                    name="phoneNumber"
                    v-model="schoolDetail.phoneNumber"
                  />
                  <div v-if="errorsUpdateSchool.PhoneNumber" class="text-danger">
                    {{ errorsUpdateSchool.PhoneNumber[0] }}
                  </div>
                </div>

                <div class="mb-3 col-md-6">
                  <label for="principalPhoneNumber" class="form-label">Số ĐT hiệu trưởng(*)</label>
                  <input
                    class="form-control"
                    type="text"
                    id="principalPhoneNumber"
                    name="principalPhoneNumber"
                    v-model="schoolDetail.principalPhone"
                  />
                  <div v-if="errorsUpdateSchool.PrincipalPhone" class="text-danger">
                    {{ errorsUpdateSchool.PrincipalPhone[0] }}
                  </div>
                </div>

                <div class="mb-3 col-md-6">
                  <label for="address" class="form-label">Địa chỉ(*)</label>
                  <input class="form-control" type="text" id="address" name="address" v-model="schoolDetail.address" />
                  <div v-if="errorsUpdateSchool.Address" class="text-danger">{{ errorsUpdateSchool.Address[0] }}</div>
                </div>

                <div class="mb-3 col-md-6">
                  <label for="fax" class="form-label">FAX(*)</label>
                  <input class="form-control" type="text" id="fax" name="fax" v-model="schoolDetail.fax" />
                  <div v-if="errorsUpdateSchool.FAX" class="text-danger">{{ errorsUpdateSchool.FAX[0] }}</div>
                </div>
                <div class="mb-3 col-md-6">
                  <label for="email" class="form-label">Email(*)</label>
                  <input class="form-control" type="text" id="email" name="email" v-model="schoolDetail.email" />
                </div>
                <div v-if="errorsUpdateSchool.Email" class="text-danger">{{ errorsUpdateSchool.Email[0] }}</div>
                <div class="mb-3 col-md-6">
                  <label for="websiteLink" class="form-label">Website Link</label>
                  <input
                    class="form-control"
                    type="text"
                    id="websiteLink"
                    name="websiteLink"
                    v-model="schoolDetail.websiteLink"
                  />
                  <div v-if="errorsUpdateSchool.WebsiteLink" class="text-danger">
                    {{ errorsUpdateSchool.WebsiteLink[0] }}
                  </div>
                </div>
                <div class="mb-3 col-md-6">
                  <label for="city" class="form-label">Thành phố(*)</label>
                  <select
                    class="form-select"
                    id="city"
                    v-model="schoolDetail.province"
                    @change="$emit('onProvinceChange')"
                    required
                  >
                    <option v-for="province in provinces" :key="province.code" :value="province.code">
                      {{ province.name }}
                    </option>
                  </select>
                </div>
                <div class="mb-3 col-md-6">
                  <label for="district" class="form-label">Quận/Huyện(*)</label>
                  <select
                    class="form-select"
                    id="district"
                    v-model="schoolDetail.district"
                    @change="$emit('onDistrictChange')"
                    required
                  >
                    <option v-for="district in districts" :key="district.code" :value="district.code">
                      {{ district.name }}
                    </option>
                  </select>
                </div>
                <div class="mb-3 col-md-6">
                  <label for="ward" class="form-label">Xã/Phường(*)</label>
                  <select class="form-select" id="ward" v-model="schoolDetail.ward" required>
                    <option v-for="ward in wards" :key="ward.code" :value="ward.code">
                      {{ ward.name }}
                    </option>
                  </select>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <div class="mt-2">
          <button type="button" class="btn btn-primary me-2" @click="$emit('confirm')">Lưu thay đổi</button>
          <button type="reset" class="btn btn-outline-secondary" @click="$emit('notSave')">Hủy</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { useSchoolDetailComposable} from '@/composables/school';
import { ErrorResponseUpdateSchoolInfo } from '@/types/model/school';
import {  PropType, watchEffect } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';

export default {
  name: 'SchoolInfoComponent',
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    schoolDetail: {
      type: Object,
      required: true,
    },
    provinces: {
      type: Object,
    },
    wards: {
      type: Object,
    },
    districts: {
      type: Object,
    },
    errorsUpdateSchool: {
      type: Object as PropType<ErrorResponseUpdateSchoolInfo>,
    },
    imageUrl: {
        type: Object,
    }
  },

  setup(props, { emit }) {

    const handleModalConfirm = () => {
      emit('confirm', localRequestAddClass.value);
      emit('update:showModal', false);
    };

    return {
    }
  },
};
</script>
