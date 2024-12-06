<template>
<div class="filters-container d-flex justify-content-between align-items-center">
    <div class="filter-item">
        <label>
            <select name="status-filter" aria-controls="status-filter" class="form-select" v-model="selectedStatus" @change="updateFilter">
                <option v-for="(status, index) in statuses" :value="status.id" :key="index">{{ status.name }}</option>
            </select>
        </label>
        <label>
            <select name="year-filter" aria-controls="year-filter" class="form-select" v-model="selectedYear" @change="applyYearFilter">
                <option value="" selected>Chọn năm</option>
                <option v-for="(schoolYear, index) in listSchoolYearResponse" :value="getYearFromDate(schoolYear.startDate)" :key="index">
                    {{ schoolYear.name }}
                </option>
            </select>
        </label>
    </div>
</div>
</template>

    
<script>
import {
    defineComponent,
    ref
} from 'vue';

export default defineComponent({
    name: 'FiltersSchoolSubscriptionComponent',
    props: {
        statuses: {
            type: Array,
            default: () => [{
                    id: null,
                    name: 'Trạng thái'
                },
                {
                    id: 1,
                    name: 'Đang kích hoạt'
                },
                {
                    id: 2,
                    name: 'Không kích hoạt'
                },
            ],
        },
        listSchoolYearResponse: {
            type: Array,
            required: true,
        },
    },
    setup(_, {
        emit
    }) {
        const selectedStatus = ref(null);
        const selectedYear = ref('');

        const updateFilter = () => {
            emit('updateStatus', selectedStatus.value);
        };

        const applyYearFilter = () => {
            emit('updateYear', selectedYear.value === '' ? null : selectedYear.value);
        };

        const getYearFromDate = (date) => {
            return new Date(date).getFullYear();
        };

        return {
            selectedStatus,
            selectedYear,
            updateFilter,
            applyYearFilter,
            getYearFromDate,
        };
    },
});
</script>

    
<style scoped>
.filter-item select {
    cursor: pointer; 
}

.filter-item select:hover {
    border-color: #007bff;
    box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
}
.filters-container {
    gap: 1rem;
    flex-wrap: wrap;
    margin: 30px 0px 30px 0px;
}

.filter-item {
    display: flex;
    align-items: center;
    gap: 0.5rem;
}

@media (max-width: 768px) {
    .filters-container {
        flex-direction: column;
    }
}
</style>
