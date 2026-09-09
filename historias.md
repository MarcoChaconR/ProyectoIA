# Historias de Usuario - Sistema de Gestión de Solicitudes LCNC

Formato: Como, quiero, para.

## 1. Historias del Rol Cliente

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

## 2. Historias del Rol Técnico

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

## 3. Historias del Rol Administrador

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

## 4. Historias Transversales de Seguridad y Calidad

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

## 5. Historias Enfocadas en Ambigüedades (Para Refinamiento)

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

## 6. Criterios de Aceptación Base (Mínimos)

Para toda historia funcional:
- Debe respetar permisos por rol.
- Debe registrar trazabilidad de acciones relevantes.
- Debe aplicar validaciones de campos obligatorios.
- Debe cumplir reglas de visibilidad de notas.

Para historias de estados:
- Debe validar transición permitida.
- Debe registrar fecha de cambio estado.
- Debe registrar usuario que ejecuta el cambio.

Para historias de duplicados:
- Debe aplicar regla de detección configurada.
- Debe informar al usuario de manera clara si bloquea o advierte.

## 7. Priorización Recomendada

Alta (MVP):
- HU-CLI-01, HU-CLI-02, HU-CLI-04.
- HU-TEC-01, HU-TEC-02, HU-TEC-03, HU-TEC-04.
- HU-ADM-01, HU-ADM-02, HU-ADM-03, HU-ADM-04.
- HU-SEG-01, HU-SEG-02, HU-SEG-03.

Media (v1.0):
- HU-CLI-03, HU-CLI-05, HU-TEC-05, HU-TEC-06, HU-ADM-05, HU-ADM-06, HU-ADM-07.

Refinamiento obligatorio antes de cierre de alcance:
- HU-AMB-01, HU-AMB-02, HU-AMB-03, HU-AMB-04, HU-AMB-05.
