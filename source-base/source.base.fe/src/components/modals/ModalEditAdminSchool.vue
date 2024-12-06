<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
    <div class="modal-body">
      <div class="text-center mb-6">
        <h4 class="mb-2">Chỉnh sửa thông tin trường</h4>
      </div>
      <form class="row g-6">
        <div class="col-12 col-md-6">
          <label class="form-label" for="name">Tên trường <span class="required">*</span></label>
          <input
            id="name"
            v-model="requestAdminSchoolUpdate.schoolName"
            class="form-control"
            name="name"
            placeholder="Nhập tên trường"
            type="text"
          />
          <div v-if="errorAdminSchool.SchoolName" class="text-danger">{{ errorAdminSchool.SchoolName[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="website">Website</label>
          <input
            id="website"
            v-model="requestAdminSchoolUpdate.websiteLink"
            class="form-control"
            name="website"
            placeholder="Nhập website"
            type="text"
          />
          <div v-if="errorAdminSchool.WebsiteLink" class="text-danger">{{ errorAdminSchool.WebsiteLink[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="phone">Số điện thoại</label>
          <input
            id="phone"
            v-model="requestAdminSchoolUpdate.phoneNumber"
            class="form-control"
            name="phone"
            placeholder="Nhập số điện thoại"
            type="text"
          />
          <div v-if="errorAdminSchool.PhoneNumber" class="text-danger">{{ errorAdminSchool.PhoneNumber[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="address">Địa chỉ</label>
          <input
            id="address"
            v-model="requestAdminSchoolUpdate.address"
            class="form-control"
            name="address"
            placeholder="Nhập địa chỉ"
            type="text"
          />
          <div v-if="errorAdminSchool.Address" class="text-danger">{{ errorAdminSchool.Address[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="standardCode">Mã trường</label>
          <input
            id="standardCode"
            v-model="requestAdminSchoolUpdate.standardCode"
            class="form-control"
            name="standardCode"
            placeholder="Nhập mã trường"
            type="text"
          />
          <div v-if="errorAdminSchool.StandardCode" class="text-danger">{{ errorAdminSchool.StandardCode[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
<!--          <label class="form-label" for="date">Ngày thành lập</label>-->
<!--          <input-->
<!--            id="date"-->
<!--            v-model="requestAdminSchoolUpdate.dateOfEstablishment"-->
<!--            class="form-control"-->
<!--            name="date"-->
<!--            placeholder="Nhập ngày thành lập"-->
<!--            type="text"-->
<!--          />-->
<!--          <div v-if="errorAdminSchool.DateOfEstablishment" class="text-danger">-->
<!--            {{ errorAdminSchool.DateOfEstablishment[0] }}-->
<!--          </div>-->
          <label class="form-label me-2" for="statusSwitch">Trạng thái <span class="required">*</span></label>
          <select id="statusSwitch" v-model="requestAdminSchoolUpdate.accountStatus" class="form-select" name="status">
            <option v-for="(status, index) in statuses" :key="index" :value="Number(status.value)">{{ status.key }}</option>
          </select>
          <div v-if="errorAdminSchool.AccountStatus" class="text-danger">{{ errorAdminSchool.AccountStatus[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="email">Email <span class="required">*</span></label>
          <input
            id="email"
            v-model="requestAdminSchoolUpdate.email"
            class="form-control"
            name="email"
            placeholder="Nhập email"
            type="email"
          />
          <div v-if="errorAdminSchool.Email" class="text-danger">{{ errorAdminSchool.Email[0] }}</div>
        </div>
      </form>
    </div>
    <template #footer>
      <div class="modal-footer">
        <div class="col-12 text-center">
          <button class="btn btn-primary me-3" type="submit" @click="confirmAction">Lưu</button>
          <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs } from 'vue';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import ModalComponent from '@/components/common/Modal.vue';
import { StatusEditSchool } from '@/constants/enums/statuses.ts';

export default defineComponent({
  name: 'ModalEditAdminSchoolComponent',
  components: {
    ModalComponent,
  },
  props: {
    isShowModal: {
      type: Boolean,
      default: false,
    },
  },
  emits: ['closeModal', 'confirmAction'],
  setup(props, { emit }) {
    const adminSchoolComposable = useAdminSchoolComposable();
    const { requestAdminSchoolUpdate, errorAdminSchool } = adminSchoolComposable;
    const { isShowModal } = toRefs(props);
    const close = (value: boolean) => {
      emit('closeModal', value);
    };
    const statuses = Object.entries(StatusEditSchool).map(([key, value]) => ({ key, value }));
    const confirmAction = () => {
      emit('confirmAction', true);
    };
    return {
      statuses,
      errorAdminSchool,
      isShowModal,
      requestAdminSchoolUpdate,
      close,
      confirmAction,
    };
  },
});
</script>

<style scoped>
.el-dialog__body {
  padding: 1.5rem !important;
}

.modal-body {
  padding: 1.5rem;
}
</style>
