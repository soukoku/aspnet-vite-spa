import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import * as fs from 'fs'
import * as path from 'path'

const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ''
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`
const certName = process.env.npm_package_name

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  server: {
    https: {
      key: fs.readFileSync(path.join(baseFolder, `${certName}.key`)),
      cert: fs.readFileSync(path.join(baseFolder, `${certName}.pem`))
    }
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
