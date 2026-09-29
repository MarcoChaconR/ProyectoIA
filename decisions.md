# Decisiones del Proyecto

Registro de decisiones de arquitectura y alcance del sistema de Gestión de Solicitudes Low Code / No Code. El objetivo es no volver a discutir lo que ya se decidió en cada sesión.

Formato de cada decisión: fecha, estado, contexto, decisión y consecuencias.

---

## D-001 — Plataforma .NET Core 10

- Fecha: 01/09/2026
- Estado: Aceptada
- Contexto: se necesitaba un backend estable y conocido para un sistema web interno.
- Decisión: usar .NET Core 10 (SDK instalado 10.0.400) como plataforma de desarrollo.
- Consecuencias: el entorno debe tener el SDK 10 instalado. `dotnet --version` tiene que devolver 10.0.400.

## D-002 — SQLite como base de datos del MVP

- Fecha: 01/09/2026
- Estado: Aceptada
- Contexto: el volumen inicial es bajo y se quería arrancar rápido sin levantar un motor servidor.
- Decisión: usar SQLite como base de datos, con la cadena de conexión `Data Source=app.db`.
- Consecuencias: se contempla migrar a un motor servidor si el volumen crece. SQLite tiene límites de concurrencia (ver D-013).

## D-003 — Tres roles del sistema

- Fecha: 01/09/2026
- Estado: Aceptada
- Contexto: se necesita separar visibilidad y permisos entre los actores.
- Decisión: definir tres roles base: Cliente, Técnico y Administrador.
- Consecuencias: la autorización se hace por rol (`[Authorize(Roles = "...")]`). No hay rol supervisor intermedio.

## D-004 — Arquitectura de 3 proyectos (en vez de Clean Architecture completa)

- Fecha: 22/09/2026
- Estado: Aceptada (reemplaza la propuesta inicial de 4 capas)
- Contexto: en el plan original se recomendaba Clean Architecture con capas Dominio, Aplicación, Infraestructura y Presentación.
- Decisión: simplificar a 3 proyectos: `ProyectoIA.Domain`, `ProyectoIA.Infrastructure` y `ProyectoIA.Web`. No se crea capa `Application` separada.
- Consecuencias: menos proyectos que mantener. La lógica de negocio simple vive en los controllers por ahora; si crece, se puede extraer después.

## D-005 — MVC + Razor Views (server-rendered, sin SPA)

- Fecha: 22/09/2026
- Estado: Aceptada
- Contexto: se necesitaba una UI navegable y estable para automatizar pruebas E2E.
- Decisión: usar ASP.NET Core MVC con Razor Views, sin framework SPA.
- Consecuencias: el HTML se renderiza en el servidor, lo que hace más confiables los selectores de Playwright.

## D-006 — Autenticación por cookie sin ASP.NET Identity

- Fecha: 22/09/2026
- Estado: Aceptada
- Contexto: Identity completo agregaba complejidad innecesaria para un MVP didáctico.
- Decisión: usar autenticación por cookie simple con 3 usuarios sembrados, sin ASP.NET Identity.
- Consecuencias: el login se resuelve en `AccountController` con claims. No hay registro, recuperación de contraseña ni bloqueo por intentos fallidos.

## D-007 — Hash de contraseñas SHA256 simple

- Fecha: 22/09/2026
- Estado: Aceptada (con deuda técnica conocida)
- Contexto: se necesitaba hashear las contraseñas sembradas y verificarlas en el login sin dependencias extra.
- Decisión: usar `PasswordHasher.Hash` con SHA256 (`Convert.ToHexString(SHA256.HashData(...))`).
- Consecuencias: suficiente para el MVP académico, pero no es seguro para producción (sin salt). Migrar a BCrypt o Identity si se publica de verdad.

## D-008 — EF Core + migraciones + seed con HasData

- Fecha: 23/09/2026
- Estado: Aceptada
- Contexto: se necesitaba un esquema reproducible y datos de prueba para desarrollo y tests.
- Decisión: usar EF Core con migraciones y sembrar los datos iniciales con `HasData` en `AppDbContext` (3 usuarios, 2 Cecos, 2 Dependencias, 3 TiposSolicitud y 2 gestiones de ejemplo).
- Consecuencias: los catálogos no tienen CRUD por UI en el MVP; se siembran por migración (fuera de alcance, ver D-014).

## D-009 — Migración automática al arrancar

- Fecha: 23/09/2026
- Estado: Aceptada
- Contexto: se quería que la app funcionara sin pasos manuales de migración.
- Decisión: llamar `db.Database.Migrate()` al arrancar en `Program.cs`.
- Consecuencias: la base se crea y actualiza sola al levantar la app. Esto permite que las pruebas E2E borren la base y la app la regenere.

## D-010 — Estados simplificados a 5 lineales

- Fecha: 22/09/2026
- Estado: Aceptada (supuesto temporal)
- Contexto: el plan proponía 7 estados (Registrada, Asignada, En atención, En espera, Resuelta, Cerrada, Anulada).
- Decisión: reducir a 5 con transición lineal: Registrada → Asignada → EnAtencion → Resuelta → Cerrada. Se eliminan "En espera" y "Anulada" del MVP.
- Consecuencias: `CambiarEstado` simplemente avanza `(int)estado + 1`. No hay bifurcaciones ni reglas por rol/tipo. Pendiente de revisar al cerrar ambigüedades.

## D-011 — Pruebas E2E con Playwright en Node.js/JavaScript

- Fecha: 22/09/2026
- Estado: Aceptada (reemplaza el binding .NET/NUnit)
- Contexto: se quería seguir la convención del ejemplo de la Semana 5 (`SOFT734_T5_Playwright`).
- Decisión: escribir las pruebas E2E con `@playwright/test` en JavaScript (Node), no con Microsoft.Playwright.NUnit.
- Consecuencias: el proyecto `tests/ProyectoIA.E2ETests` (.NET/NUnit) quedó creado sin uso. No se borra, pero no se desarrolla ahí. Las pruebas activas viven en `tests/e2e`.

## D-012 — Playwright levanta el servidor con webServer

- Fecha: 22/09/2026
- Estado: Aceptada
- Contexto: a diferencia del prototipo estático de la Semana 5, esta app es un servidor real.
- Decisión: configurar `webServer` para levantar un servidor propio antes de correr las pruebas.
- Consecuencias: `reuseExistingServer: false` evita conectarse a la app de desarrollo; `E2E_PORT` permite usar un puerto alternativo y `workers: 1` mantiene las corridas seriales.

## D-013 — Borrado de la base antes de cada corrida de pruebas

- Fecha: 22/09/2026
- Estado: Reemplazada por D-021
- Contexto: las pruebas E2E fallaban de forma no determinista cuando quedaban datos de corridas anteriores.
- Decisión: la implementación inicial borraba `app.db`, `app.db-shm` y `app.db-wal` antes de cada corrida.
- Consecuencias: se reemplazó por una base E2E separada, limpiada por `prepareDatabase.js` antes del arranque. No borrar la base versionada de desarrollo.

## D-014 — Replan MVP didáctico con supuestos temporales

- Fecha: 22/09/2026
- Estado: Aceptada
- Contexto: había ~20 ambigüedades de negocio sin cerrar (sección 13 de `Documentación.md`) que bloqueaban el desarrollo.
- Decisión: reducir el alcance a un MVP navegable tomando supuestos temporales (el cliente sí crea solicitudes, sin reapertura, sin validación de duplicados, transición lineal). Fuera de alcance: dashboards, CRUD admin por UI, duplicados, notificaciones, exportes, UI de bitácora.
- Consecuencias: no se deben implementar esas funcionalidades fuera de alcance sin pedido explícito. Los supuestos están en `Documentación.md` sección 19.4.

## D-015 — Selectores de Playwright por `id`

- Fecha: 22/09/2026
- Estado: Aceptada
- Contexto: los selectores por texto se rompían cuando cambiaba una palabra en la UI.
- Decisión: usar selectores por `id` (`#login-form`, `#estado`, `#btn-login`, etc.) en todas las pruebas.
- Consecuencias: al tocar la UI hay que mantener los `id`, no solo el texto.

## D-016 — Puerto 5212

- Fecha: 22/09/2026
- Estado: Aceptada
- Contexto: se necesitaba un puerto fijo para desarrollo y para que Playwright apunte al server.
- Decisión: usar `http://localhost:5212` (definido en `launchSettings.json` y en `playwright.config.js`).
- Consecuencias: cualquier cambio de puerto debe hacerse en ambos archivos.

## D-017 — Versionar la base de datos `app.db` en git

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: la base con datos de ejemplo no se subía al repo porque el `.gitignore` ignoraba `*.db`.
- Decisión: quitar `*.db` del `.gitignore` para versionar `app.db` (proyecto académico, datos ficticios). Se mantienen ignorados `*.db-shm` y `*.db-wal` por ser temporales.
- Consecuencias: la base queda en el repo. Si se regenera, conviene revisar el diff antes de commitear.

## D-018 — Completar el alcance del plan original

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: el MVP estaba completo, pero todavía faltaban fases del plan original (administración, dashboards y calidad/salida).
- Decisión: continuar el desarrollo hasta cubrir el plan original, conservando la arquitectura y el stack ya acordados.
- Consecuencias: las funciones excluidas temporalmente del MVP vuelven al alcance; las reglas de negocio que no se hayan definido deben confirmarse antes de implementarlas.

## D-019 — Reglas confirmadas para ampliar el flujo

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: se necesitaban reglas para implementar las fases pendientes sin alterar el flujo existente.
- Decisión: mantener los cinco estados lineales actuales, permitir un solo técnico responsable por gestión y desactivar usuarios en vez de borrarlos físicamente.
- Consecuencias: no se agregan anulación ni reapertura; las referencias históricas a usuarios se conservan.

## D-020 — Catálogo de CECOs y dependencias organizacionales

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: los CECOs son códigos numéricos como `5550` y pueden cambiar por modificaciones organizacionales; además, se solicitó mantenimiento de este catálogo.
- Decisión: mantener un catálogo administrable de CECOs y relacionar cada dependencia con un CECO. El código se almacenará como texto para conservar su formato.
- Consecuencias: crear/editar gestiones debe validar que la dependencia pertenezca al CECO seleccionado; preservar el historial de gestiones cuando cambie la estructura organizacional.

## D-021 — Base de datos E2E aislada

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: `app.db` está versionada y la configuración inicial de pruebas podía borrarla o reutilizar un servidor conectado a esa base.
- Decisión: las pruebas E2E usarán una base separada (`tests/e2e/app.e2e.db`), la limpiarán antes de arrancar su servidor y pasarán su ruta por `ConnectionStrings__DefaultConnection`.
- Consecuencias: las pruebas ya no borran ni modifican la base de desarrollo/versionada.

## D-022 — Hash seguro de contraseñas con transición desde SHA256

- Fecha: 28/09/2026
- Estado: Aceptada (reemplaza D-007)
- Contexto: la implementación SHA256 sin salt era una deuda conocida y la fase de calidad requiere endurecer autenticación.
- Decisión: usar PBKDF2-HMAC-SHA256 con salt aleatorio e iteraciones configuradas para nuevas contraseñas; validar hashes SHA256 heredados y rehashearlos después de un login válido.
- Consecuencias: la migración actualiza los hashes de las cuentas de demostración; los registros antiguos no quedan bloqueados durante el cambio.

## D-023 — Prioridades para gestión y reportería

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: la historia HU-TEC-06 requiere priorizar y filtrar solicitudes, pero el modelo inicial no tenía este campo.
- Decisión: agregar prioridad Baja, Media y Alta; las gestiones nuevas comienzan en Media.
- Consecuencias: edición autorizada, filtros y dashboards usan estos valores; los datos existentes se migran a Media salvo el seed de prioridad alta.

## D-024 — Concurrencia optimista de gestiones

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: el plan exige evitar que una edición tardía sobrescriba cambios recientes.
- Decisión: usar un campo `Version` como token de concurrencia de EF Core y mostrar un error recuperable si otra operación actualizó la gestión.
- Consecuencias: edición, asignación y cambios de estado incrementan la versión; el usuario debe recargar antes de volver a enviar una edición obsoleta.

## D-025 — Desactivación lógica de catálogos

- Fecha: 28/09/2026
- Estado: Aceptada
- Contexto: los cambios organizacionales no deben invalidar gestiones históricas que referencian CECOs, dependencias o tipos.
- Decisión: desactivar catálogos en lugar de borrarlos físicamente; permitir su reactivación desde la pantalla de mantenimiento.
- Consecuencias: nuevos registros solo pueden usar elementos activos y las gestiones existentes conservan sus claves foráneas.
