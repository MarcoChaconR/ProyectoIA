```
┌──────────────────────────────────────────────────────────────────────────────┐
│ GestionaLCNC        [Administrador]  Admin       [Cerrar sesión]           │
├──────────────────────────────────────────────────────────────────────────────┤
│ [Dashboard] [Gestiones] [Usuarios] [Cecos] [Catálogos] [Auditoría]          │
├──────────────────────────────────────────────────────────────────────────────┤
│ Reglas de Transición de Estados                            [ Guardar reglas ]│
│                                                                              │
│ Filas = estado origen · Columnas = estado destino ( • = permitido )          │
│ ┌──────────────┬─────────┬─────────┬─────────┬─────────┬────────┬─────────┐  │
│ │ De \ A       │ Regist. │ Asign.  │ En aten.│ En esp. │ Resuel.│ Cerrada │  │
│ ├──────────────┼─────────┼─────────┼─────────┼─────────┼────────┼─────────┤  │
│ │ Registrada   │         │    •    │         │         │        │         │  │
│ │ Asignada     │         │         │    •    │    •    │        │         │  │
│ │ En atención  │         │         │         │    •    │   •    │         │  │
│ │ En espera    │         │         │    •    │         │        │         │  │
│ │ Resuelta     │         │         │         │         │        │    •    │  │
│ └──────────────┴─────────┴─────────┴─────────┴─────────┴────────┴─────────┘  │
│                                                                              │
│  Roles habilitados para cada transición: [▾ por transición]                  │
└──────────────────────────────────────────────────────────────────────────────┘
```
