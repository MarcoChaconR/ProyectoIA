# Log de incidencias

Bitácora de errores, fallos y problemas que fueron apareciendo durante el proyecto, con su causa raíz y la solución que se aplicó. Si el modelo se vuelve a encontrar con alguna de estas situaciones, que mire acá antes de improvisar.

---

## INC-001 — El servidor se cae al levantarlo en primer plano

- Fecha: 28/09/2026
- Descripción: al levantar la app con `dotnet run` en primer plano, el proceso se cortaba y el servidor quedaba caído (curl devolvía 000 y el puerto 5212 libre).
- Causa raíz: `dotnet run` es un proceso de larga duración. Al ejecutarlo dentro de una sesión con timeout, el shell lo termina al cumplirse el tiempo.
- Solución: levantarlo en segundo plano con `nohup dotnet run --project src/ProyectoIA.Web --launch-profile http > /tmp/proyectoia.log 2>&1 &` y validar con `curl -s -o /dev/null -w "%{http_code}" http://localhost:5212/` que devuelva 200.

## INC-002 — La base de datos no se subía al repo

- Fecha: 28/09/2026
- Descripción: `src/ProyectoIA.Web/app.db` no aparecía en los commits, por más que estuviera en el directorio.
- Causa raíz: el `.gitignore` tenía la regla genérica `*.db` que ignora cualquier base SQLite.
- Solución: quitar `*.db` del `.gitignore` (dejar `*.db-shm` y `*.db-wal`, que son temporales) y hacer `git add` del archivo. Ver `decisions.md` D-017.

## INC-003 — Repo se llenaba con un entregable grande

- Fecha: 28/09/2026
- Descripción: `Entregable semana 5.zip` (~83 MB) se iba a versionar, ensuciando el historial y arriesgando el límite de GitHub.
- Causa raíz: archivo binario de entregable suelto en la raíz, sin regla de ignore.
- Solución: agregar el zip y `.DS_Store` al `.gitignore` para no versionarlos.

## INC-004 — Pruebas E2E no deterministas por datos residuales

- Fecha: 22/09/2026
- Descripción: las pruebas de Playwright fallaban de forma intermitente según el estado que dejara una corrida anterior.
- Causa raíz: la base SQLite acumulaba gestiones y notas entre corridas, y las pruebas asumían un estado inicial fijo.
- Solución: `globalSetup.js` borra `app.db`, `app.db-shm` y `app.db-wal` antes de cada corrida; la app las recrea con migración + seed al arrancar. Ver `decisions.md` D-013.

## INC-005 — Proyecto de pruebas E2E creado y sin uso

- Fecha: 22/09/2026
- Descripción: se creó `tests/ProyectoIA.E2ETests` (NUnit + Microsoft.Playwright.NUnit) que terminó sin usarse.
- Causa raíz: se cambió la decisión a mitad de camino y se optó por Playwright en Node.js/JavaScript para seguir la convención del ejemplo de la Semana 5.
- Solución: no borrar el proyecto (está documentado como "sin uso" en `HANDOFF.md`), pero no desarrollar pruebas ahí. Las pruebas activas están en `tests/e2e`. Ver `decisions.md` D-011.

## INC-006 — Selectores frágiles por texto en las pruebas

- Fecha: 22/09/2026
- Descripción: las pruebas se rompían cada vez que se cambiaba una palabra en la UI.
- Causa raíz: los locators apuntaban a texto visible en vez de a identificadores estables.
- Solución: usar selectores por `id` (`#btn-login`, `#estado`, etc.). Ver `decisions.md` D-015.

## INC-007 — Las pruebas E2E podían borrar la base versionada

- Fecha: 28/09/2026
- Descripción: `globalSetup.js` eliminaba `src/ProyectoIA.Web/app.db` antes de cada corrida, pero esa base ahora está versionada y puede contener cambios del proyecto.
- Causa raíz: la configuración de pruebas compartía la base de desarrollo y además permitía reutilizar cualquier servidor que estuviera escuchando en el puerto 5212.
- Solución: separar la base E2E en `tests/e2e/app.e2e.db`, pasar esa ruta al servidor de pruebas por `ConnectionStrings__DefaultConnection` y desactivar la reutilización de servidores existentes. La base de pruebas es ignorada por git.

## INC-008 — La base aislada no podía abrirse al iniciar las pruebas

- Fecha: 28/09/2026
- Descripción: Playwright iniciaba el servidor con una ruta de base E2E dentro de `.test-data/`, pero SQLite respondía `unable to open database file`.
- Causa raíz: el servidor podía arrancar antes de que el setup creara el directorio `.test-data`.
- Solución: ubicar la base directamente en `tests/e2e`, cuyo directorio ya existe.

## INC-009 — El setup E2E borraba la base después de iniciar el servidor

- Fecha: 28/09/2026
- Descripción: después de aislar la base, un flujo E2E falló con `no such table: Usuarios`.
- Causa raíz: `globalSetup` se ejecutaba después de iniciar `webServer` y borraba el archivo SQLite ya abierto; las siguientes conexiones creaban una base vacía.
- Solución: mover la limpieza a `prepareDatabase.js` y ejecutarla como primer comando de `webServer`, antes de `dotnet run`; quitar `globalSetup` para evitar borrar la base en caliente.

## INC-010 — Selector E2E ambiguo al validar una edición

- Fecha: 28/09/2026
- Descripción: una prueba de edición encontraba dos coincidencias para el objetivo, una en los datos de la gestión y otra en la bitácora.
- Causa raíz: el selector buscaba texto parcial sin exigir coincidencia exacta.
- Solución: usar `getByText(texto, { exact: true })` para que la prueba valide el campo visible sin confundirse con la entrada de auditoría.

## INC-011 — Prueba de autorización esperaba un HTTP 403 directo

- Fecha: 28/09/2026
- Descripción: una prueba de acceso no autorizado falló porque la navegación terminó con HTTP 200.
- Causa raíz: la autenticación por cookie redirige las respuestas `Forbid` a `/Account/AccessDenied`, donde la respuesta final es 200.
- Solución: comprobar que el navegador termina en la pantalla de acceso denegado en vez de esperar un 403 en la respuesta final.

## INC-012 — Una nota marcada como interna se guardaba como pública

- Fecha: 28/09/2026
- Descripción: un cliente podía ver una nota interna agregada por un administrador.
- Causa raíz: el modelo de formulario inicializaba `EsPublica` en `true`; al desmarcar el checkbox, el navegador no enviaba ese campo y el inicializador conservaba `true`.
- Solución: usar `false` como valor predeterminado del modelo; la acción del cliente sigue forzando sus notas a públicas. Se añadió una prueba E2E que verifica el indicador interno para el equipo y que el cliente no vea el texto.

---

## Pendientes conocidos (deuda técnica)

- El login conserva compatibilidad temporal con hashes SHA256 heredados; se reemplazan por PBKDF2 al iniciar sesión correctamente. Ver `decisions.md` D-022.
- SQLite no soporta bien concurrencia alta; si el volumen crece, migrar a un motor servidor (umbral a definir).
- El proyecto `tests/ProyectoIA.E2ETests` (.NET/NUnit) sigue en el repo sin uso; decidir si se elimina o se reactiva.
