import { RouteLocationNormalized, RouteMeta } from 'vue-router';

interface RouteMetaMiddleware extends RouteMeta {
  middleware: string[];
  requiresAuth?: boolean;
  role?: string;
}

export interface RouterMetaInterface extends RouteLocationNormalized {
  meta: RouteMetaMiddleware;
}