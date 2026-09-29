using Microsoft.EntityFrameworkCore;
using ProyectoIA.Domain;

namespace ProyectoIA.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Ceco> Cecos => Set<Ceco>();
    public DbSet<Dependencia> Dependencias => Set<Dependencia>();
    public DbSet<TipoSolicitud> TiposSolicitud => Set<TipoSolicitud>();
    public DbSet<Gestion> Gestiones => Set<Gestion>();
    public DbSet<NotaGestion> Notas => Set<NotaGestion>();
    public DbSet<BitacoraCambio> Bitacora => Set<BitacoraCambio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Correo)
            .IsUnique();

        modelBuilder.Entity<Gestion>()
            .HasOne(g => g.Solicitante)
            .WithMany(u => u.GestionesSolicitadas)
            .HasForeignKey(g => g.SolicitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Gestion>()
            .HasOne(g => g.TecnicoAsignado)
            .WithMany(u => u.GestionesAsignadas)
            .HasForeignKey(g => g.TecnicoAsignadoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Gestion>()
            .HasOne(g => g.Ceco)
            .WithMany()
            .HasForeignKey(g => g.CecoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Gestion>()
            .HasOne(g => g.Dependencia)
            .WithMany()
            .HasForeignKey(g => g.DependenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Gestion>()
            .HasOne(g => g.TipoSolicitud)
            .WithMany()
            .HasForeignKey(g => g.TipoSolicitudId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NotaGestion>()
            .HasOne(n => n.Gestion)
            .WithMany(g => g.Notas)
            .HasForeignKey(n => n.GestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BitacoraCambio>()
            .HasOne(b => b.Gestion)
            .WithMany(g => g.Bitacora)
            .HasForeignKey(b => b.GestionId)
            .OnDelete(DeleteBehavior.Cascade);

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Correo = "admin@proyectoia.com",
                PasswordHash = PasswordHasher.Hash("Admin123!"),
                Rol = Rol.Administrador
            },
            new Usuario
            {
                Id = 2,
                Nombre = "Técnico",
                Correo = "tecnico@proyectoia.com",
                PasswordHash = PasswordHasher.Hash("Tecnico123!"),
                Rol = Rol.Tecnico
            },
            new Usuario
            {
                Id = 3,
                Nombre = "Cliente",
                Correo = "cliente@proyectoia.com",
                PasswordHash = PasswordHasher.Hash("Cliente123!"),
                Rol = Rol.Cliente
            });

        modelBuilder.Entity<Ceco>().HasData(
            new Ceco { Id = 1, Codigo = "CECO-001", Nombre = "Centro de Costo Principal" },
            new Ceco { Id = 2, Codigo = "CECO-002", Nombre = "Centro de Costo Secundario" });

        modelBuilder.Entity<Dependencia>().HasData(
            new Dependencia { Id = 1, Nombre = "Operaciones" },
            new Dependencia { Id = 2, Nombre = "Finanzas" });

        modelBuilder.Entity<TipoSolicitud>().HasData(
            new TipoSolicitud { Id = 1, Nombre = "RPA" },
            new TipoSolicitud { Id = 2, Nombre = "BPM" },
            new TipoSolicitud { Id = 3, Nombre = "Power Platform" });

        modelBuilder.Entity<Gestion>().HasData(
            new Gestion
            {
                Id = 1,
                Fecha = new DateTime(2026, 9, 1, 9, 0, 0),
                CecoId = 1,
                DependenciaId = 1,
                TipoSolicitudId = 1,
                Objetivo = "Automatizar conciliación bancaria",
                Detalle = "Automatizar el proceso mensual de conciliación para reducir el trabajo manual.",
                ReferenciaIngreso = "MEMO-001",
                Estado = EstadoGestion.Registrada,
                FechaCambioEstado = new DateTime(2026, 9, 1, 9, 0, 0),
                SolicitanteId = 3
            },
            new Gestion
            {
                Id = 2,
                Fecha = new DateTime(2026, 9, 2, 10, 30, 0),
                CecoId = 2,
                DependenciaId = 2,
                TipoSolicitudId = 2,
                Objetivo = "Flujo de aprobación de compras",
                Detalle = "Digitalizar el flujo de aprobación de órdenes de compra.",
                ReferenciaIngreso = "MEMO-002",
                Estado = EstadoGestion.Asignada,
                FechaCambioEstado = new DateTime(2026, 9, 2, 10, 30, 0),
                SolicitanteId = 3,
                TecnicoAsignadoId = 2,
                FechaAsignacion = new DateTime(2026, 9, 2, 11, 0, 0)
            });
    }
}
