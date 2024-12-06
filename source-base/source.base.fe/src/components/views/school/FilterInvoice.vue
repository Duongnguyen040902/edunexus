<template>
<div class="dataTables_length" id="DataTables_Table_1_length">
    <div class="filter-status">
        <label>
            <select name="DataTables_Table_1_length" aria-controls="DataTables_Table_1" class="form-select mx-0" v-model="selectedStatus" @change="updateStatus">
                <option v-for="(status, index) in statuses" :value="status.id" :key="index">{{ status.name }}</option>
            </select>
        </label>
    </div>
    <div class="filter-date">
        <el-date-picker placeholder="Chọn ngày bắt đầu" type="date" v-model="startDate"></el-date-picker>
        <el-date-picker placeholder="Chọn ngày kết thúc" type="date" v-model="endDate"></el-date-picker>
        <i class="bx bx-search fs-4 lh-0" @click="applyDateFilter"></i>
    </div>
</div>
</template>

<script>
import {
    defineComponent,
    ref
} from 'vue'
export default defineComponent({
    name: 'FiltersInvoiceComponent',
    props: {
        statuses: {
            type: Array,
            default: () => [{
                    id: null,
                    name: 'Trạng thái'
                },
                {
                    id: 1,
                    name: 'Đang xử lý'
                },
                {
                    id: 2,
                    name: 'Đã thanh toán'
                },
                {
                    id: 3,
                    name: 'Đã hủy'
                },
            ]
        },
    },
    components: {},
    emits: [],
    setup(_, {
        emit
    }) {
        const selectedStatus = ref(null);
        const startDate = ref(null);
        const endDate = ref(null);

        const updateStatus = () => emit('updateStatus', selectedStatus.value);

        const applyDateFilter = () => {
            if (startDate.value || endDate.value) {
                const dateFilter = {
                    startDate: startDate.value ? new Date(startDate.value).toJSON() : null,
                    endDate: endDate.value ? new Date(endDate.value).toJSON() : null,
                };
                emit('applyDateFilter', dateFilter);
            } else {
                const dateFilter = {
                    startDate: null,
                    endDate: null,
                };
                emit('applyDateFilter', dateFilter);
            }
        };

        return {
            selectedStatus,
            updateStatus,
            startDate,
            endDate,
            applyDateFilter
        };
    },
})
</script>

<style scoped>
.dataTables_length {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 10px;
}

.dataTables_length label,
.dataTables_length .el-date-picker,
.dataTables_length i {
    margin: 0;
}

.filter-date {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 10px;
}
.dataTables_length .filter-date i {
    cursor: pointer;
    transition: transform 0.2s ease;
}

.dataTables_length .filter-date i:hover {
    transform: scale(1.2); 
}
.dataTables_length .filter-status select {
    cursor: pointer;
}

.dataTables_length .filter-status select:hover {
    border-color: #007bff;
    box-shadow: 0 0 5px rgba(0, 123, 255, 0.5); 
}

</style>
