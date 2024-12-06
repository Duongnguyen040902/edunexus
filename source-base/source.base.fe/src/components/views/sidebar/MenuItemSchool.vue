<template>
  <div class="app-brand demo">
    <a href="/school/dashboard" class="app-brand-link">
  <span class="app-brand-logo demo">
    <img src="/src/assets/images/logo/edunexus_logo.png" alt="Logo" width="150" style="padding: 15px 5px 5px 0px" />
  </span>
    </a>
  </div>
<div class="menu-inner-shadow"></div>

<ul class="menu-inner py-1">
    <li v-for="(menu, index) in menus" :key="index" :class="[
        'menu-item',
        isActive(menu.id) ? 'active open' : '',
        { 'has-submenu': menu.subMenus && menu.subMenus.length > 0 },
      ]">
        <router-link v-if="menu.link" :to="menu.link" class="menu-link menu-toggle" @click.native="toggleMenu(menu.id)">
            <i :class="menu.icon"></i>
            <div :data-i18n="menu.label" class="text-truncate">{{ menu.label }}</div>
            <span v-if="menu.badge" class="badge rounded-pill bg-danger ms-auto">
                {{ menu.badge }}
            </span>
        </router-link>
        <a v-else class="menu-link menu-toggle" href="#" @click="toggleMenu(menu.id)">
            <i :class="menu.icon"></i>
            <div :data-i18n="menu.label" class="text-truncate">{{ menu.label }}</div>
            <span v-if="menu.badge" class="badge rounded-pill bg-danger ms-auto">
                {{ menu.badge }}
            </span>
        </a>
        <ul v-if="isActive(menu.id)" class="menu-sub">
            <li v-for="(subMenu, subIndex) in menu.subMenus" :key="subIndex" class="menu-item">
                <router-link :to="subMenu.link" class="menu-link">
                    <i :class="subMenu.icon"></i>
                    <div :data-i18n="subMenu.label" class="text-truncate">
                        {{ subMenu.label }}
                    </div>
                </router-link>
            </li>
        </ul>
    </li>
    <LogoutComponent />
</ul>
</template>

<script lang="ts">
import {
    defineComponent,
    ref
} from 'vue';
import {
    SchoolMenu
} from '@/helpers/menus';
import {
    useMenu
} from '@/composables/menu';
import LogoutComponent from '@/components/common/Logout.vue';

export default defineComponent({
    name: 'MenuItemSchoolComponent',
    components: {
        LogoutComponent,
    },
    setup() {
        const menus = ref(SchoolMenu);
        const {
            activeMenus,
            toggleMenu,
            isActive
        } = useMenu();
        return {
            menus,
            activeMenus,
            toggleMenu,
            isActive,
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
