# source.base.fe

This template should help get you started developing with Vue 3 in Vite.

## Recommended IDE Setup

[VSCode](https://code.visualstudio.com/) + [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vitejs.dev/config/).

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Lint with [ESLint](https://eslint.org/)

```sh
npm run lint
```

### template file .vue

````
# source.base.fe

This template should help get you started developing with Vue 3 in Vite.

## Recommended IDE Setup

[VSCode](https://code.visualstudio.com/) + [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vitejs.dev/config/).

## Project Setup

```sh
npm install
````

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Lint with [ESLint](https://eslint.org/)

```sh
npm run lint
```

### template file .vue

```
    <template>

    </template>
    <script lang="ts">
    import { defineComponent } from 'vue'
    export default defineComponent({
        name: 'LoginPage',
        components: {},
        setup(props, { emit }) {
            return {}
        }
    })
    </script>

```

### Template store

```
import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { AuthRepository } from '@/repositories/repository-auth';

export const useAuthStore = defineStore('auth', () => {

});

```

### Template composables

```
import { useAuthStore } from '@/stores/auth';
import { LoginRequestInterface } from '@/types/model/auth';
import { reactive } from 'vue';
import { startLoading, endLoading } from '@/helpers/mixins';
export const useAuthComposable = () => {
  const authStore = useAuthStore();
  const { loginMode } = authStore;
  const loginRequest = reactive<LoginRequestInterface>({ mode: 0, username: '', password: '' });
  const handleSubmitLoginByMode = (): void => {
    startLoading();
    loginMode(
      loginRequest,
      () => {
        endLoading();
      },
      () => {
        endLoading();
      },
    );
  };
  return {
    loginRequest,
    loginMode,
    handleSubmitLoginByMode,
  };
};

```
