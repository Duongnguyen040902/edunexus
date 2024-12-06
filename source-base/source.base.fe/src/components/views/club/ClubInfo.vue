<template>
  <div class="row g-6 mb-6 mt-3">
  <div class="col-sm-6 col-xl-5 d-flex">
      <div class="card flex-grow-1">
        <div class="card-body">
          <div class="d-flex align-items-start justify-content-between">
            <div class="content-left">
              <span class="text-heading">Thông tin câu lạc bộ</span>
              <div class="d-flex align-items-center my-1">
                <h4 class="mb-0 me-2">{{ clubDetails?.name ?? ' ' }}</h4>
              </div>
              <small class="mb-0">{{ clubDetails?.description ?? ' ' }}</small>
            </div>
            <div class="avatar">
              <span class="avatar-initial rounded bg-label-success"><i class="bx bx-windows"></i> </span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-sm-6 col-xl-4 d-flex">
      <div class="card flex-grow-1">
        <div class="card-body">
          <div class="d-flex align-items-start justify-content-between">
            <div class="content-left">
              <span class="text-heading">Giáo viên</span>
              <div class="d-flex align-items-center my-1">
                <h4 class="mb-0 me-2" v-if="clubDetails?.teacher">
                  {{ clubDetails.teacher?.firstName ?? '' }} {{ clubDetails.teacher?.lastName ?? ' ' }}
                </h4>
                <h4 class="mb-0 me-2" v-else>Hiện chưa có giáo viên phụ trách</h4>
              </div>
              <small class="mb-0" v-if="clubDetails?.teacher"
                >Số điện thoại: {{ clubDetails.teacher?.phoneNumber ?? ' ' }}
              </small>
            </div>
            <div class="avatar">
              <span class="avatar-initial rounded bg-label-danger"><i class="bx bxs-user-detail"></i> </span>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <div class="col-sm-6 col-xl-3 d-flex">
      <div class="card flex-grow-1">
        <div class="card-body">
          <div class="d-flex align-items-start justify-content-between">
            <div class="content-left">
              <span class="text-heading">Học Kỳ</span>
              <div class="d-flex align-items-center my-1">
                <h4 class="mb-0 me-2">{{ currentSemester.semesterName || 'Hiện không có kỳ học nào hoạt động' }}</h4>
              </div>
              <small class="mb-0">{{ clubDetails?.schoolYearName ?? '' }}</small>
            </div>
            <div class="avatar">
              <span class="avatar-initial rounded bg-label-primary">
                <i class="bx bxs-calendar"></i>
              </span>
            </div>
              </div>
            </div>
          </div>
        </div>
      </div>
</template>
<script lang="ts">
import { defineComponent, onMounted, PropType } from 'vue';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { ResponseGetClubDetailInterface } from '@/types/model/club';

export default defineComponent({
  name: 'ClubInfoComponent',
  props: {
    clubDetails: {
      type: Object as PropType<ResponseGetClubDetailInterface>,
      required: true,
    },
    currentSemester: {
      type: Object as PropType<ResponseGetSemesterInterface>,
      required: true,
    },
  },
  setup(props) {
    onMounted(() => {
      console.log('clubDetail', props.clubDetails);
    });

    return {};
  },
});
</script>
