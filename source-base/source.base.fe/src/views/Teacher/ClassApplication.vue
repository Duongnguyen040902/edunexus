<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <ClassSiderbarComponent />

    <div class="card mb-4 mt-3">
      <h5 class="card-header">Danh sách đơn</h5>
      <hr class="m-0" />
      <div class="card-body">
        <div class="row">
          <!-- Contextual List group -->
          <div :class="classApplicationSelected.title ? 'col-lg-6 mb-4 mb-xl-0' : 'col-lg-12 mb-4 mb-xl-0'">
            <small class="text-light fw-semibold">Loại đơn</small>
            <div class="list-group list-group-horizontal-md text-md-center mt-3">
              <a
                v-for="category in classApplicationCategory"
                :key="category.id"
                class="list-group-item list-group-item-action"
                :class="{ active: selectedCategoryId === category.id }"
                @click="handleSelectCategory(category.id)"
                href="javascript:void(0);"
              >
                {{ category.name }}
              </a>
            </div>
            <small class="text-light fw-semibold">Danh sách đơn</small>
            <div class="demo-inline-spacing mt-3 list-container">
              <ul class="list-group" v-if="classApplication.length > 0">
                <li
                  v-for="application in classApplication"
                  :key="application.id"
                  @click="selectClassApplication(application.id)"
                  class="list-group-item d-flex justify-content-between align-items-center"
                  :class="{
                    'list-group-item-primary': selectedApplicationId === application.id,
                  }"
                >
                  <div class="d-flex flex-column" style="width: 50%">
                    <span>{{ application.title }}</span>
                    <small class="text-muted">{{ truncatedDescription(application.description) }}</small>
                  </div>

                  <div class="d-flex flex-column align-items-end" style="width: 50%">
                    <small class="text-muted">{{ application.donorName }}</small>
                    <small>{{ formatDate(application.createDate, 'dd/mm/yyyy') }}</small>
                    <small
                      class="badge"
                      :class="
                        application.status === ApplicationStatus.PENDING
                          ? 'bg-label-dark'
                          : application.status === ApplicationStatus.REJECTED
                            ? 'bg-label-danger'
                            : 'bg-label-success'
                      "
                      style="width: 65px; font-size: 8px"
                    >
                      {{ application.statusName }}
                    </small>
                  </div>
                </li>
              </ul>
              <div v-else class="text-center text-muted">Chưa có đơn nào</div>
            </div>
          </div>
          <div class="col-lg-6 mb-4 mb-xl-0" v-if="classApplicationSelected.title">
            <ClassApplicationDetailComponent
              :class-application="classApplicationSelected"
              @update-list="updateListApplication"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { useClassApplicationComposable } from '@/composables/class-application';
import { defineComponent, onMounted, ref } from 'vue';
import ClassApplicationDetailComponent from '@/components/views/class/ApplicationDetail.vue';
import ClassSiderbarComponent from '@/components/views/Class/ClassSiderbar.vue';
import { formatDate } from '@/helpers/formatDate.ts';
import { truncatedDescription } from '@/helpers/truncated.ts';
import { ApplicationStatus } from '@/types/model/class-application.ts';

export default defineComponent({
  name: 'ClassApplication',
  computed: {
    ApplicationStatus() {
      return ApplicationStatus;
    },
  },
  components: {
    ClassApplicationDetailComponent,
    ClassSiderbarComponent,
  },
  setup() {
    const {
      handleSelectCategory,
      selectedCategoryId,
      fetchClassApplicationList,
      fetchCurrentSemester,
      classApplicationCategory,
      classApplication,
      classId,
      currentSemester,
      classApplicationSelected,
      selectedApplicationId,
      handleSelectClassApplication,
      fetchClassApplicationCategory,
      updateListApplication,
    } = useClassApplicationComposable();

    onMounted(async () => {
      await fetchCurrentSemester();
      await fetchClassApplicationCategory();
      await fetchClassApplicationList(classId.value, currentSemester.id, 0);
      if (classApplication.value.length > 0) {
        handleSelectClassApplication(classApplication.value[0].id);
      }
    });

    return {
      truncatedDescription,
      fetchClassApplicationList,
      classApplication,
      classApplicationCategory,
      selectClassApplication: handleSelectClassApplication,
      classApplicationSelected,
      selectedApplicationId,
      selectedCategoryId,
      handleSelectCategory,
      updateListApplication,
      formatDate,
    };
  },
});
</script>

<style>
.demo-inline-spacing .list-group-item img {
  width: 100%;
  height: auto;
  flex-direction: row;
}

.demo-inline-spacing .list-group-item {
  display: flex;
  flex-direction: row;
  align-items: center;
  text-align: left;
}
.list-container {
  max-height: 400px; /* Adjust the height as needed */
  overflow-y: auto;
}
@media (max-width: 700px) {
  .list-container {
    max-height: calc(3 * 80px); /* Adjust the height to show approximately 3 notifications */
  }
}

.list-group-item-primary {
  border-color: aliceblue;
}
</style>
