using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClientAuthorization.Models.Database
{
    public partial class geopagos_dbContext : DbContext
    {
        public geopagos_dbContext()
        {
        }

        public geopagos_dbContext(DbContextOptions<geopagos_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OperationAction> OperationActions { get; set; } = null!;
        public virtual DbSet<OperationLog> OperationLogs { get; set; } = null!;
        public virtual DbSet<OperationPending> OperationPendings { get; set; } = null!;
        public virtual DbSet<OperationStatus> OperationStatuses { get; set; } = null!;

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost:5432;Database=geopagos_db;Username=postgres;Password=postgres");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationAction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("operation_action", "gp_schema");

                entity.Property(e => e.Action)
                    .HasColumnType("character varying")
                    .HasColumnName("action");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("description");
            });

            modelBuilder.Entity<OperationLog>(entity =>
            {
                entity.HasKey(e => e.OperationId)
                    .HasName("operation_log_pk");

                entity.ToTable("operation_log", "gp_schema");

                entity.Property(e => e.OperationId)
                    .HasColumnName("operation_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CardCvc)
                    .HasColumnType("character varying")
                    .HasColumnName("card_cvc");

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnType("character varying")
                    .HasColumnName("card_expiration_date");

                entity.Property(e => e.CardHolderName)
                    .HasColumnType("character varying")
                    .HasColumnName("card_holder_name");

                entity.Property(e => e.CardNumber)
                    .HasColumnType("character varying")
                    .HasColumnName("card_number");

                entity.Property(e => e.CardType)
                    .HasColumnType("character varying")
                    .HasColumnName("card_type");

                entity.Property(e => e.ClientId)
                    .HasColumnType("character varying")
                    .HasColumnName("client_id");

                entity.Property(e => e.ClientType)
                    .HasColumnType("character varying")
                    .HasColumnName("client_type");

                entity.Property(e => e.LastModification).HasColumnName("last_modification");

                entity.Property(e => e.OperationAction)
                    .HasColumnType("character varying")
                    .HasColumnName("operation_action");

                entity.Property(e => e.OperationStatus)
                    .HasColumnType("character varying")
                    .HasColumnName("operation_status");

                entity.Property(e => e.TransactionAmount).HasColumnName("transaction_amount");

                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");

                entity.HasOne(d => d.OperationStatusNavigation)
                    .WithMany(p => p.OperationLogs)
                    .HasForeignKey(d => d.OperationStatus)
                    .HasConstraintName("operation_log_operation_status_fk");
            });

            modelBuilder.Entity<OperationPending>(entity =>
            {
                entity.ToTable("operation_pending", "gp_schema");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");

                entity.Property(e => e.OperationId).HasColumnName("operation_id");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.OperationPendings)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("operation_pending_operation_log_fk");
            });

            modelBuilder.Entity<OperationStatus>(entity =>
            {
                entity.HasKey(e => e.Status)
                    .HasName("operation_status_pk");

                entity.ToTable("operation_status", "gp_schema");

                entity.Property(e => e.Status)
                    .HasColumnType("character varying")
                    .HasColumnName("status");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("description");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
