<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="col-xxl-12 mb-6 order-0">
      <div class="card">
        <div class="d-flex align-items-start row">
          <div class="col-sm-7">
            <div class="card-body">
              <h4 class="card-title text-primary mb-3">Chào mừng đến với EduNexus</h4>
              <p class="mb-6" style="max-width: 700px">
                Chúng tôi tự hào là cầu nối giữa nhà trường và phụ huynh, đồng hành cùng bạn trên hành trình phát triển
                giáo dục.
              </p>
            </div>
          </div>
          <div class="col-sm-5 text-center text-sm-left">
            <div class="card-body pb-0 px-0 px-md-6">
              <img
                src="@assets/images/avatars/man-with-laptop.png"
                height="175"
                class="scaleX-n1-rtl"
                alt="View Badge User"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row g-6">
      <div class="col-lg-3 col-sm-6">
        <div class="card card-border-shadow-primary h-100">
          <div class="card-body">
            <div class="d-flex align-items-center mb-2">
              <div class="avatar me-4">
                <span class="avatar-initial rounded bg-label-primary"><i class="bx bx-chalkboard bx-lg"></i></span>
              </div>
              <h4 class="mb-0">{{ dashboard.countActiveClass }}</h4>
            </div>
            <p class="mb-2">Lớp học</p>
          </div>
        </div>
      </div>

      <div class="col-lg-3 col-sm-6">
        <div class="card card-border-shadow-warning h-100">
          <div class="card-body">
            <div class="d-flex align-items-center mb-2">
              <div class="avatar me-4">
                <span class="avatar-initial rounded bg-label-warning"><i class="bx bx-group bx-lg"></i></span>
              </div>
              <h4 class="mb-0">{{ dashboard.countActiveClub }}</h4>
            </div>
            <p class="mb-2">Câu lạc bộ</p>
          </div>
        </div>
      </div>

      <div class="col-lg-3 col-sm-6">
        <div class="card card-border-shadow-danger h-100">
          <div class="card-body">
            <div class="d-flex align-items-center mb-2">
              <div class="avatar me-4">
                <span class="avatar-initial rounded bg-label-danger"><i class="bx bxs-bus bx-lg"></i></span>
              </div>
              <h4 class="mb-0">{{ dashboard.countActiveBusRoute }}</h4>
            </div>
            <p class="mb-2">Tuyến Xe</p>
          </div>
        </div>
      </div>

      <div class="col-lg-3 col-sm-6">
        <div class="card card-border-shadow-info h-100">
          <div class="card-body">
            <div class="d-flex align-items-center mb-2">
              <div class="avatar me-4">
                <span class="avatar-initial rounded bg-label-info"><i class="bx bx-calendar bx-lg"></i></span>
              </div>
              <h4 class="mb-0">{{ dashboard.semesterName }}</h4>
            </div>
            <p class="mb-2">Kỳ hiện tại</p>
          </div>
        </div>
      </div>

      <div class="col-12 col-xl-6 col-md-6">
        <div class="card h-100">
          <div class="card-header d-flex align-items-center justify-content-between">
            <div class="card-title mb-0">
              <h5 class="m-0 me-2">Tổng số tài khoản</h5>
            </div>
          </div>
          <div class="px-5 py-4 border border-start-0 border-end-0">
            <div class="d-flex justify-content-between align-items-center">
              <p class="mb-0 text-uppercase">Loại</p>
              <p class="mb-0 text-uppercase">Tổng</p>
            </div>
          </div>
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-6">
              <div class="d-flex align-items-center">
                <div>
                  <div>
                    <h6 class="mb-0 text-truncate">Giáo viên</h6>
                  </div>
                </div>
              </div>
              <div class="text-end">
                <h6 class="mb-0">{{ dashboard.countActiveTeacher }}</h6>
              </div>
            </div>
            <div class="d-flex justify-content-between align-items-center mb-6">
              <div class="d-flex align-items-center">
                <div>
                  <div>
                    <h6 class="mb-0 text-truncate">Học sinh</h6>
                  </div>
                </div>
              </div>
              <div class="text-end">
                <h6 class="mb-0">{{ dashboard.countActivePupil }}</h6>
              </div>
            </div>
            <div class="d-flex justify-content-between align-items-center mb-6">
              <div class="d-flex align-items-center">
                <div>
                  <div>
                    <h6 class="mb-0 text-truncate">Giám sát xe tuyến</h6>
                  </div>
                </div>
              </div>
              <div class="text-end">
                <h6 class="mb-0">{{ dashboard.countActiveSupervisor }}</h6>
              </div>
            </div>
            <div class="d-flex justify-content-between align-items-center mb-6">
              <div class="d-flex align-items-center">
                <div>
                  <h6 class="mb-0 text-truncate">Tổng số tài khoản đang hoạt động</h6>
                </div>
              </div>
              <div class="text-end">
                <h6 class="mb-0">{{ dashboard.totalActiveAccount }}</h6>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-12 col-xl-6 col-md-6">
        <div class="card h-100">
          <div class="card-header d-flex align-items-center justify-content-between">
            <div class="card-title mb-0">
              <h5 class="m-0 me-2">Gói đăng ký</h5>
            </div>
          </div>
          <div class="card-body">
            <p class="mb-2">
              Đang sử dụng gói {{ responseCurrentSchoolSubscription.value.subscriptionPlanName || 'N/A' }}
            </p>
            <p class="text-muted mb-4">{{ responseCurrentSchoolSubscription.value.description || 'N/A' }}</p>
            <p class="mb-2">Có hiệu lực đến ngày {{ formatDate(responseCurrentSchoolSubscription.value.endDate) }}</p>
            <p class="text-muted mb-4">Chúng tôi sẽ gửi cho bạn thông báo khi Đăng ký hết hạn</p>
            <div class="d-flex justify-content-between mb-1">
              <small>Ngày</small>
              <small>{{ totalDays }} Ngày</small>
            </div>
            <div class="progress" style="height: 8px">
              <div
                class="progress-bar bg-primary"
                role="progressbar"
                :style="{ width: progressPercentage + '%' }"
                aria-valuenow="40"
                aria-valuemin="0"
                aria-valuemax="100"
              ></div>
            </div>
            <p class="text-muted mt-1">
              Còn {{ Math.max(totalDays - daysUsed, 0) }} ngày nữa là đến ngày gói của bạn cần cập nhật
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useSchoolDetailComposable } from '@/composables/school';
import { useSchoolSubscriptionComposable } from '@/composables/school-supscription';

export default defineComponent({
  name: 'SchoolAdminDashboard',
  setup() {
    const { fetchDashboard, dashboard } = useSchoolDetailComposable();
    const {
      responseCurrentSchoolSubscription,
      daysUsed,
      progressPercentage,
      totalDays,
      formatDate,
      handleGetCurrentSubscription,
    } = useSchoolSubscriptionComposable();

    onMounted(async () => {
      await fetchDashboard();
      await handleGetCurrentSubscription();
    });

    return {
      dashboard,
      responseCurrentSchoolSubscription,
      daysUsed,
      progressPercentage,
      totalDays,
      formatDate,
    };
  },
});
</script>
