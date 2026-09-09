# Supuestos del Proyecto

Documento de supuestos base identificados durante la definición de plan e historias para el sistema de Gestión de Solicitudes Low Code / No Code.

Objetivo:
- Hacer explícitas las condiciones asumidas para planificar y diseñar.
- Reducir ambigüedades y retrabajo.
- Facilitar validación con negocio antes de desarrollo.

---

## 1. Supuestos Funcionales

1. El sistema centralizará solicitudes de soluciones RPA, BPM y Power Platform.
2. El alcance inicial corresponde a un MVP operativo.
3. Los roles base serán: cliente, técnico y administrador.
4. Se implementarán las pantallas mínimas: consulta de gestiones, crear gestión, detalle/edición, dashboards, gestión de usuarios y gestión de cecos.
5. Cada gestión registrará los campos definidos en el requerimiento inicial.
6. Cada gestión contará con historial cronológico de notas.
7. Las notas manejarán al menos dos visibilidades: pública e interna.
8. El cliente no visualizará notas internas.
9. Cada cambio de estado registrará fecha de cambio de estado.
10. El flujo de estados existirá desde MVP, aunque su matriz final está pendiente de aprobación.

---

## 2. Supuestos Técnicos

1. El backend se desarrollará con .NET Core 10.
2. La base de datos inicial será SQLite.
3. Se usará arquitectura en capas con separación de dominio, aplicación e infraestructura.
4. Habrá autenticación y autorización basada en roles.
5. Se registrará trazabilidad de eventos relevantes en bitácora de auditoría.
6. El sistema contemplará validaciones de datos tanto en backend como frontend.
7. Se incorporará manejo de concurrencia para evitar pérdida de cambios en edición simultánea.
8. La solución se diseñará con posibilidad de migrar a un motor de base de datos servidor en crecimiento.

---

## 3. Supuestos de Seguridad y Gobierno

1. La información interna entre técnicos y administradores requiere control estricto de visibilidad.
2. Los permisos se controlarán por rol y por acción.
3. Toda acción crítica debe dejar evidencia de usuario y fecha.
4. El administrador tendrá acceso a auditoría y trazabilidad completa.
5. Las reglas de cumplimiento normativo específicas se definirán con negocio/seguridad en fase de definición.

---

## 4. Supuestos Operativos

1. El MVP será de uso interno organizacional.
2. Los catálogos de cecos, dependencias y tipos de solicitud tendrán administración controlada.
3. Se espera disponer de ambientes de desarrollo y pruebas antes de producción.
4. Existirá estrategia de respaldo para SQLite antes de salida productiva.
5. El proceso incluirá validación UAT con usuarios clave antes del pase a producción.

---

## 5. Supuestos Pendientes de Confirmación

1. Si el cliente podrá crear solicitudes directamente o solo consultar.
2. Si se permite anular solicitudes y bajo qué condiciones.
3. Si se permite reabrir solicitudes cerradas.
4. Regla definitiva para manejo de duplicados: bloquear, advertir o fusionar.
5. Definición final de estados y transiciones permitidas.
6. Definición de SLA por tipo de solicitud.
7. Necesidad de notificaciones por correo en eventos clave.
8. Necesidad de adjuntos en solicitudes o notas.
9. Definición de KPIs obligatorios para dashboard MVP.
10. Volumen esperado y concurrencia objetivo para validar límites de SQLite.

---

## 6. Hallazgo Principal

Actualmente, el proyecto tiene un conjunto robusto de supuestos para iniciar diseño y estimación, pero aún depende de decisiones funcionales críticas para congelar alcance MVP sin riesgo de retrabajo.

Las decisiones más urgentes a cerrar son:
1. Anulación y reapertura de solicitudes.
2. Detección y tratamiento de duplicados.
3. Matriz de estados y permisos por rol.
4. Visibilidad exacta de notas internas.

---

## 7. Recomendación de Uso

Se recomienda usar este documento como checklist de validación al inicio de cada fase:
1. Confirmar supuestos vigentes.
2. Marcar supuestos validados por negocio.
3. Registrar cambios de supuesto con fecha y responsable.
4. Trasladar supuestos invalidados a la lista de riesgos y acciones de mitigación.
