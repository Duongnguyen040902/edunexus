<template>
    <nav class="navbar navbar-expand-lg bg-white">
        <div class="container-fluid">
      <a class="navbar-brand highlighted" href="javascript:void(0)" @click="goToBusDetail(busId)">Quản lý xe tuyến</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbar-ex-6">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbar-ex-6">
                <div class="navbar-nav me-auto">
                    <a class="nav-item nav-link" @click="goToBusAttendance(busId)" href="javascript:void(0)">Điểm danh</a>
                </div>
        <ul class="navbar-nav ms-lg-auto"></ul>
            </div>
        </div>
    </nav>
</template>
<script lang="ts">
import { useBusDetailComposable } from '@/composables/bus';
import { defineComponent, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
export default defineComponent({
    name: 'BusSidebarComponent',
    setup() {
    const { goToBusDetail, busId, goToBusAttendance } = useBusDetailComposable();

        return {
            busId,
            goToBusDetail,
      goToBusAttendance,
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
