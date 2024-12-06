<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <div class="card mt-3">
      <div class="card table-responsive" style="margin-bottom: 0">
        <div class="dataTables_wrapper no-footer">
          <div class="card-header border-bottom d-flex justify-content-between align-items-center">
            <h5 class="card-title mb-0">Tổng hợp điểm</h5>
            <div class="d-flex justify-content-end align-items-center col-md-7">
              <div class="col-md-6 user_role d-flex align-items-center">
                <h6 class="mb-0 me-2 nowrap-text">Chọn năm:</h6>
                <select
                  id="UserRole"
                  class="form-select text-capitalize"
                  v-model="selectedSemester"
                  @change="handleSelect(selectedSemester)"
                >
                  <option v-for="semester in filteredSemesters" :key="semester.id" :value="semester.id">
                    {{ semester.schoolYearName }}
                  </option>
                </select>
              </div>
            </div>
          </div>
          <table class="table" v-if="individualScore.length != 0">
            <thead>
            <tr>
              <th class="col-1">Môn học</th>
              <th class="col-2">Điểm kỳ 1</th>
              <th class="col-2">Điểm kỳ 2</th>
              <th class="col-2">Trung bình năm</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="(score, index) in individualScore" :key="index">
              <td>{{ score.subjectName }}</td>
              <td v-if="score.scores && score.scores.length > 0">{{ score.scores[0] || '-'}}</td>
              <td v-else>-</td>
              <td v-if="score.scores && score.scores.length > 1">{{ score.scores[1] || '-'}}</td>
              <td v-else>-</td>
              <td v-if="score.scores && score.scores.length > 1">{{ (score.scores[0] !== null && score.scores[1] !== null) ? ((score.scores[0] + 2 * score.scores[1]) / 3).toFixed(1) : '-' }}</td>
              <td v-else>-</td>
            </tr>
            </tbody>
          </table>
          <div v-else class="text-center p-3">Hiện chưa có điểm trong kỳ này</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { useScoreComposable } from '@/composables/score.ts';
import { defineComponent, onMounted, ref, computed } from 'vue';

export default defineComponent({
  name: 'PupilScoreComponent',
  components: {},
  setup(props, { emit }) {
    const { individualScore, fetchIndividualScore, fetchSemester, semester, currentSemester, fetchCurrentSemester } =
      useScoreComposable();

    const selectedSemester = ref('');

    const handleSelect = (id: number) => {
      fetchIndividualScore({ semesterId: id });
    };

    const filteredSemesters = computed(() => {
      const seenYears = new Set();
      return semester.value.filter(s => {
        if (s.id === currentSemester.id) {
          return true;
        }
        if (seenYears.has(s.schoolYearName)) {
          return false;
        } else {
          seenYears.add(s.schoolYearName);
          return true;
        }
      });
    });

    onMounted(async () => {
      await fetchSemester();
      await fetchCurrentSemester();
      selectedSemester.value = currentSemester.id;
      handleSelect(currentSemester.id);
    });

    return { individualScore, semester, selectedSemester, handleSelect, filteredSemesters };
  },
});
</script>

<style scoped>
.nowrap-text {
  white-space: nowrap;
}
</style>