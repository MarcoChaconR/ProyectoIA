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
  await page.goto('/Gestiones/Details/1');
  await expect(page).toHaveURL(/Account\/AccessDenied/);

  await page.goto('/Gestiones');
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

  await page.fill('#texto-nota', 'Nota interna confidencial E2E');
  await page.uncheck('#es-publica');
  await page.click('#btn-agregar-nota');
  await expect(page.getByText('Nota interna confidencial E2E', { exact: true })).toBeVisible();
  await expect(page.getByText('Interna', { exact: true })).toBeVisible();

  await page.selectOption('#tecnicoId', '2');
  await page.click('#btn-asignar-tecnico');

  await expect(page.locator('#estado-actual')).toHaveText('Registrada');
  await expect(page.locator('#btn-avanzar-estado')).toBeVisible();

  await page.click('#btn-avanzar-estado');
  await expect(page.locator('#estado-actual')).toHaveText('Asignada');
});

test('cliente: no puede ver notas internas ni acceder a administración', async ({ page }) => {
  await login(page, 'cliente@proyectoia.com', 'Cliente123!');
  await page.goto('/Gestiones/Details/1');
  await expect(page.getByText('Nota interna confidencial E2E', { exact: true })).toHaveCount(0);

  const usuarios = await page.goto('/Usuarios');
  await expect(page).toHaveURL(/Account\/AccessDenied/);
});

test('tecnico: edita una gestión asignada y filtra por prioridad', async ({ page }) => {
  await login(page, 'tecnico@proyectoia.com', 'Tecnico123!');
  await page.click('#ver-detalle-2');
  await page.click('#btn-editar-gestion');
  await page.fill('#objetivo', 'Flujo de aprobación priorizado');
  await page.selectOption('#prioridad', '1');
  await page.click('#btn-guardar');

  await expect(page).toHaveURL(/Gestiones\/Details\/2/);
  await expect(page.locator('#estado-actual')).toHaveText('EnAtencion');
  await expect(page.getByText('Flujo de aprobación priorizado', { exact: true })).toBeVisible();

  await page.goto('/Gestiones?prioridad=1');
  await expect(page.locator('#ver-detalle-2')).toBeVisible();
  await expect(page.locator('#tabla-gestiones tbody tr')).toHaveCount(1);
});

test('administrador: detecta una edición concurrente', async ({ page, browser }) => {
  const secondContext = await browser.newContext();
  const secondPage = await secondContext.newPage();
  try {
    await login(page, 'admin@proyectoia.com', 'Admin123!');
    await login(secondPage, 'admin@proyectoia.com', 'Admin123!');
    await page.goto('/Gestiones/Edit/1');
    await secondPage.goto('/Gestiones/Edit/1');

    await page.fill('#objetivo', 'Edición más reciente');
    await page.click('#btn-guardar');
    await secondPage.fill('#objetivo', 'Edición obsoleta');
    await secondPage.click('#btn-guardar');

    await expect(secondPage.locator('.validation-summary-errors'))
      .toContainText('La gestión cambió desde que la abriste');
    await page.reload();
    await expect(page.getByText('Edición más reciente', { exact: true })).toBeVisible();
  } finally {
    await secondContext.close();
  }
});

test('administrador: consulta dashboard y mantiene CECOs, dependencias y usuarios', async ({ page, browser }) => {
  const userContext = await browser.newContext();
  const userPage = await userContext.newPage();
  try {
    await login(page, 'admin@proyectoia.com', 'Admin123!');

    await page.goto('/Dashboard');
    await expect(page.locator('#metric-total')).toHaveText('3');
    await expect(page.locator('#tabla-estados')).toBeVisible();

    await page.goto('/Catalogos');
    await page.click('#btn-nuevo-ceco');
    await page.fill('#codigo-ceco', '6999');
    await page.fill('#nombre-ceco', 'Innovación');
    await page.click('#btn-guardar-ceco');
    const cecoFila = page.locator('#tabla-cecos tbody tr').filter({ hasText: '6999' });
    await expect(cecoFila).toBeVisible();

    await page.click('#btn-nueva-dependencia');
    const cecoId = await page.locator('#ceco-dependencia option').filter({ hasText: '6999' }).getAttribute('value');
    await page.selectOption('#ceco-dependencia', cecoId);
    await page.fill('#nombre-dependencia', 'Transformación digital');
    await page.click('#btn-guardar-dependencia');
    await expect(page.locator('#tabla-dependencias')).toContainText('Transformación digital');

    await page.goto('/Usuarios');
    await page.click('#btn-nuevo-usuario');
    await page.fill('#nombre-usuario', 'Técnico de prueba');
    await page.fill('#correo-usuario', 'tecnico.e2e@proyectoia.com');
    await page.fill('#password-usuario', 'Temporal123!');
    await page.selectOption('#rol-usuario', '2');
    await page.click('#btn-guardar-usuario');
    const usuarioFila = page.locator('#tabla-usuarios tbody tr').filter({ hasText: 'tecnico.e2e@proyectoia.com' });
    await expect(usuarioFila).toContainText('Activo');
    await login(userPage, 'tecnico.e2e@proyectoia.com', 'Temporal123!');
    await usuarioFila.locator('button').click();
    await expect(usuarioFila).toContainText('Inactivo');
    await userPage.goto('/Gestiones');
    await expect(userPage).toHaveURL(/Account\/Login/);

    await userPage.fill('#correo', 'tecnico.e2e@proyectoia.com');
    await userPage.fill('#password', 'Temporal123!');
    await userPage.click('#btn-login');
    await expect(userPage.locator('.validation-summary-errors')).toContainText('inválidos');
  } finally {
    await userContext.close();
  }
});
