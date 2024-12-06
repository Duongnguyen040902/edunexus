<template>
  <div class="container-fluid flex-grow-1 container-p-y">

    <ClubInfoComponent :club-details="clubDetail" :current-semester="currentSemester" />
    <TableClubEnrollmentComponent :clubDetail="clubDetail" />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, watch } from "vue";
import ClubInfoComponent from "@/components/views/Club/ClubInfo.vue";
import TableClubEnrollmentComponent from "@/components/views/Club/TableClubEnrollment.vue";
import { useClubDetailComposable } from "@/composables/club";
import ClubSidebarComponent from "@/components/views/club/ClubSidebar.vue";
export default defineComponent({
  name: "ClubDetail",
  components: {
    ClubInfoComponent,
    TableClubEnrollmentComponent,
    ClubSidebarComponent,
  },
  setup() {
    const {
      fetchClubDetail,
      clubDetail,
      clubId,
      currentSemester,
      fetchCurrentSemester,
    } = useClubDetailComposable();

    onMounted(async () => {
      await fetchCurrentSemester();
      await fetchClubDetail(clubId.value);
    });

    watch(
      () => clubId.value,
      async (newVal) => {
        await fetchClubDetail(newVal);
      }
    );

    return {
      currentSemester,
      clubDetail,
      clubId,
      fetchCurrentSemester,
    };
  },
});
</script>
