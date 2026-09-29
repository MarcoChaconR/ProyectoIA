# HANDOFF — Estado actual del desarrollo

> Actualizado el 28/09/2026. Leer este archivo junto con `Documentación.md`,
> `decisions.md` y `logs.md` antes de continuar.

## 1. Proyecto

Sistema interno para centralizar solicitudes de RPA, BPM y Power Platform.
Roles: Cliente, Técnico y Administrador. `Documentación.md` conserva el plan
original, las historias de usuario y las ambigüedades todavía pendientes.

## 2. Decisiones técnicas y de negocio vigentes

- **Stack:** .NET 10, ASP.NET Core MVC + Razor Views, EF Core y SQLite.
- **Arquitectura:** tres proyectos (`ProyectoIA.Domain`,
  `ProyectoIA.Infrastructure`, `ProyectoIA.Web`); no agregar una capa
  `Application` sin una nueva decisión.
- **Autenticación:** cookies y cuentas administrables, sin ASP.NET Identity.
  Las nuevas contraseñas usan PBKDF2-HMAC-SHA256 con salt aleatorio; los SHA256
  heredados se validan y se rehashean después de un login correcto.
- **Estados:** cinco estados lineales: Registrada → Asignada → EnAtencion →
  Resuelta → Cerrada. No hay anulación ni reapertura.
- **Asignación:** un técnico responsable por gestión.
- **Prioridad:** Baja, Media o Alta; el valor inicial es Media.
- **CECO y dependencias:** código CECO numérico almacenado como texto (ej.
  `5550`); cada dependencia pertenece a un CECO.
- **Historial:** usuarios y catálogos se desactivan, no se borran físicamente.
  La edición, asignación y transición de estado se registran en bitácora.
- **Concurrencia:** `Gestion.Version` es token optimista; una edición obsoleta
  devuelve un mensaje para recargar, no sobrescribe cambios.
- **E2E:** Playwright JavaScript en `tests/e2e`; el antiguo proyecto
  `tests/ProyectoIA.E2ETests` sigue sin uso.

## 3. Estado por fases

- **Fase 0 — Definición:** reglas críticas confirmadas en `decisions.md`
  (D-018 a D-025). Duplicados, notificaciones, adjuntos y exportaciones no se
  agregaron porque no forman parte del alcance implementado.
- **Fase 1 — Fundaciones:** solución, entidades, SQLite, migraciones, seed,
  autenticación y autorización por rol.
- **Fase 2 — Gestión:** crear, consultar y filtrar; detalle, notas públicas e
  internas; edición por técnico asignado/administrador; asignación; flujo de
  estados; auditoría y concurrencia optimista.
- **Fase 3 — Administración:** mantenimiento de CECOs, dependencias y tipos de
  solicitud; gestión de usuarios y roles; desactivación lógica.
- **Fase 4 — Dashboards:** vista global administrativa y vista del técnico con
  conteos por estado, prioridades altas, pendientes de asignación y tiempos
  promedio de asignación/cierre.
- **Fase 5 — Calidad:** build limpio; 9 pruebas E2E pasan, incluyendo
  autorización, privacidad de notas, edición concurrente y mantenimiento.
  PBKDF2 y base E2E aislada implementados. Aún falta ejecutar UAT con negocio
  y completar el procedimiento de restauración/despliegue del entorno piloto.

## 4. Estructura relevante

```text
src/
  ProyectoIA.Domain/          Entidades, enums y PasswordHasher
  ProyectoIA.Infrastructure/  AppDbContext y migraciones EF Core
  ProyectoIA.Web/
    Controllers/              Gestiones, Account, Usuarios, Catalogos, Dashboard
    Models/                   ViewModels validados
    Views/                    Razor Views
    app.db                    Base SQLite versionada
tests/
  e2e/                        Playwright JS, specs y app.e2e.db aislada
  ProyectoIA.E2ETests/        Proyecto legado NUnit, no activo
Documentación.md              Plan maestro y estado del alcance
decisions.md                  Decisiones técnicas y funcionales
logs.md                       Incidencias conocidas y soluciones
prompts.md                    Prompts usados para el proyecto
HANDOFF.md                    Este resumen actualizado
```

## 5. Cuentas de demostración

- Cliente: `cliente@proyectoia.com` / `Cliente123!`
- Técnico: `tecnico@proyectoia.com` / `Tecnico123!`
- Administrador: `admin@proyectoia.com` / `Admin123!`

## 6. Verificación

Desde la raíz:

```bash
dotnet build src/ProyectoIA.slnx
cd tests/e2e
E2E_PORT=5213 npx playwright test
```

La aplicación normal usa `http://localhost:5212`. Las pruebas no reutilizan
servidores ya activos; si 5212 está ocupado, usar otro puerto con `E2E_PORT`.
El servidor E2E se inicia con `ConnectionStrings__DefaultConnection` apuntando
a `tests/e2e/app.e2e.db`. `prepareDatabase.js` elimina únicamente esa base y
sus archivos auxiliares antes de iniciar el servidor.

## 7. Seguridad y cuidado de datos

- `src/ProyectoIA.Web/app.db` está versionada. No ejecutar pruebas contra ella,
  no borrarla y revisar cualquier diff de base antes de versionarlo.
- La aplicación llama `Database.Migrate()` al inicio. Hacer una copia SQLite
  consistente fuera del repositorio antes de actualizar/desplegar una base:

  ```bash
  sqlite3 src/ProyectoIA.Web/app.db ".backup '/ruta-segura/proyectoia-$(date +%Y%m%d-%H%M%S).db'"
  ```

- Para restaurar, detener la aplicación y restaurar una copia verificada; no
  sobrescribir la base activa mientras haya conexiones abiertas.
- La configuración y las contraseñas sembradas son didácticas. Revisar
  secretos, HTTPS, políticas operativas, respaldos y UAT antes de producción.
- Mantener selectores E2E por `id`.

## 8. Documentación y continuación

Actualizar `HANDOFF.md`, `decisions.md`, `prompts.md` y `logs.md` cuando cambie
el estado, se tome una decisión o se resuelva una incidencia. Actualizar
`Documentación.md` si cambia el alcance o una regla de negocio. No resolver
las ambigüedades de la sección 13 por cuenta propia; pedir confirmación antes
de implementar políticas nuevas.
