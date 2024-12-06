<template>
  <li class="menu-item">
    <router-link class="menu-link" to="#" @click.native="handleLogout">
      <i class="menu-icon bx bx-log-out"></i>
      <div class="text-truncate">Đăng xuất</div>
    </router-link>
  </li>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useAuthComposable } from '@/composables/auth.ts';

export default defineComponent({
  name: 'Logout',
  components: {},
  props: {},
  emits: [],
  setup(props, { emit }) {
    const { handleLogout } = useAuthComposable();
    return {
      handleLogout,
    };
  },
});
</script>

<style scoped></style>
