<template>
<div class="container-xxl flex-grow-1 container-p-y">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <!-- Hộp nội dung với kích thước nhỏ hơn màn hình, ví dụ col-lg-4 để chiếm khoảng 1/3 chiều rộng màn hình -->
        <div class="col-lg-4 col-md-6 col-sm-8 card-body p-md-8" style="max-width: 700px;">
            <h4 class="mb-2">Gói của bạn</h4>
            <div class="bg-lighter p-4 rounded fw-bold">
                <h4 class="fw-bold">{{ responseInvoiceInterface.value.subscriptionPlanName}}</h4>
                <h5 class="text-heading mb-0">{{ responseInvoiceInterface.value.durationDays === 30 ? '1 tháng' : '1 năm' }}</h5>
                <p>Chỉ {{ formatCurrency(responseInvoiceInterface.value.totalAmount) }}</p>
                <hr>
                <div class="d-flex justify-content-between align-items-center mt-4">
                    <p class="mb-0">Tổng</p>
                    <h6 class="mb-0">{{ formatCurrency(responseInvoiceInterface.value.totalAmount) }}</h6>
                </div>
                <div class="d-flex justify-content-between align-items-center mt-2">
                    <p class="mb-0">Bắt đầu từ {{ formatDate(responseInvoiceInterface.value.startDate) }}</p>
                    <h6 class="mb-0">{{ formatCurrency(responseInvoiceInterface.value.totalAmount) }} / tháng</h6>
                </div>
                <p class="mt-2"></p>
                <ul>
                    <li>Ngày thanh toán tiếp theo của bạn sẽ là {{ formatDate(responseInvoiceInterface.value.endDate) }}</li>
                </ul>
                <div class="d-grid">
                    <button type="button" data-bs-target="#pricingModal" data-bs-toggle="modal" class="btn btn-label-primary" @click="handleOpen">Thay đổi gói</button>
                </div>
            </div>

            <div class="mt-4">
                <p class="mb-2"></p>
                <div class="bg-lighter p-3 rounded">
                    <div class="form-check mb-2">

                    </div>
                    <div class="form-check mb-2">

                    </div>
                </div>

                <div class="d-grid mt-4">
                    <button class="btn btn-success" @click="handlePayment(responseInvoiceInterface.value.id)">
                        <span class="me-2">Tiếp tục thanh toán</span>
                        <i class="bx bx-right-arrow-alt scaleX-n1-rtl"></i>
                    </button>
                </div>
                <p class="mt-4">Bằng cách tiếp tục, bạn đồng ý với Điều khoản dịch vụ và Chính sách bảo mật của chúng tôi. Lưu ý rằng các khoản thanh toán không được hoàn lại.</p>
            </div>
        </div>
    </div>
    <UpdateSubscriptionModal 
        :isShowModal="isShowModal" 
        @handleClose="handleClose" 
        @handleUpgrade="handleUpgrade" 
        :handleGetSubscriptionByDuration="handleGetSubscriptionByDuration" 
        :responseSubscription="responseSubscription.value" 
        :subscriptionCurrent="responseCurrentSchoolSubscription.value" 
        :formatCurrency="formatCurrency" 
    />
</div>
</template>

    
<script lang="ts">
import UpdateSubscriptionModal from '@/components/modals/school/UpdateSubscriptionModal.vue';
import {
    useSchoolSubscriptionComposable
} from '@/composables/school-supscription';
import {
    defineComponent,
    onMounted
} from 'vue';
import {
    useRoute
} from 'vue-router';
export default defineComponent({
    name: 'OrderCheckoutComponent',
    components: {
        UpdateSubscriptionModal,
    },
    setup(props, {
        emit
    }) {
        const route = useRoute();
        const schoolSubscription = useSchoolSubscriptionComposable();
        const {
            responseInvoiceInterface,
            formatDate,
            formatCurrency,
            handlePayment,
            handleGetInvoiceBySubscription,
            handleOpen,
            isShowModal,
            handleClose,
            handleUpgrade,
            handleGetSubscriptionByDuration,
            responseSubscription,
            responseCurrentSchoolSubscription,

        } = schoolSubscription;

        onMounted(async () => {
            const subscriptionId = route.query.id;
            if (subscriptionId) {
                await handleGetInvoiceBySubscription(Number(subscriptionId));
            }
        });
        return {
            responseInvoiceInterface,
            formatDate,
            formatCurrency,
            handlePayment,
            handleOpen,
            isShowModal,
            handleClose,
            handleUpgrade,
            handleGetSubscriptionByDuration,
            responseSubscription,
            responseCurrentSchoolSubscription,
        };
    },
});
</script>
