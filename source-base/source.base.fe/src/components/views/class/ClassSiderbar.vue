<template>
  <nav class="navbar navbar-expand-lg bg-white">
    <div class="container-fluid">
      <a class="navbar-brand highlighted" href="javascript:void(0)" @click="gotoClassDetail">Quản lý lớp</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbar-ex-6">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" id="navbar-ex-6">
        <div class="navbar-nav me-auto">
          <a class="nav-item nav-link" @click="handleGoToTimeTable" href="javascript:void(0)">Thời khóa biểu</a>
          <a class="nav-item nav-link" @click="handleGoToAttendance" href="javascript:void(0)">Điểm danh</a>
          <a class="nav-item nav-link" @click="handleGoToScore" href="javascript:void(0)">Điểm</a>
          <a class="nav-item nav-link" @click="handleGoToFeedback" href="javascript:void(0)">Phản hồi</a>
          <a class="nav-item nav-link" @click="handleGoToClassApplication" href="javascript:void(0)">Đơn từ</a>
          <a class="nav-item nav-link" @click="handleGoToNotification" href="javascript:void(0)">Thông báo</a>
        </div>
        <ul class="navbar-nav ms-lg-auto"></ul>
      </div>
    </div>
  </nav>
</template>
<script lang="ts">
import { useClassDetailComposable } from '@/composables/class';
import { defineComponent, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ROUTER_PATHS } from '@/constants/api/router-paths.ts';
export default defineComponent({
  name: 'ClassSidebarComponent',
  setup() {
    const { goToTimeTable, goToNotification, goToClassApplication, goToClassDetail, gotoFeedback } =
      useClassDetailComposable();
    const route = useRoute();
    const router = useRouter();

    const classId = ref<number>(parseInt(route.query.classId as string));

    const gotoClassDetail = () => {
      goToClassDetail(classId.value);
    };
    const handleGoToTimeTable = () => {
      goToTimeTable(classId.value);
    };
    const handleGoToNotification = () => {
      goToNotification(classId.value);
    };

    const handleGoToClassApplication = () => {
      goToClassApplication(classId.value);
    };

    const handleGoToAttendance = () => {
      router.push({ path: ROUTER_PATHS.TEACHER.ATTENDANCE, query: { classId: classId.value } });
    };
    const handleGoToScore = () => {
      router.push({ path: ROUTER_PATHS.TEACHER.SCORE, query: { classId: classId.value } });
    };
    const handleGoToFeedback = () => {
      gotoFeedback(classId.value);
    };
    return {
      handleGoToClassApplication,
      handleGoToNotification,
      gotoClassDetail,
      handleGoToTimeTable,
      handleGoToAttendance,
      handleGoToScore,
      handleGoToFeedback,
    };
  },
});
</script>
<style>
.highlighted {
  font-size: 1.1rem; /* Increase font size */
  font-weight: bold;
  color: #ffffff;
  padding: 10px 10px;
  text-decoration: none;
}
.highlighted:hover {
  background-color: #ffffff;
}
</style>
