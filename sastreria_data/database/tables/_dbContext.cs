using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

public partial class _dbContext : DbContext
{
    public _dbContext()
    {
    }

    public _dbContext(DbContextOptions<_dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<CitaImagen> CitaImagen { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Dato> Datos { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<ModeloImagen> ModeloImagen { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Sastre> Sastres { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=ep-super-hill-adq2grkc-pooler.c-2.us-east-1.aws.neon.tech; Database=DBSASTRERIA; Username=neondb_owner; Password=npg_qNd2vS6fDKFn; SSL Mode=VerifyFull; Channel Binding=Require;",
            b => b.MigrationsAssembly("WebSastreria"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A1032B13906");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Cita__394B02026E23F138");

            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cita).HasConstraintName("FK__Cita__IdCliente__5441852A");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__D5946642D6F2BE01");

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Clientes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cliente__IdTipoD__45F365D3");
        });

        modelBuilder.Entity<Dato>(entity =>
        {
            entity.HasKey(e => e.IdDatos).HasName("PK__Datos__A4BC7BC551DEEEC5");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__FBB0EDC135F48D85");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horario__1539229B8DE62D0D");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.IdModelo).HasName("PK__Modelo__CC30D30CD4075261");

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(NOW())");


        });

        modelBuilder.Entity<ModeloImagen>(entity =>
        {
            entity.HasKey(e => e.IdModeloImagen).HasName("PK__ModeloIm__75E9782729A4E057");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__9D335DC383EAADC3");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedidos).HasConstraintName("FK__Pedido__IdClient__49C3F6B7");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Pedidos).HasConstraintName("FK__Pedido__IdEstado__4AB81AF0");

            entity.HasOne(d => d.IdModeloNavigation).WithMany(p => p.Pedidos).HasConstraintName("FK__Pedido__IdModelo__4BAC3F29");

            entity.HasOne(d => d.IdSastreNavigation).WithMany(p => p.Pedidos).HasConstraintName("FK__Pedido__IdSastre__48CFD27E");
        });

        modelBuilder.Entity<Sastre>(entity =>
        {
            entity.HasKey(e => e.IdSastre).HasName("PK__Sastre__06563E870BA6D5DE");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento).HasName("PK__TipoDocu__3AB3332FE8CECEF4");
        });

        OnModelCreatingPartial(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Modelo>()
        .HasMany(m => m.Categorias)
        .WithMany(c => c.Modelos)
        .UsingEntity(j => j.ToTable("ModeloCategoria"));

        modelBuilder.Entity<Citum>()
        .HasOne(c => c.Pedido)
        .WithMany() // o .WithMany(p => p.Citas) si en Pedido agregas ICollection<Cita>
        .HasForeignKey(c => c.PedidoId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
