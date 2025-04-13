using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HouseOfMysteries.Models;

public partial class HouseofmysteriesContext : DbContext
{
    public HouseofmysteriesContext()
    {
    }

    public HouseofmysteriesContext(DbContextOptions<HouseofmysteriesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySQL("server=localhost;database=houseofmysteries;user=root;password=;sslmode=none;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PRIMARY");

            entity.ToTable("booking");

            entity.HasIndex(e => new { e.RoomId, e.TeamId }, "roomId");

            entity.HasIndex(e => e.TeamId, "teamId");

            entity.Property(e => e.BookingId)
                .HasColumnType("int(11)")
                .HasColumnName("bookingId");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("bookingDate");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("comment");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("isAvailable");
            entity.Property(e => e.Result)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("time")
                .HasColumnName("result");
            entity.Property(e => e.RoomId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("roomId");
            entity.Property(e => e.TeamId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("teamId");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("booking_ibfk_1");

            entity.HasOne(d => d.Team).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("booking_ibfk_2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PRIMARY");

            entity.ToTable("rooms");

            entity.Property(e => e.RoomId)
                .HasColumnType("int(11)")
                .HasColumnName("roomId");
            entity.Property(e => e.RoomName)
                .HasMaxLength(100)
                .HasColumnName("roomName");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PRIMARY");

            entity.ToTable("teams");

            entity.Property(e => e.TeamId)
                .HasColumnType("int(11)")
                .HasColumnName("teamId");
            entity.Property(e => e.TeamName)
                .HasMaxLength(100)
                .HasColumnName("teamName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "roleID");

            entity.HasIndex(e => new { e.TeamId, e.RoleId }, "teamID");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Hash)
                .HasMaxLength(64)
                .HasColumnName("HASH");
            entity.Property(e => e.NickName)
                .HasMaxLength(255)
                .HasColumnName("nickName");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.RealName)
                .HasMaxLength(255)
                .HasColumnName("realName");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("roleId");
            entity.Property(e => e.Salt)
                .HasMaxLength(64)
                .HasColumnName("SALT");
            entity.Property(e => e.TeamId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("teamId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_ibfk_1");

            entity.HasOne(d => d.Team).WithMany(p => p.Users)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_ibfk_2");
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
