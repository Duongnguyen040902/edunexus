import { RouteLocationNormalized, NavigationGuardNext } from 'vue-router';
import { Cookie } from '@/classes/cookie';
import Cookies from 'js-cookie';
import { RouterMetaInterface } from '@/types/router';
import { MIDDLEWARE_AUTH } from '@/constants/routes/auth';

const authenticateMiddleware = (
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext,
) => {
  window.scrollTo(0, 0);

  const token = Cookies.get('token');
  const { middleware } = (to as RouterMetaInterface).meta;
  const pathLogin = '/authen/login';

  if (to.path !== pathLogin && middleware && middleware.includes(MIDDLEWARE_AUTH) && !token) {
    Cookie.set('redirect', to.path);

    return next(pathLogin);
  }

  next();
};

export default authenticateMiddleware;