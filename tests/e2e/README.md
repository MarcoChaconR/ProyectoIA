# Pruebas de interfaz (E2E) con Playwright

Este proyecto sigue la misma convención del ejemplo de Semana 5
(`SOFT734_T5_Playwright`): Playwright en Node.js/JavaScript, con
`package.json` + `playwright.config.js` + archivos `*.spec.js`.

**Diferencia clave con el ejemplo de Semana 5:** allá se probaba un prototipo
estático (`index.html` vía `file://`, sin servidor). Aquí la app es un
servidor real (ASP.NET Core + SQLite), así que `playwright.config.js` usa la
opción `webServer` para levantar `dotnet run` automáticamente antes de correr
las pruebas (y reutiliza el servidor si ya está corriendo en local).

## Qué hay en esta carpeta

| Archivo | Qué es |
|---|---|
| `smoke.spec.js` | Prueba de humo: confirma que el servidor responde. |
| `flujo-gestiones.spec.js` | Flujo E2E por rol: login, crear gestión, listado, detalle/notas, cambio de estado y asignación de técnico. |
| `globalSetup.js` | Borra `app.db` antes de cada corrida para pruebas deterministas (la app la recrea al arrancar). |
| `playwright.config.js` | Config: Chromium, reporte de lista, `webServer` apuntando a `ProyectoIA.Web`, `workers: 1`. |
| `package.json` | Dependencias (`@playwright/test`) y scripts (`test`, `codegen`). |
| `capturas/` | Evidencia visual (screenshots) generada por las pruebas. |

Las pruebas de flujo (login, crear gestión, listado, detalle/notas, cambio de
estado) se agregan en archivos separados a medida que cada pantalla queda
funcional en `ProyectoIA.Web`.

## Correr las pruebas

Requiere **Node** (incluye `npm` y `npx`) y el SDK de **.NET 10**.

```bash
cd tests/e2e
npm install                        # instala @playwright/test
npx playwright install chromium    # descarga el navegador una sola vez
npx playwright test                # levanta la app (dotnet run) y corre las pruebas
```

## Generar pruebas nuevas con codegen

```bash
npm run codegen   # abre el navegador contra la app corriendo en :5212
```

## Selectores estables

Igual que en el ejemplo de Semana 5: se prefieren selectores por `id`
(`#login-form`, `#estado`, etc.) en vez de texto, para que la prueba no se
rompa si cambia una palabra en la UI.
