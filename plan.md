# Plan de Desarrollo - Sistema de Gestión de Solicitudes Low Code / No Code

## 1. Resumen Ejecutivo

Este plan define la construcción de un sistema centralizado para gestionar solicitudes de automatización y soluciones digitales en RPA, BPM y Power Platform.

Objetivos principales:
- Centralizar el ingreso y seguimiento de solicitudes.
- Estandarizar el flujo de atención por estados.
- Asegurar trazabilidad completa por medio de bitácora y notas cronológicas.
- Separar visibilidad entre cliente y equipo técnico/administrativo.
- Proveer dashboards operativos y administrativos.

Stack tecnológico:
- Backend: .NET Core 10.
- Base de datos: SQLite.
- Arquitectura recomendada: Clean Architecture (Dominio, Aplicación, Infraestructura, Presentación).

Roles del sistema:
- Cliente.
- Técnico.
- Administrador.

---

## 2. Alcance Funcional

Módulos/pantallas base:
- Consulta de gestiones.
- Crear gestión.
- Consulta y edición individual de gestión.
- Dashboards.
- Gestión de usuarios.
- Gestión de Cecos.

Campos básicos del registro de gestión:
- Fecha.
- Centro de Costo.
- Dependencia.
- Tipo de solicitud.
- Objetivo de la solicitud.
- Detalle de la solicitud.
- Referencia de ingreso (oficio, memo, otro).
- Estado.
- Nombre del solicitante.
- Correo del solicitante.
- Técnico asignado.
- Fecha de cambio estado.
- Notas.
- Fecha asignación.

Regla clave:
- Cada gestión debe mantener un historial cronológico de notas.
- Las notas deben soportar visibilidad:
  - Públicas: visibles para cliente.
  - Internas: visibles solo para técnicos y administradores.

---

## 3. Requerimientos No Funcionales

- Seguridad por roles y permisos.
- Auditoría de cambios de estado, asignación y edición.
- Trazabilidad de autor/fecha en cada acción.
- Validación de datos en backend y frontend.
- Diseño responsive (desktop/móvil).
- Tiempo de respuesta objetivo para operaciones de consulta: <= 2 segundos en carga normal.

---

## 4. Diseño de Dominio (Alto Nivel)

Entidades principales:
- Gestion.
- NotaGestion.
- Usuario.
- Rol.
- Ceco.
- Dependencia.
- TipoSolicitud.
- EstadoGestion.
- BitacoraCambio.

Relaciones clave:
- Una gestión tiene muchas notas.
- Una gestión pertenece a un solicitante y puede tener técnico asignado.
- Un usuario posee un rol.
- Una gestión referencia ceco, dependencia y tipo de solicitud.

---

## 5. Matriz de Permisos (Resumen)

Cliente:
- Ver sus gestiones.
- Ver estados y notas públicas.
- (A definir) Crear nuevas solicitudes.

Técnico:
- Ver gestiones asignadas.
- Editar campos permitidos.
- Cambiar estado según reglas.
- Agregar notas públicas e internas.

Administrador:
- Ver y gestionar todas las gestiones.
- Asignar técnicos.
- Administrar usuarios.
- Administrar cecos y catálogos.
- Acceder a auditoría y dashboards globales.

---

## 6. Flujo Operativo Propuesto

1. Registro de gestión.
2. Validación de campos obligatorios.
3. Estado inicial (por ejemplo: Registrada/Pendiente).
4. Asignación de técnico.
5. Atención técnica con notas cronológicas.
6. Cambios de estado hasta cierre/anulación.
7. Consulta histórica y trazabilidad.

Estados sugeridos iniciales:
- Registrada.
- Asignada.
- En atención.
- En espera.
- Resuelta.
- Cerrada.
- Anulada.

---

## 7. Plan por Fases

### Fase 0 - Descubrimiento y Definición
- Cierre de ambigüedades funcionales.
- Definición de estados y transiciones.
- Definición de reglas de visibilidad de notas.
- Alineación de alcance MVP.

Entregables:
- Reglas de negocio aprobadas.
- Matriz de permisos aprobada.

### Fase 1 - Fundaciones Técnicas
- Estructura solución .NET Core 10.
- Configuración SQLite + migraciones.
- Módulo de autenticación/autorización.
- Modelo de datos inicial.

Entregables:
- Proyecto base compilable.
- Esquema BD inicial desplegable.

### Fase 2 - Núcleo Funcional de Gestión
- Crear gestión.
- Consultar listado y detalle.
- Editar gestión.
- Asignar técnico.
- Cambiar estado.
- Notas cronológicas con visibilidad.

Entregables:
- Flujo end-to-end de gestión funcional.

### Fase 3 - Administración
- Gestión de usuarios.
- Gestión de Cecos.
- Catálogos auxiliares (dependencias, tipos de solicitud).

Entregables:
- Módulos administrativos operativos.

### Fase 4 - Dashboards y Reportería
- Dashboard operativo por técnico.
- Dashboard global administrador.
- Indicadores base: volumen, estados, tiempos de atención.

Entregables:
- Tableros funcionales para monitoreo.

### Fase 5 - Calidad y Salida a Producción
- Pruebas unitarias, integración y aceptación.
- Hardening de seguridad.
- Plan de respaldos SQLite.
- Checklist de salida productiva.

Entregables:
- Versión MVP lista para piloto.

---

## 8. Backlog Inicial (MVP)

- Autenticación y roles.
- CRUD de gestiones.
- Reglas de estado.
- Asignación de técnico.
- Notas cronológicas (públicas/internas).
- Listado con filtros por estado, ceco, dependencia, técnico.
- Pantalla detalle con historial.
- Gestión básica de usuarios.
- Gestión básica de cecos.
- Dashboard básico.

---

## 9. Ambigüedades del Pedido (Preguntas Abiertas)

1. ¿Se puede anular una solicitud?
2. Si se puede anular, ¿en qué estados y con qué rol?
3. ¿Cuál es la diferencia entre anular, rechazar y cerrar?
4. ¿Se puede reabrir una solicitud cerrada?
5. ¿Qué pasa si se generan dos solicitudes para un mismo asunto?
6. ¿Debe bloquearse duplicado o solo advertirse?
7. ¿Cómo se define duplicado (solicitante + tipo + ventana temporal + similitud de texto)?
8. ¿El cliente crea solicitudes o solo consulta?
9. ¿El cliente puede agregar comentarios/notas públicas?
10. ¿Puede haber múltiples técnicos en una misma gestión?
11. ¿Qué campos son editables luego de crear la gestión?
12. ¿Referencia de ingreso es obligatoria siempre?
13. ¿Dependencia y Ceco son jerárquicos o independientes?
14. ¿Las notas internas las ve cualquier técnico o solo asignado + administrador?
15. ¿Se requieren notificaciones por correo en eventos clave?
16. ¿Se requiere adjuntar archivos a gestión/notas?
17. ¿Hay SLA por tipo de solicitud?
18. ¿Qué KPIs son obligatorios para dashboard MVP?
19. ¿Se requiere exportar a Excel/PDF?
20. ¿Volumen esperado de solicitudes y concurrencia de usuarios?

---

## 10. Criterios de Éxito del MVP

- Registro y seguimiento completo de solicitudes con todos los campos requeridos.
- Separación efectiva de visibilidad de notas.
- Flujo de estados auditable y consistente.
- Administración operativa de usuarios y cecos.
- Dashboard mínimo para toma de decisiones.
- UAT satisfactoria con usuarios de negocio.

---

## 11. Riesgos y Mitigaciones

Riesgo: ambigüedad funcional.
- Mitigación: cerrar preguntas abiertas en Fase 0 con acta aprobada.

Riesgo: escalabilidad de SQLite.
- Mitigación: definir umbrales de crecimiento y estrategia futura de migración a motor servidor.

Riesgo: fuga de información interna en notas.
- Mitigación: autorización estricta por rol y pruebas de seguridad específicas.

Riesgo: conflictos de edición concurrente.
- Mitigación: control de concurrencia optimista y trazabilidad de cambios.

---

## 12. Próximos Pasos Recomendados

1. Validar y responder preguntas de ambigüedad.
2. Aprobar matriz de estados y permisos.
3. Confirmar alcance exacto del MVP.
4. Iniciar especificación técnica detallada por módulo.
