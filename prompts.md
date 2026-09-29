# Prompts usados

Acá voy anotando los prompts que le fui pasando al modelo para armar el proyecto, así me acuerdo de cómo le pedí las cosas y puedo repetirlas o mejorarlas. Los escribo tal cual se los dije, sin adornos.

## Arranque y definición

Requiero realizar toda la documentación en MD como un plan de desarrollo.md para la idea: Gestión de solicitudes para low code - no code, el cual es un sistema para centralizar las solicitudes de los usuarios para la creación de solicitudes para desarrollo de soluciones para RPA, BPM, power Platform.

Stack tecnologico:

El sistema se debe desarrollar en .net core 10, con base de datos en sqlite para el proyecto, algunos de los roles que debe tener el sistema son (cliente, técnico y administrador),

Especificaciones:

Campos básicos que debe tener el registro de gestion:

Fecha,
Centro de Costo,
dependencia,
tipo de solicitud,
objetivo de la solicitud,
detalle de la solicitud,
referencia de ingreso (oficio, memo, otro),
Estado,
Nombre del solicitante,
Correo del solicitante,
Técnico asignado,
Fecha de cambio estado,
Notas,
Fecha asignación
Pantallas básicas que dene incorporar el sistema:

Consulta de gestiones
Crear gestión
Consulta y edición individual de gestión
Pantallas de dashboard
Gestion de Usuarios
Gestion de Cecos
Cada gestión debe tener un registro de notas cronológico de la atención de cada gestión, privadas para el cliente y entre los técnicos y administradores.

Entre toda esta documentación requiero la elaboración de historias de usuarios con el formato "Como, quiero, para".

Quiero ambiguedeades del pedido, por ejemplo "se puede anular una solicitud?", "que pasa si se generan dos solicitudes para un mismo asunto?"

## Prompt para Lovable.dev

Crea una aplicación web interna responsive (desktop y móvil) para la gestión de solicitudes de soluciones low code / no code (RPA, BPM y Power Platform). El nombre del sistema es GestionaLCNC. La app centraliza el registro, asignación, seguimiento y auditoría de solicitudes que hoy se manejan por correo y documentos dispersos.
1. Roles y permisos (autenticación y autorización):
- Cliente: registra solicitudes, consulta y hace seguimiento de SUS propias solicitudes, y solo ve las notas públicas.
- Técnico: gestiona la bandeja de solicitudes asignadas, cambia estados, registra notas (públicas o internas) y consulta el historial.
- Administrador: ve todas las solicitudes, asigna/reasigna técnicos, administra usuarios/roles, Cecos, dependencias, tipos de solicitud y reglas de estados, y accede a dashboards y auditoría.
Incluye login con los tres roles y menús/opciones visibles según el rol (el cliente nunca debe ver notas internas ni solicitudes de otros).
2. Módulos y pantallas:
Consulta de gestiones (listado): tabla con filtros por estado, Ceco, dependencia, tipo de solicitud, técnico y solicitante. Con paginación y búsqueda. Columnas: fecha, solicitante, tipo, Ceco/dependencia, estado, técnico asignado, fecha de cambio de estado.
Crear gestión: formulario con los campos obligatorios: fecha, centro de costo (Ceco), dependencia, tipo de solicitud, objetivo de la solicitud, detalle de la solicitud, referencia de ingreso (oficio, memo, otro), nombre y correo del solicitante. Validación de campos obligatorios en el frontend con mensajes claros.
Detalle de gestión: muestra todos los datos, estado actual, técnico asignado, fecha de asignación, fecha de cambio de estado, y el historial cronológico de notas. Botones de acción según rol y estado (cambiar estado, asignar técnico, editar, agregar nota).
Notas cronológicas: sección de timeline con cada nota (autor, fecha, tipo público/interno). El técnico elige al crear la nota si es pública (visible para todos) o interna (visible solo para técnicos y administrador). Distinguir visualmente ambos tipos (color o ícono).
Cambio de estado: selector con los estados del flujo. Estados: Registrada, Asignada, En atención, En espera, Resuelta, Cerrada, Anulada. Controlar transiciones válidas y registrar usuario + fecha de cada cambio.
Asignación de técnico: pantalla/selector para asignar o reasignar un técnico a una gestión (rol administrador).
Gestión de usuarios: CRUD de usuarios con rol (cliente/técnico/administrador).
Gestión de Cecos y dependencias: CRUD de catálogos para clasificación organizacional.
Gestión de tipos de solicitud: CRUD del catálogo de tipos.
Dashboards: 
- Dashboard técnico: mis gestiones asignadas, por estado.
- Dashboard administrativo global: volumen de solicitudes, distribución por estado, por tipo, por Ceco, tiempos de atención.
- Usar tarjetas de métricas y gráficas simples (barras y líneas).
Auditoría: registro de cambios (bitácora) de estado, asignación y edición, con usuario y fecha (visible para administrador).
3. Reglas de negocio importantes:
- Toda acción relevante (crear, cambiar estado, asignar, editar, nota) queda registrada con usuario y fecha.
- Las notas internas son invisibles para el rol cliente.
- Aviso de posible solicitud duplicada al crear (por solicitante + tipo + ventana de tiempo): mostrar una advertencia, no bloquear.
- Manejo de concurrencia: si dos usuarios editan a la vez, mostrar conflicto y pedir recargar.
- El cliente solo puede editar su solicitud antes de que sea atendida (estado inicial).
4. Diseño y experiencia:
- Interfaz limpia, profesional, tipo herramienta interna corporativa.
- Barra lateral de navegación (sidebar) con las opciones según rol.
- Diseño responsive que funcione bien en escritorio y móvil.
- Uso de badges de color para los estados (ej. Registrada=gris, En atención=azul, Resuelta=verde, Cerrada=verde oscuro, Anulada=rojo, En espera=amarillo).
- Modales para confirmar acciones importantes (cambiar estado, asignar, anular).
- Estados vacíos, carga y mensajes de error claros en español.
5. Idioma y datos:
- Toda la interfaz en español.
- Puedes usar datos de ejemplo (seed) para los tres roles y varias solicitudes en distintos estados para poder navegar y probar.
Genera la aplicación completa con las pantallas, navegación, autenticación por roles y datos de ejemplo.

## Reducir alcance

- esto esta muy grande para empezar, reduci el alcance a un mvp que sea navegable rapido para poder hacerle pruebas e2e, toma supuestos temporales y no esperes a cerrar las ambiguedades

## Backend .NET

- crea la solucion en .net core 10 con tres proyectos, dominio, infraestructura y web, no me hagas clean architecture completa con capa de aplicacion
- configura sqlite con entity framework, hacé las migraciones y sembra datos de prueba (usuarios, cecos, dependencias, tipos de solicitud y un par de gestiones)
- haceme un login simple con cookies, sin identity completo, con 3 usuarios: admin, tecnico y cliente
- haceme el crud de gestiones: crear, listar con filtro por estado, detalle, notas publicas e internas, cambio de estado y asignacion de tecnico
- el cambio de estado tiene que ser lineal: registrada, asignada, en atencion, resuelta, cerrada

## Pruebas E2E

- haceme las pruebas e2e con playwright en javascript, igual que el ejemplo de la semana 5, no en .net
- hace que las pruebas borren la base antes de correr para que siempre partan limpias
- usa selectores por id en las pruebas asi no se rompen si cambio un texto

## Handoff

- haceme un documento handoff para que otro modelo pueda continuar el proyecto sin releer todo el historial, con las decisiones tecnicas, la estructura, como verificar que funciona y las reglas a respetar

## Entrega (semana 5)

- en este fichero tengo un proyecto en .netcore 10, puedes levantarlo y brindarle el puerto de localhost para abrirlo
- brindame el estatus
- requiero realizar commit y push al proyecto, ayudame a cargarlo, además requiero cargar la base de datos, exluirlo del gitgnore porque no lo está cargando, es un proyecto academico los datos son ficticios
- busca dentro de toda la documentación para armar un docmuento desitions.md, tambien requiero crear un prompts.md que es como una recopiación de prompts que se utilizaron para crear este proyecto, pero tiene que verse como muy natural asi como te escribo yo, no algo que se vea generado por un modelo de IA, otra cosa que requiero es un archivo logs.md que sirve para recopilar incidencias, errores, fallos, de modo que si el modelo vuelve a encontrar de nuevo una incidencia sepa como corregirlo, recopila, fecha, descripción, causa raiz y solucion

## Continuación del desarrollo

- ayudame a terminar el desarrollo segun el plan de desarrollo, el archivo handsoff dice indica como está el proyecto
- ocupo terminar todo el desarrollo
- los cecos son numeros por ejemplo 5550 desarrollo de software, debemos crear una tabla para darle mantenimiento a los cecos, por cambios a nivel organizacional
- tienes que darle mantenimiento al archivo hadnsoff al logs, decisions, y prompts



