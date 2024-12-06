<template>
    <div class="d-flex justify-content-between align-items-center row pt-4 g-4 col-md-5">
        <div class="col-md-6 user_role">
            <select id="UserRole" class="form-select text-capitalize" v-model="selectedSemester" @change="onFilterChange">
        <option value="">Chọn kỳ</option>
                <option v-for="semester in semester.value" :key="semester.id" :value="semester.id">
                    {{ semester.semesterName }} - {{ semester.schoolYearName }}
                </option>
            </select>
        </div>
        <div class="col-md-6 user_plan">
            <input type="date" class="form-control" v-model="selectedDate" @change="onFilterChange" />
        </div>
    </div>
</template>

<script lang="ts">
import { useAttendanceComposable } from '@/composables/class-attendance';
import { defineComponent, onMounted, ref } from 'vue';
import { getCurrentDateFormatted } from '@/helpers/formatDate.ts';
export default defineComponent({
    name: 'FilterAttendance',
    components: {},
    props: {},
    setup(props, { emit }) {
        const selectedSemester = ref('');
    const selectedDate = ref(getCurrentDateFormatted());
        const { semester, fetchSemester, currentSemester, fetchCurrentSemester } = useAttendanceComposable();

        const onFilterChange = () => {
            emit('filter-change', { semesterId: selectedSemester.value, date: selectedDate.value });
        };

        onMounted(async () => {
            await fetchSemester();
            await fetchCurrentSemester();
            if (currentSemester) {
                selectedSemester.value = currentSemester.id.toString();
                onFilterChange(); // Emit the filter change event with the default values
            }
        });

        return { semester, onFilterChange, selectedSemester, selectedDate };
  },
});
</script>