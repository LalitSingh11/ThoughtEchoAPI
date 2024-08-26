using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ThoughtEcho.Entities;

public partial class ThoughtEchoDbContext : DbContext
{
    public ThoughtEchoDbContext()
    {
    }

    public ThoughtEchoDbContext(DbContextOptions<ThoughtEchoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EmailVerificationOtp> EmailVerificationOtps { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<UserCred> UserCreds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ThoughtEchoDB;Encrypt=False;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailVerificationOtp>(entity =>
        {
            entity.HasKey(e => e.UserEmail).HasName("PK__email_ve__B0FBA213B7B1EA4C");

            entity.ToTable("email_verification_otp");

            entity.Property(e => e.UserEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user_email");
            entity.Property(e => e.Expires)
                .HasColumnType("datetime")
                .HasColumnName("expires");
            entity.Property(e => e.Otp).HasColumnName("otp");

            entity.HasOne(d => d.UserEmailNavigation).WithOne(p => p.EmailVerificationOtp)
                .HasPrincipalKey<UserCred>(p => p.Email)
                .HasForeignKey<EmailVerificationOtp>(d => d.UserEmail)
                .HasConstraintName("FK__email_ver__user___398D8EEE");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__refresh___B9BE370FA0082E99");

            entity.ToTable("refresh_token");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Expires)
                .HasColumnType("datetime")
                .HasColumnName("expires");
            entity.Property(e => e.Token)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.HasOne(d => d.User).WithOne(p => p.RefreshToken)
                .HasForeignKey<RefreshToken>(d => d.UserId)
                .HasConstraintName("FK__refresh_t__user___33D4B598");
        });

        modelBuilder.Entity<UserCred>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_cre__3213E83FD5397952");

            entity.ToTable("user_cred");

            entity.HasIndex(e => e.Email, "UQ__user_cre__AB6E61649D150C49").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
