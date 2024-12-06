<template>
  <div class="card">
    <div class="card-header border-bottom">
      <h5 class="card-title mb-0">Danh sách lớp</h5>
    </div>
    <div class="table-responsive text-nowrap">
      <template v-if="clubDetail?.pupils?.length > 0">
        <table class="table">
          <thead>
          <tr>
            <th>Stt</th>
            <th>Họ và Tên</th>
            <th>Ảnh</th>
            <th>Giới tính</th>
            <th>Họ và Tên (Phụ Huynh)</th>
            <th>Số Điện Thoại (Phụ Huynh)</th>
          </tr>
          </thead>
          <tbody class="table-border-bottom-0">
          <tr v-for="(pupil, index) in clubDetail.pupils" :key="pupil.id">
            <td>{{ index + 1 }}</td>
            <td><strong>{{ pupil.firstName }} {{ pupil.lastName }}</strong></td>
            <td>
              <ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">
                <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top"
                    class="avatar avatar-xl pull-up" :title="pupil.firstName + ' ' + pupil.lastName">
                  <img :src="`${apiUrl}${pupil.image}`" alt="Avatar" class="rounded-circle" />
                </li>
              </ul>
            </td>
            <td>{{ pupil.gender ? gender.MALE : gender.FEMALE }}</td>
            <td>{{ pupil.donorName }}</td>
            <td>{{ pupil.donorPhoneNumber }}</td>
          </tr>
          </tbody>
        </table>
      </template>
      <template v-else>
        <p class="text-center">Kỳ này chưa có học sinh</p>
      </template>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { ResponseGetClubDetailInterface } from '@/types/model/club';
import { Gender } from '@/constants/enums/gender';

export default defineComponent({
  name: 'TableClubEnrollmentComponent',
  props: {
    clubDetail: {
      type: Object as PropType<ResponseGetClubDetailInterface>,
      required: true
    }
  },
  setup(props) {
    const apiUrl = (import.meta as any).env.VITE_APP_API_URL;
    const gender = Gender;
    return { apiUrl, gender };
  }
});
</script>