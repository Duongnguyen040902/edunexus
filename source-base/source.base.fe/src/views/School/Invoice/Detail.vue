<template>
<div class="container-xxl flex-grow-1 container-p-y">
    <div class="col-xl-9 col-md-8 col-12 mb-md-0 mb-6">
        <div class="card invoice-preview-card p-sm-12 p-6">
            <div class="card-body invoice-preview-header rounded">
                <h4 class="mb-2">Chi tiết hóa đơn</h4>
                <div class="d-flex justify-content-between flex-xl-row flex-md-column flex-sm-row flex-column align-items-xl-center align-items-md-start align-items-sm-center align-items-start">
                    <div class="mb-xl-0 mb-6 text-heading">
                        <table>
                            <tbody>
                                <tr>
                                    <td class="pe-4">Gói đăng ký:</td>
                                    <td class="fw-medium">{{ invoiceDetailResponse.value.subscriptionPlanName }}</td>
                                </tr>
                                <tr>
                                    <td class="pe-4">Thời hạn:</td>
                                    <td>{{ invoiceDetailResponse.value.durationDays === 30 ? '1 tháng' : '1 năm'}}</td>
                                </tr>
                                <tr>
                                    <td class="pe-4">Ngày hết hiệu lực gói:</td>
                                    <td>{{ formatDate(invoiceDetailResponse.value.endDate) }}</td>
                                </tr>
                                <tr>
                                    <td class="pe-4">Đơn giá:</td>
                                    <td>{{ formatCurrency(invoiceDetailResponse.value.totalAmount) }}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <table>
                        <thead>
                            <tr>
                                <th colspan="2">
                                    <h5 class="mb-6">Hóa đơn #{{ invoiceDetailResponse.value.id }}</h5>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="pe-4">Ngày phát hành:</td>
                                <td class="fw-medium">{{ formatDate(invoiceDetailResponse.value.issueDate) }}</td>
                            </tr>
                            <tr>
                                <td class="pe-4">Ngày hết hạn:</td>
                                <td class="fw-medium">{{ formatDate(invoiceDetailResponse.value.dueDate) }}</td>
                            </tr>
                            <tr>
                                <td class="pe-4">Trạng thái:</td>
                                <td class="fw-medium" style="margin-top: 40px;">{{ invoiceDetailResponse.value.statusName }}</td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </div>
            <div class="card-body px-0">
                <div class="row">
                    <div class="col-xl-6 col-md-12 col-sm-7 col-12">
                    </div>
                </div>
            </div>
            <span v-if="!invoiceDetailResponse.value.payments || invoiceDetailResponse.value.payments.length === 0" 
            class="text-heading d-flex justify-content-center align-items-center font-weight-bold" style="height: 200px; font-size: 1.25rem;">
                Chưa được thực hiện thanh toán
            </span>
            <PaymentTableComponent v-else 
            :responseInvoiceDetail="invoiceDetailResponse.value" 
            :formatDate="formatDate" 
            :formatCurrency="formatCurrency" 
            />

            <div class="table-responsive">
                <table class="table m-0 table-borderless">
                    <tbody>
                        <tr>
                            <td class="align-top pe-6 ps-0 py-6 text-body">
                                <p class="mb-1">
                                    <span class="me-2 h6"></span>
                                    <span></span>
                                </p>
                                <span></span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <hr class="mt-0 mb-6">
            <div class="card-body p-0">
                <div class="row">
                    <div class="col-12">
                        <span class="fw-medium text-heading">Note:</span>
                        <span>It was a pleasure working with you and your team. We hope you will keep us in mind for future freelance projects. Thank You!</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</template>

    
<script lang="ts">
import {
    defineComponent,
    onMounted,
} from 'vue';
import PaymentTableComponent from '@/components/views/school/PaymentTable.vue';
import {
    useInvoiceComposable
} from '@/composables/invoice';
import {
    useRouter
} from 'vue-router';

export default defineComponent({
    name: 'InvoiceDetailComponent',
    props: {},
    components: {
        PaymentTableComponent,
    },
    setup(props, {
        emit
    }) {
        const router = useRouter();
        const invoiceComposable = useInvoiceComposable();
        const {
            invoiceDetailResponse,
            formatDate,
            formatCurrency,
            handleFetchInvoiceDetail
        } = invoiceComposable;

        onMounted(async () => {
            if (router.currentRoute.value.params.id) {
                console.log("router.currentRoute.value.params.id", Number(router.currentRoute.value.params.id));
                await handleFetchInvoiceDetail(Number(router.currentRoute.value.params.id));
            }
        });

        return {
            invoiceDetailResponse,
            formatDate,
            formatCurrency,
        };
    }
})
</script>
