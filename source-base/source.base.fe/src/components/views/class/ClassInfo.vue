<template>
  <div class="row g-6 mb-6 mt-3">
    <div class="col-sm-6 col-xl-5 d-flex">
      <div class="card flex-grow-1">
        <div class="card-body">
          <div class="d-flex align-items-start justify-content-between">
            <div class="content-left">
              <span class="text-heading">Thông tin lớp học</span>
              <div class="d-flex align-items-center my-1">
                <h4 class="mb-0 me-2">{{ classDetail?.className ?? " " }}</h4>
              </div>
              <small class="mb-0">{{ classDetail?.schoolName ?? " " }}</small>
            </div>
            <div class="avatar">
              <span class="avatar-initial rounded bg-label-success"
                ><i class="bx bx-windows"></i>
              </span>
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
                <h4 class="mb-0 me-2" v-if="classDetail?.homeroomTeacher">
                  {{ classDetail.homeroomTeacher?.firstName ?? "" }}
                  {{ classDetail.homeroomTeacher?.lastName ?? " " }}
                </h4>
                <h4 class="mb-0 me-2" v-else>Hiện chưa có giáo viên phụ trách</h4>
              </div>
              <small class="mb-0" v-if="classDetail?.homeroomTeacher"
                >Số điện thoại: {{ classDetail.homeroomTeacher?.phoneNumber ?? " " }}
              </small>
            </div>
            <div class="avatar">
              <span class="avatar-initial rounded bg-label-danger"
                ><i class="bx bxs-user-detail"></i>
              </span>
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
                <h4 class="mb-0 me-2">
                  {{
                    currentSemester.semesterName || "Hiện không có kỳ học nào hoạt động"
                  }}
                </h4>
              </div>
              <small class="mb-0">{{ classDetail?.schoolYearName ?? "" }}</small>
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
import { ResponseGetClassDetailInterface } from "@/types/model/class";
import { defineComponent, PropType } from "vue";
import { useClassDetailComposable } from "@/composables/class";
import { ResponseGetSemesterInterface } from "@/types/model/semester";

export default defineComponent({
  name: "ClassInfoComponent",
  props: {
    classDetail: {
      type: Object as PropType<ResponseGetClassDetailInterface>,
      required: true,
    },
    currentSemester: {
      type: Object as PropType<ResponseGetSemesterInterface>,
      required: true,
    },
  },
  setup(props) {
  },
});
</script>
<style scoped>
.card {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}
</style>
