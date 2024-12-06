<template>
  <div class="card-header border-bottom">
    <h5 class="card-title mb-0">Tìm kiếm trường</h5>
    <form
      class="d-flex justify-content-between align-items-center row pt-4 gap-4 gap-md-0 g-6"
      @submit.prevent="handleGetAllAcccountSchoolAdmin"
    >
      <div class="col-md-4 user_role">
        <label>
          <input
            v-model="requestAdminSchoolIndex.keyword"
            aria-controls="DataTables_Table_0"
            class="form-control"
            placeholder="Tìm kiếm"
            type="search"
            @change="handleGetAllAcccountSchoolAdmin"
          />
        </label>
      </div>
      <div class="col-md-4 user_plan">
        <select
          id="UserPlan"
          v-model="requestAdminSchoolIndex.subscriptionPlanId"
          class="form-select text-capitalize"
          @change="handleGetAllAcccountSchoolAdmin"
        >
          <option value="">Chọn gói</option>
          <option v-for="(sub, index) in responseSubscription.value" :key="index" :value="sub.id">
            {{ sub.name }}
          </option>
        </select>
      </div>
      <div class="col-md-4 user_status">
        <select
          id="FilterTransaction"
          v-model="requestAdminSchoolIndex.status"
          class="form-select text-capitalize"
          @change="handleGetAllAcccountSchoolAdmin"
        >
          <option value="">Chọn trạng thái</option>
          <option v-for="(status, key) in statuses" :key="key" :value="status.key">{{ status.value }}</option>
        </select>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import {defineComponent, onMounted} from 'vue';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import { AccountStatusLabels } from '@/constants/enums/statuses.ts';
import { useSubscriptionComposable } from '@/composables/subscription.ts';

export default defineComponent({
  name: 'SearchAdminSchoolComponent',
  components: {},
  props: {},
  setup(props, { emit }) {
    const adminSchoolComposable = useAdminSchoolComposable();
    const { requestAdminSchoolIndex, handleGetAllAcccountSchoolAdmin } = adminSchoolComposable;
    const statuses = Object.entries(AccountStatusLabels).map(([key, value]) => ({ key: Number(key), value }));
    const subscription = useSubscriptionComposable();
    const { responseSubscription, handleGetAllSubscription } = subscription;
    onMounted(async () => {
      await handleGetAllSubscription();
    });
    return {
      statuses,
      responseSubscription,
      requestAdminSchoolIndex,
      handleGetAllAcccountSchoolAdmin,
    };
  },
});
</script>
