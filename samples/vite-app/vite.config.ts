import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

import fs from 'fs'
import path from 'path'
import child_process from 'child_process'
import { env } from 'process'
import pkg from './package.json' with { type: 'json' }

const isDev = process.env.NODE_ENV === 'development'

let keyFilePath = ''
let certFilePath = ''
if (isDev) {
  const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
      ? `${env.APPDATA}/ASP.NET/https`
      : `${env.HOME}/.aspnet/https`

  const certificateName = pkg.name || 'appname'
  certFilePath = path.join(baseFolder, `${certificateName}.pem`)
  keyFilePath = path.join(baseFolder, `${certificateName}.key`)

  if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    fs.mkdirSync(baseFolder, { recursive: true })
    if (
      0 !==
      child_process.spawnSync(
        'dotnet',
        ['dev-certs', 'https', '--export-path', certFilePath, '--format', 'Pem', '--no-password', '--trust'],
        { stdio: 'inherit' }
      ).status
    ) {
      throw new Error('Could not create certificate.')
    }
  }
}


// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
  ],
  server: {
    https: isDev
      ? {
          key: fs.readFileSync(keyFilePath),
          cert: fs.readFileSync(certFilePath)
        }
      : undefined,
    port:3000,
    hmr: { host: 'localhost', clientPort: 3000 }
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  build: {
    // outDir: '../wwwroot',
    // generate manifest.json in outDir
    manifest: true,
    rollupOptions: {
      // overwrite default .html entry
      // input: {
      //   'home-index.ts': 'src/pages/home-index.ts',
      //   'home-another.ts': 'src/pages/home-another.ts'
      // }

      input: ['src/main.ts']
    }
  }
})
