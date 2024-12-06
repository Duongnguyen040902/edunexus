<template>
    <nav class="navbar navbar-expand-lg bg-white">
        <div class="container-fluid">
            <a class="navbar-brand highlighted" href="javascript:void(0)" @click="gotoClubDetail">Quản lý câu lạc bộ</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbar-ex-6">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbar-ex-6">
                <div class="navbar-nav me-auto">
                    <a class="nav-item nav-link" @click="handleGoToAttendance" href="javascript:void(0)">Điểm danh</a>
                </div>
                <ul class="navbar-nav ms-lg-auto">
                </ul>
            </div>
        </div>
    </nav>
</template>
<script lang="ts">

import { useClassDetailComposable } from '@/composables/class';
import { defineComponent, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
export default defineComponent({
    name: 'ClubSidebarComponent',
    setup() {
        const { goToTimeTable, goToNotification } = useClassDetailComposable();
        const route = useRoute();
        const router = useRouter();

        const clubId = ref<number>(parseInt(route.query.clubId as string));
        
        const gotoClubDetail = () => {
            router.push({ path: '/teacher/club-detail', query: { clubId: clubId.value } });
        };

        const handleGoToAttendance =() => {
            router.push({ path: '/teacher/club-attendance', query: { clubId: clubId.value } });
        }


        return {
            handleGoToAttendance,
            gotoClubDetail
        };
    }
});
</script>
<style scoped>
.highlighted {
  font-size: 1.1rem; /* Increase font size */
  font-weight: bold; /* Make text bold */
  color: #ffffff; /* Change text color */
  padding: 10px 10px; /* Add padding */
  text-decoration: none; /* Remove underline */
}

.highlighted:hover {
  background-color: #ffffff; /* Change background color on hover */
}
</style>