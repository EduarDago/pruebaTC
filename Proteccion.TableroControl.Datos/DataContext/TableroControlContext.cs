using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Datos.DataContext
{
    public partial class TableroControlContext : DbContext
    {
        private readonly string _connection;

        public TableroControlContext()
        {

        }

        public TableroControlContext(string connection)
        {
            _connection = connection;
        }

        public TableroControlContext(DbContextOptions<TableroControlContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connection);
            }
        }

        public virtual DbSet<CampoOrigen> CampoOrigen { get; set; }
        public virtual DbSet<EjecucionValidacion> EjecucionValidacion { get; set; }
        public virtual DbSet<DetalleEjecucionValidacion> DetalleEjecucionValidacion { get; set; }
        public virtual DbSet<Estadistica> Estadistica { get; set; }
        public virtual DbSet<EjecucionOrigen> EjecucionOrigen { get; set; }
        public virtual DbSet<Inconsistencia> Inconsistencia { get; set; }
        public virtual DbSet<OrigenDato> OrigenDato { get; set; }
        public virtual DbSet<ParametroOrigen> ParametroOrigen { get; set; }
        public virtual DbSet<Parametro> Parametro { get; set; }
        public virtual DbSet<Topico> Topico { get; set; }
        public virtual DbSet<Validacion> Validacion { get; set; }
        public virtual DbSet<LogEjecucion> LogEjecucion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CampoOrigen>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreExcel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDato)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.OrigenDato)
                    .WithMany(p => p.Campos)
                    .HasForeignKey(d => d.IdOrigenDato)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CampoOrigen_OrigenDato");
            });

            modelBuilder.Entity<EjecucionOrigen>(entity =>
            {
                entity.HasKey(e => e.IdEjecucion);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEjecucion).HasColumnType("date");

                entity.HasOne(d => d.OrigenDato)
                    .WithMany(p => p.Ejecuciones)
                    .HasForeignKey(d => d.IdOrigenDato)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EjecucionOrigen_OrigenDato");
            });

            modelBuilder.Entity<EjecucionValidacion>(entity =>
            {
                entity.HasKey(e => e.IdEjecucion);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEjecucion).HasColumnType("date");

                entity.Property(e => e.Equipo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoValidacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetalleEjecucionValidacion>(entity =>
            {
                entity.HasKey(e => e.IdDetalle);

                entity.HasOne(d => d.EjecucionValidacion)
                    .WithMany(p => p.DetalleEjecuciones)
                    .HasForeignKey(d => d.IdEjecucion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__DetalleEj__IdEje__63D8CE75");
            });

            modelBuilder.Entity<Estadistica>(entity =>
            {
                entity.HasKey(e => e.IdEstadistica);

                entity.HasOne(d => d.Validacion)
                    .WithMany(p => p.Estadisticas)
                    .HasForeignKey(d => d.IdEstadistica)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Estadisti__IdVal__60FC61CA");
            });

            modelBuilder.Entity<Inconsistencia>(entity =>
            {
                entity.HasKey(e => e.IdInconsistencia);

                entity.Property(e => e.IdInconsistencia).ValueGeneratedNever();

                entity.Property(e => e.Detalle)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Validacion)
                    .WithMany(p => p.Inconsistencias)
                    .HasForeignKey(d => d.IdValidacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inconsist__IdVal__5CD6CB2B");
            });

            modelBuilder.Entity<OrigenDato>(entity =>
            {
                entity.HasKey(e => e.IdOrigenDato);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EsquemaProcedimiento)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.EsquemaTabla)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTabla)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Procedimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaArchivo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Separador)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ConcatenarFecha)
                .HasColumnType("bit");

                entity.Property(e => e.RutaOrigenSftp)
                    .IsRequired(false)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RutaDestinoSftp)
                    .IsRequired(false)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.TipoOrigen)
                    .WithMany(p => p.OrigenesDatos)
                    .HasForeignKey(d => d.IdTipoOrigen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrigenDato_Topico");
            });

            modelBuilder.Entity<ParametroOrigen>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDato)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.OrigenDato)
                    .WithMany(p => p.Parametros)
                    .HasForeignKey(d => d.IdOrigenDato)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ParametroOrigen_OrigenDato");
            });

            modelBuilder.Entity<Parametro>(entity =>
            {
                entity.HasKey(e => e.IdParametro);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Topico>(entity =>
            {
                entity.HasKey(e => e.IdTopico);

                entity.Property(e => e.Identificador)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TextoMostrar)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.HasMany(v => v.Validaciones)
                .WithOne();

                entity.HasMany(v => v.Validaciones2)
                .WithOne();

            });

            modelBuilder.Entity<Validacion>(entity =>
            {
                entity.HasKey(e => e.IdValidacion);

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Esquema)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sp)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.TipoValidacion)
                    .WithMany(p => p.Validaciones)
                    .HasForeignKey(d => d.IdTipoValidacion);

                entity.HasOne(d => d.Equipo)
                    .WithMany(p => p.Validaciones2)
                    .HasForeignKey(d => d.IdEquipo);

            });

            modelBuilder.Entity<LogEjecucion>(entity =>
            {
                entity.HasKey(e => e.IdLogEjecucion);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FechaEjecucion)
                    .IsRequired();

                entity.Property(e => e.Script)
                    .IsRequired();

                entity.Property(e => e.ResultadoScript)
                    .IsRequired();
            });

        }

        public bool TestModelCreating(ModelBuilder model)
        {
            try
            {
                OnModelCreating(model);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
