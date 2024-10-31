﻿// <auto-generated />
using System;
using Hospital.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hospital.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Hospital")
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Hospital.Domain.Entities.Name", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Use")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("Names", "Hospital");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b83dc8ef-c2ed-43bd-81d5-3bc6d0049a26"),
                            Family = "family",
                            PatientId = new Guid("b0710e24-d505-401c-b8c3-6f161e13b6a7"),
                            Use = "use"
                        });
                });

            modelBuilder.Entity("Hospital.Domain.Entities.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Patients", "Hospital");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b0710e24-d505-401c-b8c3-6f161e13b6a7"),
                            Active = true,
                            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = "Male"
                        });
                });

            modelBuilder.Entity("Hospital.Domain.Values.GivenName", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NameId");

                    b.ToTable("GivenNames", "Hospital");

                    b.HasData(
                        new
                        {
                            Id = new Guid("42169402-5157-4947-81e5-8bcdb8510a46"),
                            NameId = new Guid("b83dc8ef-c2ed-43bd-81d5-3bc6d0049a26"),
                            Value = "given1"
                        },
                        new
                        {
                            Id = new Guid("c9f4bcfb-b1e5-49d4-8fbe-19b691935d40"),
                            NameId = new Guid("b83dc8ef-c2ed-43bd-81d5-3bc6d0049a26"),
                            Value = "given2"
                        });
                });

            modelBuilder.Entity("Hospital.Domain.Entities.Name", b =>
                {
                    b.HasOne("Hospital.Domain.Entities.Patient", null)
                        .WithOne("Name")
                        .HasForeignKey("Hospital.Domain.Entities.Name", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hospital.Domain.Values.GivenName", b =>
                {
                    b.HasOne("Hospital.Domain.Entities.Name", null)
                        .WithMany("Given")
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hospital.Domain.Entities.Name", b =>
                {
                    b.Navigation("Given");
                });

            modelBuilder.Entity("Hospital.Domain.Entities.Patient", b =>
                {
                    b.Navigation("Name")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
