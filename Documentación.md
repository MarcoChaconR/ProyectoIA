# Documentación Maestra - Sistema de Gestión de Solicitudes Low Code / No Code

## 1. Propósito del Documento

Este documento consolida en un solo lugar el plan de desarrollo, historias de usuario, supuestos y ambigüedades del proyecto para facilitar continuidad en futuras sesiones.

Objetivo del sistema:
- Centralizar solicitudes de usuarios para soluciones de RPA, BPM y Power Platform.
- Estandarizar el flujo de atención.
- Garantizar trazabilidad y control por roles.

---

## 2. Contexto de la Solución

Problema a resolver:
- Las solicitudes para soluciones low code / no code suelen gestionarse en canales dispersos (correo, mensajería, documentos), dificultando priorización, trazabilidad y auditoría.

Solución propuesta:
- Implementar un sistema único de gestión de solicitudes con estados, asignación técnica, notas cronológicas y reportes operativos.

---

## 3. Stack Tecnológico

- Plataforma: .NET Core 10.
- Base de datos: SQLite.
- Enfoque de arquitectura recomendado: Clean Architecture.
- Capas sugeridas:
  - Dominio.
  - Aplicación.
  - Infraestructura.
  - Presentación.

---

## 4. Roles del Sistema

- Cliente.
- Técnico.
- Administrador.

Resumen de intención por rol:
- Cliente: consulta y seguimiento de sus solicitudes.
- Técnico: atención operativa de solicitudes asignadas.
- Administrador: control global, configuración y gobernanza.

---

## 5. Alcance Funcional

### 5.1 Pantallas base requeridas

- Consulta de gestiones.
- Crear gestión.
- Consulta y edición individual de gestión.
- Pantallas de dashboard.
- Gestión de usuarios.
- Gestión de Cecos.

### 5.2 Campos básicos del registro de gestión

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

### 5.3 Regla clave de trazabilidad

Cada gestión debe incluir un registro cronológico de notas de atención:
- Notas públicas: visibles para cliente, técnico y administrador.
- Notas internas: visibles solo para técnicos y administradores.

---

## 6. Requerimientos No Funcionales

- Seguridad por autenticación y autorización por roles.
- Trazabilidad de cambios con fecha y usuario.
- Auditoría de cambios relevantes (estado, asignación, edición).
- Validaciones de datos en backend y frontend.
- Interfaz responsive (desktop y móvil).
- Rendimiento objetivo inicial en consultas comunes menor o igual a 2 segundos en carga normal.

---

## 7. Modelo de Dominio (Alto Nivel)

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
- Una Gestion tiene muchas NotaGestion.
- Una Gestion pertenece a un solicitante y puede estar asignada a un técnico.
- Un Usuario tiene un Rol.
- Una Gestion referencia Ceco, Dependencia y TipoSolicitud.

---

## 8. Flujo Operativo Propuesto

1. Registro de gestión.
2. Validación de campos obligatorios.
3. Estado inicial.
4. Asignación de técnico.
5. Atención y notas cronológicas.
6. Cambios de estado según reglas.
7. Cierre o anulación según política definida.
8. Consulta histórica y auditoría.

Estados sugeridos iniciales:
- Registrada.
- Asignada.
- En atención.
- En espera.
- Resuelta.
- Cerrada.
- Anulada.

---

## 9. Plan de Desarrollo por Fases

### Fase 0 - Descubrimiento y Definición

- Cierre de ambigüedades críticas.
- Definición de estados y transiciones.
- Definición final de permisos por rol.
- Alineación de alcance MVP.

Entregables:
- Reglas de negocio aprobadas.
- Matriz de permisos aprobada.
- Acta de decisiones de ambigüedades.

### Fase 1 - Fundaciones Técnicas

- Estructura de solución .NET Core 10.
- Configuración SQLite y migraciones.
- Seguridad base (autenticación/autorización).
- Modelo de datos inicial.

Entregables:
- Solución compilable.
- Esquema base de datos desplegable.

### Fase 2 - Núcleo Funcional

- Crear gestión.
- Consulta de gestiones con filtros.
- Detalle de gestión.
- Edición individual.
- Asignación de técnico.
- Cambio de estado.
- Notas cronológicas públicas/internas.

Entregable:
- Flujo end-to-end funcional de gestión.

### Fase 3 - Administración

- Gestión de usuarios.
- Gestión de cecos.
- Gestión de catálogos auxiliares.

Entregable:
- Módulos administrativos operativos.

### Fase 4 - Dashboards

- Dashboard operativo técnico.
- Dashboard administrativo global.
- Indicadores base de operación.

Entregable:
- Tableros de monitoreo funcionales.

### Fase 5 - Calidad y Salida a Producción

- Pruebas unitarias, integración y aceptación.
- Pruebas de seguridad funcional.
- Plan de respaldo de SQLite.
- Checklist de salida a producción.

Entregable:
- MVP apto para piloto.

---

## 10. Backlog Inicial MVP

- Autenticación y roles.
- CRUD de gestiones.
- Reglas de estado.
- Asignación de técnico.
- Notas cronológicas con visibilidad.
- Consulta y filtros por estado, ceco, dependencia y técnico.
- Pantalla de detalle con historial.
- Gestión básica de usuarios.
- Gestión básica de cecos.
- Dashboard base.

---

## 11. Historias de Usuario (Formato Como, quiero, para)

### 11.1 Rol Cliente

HU-CLI-01
- Como cliente, quiero registrar una solicitud con los campos requeridos, para iniciar formalmente la atención de una necesidad de automatización.

HU-CLI-02
- Como cliente, quiero consultar mis solicitudes por estado, para conocer su avance en tiempo real.

HU-CLI-03
- Como cliente, quiero visualizar el detalle de cada solicitud, para validar que la información registrada sea correcta.

HU-CLI-04
- Como cliente, quiero ver solo las notas públicas de la gestión, para mantenerme informado sin acceder a discusiones internas.

HU-CLI-05
- Como cliente, quiero conocer la fecha de asignación y el técnico responsable, para saber quién atiende mi solicitud.

HU-CLI-06
- Como cliente, quiero recibir claridad del estado final (cerrada, anulada o rechazada), para entender el resultado y próximos pasos.

### 11.2 Rol Técnico

HU-TEC-01
- Como técnico, quiero ver mi bandeja de gestiones asignadas, para priorizar y planificar mi carga de trabajo.

HU-TEC-02
- Como técnico, quiero cambiar el estado de una gestión según reglas definidas, para reflejar el progreso real de la atención.

HU-TEC-03
- Como técnico, quiero registrar notas cronológicas en cada gestión, para dejar trazabilidad de análisis y acciones ejecutadas.

HU-TEC-04
- Como técnico, quiero marcar notas como públicas o internas, para comunicar al cliente cuando corresponda y reservar coordinación interna.

HU-TEC-05
- Como técnico, quiero editar los datos permitidos de la gestión, para corregir información y mantener la calidad del registro.

HU-TEC-06
- Como técnico, quiero filtrar gestiones por estado, tipo de solicitud y prioridad, para atender primero lo más crítico.

HU-TEC-07
- Como técnico, quiero consultar el historial completo de cambios de estado, para comprender el contexto antes de continuar la atención.

### 11.3 Rol Administrador

HU-ADM-01
- Como administrador, quiero visualizar todas las gestiones del sistema, para controlar la operación end-to-end.

HU-ADM-02
- Como administrador, quiero asignar o reasignar técnicos a una gestión, para balancear carga y cumplir tiempos de atención.

HU-ADM-03
- Como administrador, quiero administrar usuarios y roles, para asegurar que cada perfil tenga permisos adecuados.

HU-ADM-04
- Como administrador, quiero gestionar Cecos y dependencias, para estandarizar la clasificación organizacional de solicitudes.

HU-ADM-05
- Como administrador, quiero administrar el catálogo de tipos de solicitud, para ordenar y analizar la demanda de servicios.

HU-ADM-06
- Como administrador, quiero acceder a dashboards con métricas de volumen, estados y tiempos, para apoyar la toma de decisiones.

HU-ADM-07
- Como administrador, quiero auditar cambios en datos y estados de cada gestión, para cumplir gobernanza y trazabilidad.

HU-ADM-08
- Como administrador, quiero detectar posibles solicitudes duplicadas, para evitar esfuerzos repetidos y reprocesos.

HU-ADM-09
- Como administrador, quiero definir reglas de transición de estados, para mantener consistencia del flujo operativo.

### 11.4 Historias Transversales

HU-SEG-01
- Como organización, quiero que el acceso al sistema esté protegido por autenticación y autorización por roles, para resguardar la información.

HU-SEG-02
- Como organización, quiero que toda acción relevante quede registrada con usuario y fecha, para asegurar auditoría completa.

HU-SEG-03
- Como organización, quiero restringir visibilidad de notas internas, para evitar exposición de información sensible.

HU-CAL-01
- Como organización, quiero validaciones obligatorias en captura de datos, para prevenir registros incompletos o inconsistentes.

HU-CAL-02
- Como organización, quiero manejo de concurrencia en edición de gestiones, para evitar pérdida de cambios cuando dos usuarios actualizan al mismo tiempo.

### 11.5 Historias para Refinamiento de Ambigüedades

HU-AMB-01
- Como administrador, quiero poder anular una solicitud bajo condiciones definidas, para cancelar casos no procedentes con trazabilidad.

HU-AMB-02
- Como administrador, quiero diferenciar claramente cerrar, rechazar y anular solicitudes, para aplicar correctamente cada resultado del proceso.

HU-AMB-03
- Como técnico, quiero poder identificar solicitudes posiblemente duplicadas durante el registro, para evitar atender dos veces el mismo asunto.

HU-AMB-04
- Como administrador, quiero definir si el sistema bloquea o solo advierte duplicados, para equilibrar control y continuidad operativa.

HU-AMB-05
- Como cliente, quiero saber si puedo reabrir una solicitud cerrada y en qué condiciones, para gestionar casos no resueltos.

---

## 12. Supuestos Asumidos

### 12.1 Supuestos funcionales

1. El sistema inicial será web y orientado a uso interno.
2. El alcance MVP cubre gestión de solicitudes de punta a punta.
3. Se aplicarán tres roles base: cliente, técnico y administrador.
4. Las notas internas no serán visibles para cliente.
5. El flujo de estados existirá desde MVP, aunque su definición final está pendiente.
6. Catálogos de cecos/dependencias/tipos de solicitud serán administrables.

### 12.2 Supuestos técnicos

1. .NET Core 10 es la plataforma de desarrollo aprobada.
2. SQLite es suficiente para el arranque del MVP.
3. El sistema implementará auditoría y trazabilidad de acciones críticas.
4. Habrá validaciones tanto del lado servidor como de interfaz.
5. Se considerará una futura migración de base de datos si el volumen crece.

### 12.3 Supuestos de operación

1. Existirá una etapa de validación UAT con usuarios de negocio.
2. Se definirá un procedimiento de respaldos para SQLite antes de producción.
3. La gobernanza operativa recaerá en el rol administrador.

---

## 13. Ambigüedades Pendientes (Preguntas Abiertas)

### 13.1 Proceso y reglas de negocio

1. ¿Se puede anular una solicitud?
2. Si se puede anular, ¿en qué estados y con qué rol?
3. ¿Cuál es la diferencia exacta entre cerrar, rechazar y anular?
4. ¿Se puede reabrir una solicitud cerrada? ¿Quién y en qué plazo?
5. ¿Qué pasa si se generan dos solicitudes para un mismo asunto?
6. ¿El sistema bloquea duplicados o solo advierte?
7. ¿Cómo se define un duplicado (solicitante, tipo, similitud de texto, tiempo)?

### 13.2 Roles y permisos

1. ¿Cliente crea solicitudes o solo consulta?
2. ¿Cliente puede agregar notas públicas?
3. ¿Puede haber varios técnicos por solicitud?
4. ¿Qué acciones son exclusivas del administrador?
5. ¿Notas internas visibles para todos los técnicos o solo técnico asignado?

### 13.3 Datos y catálogos

1. ¿Referencia de ingreso es obligatoria en todos los casos?
2. ¿Qué campos son editables e inmutables por etapa?
3. ¿Ceco y dependencia son independientes o jerárquicos?
4. ¿Se permiten adjuntos en gestión o notas?

### 13.4 Métricas y operación

1. ¿Qué KPIs son obligatorios en dashboard MVP?
2. ¿Se requiere exportación a Excel/PDF?
3. ¿Se requieren notificaciones por correo?
4. ¿Cuál es la concurrencia esperada y volumen mensual?
5. ¿Cuál es el SLA objetivo por tipo de solicitud?

---

## 14. Criterios de Éxito del MVP

- Registro y seguimiento completo de solicitudes con campos obligatorios.
- Trazabilidad de cambios de estado y notas cronológicas.
- Separación efectiva de notas públicas e internas.
- Administración operativa de usuarios y cecos.
- Dashboard mínimo útil para gestión operativa.
- Validación satisfactoria en UAT.

---

## 15. Riesgos y Mitigaciones

Riesgo 1: Ambigüedad funcional no resuelta.
- Mitigación: cerrar decisiones críticas en Fase 0 con acta formal.

Riesgo 2: Escalabilidad limitada de SQLite.
- Mitigación: definir umbral de migración a motor servidor.

Riesgo 3: Exposición de información interna.
- Mitigación: controles estrictos de autorización y pruebas de seguridad.

Riesgo 4: Ediciones concurrentes.
- Mitigación: control de concurrencia optimista y manejo de conflictos.

---

## 16. Priorización de Cierre de Ambigüedades

Críticas (antes de iniciar desarrollo):
- Reglas de anulación/reapertura.
- Política de duplicados.
- Estados y transiciones oficiales.
- Permisos por rol.
- Visibilidad de notas.
- Alcance real del rol cliente.
- Campos obligatorios/editables por etapa.

Alta prioridad (antes de cierre MVP):
- KPIs de dashboard.
- Exportaciones.
- Notificaciones.
- Adjuntos.
- Criterios de rendimiento y concurrencia.

---

## 17. Trazabilidad de Documentos Relacionados

Este documento consolida la información inicialmente separada en:
- [plan.md](plan.md)
- [historias.md](historias.md)
- [ambiguedades.md](ambiguedades.md)

---

## 18. Próximos Pasos

1. Validar este documento con negocio y líderes técnicos.
2. Resolver ambigüedades críticas y registrar decisiones.
3. Confirmar alcance final del MVP.
4. Iniciar especificación técnica detallada por módulo.
5. Planificar primer sprint de implementación.
