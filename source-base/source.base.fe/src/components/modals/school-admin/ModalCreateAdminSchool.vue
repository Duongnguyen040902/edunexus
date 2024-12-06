<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
    <div class="modal-body">
      <div class="text-center mb-6">
        <h4 class="mb-2">Tạo tài khoản trường</h4>
      </div>
      <form class="row g-6">
        <div class="col-12 col-md-6">
          <label class="form-label" for="name">Tên đăng nhập <span class="required">*</span></label>
          <input
            id="name"
            v-model="requestCreateSchoolAdmin.username"
            class="form-control"
            name="name"
            placeholder="Nhập tên đăng nhập"
            type="text"
          />
          <div v-if="errorAdminSchool.Username" class="text-danger">{{ errorAdminSchool.Username[0] }}</div>
        </div>

        <div class="col-12 col-md-6">
          <label class="form-label" for="name">Tên trường <span class="required">*</span></label>
          <input
            id="name"
            v-model="requestCreateSchoolAdmin.schoolName"
            class="form-control"
            name="name"
            placeholder="Nhập tên trường"
            type="text"
          />
          <div v-if="errorAdminSchool.SchoolName" class="text-danger">{{ errorAdminSchool.SchoolName[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="website">Email <span class="required">*</span></label>
          <input
            id="website"
            v-model="requestCreateSchoolAdmin.email"
            class="form-control"
            name="website"
            placeholder="Nhập email"
            type="text"
          />
          <div v-if="errorAdminSchool.Email" class="text-danger">{{ errorAdminSchool.Email[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="website">Mật khẩu <span class="required">*</span></label>
          <input
            id="website"
            v-model="requestCreateSchoolAdmin.password"
            class="form-control"
            name="website"
            placeholder="Nhập mât khẩu"
            type="password"
          />
          <div v-if="errorAdminSchool.Password" class="text-danger">{{ errorAdminSchool.Password[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="phone">Số điện thoại</label>
          <input
            id="phone"
            v-model="requestCreateSchoolAdmin.phoneNumber"
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
            v-model="requestCreateSchoolAdmin.address"
            class="form-control"
            name="address"
            placeholder="Nhập địa chỉ"
            type="text"
          />
          <div v-if="errorAdminSchool.Address" class="text-danger">{{ errorAdminSchool.Address[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label v-if="isShowMethod" class="form-label me-2" for="statusSwitch">Chọn gói <span class="required">*</span></label>
          <div class="d-flex align-items-center">
            <el-select
              v-model="requestCreateSchoolAdmin.subscriptionPlanId"
              class="mb-4 w-50"
              filterable
              placeholder="Chọn gói"
            >
              <el-option :label="`Chọn gói`" :value="0"></el-option>
              <el-option
                v-for="(sub, index) in responseSubscription.value"
                :key="index"
                :label="sub.name"
                :value="sub.id"
              >
              </el-option>
            </el-select>
          </div>
          <div v-if="errorAdminSchool.SubscriptionPlanId" class="text-danger">
            {{ errorAdminSchool.SubscriptionPlanId[0] }}
          </div>
        </div>
        <div class="col-12 col-md-6">
          <label v-if="isShowMethod" class="form-label me-2" for="statusSwitch">Chọn phương thức thanh toán</label>
          <div class="d-flex align-items-center">
            <el-select
              v-if="isShowMethod"
              v-model="requestCreateSchoolAdmin.paymentMethod"
              class="mb-4 w-50"
              filterable
              placeholder="Phương thức thanh toán"
            >
              <el-option v-for="(method, value) in paymentMethod" :key="value" :label="method" :value="method">
              </el-option>
            </el-select>
          </div>
          <div v-if="errorAdminSchool.PaymentMethod" class="text-danger">{{ errorAdminSchool.PaymentMethod[0] }}</div>
        </div>
      </form>
    </div>
    <template #footer>
      <div class="modal-footer">
        <div class="col-12 text-center">
          <button class="btn btn-primary me-3" type="submit" @click="confirmAction">Tạo</button>
          <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, toRefs, watch } from 'vue';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import ModalComponent from '@/components/common/Modal.vue';
import { useSubscriptionComposable } from '@/composables/subscription.ts';
import { PaymentMethod, SubscriptionPlan } from '@/constants/enums/statuses.ts';

export default defineComponent({
  name: 'ModalCreateAdminSchoolComponent',
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
    const isShowMethod = ref(false);
    const { errorAdminSchool, requestCreateSchoolAdmin } = adminSchoolComposable;
    const { isShowModal } = toRefs(props);
    const close = (value: boolean) => {
      emit('closeModal', value);
    };

    const confirmAction = () => {
      emit('confirmAction', true);
    };
    const subscription = useSubscriptionComposable();
    const { responseSubscription, handleGetAllSubscription } = subscription;

    const updateIsShowMethod = () => {
      isShowMethod.value =
        Number(requestCreateSchoolAdmin.subscriptionPlanId) !== SubscriptionPlan.TRIAL &&
        Number(requestCreateSchoolAdmin.subscriptionPlanId) !== 0 &&
        requestCreateSchoolAdmin.subscriptionPlanId !== null;
    };

    onMounted(async () => {
      await handleGetAllSubscription();
      updateIsShowMethod();
    });

    watch(
      () => requestCreateSchoolAdmin.subscriptionPlanId,
      () => {
        updateIsShowMethod();
      },
    );

    const paymentMethod = Object.values(PaymentMethod);

    return {
      paymentMethod,
      isShowMethod,
      responseSubscription,
      requestCreateSchoolAdmin,
      errorAdminSchool,
      isShowModal,
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
