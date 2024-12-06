import vue from '@vitejs/plugin-vue';
import { resolve, dirname } from 'path';
import { fileURLToPath } from 'url';
import { defineConfig, loadEnv } from 'vite';
import { viteStaticCopy } from 'vite-plugin-static-copy';

const __dirname = dirname(fileURLToPath(import.meta.url));

export default ({ mode }) => {
  process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };

  return defineConfig({
    plugins: [
      vue(),
      viteStaticCopy({
        targets: [{ src: 'src/assets/images', dest: 'assets' }],
      }),
    ],
    resolve: {
      alias: {
        '@': resolve(__dirname, './src'),
        '@assets': resolve(__dirname, './src/assets'),
        '@components': resolve(__dirname, './src/components'),
        '@composables': resolve(__dirname, './src/composables'),
        '@constants': resolve(__dirname, './src/constants'),
        '@helpers': resolve(__dirname, './src/helpers'),
        '@js': resolve(__dirname, './src/js'),
        '@layouts': resolve(__dirname, './src/layouts'),
        '@locales': resolve(__dirname, './src/locales'),
        '@middleware': resolve(__dirname, './src/middleware'),
        '@plugins': resolve(__dirname, './src/plugins'),
        '@repositories': resolve(__dirname, './src/repositories'),
        '@router': resolve(__dirname, './src/router'),
        '@scss': resolve(__dirname, './src/scss'),
        '@stores': resolve(__dirname, './src/stores'),
        '@types': resolve(__dirname, './src/types'),
        '@views': resolve(__dirname, './src/views'),
      },
    },
    server: {
      port: parseInt(process.env.VITE_APP_PORT || '5173'),
      host: process.env.VITE_APP_RESOURCE_HOST || 'localhost',
    },
    build: {
      outDir: 'dist',
      rollupOptions: {
        input: {
          main: resolve(__dirname, 'src/main.ts'),
          bootstrap: resolve(__dirname, 'src/js/bootstrap.js'),
          helpers: resolve(__dirname, 'src/js/helpers.js'),
          menu: resolve(__dirname, 'src/js/menu.js'),
        },
      },
    },
  });
};
