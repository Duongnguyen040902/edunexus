<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="row invoice-add">
      <!-- Invoice Add-->
      <div class="col-lg-9 col-12 mb-lg-0 mb-6">
        <div class="card invoice-preview-card p-sm-12 p-6">
          <div class="card-body invoice-preview-header rounded">
            <div class="d-flex flex-wrap flex-column flex-sm-row justify-content-between text-heading">
              <div class="mb-md-0 mb-6">
                <div class="d-flex svg-illustration mb-6 gap-2 align-items-center">
                  <span class="app-brand-logo demo">
                    <img alt="Brand Logo" src="@/assets/images/logo/edunexus_logo.png" width="100px" />
                  </span>
                </div>
                <p class="mb-2">Khu GD&ĐT, khu CNC Hoà Lạc</p>
                <p class="mb-2">KM29, Đại lộ Thăng Long, huyện Thạch Thất</p>
                <p class="mb-3">+024 7300 5588</p>
              </div>
              <div class="col-md-5 col-8 pe-0 ps-0 ps-md-2">
                <dl class="row mb-0 gx-4">
                  <dt class="col-sm-5 mb-2 d-md-flex align-items-center justify-content-end">
                    <span class="h5 text-capitalize mb-0 text-nowrap">Số hóa đơn</span>
                  </dt>
                  <dd class="col-sm-7">
                    <input
                      id="invoiceId"
                      :value="responseInvoiceData.value.id"
                      class="form-control"
                      placeholder="#3905"
                      readonly
                      type="text"
                    />
                  </dd>
                  <dt class="col-sm-5 mb-1 d-md-flex align-items-center justify-content-end">
                    <span class="fw-normal">Ngày phát hành:</span>
                  </dt>
                  <dd class="col-sm-7">
                    <el-date-picker
                      v-model="requestInvoiceManagerCreate.issueDate"
                      :disabled="isComplete.value"
                      placeholder="Chọn ngày kết thúc"
                      type="date"
                    >
                    </el-date-picker>
                    <div v-if="errorInvoice.IssueDate" class="text-danger">{{ errorInvoice.IssueDate[0] }}</div>
                  </dd>
                  <dt class="col-sm-5 d-md-flex align-items-center justify-content-end">
                    <span class="fw-normal">Ngày hết hạn:</span>
                  </dt>
                  <dd class="col-sm-7 mb-0">
                    <el-date-picker
                      v-model="requestInvoiceManagerCreate.dueDate"
                      :disabled="isComplete.value"
                      placeholder="Chọn ngày kết thúc"
                      type="date"
                    >
                    </el-date-picker>
                    <div v-if="errorInvoice.DueDate" class="text-danger">{{ errorInvoice.DueDate[0] }}</div>
                  </dd>
                </dl>
              </div>
            </div>
          </div>

          <div class="card-body px-0">
            <div class="row">
              <div class="col-md-6 col-sm-5 col-12 mb-sm-0 mb-6">
                <h6>Hóa đơn đến:</h6>
                <el-select
                  v-model="requestInvoiceManagerCreate.schoolId"
                  :disabled="isUpdate"
                  class="mb-4 w-50"
                  filterable
                  placeholder="Chọn trường"
                >
                  <el-option :label="`Chọn trường`" :value="0"></el-option>
                  <el-option
                    v-for="(school, index) in dataSchoolAdmin.value.data"
                    :key="index"
                    :label="school.schoolName"
                    :value="school.id"
                  >
                  </el-option>
                </el-select>
                <div v-if="errorInvoice.SchoolId" class="text-danger mb-2">{{ errorInvoice.SchoolId[0] }}</div>
                <h6>Chọn gói:</h6>
                <el-select
                  v-model="requestInvoiceManagerCreate.subscriptionPlanId"
                  :disabled="isUpdate"
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
                <div v-if="errorInvoice.SubscriptionPlanId" class="text-danger mb-2">
                  {{ errorInvoice.SubscriptionPlanId[0] }}
                </div>
                <p class="mb-1 address"><span>Địa chỉ: </span>{{ selectedSchool.address }}</p>
                <p class="mb-1 website"><span>Website: </span>{{ selectedSchool.website }}</p>
                <p class="mb-1 phone"><span>Số điện thoại: </span>{{ selectedSchool.phone }}</p>
                <p class="mb-0 email"><span>Email: </span>{{ selectedSchool.email }}</p>
              </div>
              <div class="col-md-6 col-sm-7">
                <h6>Trạng thái:</h6>
                <el-select
                  v-model="requestInvoiceManagerCreate.status"
                  :disabled="isComplete.value"
                  class="mb-4 w-50"
                  filterable
                  placeholder="Trạng thái"
                >
                  <el-option :label="`Trạng thái`" :value="0"></el-option>
                  <el-option
                    v-for="(label, status) in InvoiceStatusLabels"
                    :key="status"
                    :label="label"
                    :value="Number(status)"
                  >
                  </el-option>
                </el-select>
                <div v-if="errorInvoice.Status" class="text-danger mb-2">
                  {{ errorInvoice.Status[0] }}
                </div>
                <h6 v-if="isShowMethod">Phương thức thanh toán:</h6>
                <el-select
                  v-if="isShowMethod"
                  v-model="requestInvoiceManagerCreate.paymentMethod"
                  :disabled="isComplete.value"
                  class="mb-4 w-50"
                  filterable
                  placeholder="Phương thức thanh toán"
                >
                  <el-option v-for="(method, value) in paymentMethod" :key="value" :label="method" :value="method">
                  </el-option>
                </el-select>
                <div v-if="isShowMethod && errorInvoice.PaymentMethod" class="text-danger mb-2">
                  {{ errorInvoice.PaymentMethod[0] }}
                </div>
                <h6>Thông tin:</h6>
                <table>
                  <tbody>
                    <tr>
                      <td class="pe-4">Giá:</td>
                      <td>{{ selectedSubscription.priceSub }}</td>
                    </tr>
                    <tr>
                      <td class="pe-4">Tên gói:</td>
                      <td>{{ selectedSubscription.name }}</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
          <hr class="mt-0 mb-6" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, watch } from 'vue';
import RightSideBarInvoiceDetailComponent from '@/components/views/admin/Invoice/RightSideBarInvoiceDetail.vue';
import { useSubscriptionComposable } from '@/composables/subscription.ts';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import { useInvoiceManagerComposable } from '@/composables/invoice-manager.ts';
import { useRouter } from 'vue-router';
import { InvoiceStatus, InvoiceStatusLabels, PaymentMethod } from '@/constants/enums/statuses';

export default defineComponent({
  name: 'DetailInvoiceComponent',
  components: {
    RightSideBarInvoiceDetailComponent,
  },
  props: {},
  emits: [],
  setup(props, { emit }) {
    const subscription = useSubscriptionComposable();
    const schoolAdmin = useAdminSchoolComposable();
    const invoiceManager = useInvoiceManagerComposable();
    const { responseSubscription, handleGetAllSubscription } = subscription;
    const { dataSchoolAdmin, handleGetAllAcccountSchoolAdmin } = schoolAdmin;
    const {
      isComplete,
      isUpdate,
      requestInvoiceManagerCreate,
      errorInvoice,
      responseInvoiceData,
      handleDetailInvoice,
    } = invoiceManager;
    const selectedSchool = ref({ address: '', phone: '', website: '', email: '' });
    const selectedSubscription = ref({ name: '', priceSub: '', expiredDate: '' });
    const router = useRouter();
    const isShowMethod = ref(false);
    onMounted(async () => {
      if (router.currentRoute.value.params.id) {
        await handleDetailInvoice(Number(router.currentRoute.value.params.id));
      }

      await handleGetAllAcccountSchoolAdmin();
      await handleGetAllSubscription();
      if (Number(requestInvoiceManagerCreate.status) === InvoiceStatus.PAID) {
        isShowMethod.value = true;
      } else {
        isShowMethod.value = false;
      }
      handleSchoolChange(requestInvoiceManagerCreate.schoolId);
      handleSubscriptionChange(requestInvoiceManagerCreate.subscriptionPlanId);
    });

    const handleSchoolChange = async (schoolId: number) => {
      if (schoolId) {
        const school = dataSchoolAdmin.value.data.find(school => school.id === schoolId);
        if (school) {
          selectedSchool.value = {
            phone: school.phoneNumber || '',
            address: school.address || '',
            website: school.websiteLink || '',
            email: school.email || '',
          };
        } else {
          selectedSchool.value = {
            phone: '',
            address: '',
            website: '',
            email: '',
          };
        }
      }
    };

    const handleSubscriptionChange = async (subscriptionPlanId: number) => {
      if (subscriptionPlanId) {
        const subscription = responseSubscription.value.find(sub => sub.id === subscriptionPlanId);
        if (subscription) {
          selectedSubscription.value = {
            name: subscription.name || '',
            priceSub: subscription.price.toString() || '',
            expiredDate: subscription.durationDays.toString() || '',
          };
        } else {
          selectedSubscription.value = {
            name: '',
            priceSub: '',
            expiredDate: '',
          };
        }
      }
    };

    watch(() => requestInvoiceManagerCreate.schoolId, handleSchoolChange);

    watch(() => requestInvoiceManagerCreate.subscriptionPlanId, handleSubscriptionChange);

    watch(
      () => requestInvoiceManagerCreate.status,
      newValue => {
        if (Number(newValue) === InvoiceStatus.PAID) {
          isShowMethod.value = true;
        } else {
          isShowMethod.value = false;
        }
      },
    );

    const paymentMethod = Object.values(PaymentMethod);

    return {
      isComplete,
      isUpdate,
      paymentMethod,
      isShowMethod,
      responseInvoiceData,
      errorInvoice,
      selectedSubscription,
      selectedSchool,
      requestInvoiceManagerCreate,
      dataSchoolAdmin,
      responseSubscription,
      InvoiceStatusLabels,
      handleGetAllSubscription,
      handleGetAllAcccountSchoolAdmin,
    };
  },
});
</script>

<style scoped></style>
