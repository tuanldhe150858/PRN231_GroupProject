using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN_Final_API.Models
{
    public partial class final_projectContext : DbContext
    {
        public final_projectContext()
        {
        }

        public final_projectContext(DbContextOptions<final_projectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<FileDetail> FileDetails { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("FinalProject"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("class");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("class_name");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__class__teacher_i__4CA06362");
            });

            modelBuilder.Entity<FileDetail>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__file_det__07D884C635AAFAA5");

                entity.ToTable("file_detail");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.FileName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("file_name");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("file_path");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.FileDetails)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__file_deta__subje__5629CD9C");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("student");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.Mail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role")
                    .HasDefaultValueSql("('student')");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__student__class_i__5070F446");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("subject");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.SubjectName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("subject_name");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__subject__class_i__534D60F1");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("teacher");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.Property(e => e.Mail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role")
                    .HasDefaultValueSql("('teacher')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
