# Ambigüedades del Proyecto

Documento de preguntas abiertas para cerrar definición funcional, técnica y operativa del sistema de Gestión de Solicitudes Low Code / No Code.

Objetivo:
- Identificar todo lo que aún requiere decisión.
- Evitar retrabajo en desarrollo.
- Asegurar criterios claros para alcance MVP y versiones posteriores.

---

## 1. Ambigüedades Funcionales del Proceso

1. ¿Se puede anular una solicitud?
2. Si se puede anular, ¿en qué estados se permite?
3. ¿Qué rol puede anular una solicitud: cliente, técnico o administrador?
4. ¿Anular y rechazar significan lo mismo o son resultados distintos?
5. ¿Cuál es la diferencia exacta entre estado cerrada, anulada y rechazada?
6. ¿Se puede reabrir una solicitud cerrada?
7. Si se puede reabrir, ¿quién puede hacerlo y en qué plazo?
8. ¿La reapertura crea una nueva gestión o continúa la existente?
9. ¿Qué pasa si se generan dos solicitudes para un mismo asunto?
10. ¿El sistema debe bloquear solicitudes duplicadas o solo advertir?
11. ¿Cómo se define duplicado: por solicitante, tipo, texto similar y ventana de tiempo?
12. ¿Las solicitudes relacionadas deben poder fusionarse?
13. Si se fusionan, ¿qué solicitud queda como principal y qué pasa con el historial?
14. ¿El cliente puede crear solicitudes o solo consultarlas?
15. ¿El cliente puede agregar comentarios/notas públicas después de crear la solicitud?
16. ¿El cliente puede editar su solicitud luego del registro?
17. ¿Qué campos se consideran obligatorios al crear y cuáles al asignar?
18. ¿Qué campos son editables y cuáles inmutables después de crear la gestión?
19. ¿Referencia de ingreso es siempre obligatoria o solo en ciertos tipos de solicitud?
20. ¿Se requiere adjuntar documentos (oficio/memo) al crear la gestión?

---

## 2. Ambigüedades del Flujo de Estados

1. ¿Cuáles son los estados oficiales del ciclo de vida?
2. ¿Cuál es el estado inicial por defecto al crear una solicitud?
3. ¿Qué transiciones de estado están permitidas?
4. ¿Qué transiciones están prohibidas?
5. ¿Se requiere justificación obligatoria para cada cambio de estado?
6. ¿Cambiar a estado cerrada exige nota pública final?
7. ¿Cambiar a estado anulada exige motivo obligatorio?
8. ¿Se puede mover de cerrada a en atención directamente?
9. ¿Se requiere aprobación de administrador para ciertas transiciones?
10. ¿Debe registrarse automáticamente fecha de cambio de estado en cada transición?
11. ¿Debe existir un historial visible de todas las transiciones?
12. ¿Se manejarán SLA por estado o por tipo de solicitud?
13. ¿Qué ocurre cuando se incumple un SLA?
14. ¿Habrá estado en espera y bajo qué reglas entra/sale?
15. ¿Se permite cierre automático por inactividad?

---

## 3. Ambigüedades de Roles y Permisos

1. ¿El rol cliente tiene acceso autenticado propio o acceso por enlace/código?
2. ¿El rol técnico puede ver solo sus gestiones o todas las del área?
3. ¿El rol técnico puede reasignar una gestión o solo administrador?
4. ¿Puede existir más de un técnico asignado por gestión?
5. ¿El administrador puede actuar como técnico dentro de una gestión?
6. ¿Las notas internas las ve cualquier técnico o solo el técnico asignado y administradores?
7. ¿El cliente puede ver nombre del técnico asignado?
8. ¿Quién puede editar cecos, dependencias y tipos de solicitud?
9. ¿Se necesita rol supervisor intermedio además de técnico y administrador?
10. ¿Se requiere segregación por dependencia/ceco para acceso a datos?
11. ¿Se necesita control de permisos por acción (ver, crear, editar, cerrar, anular)?
12. ¿Quién puede eliminar usuarios y qué pasa con su historial de acciones?

---

## 4. Ambigüedades de Notas y Trazabilidad

1. ¿Toda gestión debe tener al menos una nota inicial obligatoria?
2. ¿Las notas pueden editarse después de creadas?
3. ¿Las notas pueden eliminarse o solo marcarse como corregidas?
4. ¿Debe conservarse historial de ediciones de notas?
5. ¿Las notas internas deben poder convertirse en públicas?
6. ¿Debe permitirse adjuntar archivos en notas?
7. ¿Qué metadatos son obligatorios por nota (autor, fecha, tipo, estado asociado)?
8. ¿Las notas deben mostrarse en orden ascendente o descendente por fecha?
9. ¿Se registra trazabilidad solo de estados o también de cualquier cambio de campo?
10. ¿Se debe guardar valor anterior y nuevo en cada cambio?
11. ¿Se debe registrar IP/dispositivo del usuario en acciones críticas?
12. ¿Qué usuarios pueden consultar bitácora completa?
13. ¿Por cuánto tiempo se conserva la trazabilidad?

---

## 5. Ambigüedades de Catálogos y Datos Maestros

1. ¿Centro de costo y dependencia son catálogos independientes o jerárquicos?
2. ¿Un ceco puede pertenecer a múltiples dependencias?
3. ¿Los catálogos deben manejar estado activo/inactivo?
4. ¿Qué pasa con gestiones históricas cuando se inactiva un ceco?
5. ¿Tipo de solicitud será catálogo fijo o administrable?
6. ¿Cada tipo de solicitud tendrá SLA propio?
7. ¿Cada tipo de solicitud tendrá campos adicionales específicos?
8. ¿Se requiere codificación única para cecos y dependencias?
9. ¿Se validará correo del solicitante contra dominio corporativo?
10. ¿Se admite solicitante externo a la organización?

---

## 6. Ambigüedades de Consulta, Búsqueda y Dashboard

1. ¿Qué filtros son obligatorios en consulta de gestiones?
2. ¿Se requiere búsqueda por texto libre en objetivo/detalle/notas?
3. ¿La consulta debe incluir exportación a Excel o PDF?
4. ¿Se requiere paginación, ordenamiento y guardado de filtros favoritos?
5. ¿Qué indicadores son obligatorios para dashboard MVP?
6. ¿Qué diferencias habrá entre dashboard técnico y dashboard administrador?
7. ¿Los dashboards deben mostrar datos en tiempo real o actualización periódica?
8. ¿Se requiere comparar métricas por semana/mes/trimestre?
9. ¿Se necesitan indicadores por ceco, dependencia, tipo y técnico?
10. ¿Se deben incluir alertas visuales por SLA próximo a vencer?

---

## 7. Ambigüedades Técnicas y de Plataforma

1. ¿Se implementará como aplicación web interna, pública o híbrida?
2. ¿Qué método de autenticación se usará en MVP (local, AD, Azure AD)?
3. ¿Se requiere integración con correo corporativo para notificaciones?
4. ¿Se requiere integración con sistemas externos (ERP, mesa de ayuda, BPM)?
5. ¿Cuál es el volumen esperado de solicitudes por mes?
6. ¿Cuántos usuarios concurrentes se esperan en hora pico?
7. ¿SQLite es definitivo o temporal para MVP?
8. ¿Cuál es el umbral para migrar de SQLite a un motor servidor?
9. ¿Se necesita API pública para integración futura?
10. ¿Se requiere carga masiva de solicitudes por archivo?
11. ¿Se necesitan ambientes separados: desarrollo, QA y producción?
12. ¿Se requiere CI/CD desde el inicio?

---

## 8. Ambigüedades de Seguridad, Auditoría y Cumplimiento

1. ¿Qué estándar/regulación de seguridad aplica al proyecto?
2. ¿Se requiere cifrado de datos sensibles en reposo?
3. ¿Se requiere cifrado en tránsito con certificados corporativos?
4. ¿Qué eventos de auditoría son obligatorios por normativa interna?
5. ¿Quién puede acceder a logs de auditoría?
6. ¿Se requiere retención mínima/máxima de datos?
7. ¿Se debe anonimizar información al cerrar periodos?
8. ¿Se necesitan controles anti-CSRF y políticas de sesión específicas?
9. ¿Se requiere doble factor de autenticación?
10. ¿Cómo se gestionará bloqueo de cuenta por intentos fallidos?

---

## 9. Ambigüedades de Operación y Soporte

1. ¿Quién administrará funcionalmente catálogos y usuarios en operación diaria?
2. ¿Quién será dueño del proceso para resolver solicitudes atascadas?
3. ¿Cuál es el procedimiento de contingencia ante caída del sistema?
4. ¿Qué frecuencia de respaldo se requiere para SQLite?
5. ¿Cómo se validará recuperación de respaldo?
6. ¿Qué mesa de ayuda atenderá incidencias del sistema?
7. ¿Qué nivel de soporte (SLA de soporte) tendrá la aplicación?
8. ¿Qué criterios habilitan pase a producción?
9. ¿Qué criterios habilitan cierre de MVP?
10. ¿Qué estrategia de capacitación requerirá cada rol?

---

## 10. Priorización Recomendada de Cierre

Críticas (resolver antes de desarrollo):
1. ¿Se puede anular una solicitud y bajo qué reglas?
2. ¿Qué pasa con solicitudes duplicadas y cómo se detectan?
3. ¿Estados oficiales y transiciones permitidas?
4. ¿Permisos exactos por rol para ver/editar/cerrar/anular?
5. ¿Regla de visibilidad de notas internas?
6. ¿Cliente crea solicitudes o solo consulta?
7. ¿Campos obligatorios y editables por etapa?
8. ¿SLA y criterios de vencimiento?

Alta prioridad (resolver antes de cierre de MVP):
1. Definición de KPIs de dashboard MVP.
2. Exportaciones requeridas.
3. Notificaciones por correo.
4. Adjuntos en gestión/notas.
5. Concurrencia esperada y límites de SQLite.

Media prioridad (puede ir en v1.1):
1. Reglas avanzadas de deduplicación.
2. Automatizaciones de reasignación.
3. Integraciones externas no críticas.

---

## 11. Formato de Decisión Sugerido

Para cerrar cada ambigüedad se recomienda registrar:
- Pregunta.
- Decisión tomada.
- Responsable de la decisión.
- Fecha.
- Impacto en negocio.
- Impacto técnico.
- Versión objetivo (MVP o v1.1).

Este registro evita retrabajos y mantiene trazabilidad de alcance.
