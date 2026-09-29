// flujo-gestiones.spec.js - Lenguaje: JavaScript (Playwright)
// Flujo end-to-end del MVP: login por rol, crear gestión (cliente), listado,
// detalle con notas y cambio de estado / asignación de técnico.
// Los selectores usan `id` para no romperse con cambios de texto en la UI.
// Correr:  npx playwright test flujo-gestiones.spec.js
const { test, expect } = require('@playwright/test');

async function login(page, correo, password) {
  await page.goto('/Account/Login');
  await page.fill('#correo', correo);
  await page.fill('#password', password);
  await page.click('#btn-login');
  await expect(page).toHaveURL(/Gestiones/);
}

test.describe.configure({ mode: 'serial' });

test('login con credenciales inválidas muestra error', async ({ page }) => {
  await page.goto('/Account/Login');
  await page.fill('#correo', 'cliente@proyectoia.com');
  await page.fill('#password', 'incorrecta');
  await page.click('#btn-login');
  await expect(page.locator('.validation-summary-errors')).toContainText('inválidos');
});

test('cliente: crea gestión, ve detalle y agrega nota pública', async ({ page }) => {
  await login(page, 'cliente@proyectoia.com', 'Cliente123!');

  const objetivo = `Prueba E2E ${Date.now()}`;

  await page.click('#btn-crear-gestion');
  await page.selectOption('#ceco', '1');
  await page.selectOption('#dependencia', '1');
  await page.selectOption('#tipo-solicitud', '1');
  await page.fill('#objetivo', objetivo);
  await page.fill('#detalle', 'Detalle de prueba E2E');
  await page.click('#btn-guardar');

  await expect(page).toHaveURL(/Gestiones\/Details\/\d+/);
  await expect(page.locator('#estado-actual')).toHaveText('Registrada');

  await page.fill('#texto-nota', 'Nota pública de prueba');
  await page.click('#btn-agregar-nota');
  await expect(page.getByText('Nota pública de prueba')).toBeVisible();

  await page.goto('/Gestiones');
  await expect(page.getByText(objetivo)).toBeVisible();
});

test('tecnico: ve sus gestiones asignadas y avanza estado', async ({ page }) => {
  await login(page, 'tecnico@proyectoia.com', 'Tecnico123!');

  await expect(page.locator('#ver-detalle-2')).toBeVisible();
  await expect(page.locator('#ver-detalle-1')).toHaveCount(0);

  await page.click('#ver-detalle-2');
  await expect(page.locator('#estado-actual')).toHaveText('Asignada');

  await page.click('#btn-avanzar-estado');
  await expect(page.locator('#estado-actual')).toHaveText('EnAtencion');
});

test('administrador: asigna técnico y avanza estado de una gestión', async ({ page }) => {
  await login(page, 'admin@proyectoia.com', 'Admin123!');

  await expect(page.locator('#ver-detalle-1')).toBeVisible();
  await expect(page.locator('#ver-detalle-2')).toBeVisible();

  await page.click('#ver-detalle-1');
  await expect(page.locator('#estado-actual')).toHaveText('Registrada');

  await page.selectOption('#tecnicoId', '2');
  await page.click('#btn-asignar-tecnico');

  await expect(page.locator('#estado-actual')).toHaveText('Registrada');
  await expect(page.locator('#btn-avanzar-estado')).toBeVisible();

  await page.click('#btn-avanzar-estado');
  await expect(page.locator('#estado-actual')).toHaveText('Asignada');
});
