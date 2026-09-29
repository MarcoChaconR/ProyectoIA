# HANDOFF — Continuación de desarrollo con otro modelo de IA

> Léeme primero. Este documento te ubica rápido para continuar el desarrollo
> del MVP sin releer todo el historial de la sesión anterior.

## 1. Qué es este proyecto

Sistema de Gestión de Solicitudes (RPA/BPM/Power Platform), con 3 roles
(Cliente, Técnico, Administrador). El plan de negocio completo, historias de
usuario, ambigüedades y supuestos están en **`Documentación.md`** (documento
maestro). **Lee especialmente la sección 19 ("Replan MVP Didáctico")**: ahí se
redujo el alcance original para poder tener una app navegable rápido y
ejecutar pruebas E2E con Playwright, tomando supuestos temporales en vez de
esperar a cerrar las 20 ambigüedades de negocio pendientes (sección 13).

No resuelvas las ambigüedades de negocio por tu cuenta salvo que se te pida
explícitamente: los supuestos de la sección 19.4 ya destraban el desarrollo.

## 2. Decisiones técnicas ya tomadas (no las re-discutas salvo pedido explícito)

- **Arquitectura:** simple, 3 proyectos (`Domain`, `Infrastructure`, `Web`),
  NO Clean Architecture completa con capa `Application` separada.
- **Stack:** ASP.NET Core **MVC + Razor Views** (server-rendered, sin SPA) —
  se eligió así porque es más estable para selectores de Playwright.
- **Base de datos:** EF Core + **SQLite**.
- **Auth:** cookie simple con 3 usuarios sembrados (admin/tecnico/cliente),
  **sin ASP.NET Identity completo** (fue decisión explícita para simplificar).
- **Pruebas E2E:** **Playwright en Node.js/JavaScript** (`@playwright/test`),
  siguiendo la misma convención del ejemplo `Semana 5/SOFT734_T5_Playwright`
  (`package.json` + `playwright.config.js` + `*.spec.js`). Se descartó el
  binding .NET/NUnit para las pruebas E2E aunque el proyecto
  `tests/ProyectoIA.E2ETests` (.NET/NUnit) quedó creado sin uso — no borrarlo
  salvo que se pida, pero no desarrollar ahí.
- **.NET SDK instalado:** 10.0.400 (`dotnet --version` para confirmar).

## 3. Estructura actual del repo

```
src/
  ProyectoIA.slnx                      # solución
  ProyectoIA.Domain/                   # entidades (sin dependencias externas)
    Enums.cs                           # Rol, EstadoGestion
    Usuario.cs, Ceco.cs, Dependencia.cs, TipoSolicitud.cs
    Gestion.cs, NotaGestion.cs, BitacoraCambio.cs
    PasswordHasher.cs                  # hash SHA256 simple para login/seed
  ProyectoIA.Infrastructure/           # EF Core
    AppDbContext.cs                    # DbSets + relaciones FK + seed (HasData)
    Migrations/                        # migración inicial (generada)
  ProyectoIA.Web/                      # ASP.NET Core MVC
    Program.cs                         # registra AppDbContext, auth cookie, Migrate() al inicio
    appsettings.json                   # ConnectionStrings:DefaultConnection = app.db
    Controllers/AccountController.cs   # Login/Logout/AccessDenied
    Controllers/GestionesController.cs # Index/Create/Details/AgregarNota/CambiarEstado/AsignarTecnico
    Models/                            # LoginViewModel, GestionCreateViewModel, GestionListViewModel, NotaViewModel
    Views/Account/                     # Login.cshtml, AccessDenied.cshtml
    Views/Gestiones/                   # Index.cshtml, Create.cshtml, Details.cshtml
    Properties/launchSettings.json     # puerto: http://localhost:5212

tests/
  ProyectoIA.E2ETests/                 # proyecto .NET/NUnit + Microsoft.Playwright.NUnit — CREADO PERO SIN USO
  e2e/                                  # ← proyecto Playwright JS activo
    package.json                       # @playwright/test
    playwright.config.js               # webServer levanta "dotnet run" en :5212; workers=1, borra DB antes de correr
    globalSetup.js                     # elimina app.db para pruebas deterministas
    smoke.spec.js                      # prueba de humo (pasa)
    flujo-gestiones.spec.js            # flujo E2E por rol (pasa)
    README.md                          # explica la convención (estilo Semana 5)
    node_modules/, test-results/       # generados, en .gitignore

.gitignore                             # cubre bin/obj, node_modules, *.db, playwright artifacts
Documentación.md                       # documento maestro (plan, historias, ambigüedades, sección 19 = replan MVP)
```

## 4. Estado del desarrollo (usar la tabla `todos` de la sesión si está disponible)

**Hecho (MVP navegable completo):**
1. Documentación actualizada con el replan MVP (sección 19).
2. Solución .NET creada y referenciada correctamente (`Web` → `Infrastructure` +
   `Domain`; `Infrastructure` → `Domain`).
3. Entidades de dominio completas (ver sección 3 arriba).
4. Proyecto Playwright JS (`tests/e2e`) funcionando.
5. **EF Core + SQLite** — `AppDbContext` registrado en `Program.cs`, cadena de
   conexión `Data Source=app.db` en `appsettings.json`, migración inicial
   `Migrations/20260923022822_Initial` generada y aplicada automáticamente al
   arrancar (`db.Database.Migrate()`).
6. **Seed** — 3 usuarios (admin/tecnico/cliente), catálogos (2 Cecos, 2
   Dependencias, 3 TiposSolicitud) y 2 gestiones de ejemplo, vía `HasData`.
7. **auth-cookie** — login por cookie (`AccountController`), claims de rol,
   `[Authorize(Roles = "...")]`, logout y AccessDenied.
8. **flow-crear-gestion** — `Gestiones/Create` (rol Cliente), con catálogos en
   dropdown y validación.
9. **flow-listado** — `Gestiones/Index` con filtro por estado; visibilidad por
   rol (cliente ve solo las suyas, técnico las asignadas, admin todas).
10. **flow-detalle-notas** — `Gestiones/Details` + agregar nota pública/interna
    (cliente solo ve públicas; nota interna oculta para cliente).
11. **flow-estado-asignacion** — avance de estado lineal
    (Registrada→Asignada→EnAtencion→Resuelta→Cerrada) por técnico/admin y
    asignación de técnico por admin; bitácora registrada.
12. **build-run-verify** — `dotnet build` limpio y app responde en `:5212`.
13. **playwright specs** — `flujo-gestiones.spec.js` (5 tests, todos en verde).

**Credenciales de prueba:**
- Cliente: `cliente@proyectoia.com` / `Cliente123!`
- Técnico: `tecnico@proyectoia.com` / `Tecnico123!`
- Administrador: `admin@proyectoia.com` / `Admin123!`

**Nota para futuras sesiones:** el MVP didáctico está completo y funcional. Lo
que sigue depende de cerrar ambigüedades (sección 13 de `Documentación.md`):
anulación/reapertura, política de duplicados, edición de gestiones, catálogos
administrables por UI, dashboards, etc. No avanzar en eso sin pedirlo.

## 5. Cómo verificar que todo sigue funcionando

```bash
# Backend
cd src
dotnet build                      # debe compilar sin errores

# Levantar la web app manualmente (opcional, Playwright ya la levanta sola)
dotnet run --project ProyectoIA.Web --urls http://localhost:5212

# Pruebas E2E (recomendado: dejar que Playwright levante el server)
cd ../tests/e2e
npx playwright test               # smoke.spec.js debe seguir en verde
```

## 6. Reglas de trabajo a respetar

- No resolver ambigüedades de negocio (sección 13 de `Documentación.md`) sin
  preguntar al usuario — solo aplican los supuestos ya documentados en 19.4.
- No migrar las pruebas E2E de vuelta a .NET/NUnit sin que el usuario lo pida.
- No agregar dashboards, CRUD admin de usuarios/cecos, detección de
  duplicados, notificaciones ni exportes — están explícitamente fuera de
  alcance del MVP (sección 19.3).
- Mantener selectores de Playwright por `id` para que las pruebas no se
  rompan con cambios de texto en la UI.
- Actualizar este archivo (`HANDOFF.md`) y la sección 19 de
  `Documentación.md` si cambia el alcance o las decisiones técnicas.
