// archivo: playwright.config.js - Lenguaje: JavaScript
// Configuracion minima: un solo navegador (Chromium), reporte de lista.
// A diferencia del prototipo estatico (Semana 5), aqui la app es un servidor real
// (ASP.NET Core), asi que Playwright la levanta con "webServer" antes de correr
// las pruebas y la apaga al terminar.
const path = require('path');
const { defineConfig, devices } = require('@playwright/test');

const PORT = 5212;
const BASE_URL = `http://localhost:${PORT}`;
const WEB_PROJECT = path.join(__dirname, '..', '..', 'src', 'ProyectoIA.Web');

module.exports = defineConfig({
  testDir: '.',
  testIgnore: '**/professor_*',
  globalSetup: require.resolve('./globalSetup.js'),
  fullyParallel: false,
  workers: 1,
  reporter: 'list',
  use: {
    baseURL: BASE_URL,
    trace: 'on-first-retry',
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
  ],
  webServer: {
    command: `dotnet run --project "${WEB_PROJECT}" --urls ${BASE_URL}`,
    url: BASE_URL,
    reuseExistingServer: !process.env.CI,
    timeout: 120_000,
  },
});
