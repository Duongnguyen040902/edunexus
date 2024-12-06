<template>
<div class="container-xxl flex-grow-1 container-p-y">
    <div class="row">
        <InformationComponent/>
        <div class="col-xl-8 col-lg-7 col-md-7 order-0 order-md-1">
            <div class="card mb-4">
                <div class="card-body">
                    <h4 class="card-title" style="margin-top: 10px;">Gói đăng ký</h4>
                    <div class="row">
                        <div class="col-md-7 col-12">
                            <p class="mb-2">Đang sử dụng gói {{ responseCurrentSchoolSubscription.value.subscriptionPlanName || 'N/A' }}</p>
                            <p class="text-muted mb-4">{{ responseCurrentSchoolSubscription.value.description || 'N/A' }}</p>
                            <p class="mb-2">Có hiệu lực đến ngày {{ formatDate(responseCurrentSchoolSubscription.value.endDate) }}</p>
                            <p class="text-muted mb-4">Chúng tôi sẽ gửi cho bạn thông báo khi Đăng ký hết hạn</p>
                            <p class="mb-2">Cập nhật gói nâng cấp</p>
                            <p class="text-muted mb-4">Cập nhật gói nâng cấp để có trải nghiệp tốt hơn với nhiều tính năng quan trọng</p>
                            <div class="col-12 d-flex gap-2 flex-wrap">
                                <button class="btn btn-primary me-2" @click="handleOpen">Cập nhật</button>
                            </div>
                        </div>
                        <div class="col-md-4 col-12">
                            <div class="alert alert-warning d-flex align-items-center mb-4" role="alert">
                                <i class="bx bx-error me-2"></i>
                                <div>
                                    <strong>Chú ý!</strong>
                                    <p class="mb-0">Kế hoạch của bạn cần được cập nhật</p>
                                </div>
                            </div>

                            <div>
                                <div class="d-flex justify-content-between mb-1">
                                    <small>Ngày</small>
                                    <small>{{ totalDays }} Ngày</small>
                                </div>
                                <div class="progress" style="height: 8px;">
                                    <div class="progress-bar bg-primary" role="progressbar" :style="{ width: progressPercentage + '%' }" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100"></div>
                                </div>
                                <p class="text-muted mt-1">Còn {{ Math.max(totalDays - daysUsed, 0) }} ngày nữa là đến ngày gói của bạn cần cập nhật</p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="card mb-4">
                <div id="DataTables_Table_1_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
                    <div class="row mx-6">
                        <div class="col-sm-6 col-12 d-flex align-items-center justify-content-center justify-content-sm-start mt-6 mt-sm-0">
                            <div class="head-label">
                                <h4 class="card-title mb-0" style="margin-top: 10px">Lịch sử gói đăng ký</h4>
                            </div>
                        </div>
                        <FiltersSchoolSubscriptionComponent 
                        @updateStatus="(value) => updateRequestSchoolSubscription('status', value)" 
                        @updateYear="(value) => updateRequestSchoolSubscription('year', value)" 
                        :listSchoolYearResponse="dataSchoolYear.value.data" />
                    </div>
                    <div v-if="!listSchoolSubscriptionResponse.value.data || listSchoolSubscriptionResponse.value.data.length === 0" 
                    class="text-heading d-flex justify-content-center align-items-center font-weight-bold" style="height: 200px; font-size: 1.25rem;">
                        Không tìm thấy gói đăng ký
                    </div>
                    <SchoolSubscriptionTableComponent v-else
                    :listSchoolSubscriptionResponse="listSchoolSubscriptionResponse.value.data" 
                    :formatDate="formatDate" 
                    :formatCurrency="formatCurrency" 
                    @handleDetailClick="handleOpenInvoiceDetail" />
                    <div class="row mx-6">
                        <div class="col-sm-12 col-xxl-6 text-center text-xxl-start pb-md-2 pb-xxl-0">
                            <div class="dataTables_info" id="DataTables_Table_1_info" role="status" aria-live="polite">
                                Showing {{ calculateShowingEntries().start }} to {{ calculateShowingEntries().end }} of {{ calculateShowingEntries().total }} entries
                            </div>
                        </div>
                        <div class="col-sm-12 col-xxl-6 d-md-flex justify-content-xxl-end justify-content-center">
                            <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_1_paginate">
                                <PaginationComponent 
                                :total-records="listSchoolSubscriptionResponse.value.totalRecords" 
                                :page-size="requestAllSchoolSubscription.pageSize" 
                                :current-page="requestAllSchoolSubscription.pageNumber" 
                                @update:currentPage="(value) => updateRequestSchoolSubscription('pageNumber', value)" 
                                />
                            </div>
                        </div>
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
    </div>
</div>
</template>

    
<script lang="ts">
import {
    computed,
    defineComponent,
    onMounted
} from 'vue';
import {
    useSchoolSubscriptionComposable
} from '@/composables/school-supscription';
import {
    useInvoiceComposable
} from '@/composables/invoice';
import PaginationComponent from '@/components/common/Pagination.vue';
import UpdateSubscriptionModal from '@/components/modals/school/UpdateSubscriptionModal.vue';
import ModalConfirmDeleteComponent from '@/components/common/ModalConfirmDelete.vue';
import FiltersSchoolSubscriptionComponent from '@/components/views/school/FilterSchoolSubscription.vue';
import SchoolSubscriptionTableComponent from '@/components/views/school/SchoolSubscriptionTable.vue';
import InformationComponent from '@/components/views/school/Information.vue';
import {
    useRoute,
    useRouter
} from 'vue-router';
import {
    notifyError,
    notifySuccess
} from '@/helpers/notify';
export default defineComponent({
    name: 'ProfileManagerComponent',
    components: {
        UpdateSubscriptionModal,
        ModalConfirmDeleteComponent,
        FiltersSchoolSubscriptionComponent,
        SchoolSubscriptionTableComponent,
        PaginationComponent,
        InformationComponent,
    },
    setup(props, {
        emit
    }) {
        const router = useRouter();
        const route = useRoute();
        const schoolSubscription = useSchoolSubscriptionComposable();
        const {
            responseCurrentSchoolSubscription,
            isShowModal,
            responseSubscription,
            listSchoolSubscriptionResponse,
            requestAllSchoolSubscription,
            daysUsed,
            progressPercentage,
            handleGetCurrentSubscription,
            handleGetSubscriptionByDuration,
            formatCurrency,
            formatDate,
            handleOpen,
            handleClose,
            handleUpgrade,
            updateRequestSchoolSubscription,
            handleGetAllSchoolSubscription,
            calculateShowingEntries,
            handleGetSchoolYear,
            dataSchoolYear,
            totalDays,
        } = schoolSubscription;

        const {
            handleOpenInvoiceDetail,
        } = useInvoiceComposable();

        onMounted(async () => {
            await handleGetCurrentSubscription();
            await handleGetSchoolYear()
            await handleGetAllSchoolSubscription();
            const status = route.query.status;
            if (status === 'success') {
                notifySuccess('Thanh toán thành công');
                router.replace({
                    path: route.path
                });
            } else if (status === 'failure') {
                notifyError('Thanh toán thất bại! Vui lòng truy cập "Quản lý giao dịch" để thực hiện lại thanh toán"');
                router.replace({
                    path: route.path
                });
            }
        });

        return {
            responseCurrentSchoolSubscription,
            daysUsed,
            progressPercentage,
            isShowModal,
            responseSubscription,
            requestAllSchoolSubscription,
            listSchoolSubscriptionResponse,
            handleGetSubscriptionByDuration,
            formatDate,
            formatCurrency,
            handleOpen,
            handleClose,
            handleUpgrade,
            updateRequestSchoolSubscription,
            calculateShowingEntries,
            handleOpenInvoiceDetail,
            dataSchoolYear,
            totalDays,
        };
    },
});
</script>

    
<style scoped>
.cancel-subscription {
    background-color: #ffffff;
    color: #ff0000;
    border: 1px solid #ff0000;
    transition: all 0.3s ease;
}

.cancel-subscription:hover {
    background-color: #ff0000;
    color: #ffffff;
}
</style>
