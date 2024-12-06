<template>
<div class="container-xxl flex-grow-1 container-p-y">
    <div class="row">
        <InformationComponent/>
        <div class="col-xl-8 col-lg-7 col-md-7 order-0 order-md-1">
            <div class="card mb-4">
                <div id="DataTables_Table_1_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
                    <div class="row mx-6">
                        <div class="col-sm-6 col-12 d-flex align-items-center justify-content-center justify-content-sm-start mt-6 mt-sm-0">
                            <div class="head-label">
                                <h4 class="card-title mb-0" style="margin-top: 10px">Danh sách hóa đơn</h4>
                            </div>
                        </div>
                        <div class="col-sm-6 col-12 d-flex justify-content-center justify-content-md-end align-items-baseline">
                            <div class="dt-action-buttons d-flex justify-content-center flex-md-row align-items-baseline gap-4">
                            </div>
                        </div>
                        <FiltersInvoiceComponent 
                        @updateStatus="(value: number) => updateRequestInvoices('invoiceStatus', value)" 
                        @applyDateFilter="handleDateFilter" 
                        />
                        <div class="dt-buttons" ref="">
                            <button :disabled="!isEmitValue" aria-controls="DataTables_Table_0" class="btn btn-danger btn-small" tabindex="0" type="button" @click="handleShowModalDelete">
                                <span>
                                    <i class="bx bx-trash bx-sm me-md-2"></i>
                                    <span class="d-md-inline-block d-none">Xóa tất cả</span>
                                </span>
                            </button>
                        </div>
                    </div>
                    <div v-if="!listInvoiceResponse.value.data || listInvoiceResponse.value.data.length === 0" 
                    class="text-heading d-flex justify-content-center align-items-center font-weight-bold" style="height: 200px; font-size: 1.25rem;">
                        Không có hóa đơn
                    </div>
                    <InvoiceTableComponent v-else 
                    :invoicesData="listInvoiceResponse.value.data" 
                    :formatDate="formatDate" 
                    :formatCurrency="formatCurrency" 
                    :invoiceId="invoiceId" 
                    @handleDetailClick="handleOpenInvoiceDetail" 
                    @handlePayment="handlePayment" 
                    @delete-invoices="handleDeleteInvoices" 
                    />
                    <div class="row mx-6">
                        <div class="col-sm-12 col-xxl-6 text-center text-xxl-start pb-md-2 pb-xxl-0">
                            <div class="dataTables_info" id="DataTables_Table_1_info" role="status" aria-live="polite">
                                Showing {{ calculateShowingEntries().start }} to {{ calculateShowingEntries().end }} of {{ calculateShowingEntries().total }} entries
                            </div>
                        </div>
                        <div class="col-sm-12 col-xxl-6 d-md-flex justify-content-xxl-end justify-content-center">
                            <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_1_paginate">
                                <PaginationComponent 
                                :current-page="listInvoiceResponse.value.pageNumber" 
                                :page-size="listInvoiceResponse.value.pageSize" 
                                :total-records="listInvoiceResponse.value.totalRecords" 
                                @update:currentPage="(value) => updateRequestInvoices('pageNumber', value)" 
                                @page-changed="handlePageChange" 
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<ModalDeleteMultipleComponent 
:is-show-modal="isCheckedDelete" 
:message="'Bạn có chắc chắn muốn xóa hóa đơn ?'" 
:before-close="handleCloseModalDelete" 
@confirm-action="handleConfirmDelete" 
@close-modal="handleCloseModalDelete" 
/>
</template>

<script lang="ts">
import {
    defineComponent,
    onMounted,
    ref,
} from 'vue';
import PaginationComponent from '@/components/common/Pagination.vue';
import FiltersInvoiceComponent from '@/components/views/school/FilterInvoice.vue';
import InvoiceTableComponent from '@/components/views/school/InvoiceTable.vue';
import ModalDeleteMultipleComponent from '@/components/common/ModalConfirmDelete.vue';
import InformationComponent from '@/components/views/school/Information.vue';
import {
    useInvoiceComposable
} from '@/composables/invoice';
import {
    useSchoolSubscriptionComposable
} from '@/composables/school-supscription';

export default defineComponent({
    name: 'InvoiceManagerComponent',
    props: {},
    components: {
        FiltersInvoiceComponent,
        InvoiceTableComponent,
        PaginationComponent,
        ModalDeleteMultipleComponent,
        InformationComponent,
    },
    setup(props, {
        emit
    }) {

        onMounted(async () => {
            await handleGetListInvoice();
        });

        const {
            requestInvoiceList,
            listInvoiceResponse,
            handleGetListInvoice,
            updateRequestInvoices,
            formatDate,
            formatCurrency,
            calculateShowingEntries,
            invoiceDetailResponse,
            handleOpenInvoiceDetail,
            invoiceId,
            handlePageChange,
            requestDeleteInvoice,
            handleShowModalDelete,
            isCheckedDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            handleDateFilter,
        } = useInvoiceComposable();
        const {
            handlePayment,
        } = useSchoolSubscriptionComposable();

        const isEmitValue = ref(false);
        const handleDeleteInvoices = async (ids: number[]) => {
            if (ids.length === 0) {
                isEmitValue.value = false;
                return;
            }
            isEmitValue.value = true;
            requestDeleteInvoice.ids = ids;
        };

        return {
            handleDeleteInvoices,
            requestInvoiceList,
            listInvoiceResponse,
            updateRequestInvoices,
            formatDate,
            formatCurrency,
            calculateShowingEntries,
            invoiceDetailResponse,
            handleOpenInvoiceDetail,
            handleDateFilter,
            invoiceId,
            handlePayment,
            handleShowModalDelete,
            isEmitValue,
            isCheckedDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            handlePageChange,
        }
    }
})
</script>

<style scoped>
.dt-buttons {
    margin-bottom: 10px;
}

</style>