<template>
    <div class="card mt-5">
        <div class="card-header border-bottom">
            <h5 class="card-title mb-0">Danh sách học sinh</h5>
        </div>
        <div class="table-responsive text-nowrap">
            <table class="table">
                <thead>
                    <tr>
                        <th>Số thứ tự</th>
                        <th>Họ và Tên</th>
                        <th>Ảnh</th>
                        <th>Giới tính</th>
                        <th>Họ và Tên (Phụ Huynh)</th>
                        <th>Số Điện Thoại (Phụ Huynh)</th>
                        <th>Điểm dừng</th>
                    </tr>
                </thead>
                <tbody class="table-border-bottom-0">
                    <tr v-for="(pupil,index) in busDetail?.pupil ?? []" :key="pupil.id">
                        <td>{{ index + 1 }}</td>
                        <td><strong>{{ pupil.pupilName}}</strong></td>
                        <td>
                            <ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">
                                <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top"
                                    class="avatar avatar-xl pull-up" :title="pupil.pupilName">
                                    <img :src="`${apiUrl}${pupil.image}`" alt="Avatar" class="rounded-circle" />
                                </li>
                            </ul>
                        </td>
                        <td>{{ pupil.gender ? gender.MALE : gender.FEMALE }}</td>
                        <td>{{ pupil.donorName }}</td>
                        <td>{{ pupil.donorPhoneNumber }}</td>
                        <td>{{ pupil.busStopAddress }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { Gender } from '@/constants/enums/gender';
import { ResponseGetBusDetail } from '@/types/model/bus';

export default defineComponent({
    name: 'TableBusEnrollmentComponent',
    props: {
        busDetail: {
            type: Object as PropType<ResponseGetBusDetail>,
            required: true
        }
    },
    setup(props) {
        const apiUrl = import.meta.env.VITE_APP_API_URL;
        const gender = Gender;
        return { apiUrl, gender };
    }
});
</script>