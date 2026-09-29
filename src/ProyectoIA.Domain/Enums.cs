namespace ProyectoIA.Domain;

public enum Rol
{
    Cliente = 1,
    Tecnico = 2,
    Administrador = 3
}

/// <summary>
/// Transición lineal simple para el MVP: Registrada -> Asignada -> EnAtencion -> Resuelta -> Cerrada.
/// </summary>
public enum EstadoGestion
{
    Registrada = 1,
    Asignada = 2,
    EnAtencion = 3,
    Resuelta = 4,
    Cerrada = 5
}

public enum PrioridadGestion
{
    Baja = 1,
    Media = 2,
    Alta = 3
}
