// archivo: smoke.spec.js - Lenguaje: JavaScript (Playwright)
// Prueba de humo: confirma que el servidor de la app (ASP.NET Core) levanta y
// responde. Se corre primero para validar el entorno; las pruebas de flujo
// (login, crear gestión, etc.) se agregan en archivos separados a medida que
// las pantallas quedan listas.
// Correr:  npx playwright test smoke.spec.js
const { test, expect } = require('@playwright/test');

test('la aplicacion responde en la raiz', async ({ page }) => {
  await page.goto('/');
  await expect(page).toHaveTitle(/.+/);
});
