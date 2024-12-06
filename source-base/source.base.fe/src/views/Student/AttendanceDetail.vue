<template>
    <div class="container-xxl flex-grow-1 container-p-y">
        <div class="card">
            <div class="card table-responsive" style="margin-bottom: 0;">
                <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
                    <div class="card-header border-bottom">
                        <h5 class="card-title mb-0">Tổng hợp điểm danh</h5>
                        <FilterAttendance @filter-change="handleFilterChange" />
                    </div>
                    <table class="table" style="margin-bottom: 0;">
                        <thead>
                            <tr>
                                <th class="col-1">Ngày</th>
                                <th class="col-2" v-if="pupilAttendance?.pupilAttendanceMaterial.className != null">{{ pupilAttendance?.pupilAttendanceMaterial.className }}</th>
                                <th v-if="pupilAttendance?.pupilAttendanceMaterial.clubName != null" v-for="(clubName, index) in pupilAttendance?.pupilAttendanceMaterial.clubName" :key="index">
                                    {{ clubName }}
                                </th>
                                <th class="col-2" v-if="pupilAttendance?.pupilAttendanceMaterial.busName != null">{{ pupilAttendance?.pupilAttendanceMaterial.busName ?? "Không tham gia xe bus" }}</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item, index) in pupilAttendance?.pupilViewAttendanceDTO" :key="index">
                                <td class="control" tabindex="0">{{ formatDate(item.createDate) }}</td>
                                <td v-if="pupilAttendance?.pupilAttendanceMaterial.className != null">
                                    <span class="feedback-container">
                                        <i :class="[getAttendanceIcon(item.isAttendClass), item.isAttendClass?.feedback ? 'blink-icon' : '']"></i>
                                        <span class="feedback-text">{{ item.isAttendClass?.feedback }}</span>
                                    </span>
                                </td>
                                <td v-if="pupilAttendance?.pupilAttendanceMaterial.clubName != null" v-for="(club, clubIndex) in pupilAttendance?.pupilAttendanceMaterial.clubName" :key="clubIndex" class="">
                                    <span class="feedback-container">
                                        <i :class="[getAttendanceIcon(item.isAttendClub?.[clubIndex]), item.isAttendClub?.[clubIndex]?.feedback ? 'blink-icon' : '']"></i>
                                        <span class="feedback-text">{{ item.isAttendClub?.[clubIndex]?.feedback }}</span>
                                    </span>
                                </td>
                                <td v-if="pupilAttendance?.pupilAttendanceMaterial.busName != null" class="">
                                    <span class="feedback-container">
                                        <i :class="[getAttendanceIcon(item.isAttendBus), item.isAttendBus?.feedback ? 'blink-icon' : '']"></i>
                                        <span class="feedback-text">{{ item.isAttendBus?.feedback }}</span>
                                    </span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div style="width: 1%;"></div>
                </div>
            </div>
            <div class="card" style="margin-top: 0;">
                <div class="legend">
                    <span><i class="bx bx-message-alt-check text-success blink-icon"></i> Có phản hồi</span>
                    <span><i class="bx bx-message-alt-check text-success"></i> Có mặt</span>
                    <span><i class="bx bx-message-alt-x text-danger"></i> Vắng</span>
                    <span><i class="bx bx-message-square-minus text-warning"></i> Chưa điểm danh</span>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { useAttendanceComposable } from "@/composables/class-attendance";
import { defineComponent, onMounted, ref } from "vue";
import FilterAttendance from "@/components/views/pupil/FilterAttendance.vue"; // Adjust the path as needed

export default defineComponent({
    name: "StudentAttendanceDetail",
    components: {
        FilterAttendance
    },
    setup() {
        const { pupilAttendance, fetchPullAttendance } = useAttendanceComposable();
        const feedback = ref<string | null>(null);
        const isFeedbackModalVisible = ref(false);

        const getAttendanceIcon = (attendance: { isAttend?: boolean | null | undefined } | null | undefined): string => {
            if (attendance === null || attendance === undefined || attendance.isAttend === null) {
                return "bx bx-message-square-minus text-warning";
            } else if (attendance.isAttend === false) {
                return "bx bx-message-alt-x text-danger";
            } else {
                return "bx bx-message-alt-check text-success";
            }
        };

        const formatDate = (date: Date): string => {
            const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: '2-digit', day: '2-digit' };
            return new Date(date).toLocaleDateString('vi-VN', options);
        };

        const handleFilterChange = (filter: { semesterId: string; date: string }) => {
            const params = {
                semesterId: parseInt(filter.semesterId),
                date: new Date(filter.date)
            };
            fetchPullAttendance(params);
        };

        const showFeedback = (feedbackText: string | null | undefined) => {
            feedback.value = feedbackText || "No feedback available";
            isFeedbackModalVisible.value = true;
        };

        const closeFeedbackModal = () => {
            isFeedbackModalVisible.value = false;
        };

        onMounted(() => {
            fetchPullAttendance({ semesterId: 4, date: new Date() });
        });

        return { pupilAttendance, getAttendanceIcon, formatDate, handleFilterChange, showFeedback, closeFeedbackModal, feedback, isFeedbackModalVisible };
    },
});
</script>

<style scoped>
.legend {
    display: flex;
    margin-right: 15px;
    justify-content: flex-end;
    padding: 10px 0;
    gap: 10px;
}

.legend span {
    display: flex;
    align-items: center;
    gap: 5px;
}

.feedback-container {
    position: relative;
    display: inline-block;
}

.feedback-text {
    visibility: hidden;
    width: 200px;
    background-color: rgb(255, 254, 239);
    color: #000000;
    text-align: center;
    border-radius: 6px;
    padding: 5px 0;
    position: absolute;
    z-index: 1;
    bottom: 125%; /* Position the tooltip above the icon */
    left: 50%;
    margin-left: -100px;
    opacity: 0;
    transition: opacity 0.3s;
}

.feedback-container:hover .feedback-text {
    visibility: visible;
    opacity: 1;
}

.blink-icon {
    animation: blink 1s infinite;
}

@keyframes blink {
    50%, 100% {
        opacity: 6;
    }
    50% {
        opacity: 0.4;
    }
}
</style>