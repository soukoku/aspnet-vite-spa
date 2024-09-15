import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

import fs from 'fs'
import path from 'path'
import child_process from 'child_process'
import pkg from './package.json' assert { type: 'json' }

const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ''
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`

const certificateName = pkg.name || 'vueapp.client'
const certFilePath = path.join(baseFolder, `${certificateName}.pem`)
const keyFilePath = path.join(baseFolder, `${certificateName}.key`)
const isDev = process.env.NODE_ENV === 'development'

if (isDev && (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath))) {
  if (
    0 !==
    child_process.spawnSync(
      'dotnet',
      [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
        '--trust'
      ],
      { stdio: 'inherit' }
    ).status
  ) {
    throw new Error('Could not create certificate.')
  }
}

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
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

      input: ['src/pages/home-index.ts', 'src/pages/home-another.ts']
    }
  }
})
