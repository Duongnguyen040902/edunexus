import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import authenticateMiddleware from '@/middleware/auth';
import { LAYOUT_MAPPING, LayoutKeys } from '@/constants/layouts/path-root';
import { EXCEPT_ROUTE_REQUIRE_AUTH, MIDDLEWARE_AUTH } from '@/constants/routes/auth.ts';
import { API_URL } from '@/constants/api/endpoint.ts';

// Constants
const VIEW_PATH_PREFIX = 'src/views';
const HOME_PATH = '/home';

// @ts-expect-error - Vite injects env vars at runtime, so TypeScript doesn't recognize them
const views = import.meta.glob('@/views/**/*.vue');

// Utility Functions
function formatPath(path: string): string {
  return path
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .toLowerCase()
    .slice(1, -4);
}

function generateRoutePath(originPath: string): string {
  return originPath === HOME_PATH ? '/' : `${originPath.replaceAll('_', ':')}`;
}

function generateRouteName(originPath: string): string {
  return originPath
    .replace(/\/|_/g, '-')
    .split('-')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join('');
}

// Main Logic
const routes: RouteRecordRaw[] = Object.keys(views).map(path => {
  let originPath = path.replace(VIEW_PATH_PREFIX, '');
  const pathSegments = originPath.split('/').filter(segment => segment);
  const parentDir = pathSegments.length > 1 ? pathSegments[0].toLowerCase() : '';
  const layout = (parentDir && LAYOUT_MAPPING[parentDir as LayoutKeys]) || '';

  originPath = formatPath(originPath);
  const AUTH_PATHS = Object.values(API_URL.AUTH);

  const routePath = generateRoutePath(originPath);
  const name = generateRouteName(originPath);
  return {
    path: routePath,
    name,
    component: views[path],
    meta: {
      // middleware: AUTH_PATHS.some(authPath => routePath === authPath)
      //   ? []
      //   : [MIDDLEWARE_AUTH],
      layout,
    },
  };
});

routes.push({
  path: '/',
  redirect: '/authen/login',
});

routes.push({
  path: '/:pathMatch(.*)*',
  name: 'NotFound',
  component: () => import('@/views/error/NotFound.vue'),
});

routes.push({
  path: '/admin/invoice/detail/:id',
  name: 'DetailInvoice',
  component: () => import('@/views/Admin/Invoice/Detail.vue'),
  meta: {
    middleware: EXCEPT_ROUTE_REQUIRE_AUTH.includes('DetailInvoice') ? [] : [MIDDLEWARE_AUTH],
    layout: 'Admin',
  },
});

routes.push({
  path: '/admin/invoice/create',
  name: 'CreateInvoice',
  component: () => import('@/views/Admin/Invoice/Detail.vue'),
  meta: {
    middleware: EXCEPT_ROUTE_REQUIRE_AUTH.includes('CreateInvoice') ? [] : [MIDDLEWARE_AUTH],
    layout: 'Admin',
  },
});

routes.push({
  path: '/school/invoice/detail/:id',
  name: 'InvoiceDetail',
  component: () => import('@/views/School/Invoice/Detail.vue'),
  meta: {
    middleware: EXCEPT_ROUTE_REQUIRE_AUTH.includes('InvoiceDetail') ? [] : [MIDDLEWARE_AUTH],
    layout: 'School',
  },
});

routes.push({
  path: '/school/subscription/manager',
  name: 'SubscriptionManager',
  component: () => import('@/views/School/Subscription/Manager.vue'),
  meta: {
    middleware: EXCEPT_ROUTE_REQUIRE_AUTH.includes('SubscriptionManager') ? [] : [MIDDLEWARE_AUTH],
    layout: 'School',
  },
});

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach(authenticateMiddleware);

export default router;
