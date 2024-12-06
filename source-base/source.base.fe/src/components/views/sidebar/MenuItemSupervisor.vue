<template>
  <div class="app-brand demo">
    <a href="/supervisor/index" class="app-brand-link">
  <span class="app-brand-logo demo">
    <img src="/src/assets/images/logo/edunexus_logo.png" alt="Logo" width="150" style="padding: 15px 5px 5px 0px" />
  </span>
    </a>
  </div>
  <div class="menu-inner-shadow"></div>
  <ul class="menu-inner py-1">
    <li
      v-for="(menu, index) in menus"
      :key="index"
      :class="[
        'menu-item',
        isActive(menu.id) ? 'active open' : '',
        { 'has-submenu': menu.subMenus && menu.subMenus.length > 0 },
      ]"
    >
      <router-link v-if="menu.link" :to="menu.link" class="menu-link menu-toggle" @click.native="toggleMenu(menu.id)">
        <i :class="menu.icon"></i>
        <div :data-i18n="menu.label" class="text-truncate">{{ menu.label }}</div>
        <span v-if="menu.badge" class="badge rounded-pill bg-danger ms-auto">{{ menu.badge }}</span>
        <i v-if="menu.subMenus && menu.subMenus.length > 0" class="bx bx-chevron-down ms-auto"></i>
      </router-link>
      <a v-else class="menu-link menu-toggle" href="#" @click="toggleMenu(menu.id)">
        <i :class="menu.icon"></i>
        <div :data-i18n="menu.label" class="text-truncate">{{ menu.label }}</div>
        <span v-if="menu.badge" class="badge rounded-pill bg-danger ms-auto">{{ menu.badge }}</span>
        <i v-if="menu.subMenus && menu.subMenus.length > 0" class="bx bx-chevron-down ms-auto"></i>
      </a>
      <ul v-if="isActive(menu.id)" class="menu-sub">
        <li v-for="(subMenu, subIndex) in menu.subMenus" :key="subIndex" class="menu-item">
          <router-link :to="subMenu.link" class="menu-link">
            <div :data-i18n="subMenu.label" class="text-truncate">{{ subMenu.label }}</div>
          </router-link>
        </li>
        <li v-if="menu.id === 'bus' && assignedBus" class="menu-item">
          <a class="menu-link" @click="goToBusDetail(assignedBus.id)">
            <div>{{ assignedBus.name }}</div>
          </a>
        </li>
        <li v-if="menu.id === 'bus' && !assignedBus" class="menu-item">
          <div class="menu-link">Chưa có lớp nào được phân công</div>
        </li>
      </ul>
    </li>
    <LogoutComponent />
  </ul>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { SupervisorMenu } from '@/helpers/menus';
import { useMenu } from '@/composables/menu';
import { useBusDetailComposable } from '@/composables/bus';
import LogoutComponent from '@/components/common/Logout.vue';

export default defineComponent({
  name: 'MenuItemSupervisorComponent',
  components: {
    LogoutComponent,
  },
  setup() {
    const menus = ref(SupervisorMenu);
    const { activeMenus, toggleMenu, isActive } = useMenu();
    const { assignedBus, fetchAssignedBus, goToBusDetail } = useBusDetailComposable();

    onMounted(async () => {
      await fetchAssignedBus();
    });

    return {
      menus,
      activeMenus,
      toggleMenu,
      isActive,
      assignedBus,
      goToBusDetail,
    };
  },
});
</script>

<style scoped>
.menu-vertical .menu-item.has-submenu .menu-toggle::after {
  inset-inline-end: 0.8rem;
}

.menu-vertical .menu-item:not(.has-submenu) .menu-toggle::after {
  content: none;
}
</style>