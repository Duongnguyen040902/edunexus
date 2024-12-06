<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <ClassSidebarComponent />
    <div class="card mt-3">
      <div class="card" style="margin-bottom: 0">
          <div class="card-header border-bottom d-flex justify-content-between align-items-center">
            <h5 class="card-header mb-0">Tổng hợp điểm</h5>
            <div class="dropdown">
              <button
                class="btn btn-primary dropdown-toggle"
                type="button"
                id="dropdownMenuButton"
                data-bs-toggle="dropdown"
                aria-expanded="false"
              >
                Tạo hồ sơ điểm
              </button>
              <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                <li v-for="subject in subjects.value" :key="subject.id">
                  <a class="dropdown-item" @click="handleOpenModal(false, subject.id)" href="#">{{ subject.name }}</a>
                </li>
              </ul>
            </div>
          </div>
        <div class="table-responsive text-nowrap">
          <table class="table table-responsive" style="margin-bottom: 0">
            <thead>
              <tr>
                <th class="col-1">Stt</th>
                <th class="col-2">Họ và Tên</th>
                <th v-for="subject in classScore?.subjects" :key="subject.id" class="col-1 align-middle text-center">
                  <div class="d-flex align-items-center justify-content-center">
                    {{ subject.name }}
                    <i class="bx bx-edit" style="margin-left: 5px" @click="handleOpenModal(true, subject.id)"></i>
                  </div>
                  <div class="d-flex justify-content-between mt-2 text-center">
                    <span class="col-3" style="font-weight: normal">Kỳ 1</span>
                    <span class="col-3" style="font-weight: normal">Kỳ 2</span>
                    <span class="col-3">TBN</span>
                  </div>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-if="!classScore?.pupils || classScore.pupils.length === 0">
                <td colspan="100%" class="text-center">Lớp chưa có học sinh nào</td>
              </tr>
              <tr v-else v-for="(pupil, index) in classScore?.pupils" :key="pupil.pupilId">
                <td class="">{{ index + 1 }}</td>
                <td class="">{{ pupil.pupilName }}</td>
                <td v-for="score in pupil.subjectScores" :key="score.subjectId" class="text-center">
                  <div class="d-flex justify-content-between">
                    <span class="col-3">{{ score.scores[0] ?? '-' }}</span>
                    <span class="col-3">{{ score.scores[1] ?? '-' }}</span>
                    <span class="col-3"
                      style="color: #721c24; font-weight: bolder"
                      v-if="
                        score.scores && score.scores.length > 1 && score.scores[0] !== null && score.scores[1] !== null
                      "
                      >{{ ((score.scores[0] + 2 * score.scores[1]) / 3).toFixed(1)  }}</span
                    >
                    <span class="col-3" v-else>-</span>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <CreateAndUpdateClassScoreComponent
      :show-modal="isShowModal"
      :is-update-mode="isUpdate"
      :-scores="pupilScores"
      @update:show-modal="isShowModal = $event"
      @update:is-update-mode="isUpdate = $event"
      @refreshList="fetchScore"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ClassSidebarComponent from '@/components/views/class/ClassSiderbar.vue';
import { useScoreComposable } from '@/composables/score';
import { onMounted } from 'vue';
import CreateAndUpdateClassScoreComponent from '@/components/modals/score/CreateAndUpdateClassScore.vue';
import { notifyError } from '@/helpers/notify.ts';
export default defineComponent({
  name: 'ScoreManagement',
  components: {
    ClassSidebarComponent,
    CreateAndUpdateClassScoreComponent,
  },
  setup(props, { emit }) {
    const {
      isShowModal,
      isUpdate,
      subjects,
      getSubjects,
      classId,
      classScore,
      fetchClassScore,
      currentSemester,
      fetchCurrentSemester,
      pupilScores,
      fetchPupilForCreate,
      fetchPupilScores,
    } = useScoreComposable();

    const handleOpenModal = (isUpdateMode: boolean, subjectId: number) => {
      if (!classScore.value?.pupils || classScore.value.pupils.length === 0) {
        notifyError('Lớp chưa có học sinh nào trong kỳ này');
        return;
      }

      if (isUpdateMode) {
        fetchPupilScores({
          entityId: classId.value,
          semesterId: currentSemester.id,
          subjectId: subjectId,
        });
        isUpdate.value = true;
      } else {
        fetchPupilForCreate({
          entityId: classId.value,
          semesterId: currentSemester.id,
          subjectId: subjectId,
        });
      }
      isShowModal.value = true;
    };
    const fetchScore = async () => {
      await fetchClassScore({ classId: classId.value, semesterId: currentSemester.id });
    };
    onMounted(async () => {
      await getSubjects();
      await fetchCurrentSemester();
      await fetchClassScore({ classId: classId.value, semesterId: currentSemester.id });
    });
    return {
      classScore,
      subjects,
      handleOpenModal,
      isShowModal,
      isUpdate,
      pupilScores,
      fetchScore,
    };
  },
});
</script>

<style scoped>
.wide-button {
  width: 200px; /* Adjust the width as needed */
}
.legend {
  display: flex;
  margin-right: 15px;
  justify-content: flex-end;
  padding: 10px 0;
  gap: 10px;
}

.legend span {
  display: flex;
  align-items: center;
  gap: 5px;
}

.feedback-container {
  position: relative;
  display: inline-block;
}

.feedback-text {
  visibility: hidden;
  width: 200px;
  background-color: black;
  color: #fff;
  text-align: center;
  border-radius: 6px;
  padding: 5px 0;
  position: absolute;
  z-index: 1;
  bottom: 125%; /* Position the tooltip above the icon */
  left: 50%;
  margin-left: -100px;
  opacity: 0;
  transition: opacity 0.3s;
}

.feedback-container:hover .feedback-text {
  visibility: visible;
  opacity: 1;
}

.blink-icon {
  animation: blink 1s infinite;
}

@keyframes blink {
  0%,
  100% {
    opacity: 1;
  }

  50% {
    opacity: 0;
  }
}
</style>
