<template>
  <div class="app-brand demo">
    <a href="/pupil/index" class="app-brand-link">
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
      </router-link>
      <a v-else class="menu-link menu-toggle" href="#" @click="toggleMenu(menu.id)">
        <i :class="menu.icon"></i>
        <div :data-i18n="menu.label" class="text-truncate">{{ menu.label }}</div>
        <span v-if="menu.badge" class="badge rounded-pill bg-danger ms-auto">{{ menu.badge }}</span>
      </a>
      <ul v-if="isActive(menu.id)" class="menu-sub">
        <li v-if="menu.id === 'generalManagement'" class="menu-item">
          <a class="menu-link" @click="goToNotificationOfPupil(assignedClass.Id)">
            <i class="bx bx-bell menu-icon"></i>
            <div class="text-truncate">Thông báo</div>
          </a>
        </li>
        <li v-for="(subMenu, subIndex) in menu.subMenus" :key="subIndex" class="menu-item">
          <router-link :to="subMenu.link" class="menu-link">
            <i :class="subMenu.icon"></i>
            <div :data-i18n="subMenu.label" class="text-truncate">{{ subMenu.label }}</div>
          </router-link>
        </li>
        <li v-if="menu.id === 'generalManagement'" class="menu-item">
          <a class="menu-link" @click="goToTimeTableOfPupil(assignedClass.Id)">
            <i class="bx bx-calendar menu-icon"></i>
            <div class="text-truncate">Thời khóa biểu</div>
          </a>
        </li>
      </ul>
    </li>
    <LogoutComponent />
  </ul>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { PupilMenu } from '@/helpers/menus';
import { useMenu } from '@/composables/menu';
import { useBusDetailComposable } from '@/composables/bus.ts';
import { useClubDetailComposable } from '@/composables/club.ts';
import { useClassDetailComposable } from '@/composables/class.ts';
import LogoutComponent from '@/components/common/Logout.vue';

export default defineComponent({
  name: 'MenuItemPupilComponent',
  components: {
    LogoutComponent,
  },
  setup() {
    const menus = ref(PupilMenu);
    const { activeMenus, toggleMenu, isActive } = useMenu();
    const { assignedBus, fetchEnrolledBus, gotoBusDetailOfPupil } = useBusDetailComposable();
    const { assignedClub, fetchEnrolledClub, goToClubDetailOfPupil } = useClubDetailComposable();
    const { assignedClass, fetchEnrolledClass, goToClassDetailOfPupil, goToNotificationOfPupil, goToTimeTableOfPupil } =
      useClassDetailComposable();

    onMounted(() => {
      fetchEnrolledClass();
    });

    return {
      assignedClub,
      assignedClass,
      goToNotificationOfPupil,
      assignedBus,
      menus,
      activeMenus,
      toggleMenu,
      isActive,
      goToClubDetailOfPupil,
      goToClassDetailOfPupil,
      gotoBusDetailOfPupil,
      goToTimeTableOfPupil,
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