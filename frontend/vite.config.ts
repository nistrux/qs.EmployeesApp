import { defineConfig } from 'vite'
import { svelte } from '@sveltejs/vite-plugin-svelte'
import path from 'path'
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// https://vite.dev/config/
export default defineConfig({
  plugins: [svelte()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5194',
        changeOrigin: true,
      }
    }
  },
  resolve: {
    alias: {
      '$lib': path.resolve(__dirname, './src/lib'),
      '$components': path.resolve(__dirname, './src/components'),
      '$routes': path.resolve(__dirname, './src/routes'),
      '$services': path.resolve(__dirname, './src/services'),
      '$store': path.resolve(__dirname, './src/store'),
      '$types': path.resolve(__dirname, './src/types.ts')
    }
  }
})
