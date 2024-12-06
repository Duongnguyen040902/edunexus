import { ref } from 'vue';

export function useMenu() {
  const activeMenus = ref<string[]>([]);
  const isActive = (menuId: string) => activeMenus.value.includes(menuId);
  const toggleMenu = (menuId: string) => {
    if (!isActive(menuId)) {
      activeMenus.value = [menuId];
    } else {
      activeMenus.value = []; 
    }
  };

  return {
    activeMenus,
    isActive,
    toggleMenu,
  };
}
