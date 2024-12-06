<template>
<div class="card-header">
    <h5 class="card-title mb-0">Search Filters</h5>
    <div class="d-flex justify-content-between align-items-center row pt-4 gap-4 gap-md-0 g-6">
        <div class="col-md-4 user_role">
            <div class="nav-item d-flex align-items-center">
                <i class="bx bx-search fs-4 lh-0" @click="performSearch"></i>
                <input type="text" class="form-control border-0 shadow-none" placeholder="Search..." aria-label="Search..." 
                v-model="searchKey" 
                @keydown.enter="performSearch" 
                />
            </div>
        </div>
        <div class="col-md-4 user_plan">
            <select class="form-select text-capitalize" v-model="selectedSubject" @change="updateSubject">
                <option v-for="subject in computedSubjectFilter" :key="subject.id" :value="subject.id">
                    {{ subject.name }}
                </option>
            </select>
        </div>
        <div class="col-md-4 user_status">
            <select class="form-select text-capitalize" v-model="selectedStatus" @change="updateStatus">
                <option v-for="(status, index) in statuses" :value="status.id" :key="index">{{ status.name }}</option>
            </select>
        </div>
        <div class="col-md-4 user_status">
        </div> 
    </div>
</div>
</template>

<script lang="ts">
import {
    computed,
    defineComponent,
    ref
} from 'vue'
export default defineComponent({
    name: 'SearchFiltersTeacherComponent',
    props: {
        subjectFilter: {
            type: Array,
            required: true,
        },
        statuses: {
            type: Array,
            default: () => [{
                    id: null,
                    name: 'Trạng thái'
                },
                {
                    id: 1,
                    name: 'Kích hoạt'
                },
                {
                    id: 2,
                    name: 'Chưa kích hoạt'
                },
                {
                    id: 3,
                    name: 'Đã vô hiệu hóa'
                },
            ]
        }, 
    },
    setup(props, {
        emit
    }) {

        const computedSubjectFilter = computed(() => [{
                id: null,
                name: 'Chọn Chuyên Môn'
            },
            ...props.subjectFilter
        ]);

        const searchKey = ref('');
        const selectedSubject = ref(null);
        const selectedStatus = ref(null);
        const selectedRecord = ref(10);

        const performSearch = () => {
            emit('updateSearch', searchKey.value);
        };

        const updateSubject = () => emit('updateSubject', selectedSubject.value);
        const updateStatus = () => emit('updateStatus', selectedStatus.value);

        return {
            computedSubjectFilter,
            searchKey,
            selectedSubject,
            selectedStatus,
            selectedRecord,
            performSearch,
            updateSubject,
            updateStatus,
        };
    },
});
</script>
