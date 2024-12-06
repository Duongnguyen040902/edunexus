<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="row overflow-hidden">
      <h2>Lớp học của tôi</h2>
      <div class="col-12">
        <ul class="timeline timeline-center mt-12" v-if="pupilClasses.length > 0">
          <li
            v-for="pupilClass in pupilClasses"
            :key="pupilClass.id"
            class="timeline-item"
            style="padding-bottom: 5px"
            @click="handleClassClick(pupilClass.classId, pupilClass.semesterId)"
          >
            <span
              class="timeline-indicator timeline-indicator-success aos-init aos-animate"
              data-aos="zoom-in"
              data-aos-delay="200"
            >
              <i class="bx bx-book"></i>
            </span>
            <div class="timeline-event card p-0 aos-init aos-animate" data-aos="fade-right">
              <h6 class="card-header" style="padding: 10px">{{ pupilClass.className }}</h6>
              <div class="card-body" style="padding: 10px">
                <ul class="list-unstyled">
                  <li class="d-flex justify-content-start align-items-center text-success mb-4">
                    <i class="bx bx-calendar bx-sm me-4"></i>
                    <div class="ps-4 border-start">
                      <small class="text-muted mb-1">Kỳ</small>
                      <h5 class="mb-0">{{ pupilClass.semesterName }}, Thời gian: {{ formatDate(pupilClass.startDate)}} - {{formatDate(pupilClass.endDate)}}</h5>
                    </div>
                  </li>
                  <li class="d-flex justify-content-start align-items-center text-info mb-1">
                    <i class="bx bx-calendar-alt bx-sm me-4"></i>
                    <div class="ps-4 border-start">
                      <small class="text-muted mb-1">Năm học</small>
                      <h5 class="mb-0">{{ pupilClass.schoolYearName }}</h5>
                    </div>
                  </li>
                </ul>
              </div>
              <div class="timeline-event-time">
                {{ getClassStatus(pupilClass) }}
              </div>
            </div>
          </li>
        </ul>
        <div v-else class="text-center mt-4">
          <p>Chưa tham gia lớp học nào</p>
        </div>
      </div>
    </div>
    <ModalViewClassDetail :showModal="showModal" :classDetail="classDetail" @update-showModal="showModal = $event" />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useClassDetailComposable } from '@/composables/class.ts';
import ModalViewClassDetail from '@/components/modals/class/ClassDetailModal.vue';
import { formatDate } from '@/helpers/formatDate.ts';

export default defineComponent({
  name: 'MyClass',
  components: { ModalViewClassDetail },
  metaInfo: {
    title: 'Lớp học của tôi',
  },
  setup() {
    const { classDetail, pupilClasses, fetchPupilClasses, fetchPupilClassDetail } = useClassDetailComposable();
    const showModal = ref(false);

    onMounted(async () => {
      console.log('Component mounted');
      await fetchPupilClasses();
    });

    const handleClassClick = async (classId: number, semesterId: number) => {
      await fetchPupilClassDetail(classId, semesterId);
      showModal.value = true;
    };

    const getClassStatus = (pupilClass: any) => {
      const currentDate = new Date();
      const startDate = new Date(pupilClass.startDate);
      const endDate = new Date(pupilClass.endDate);

      if (endDate < currentDate) {
        return 'Đã kết thúc';
      } else if (startDate > currentDate) {
        return 'Sắp tới';
      } else {
        return 'Đang học';
      }
    };

    return {
      pupilClasses,
      showModal,
      classDetail,
      handleClassClick,
      formatDate,
      getClassStatus,
    };
  },
});
</script>