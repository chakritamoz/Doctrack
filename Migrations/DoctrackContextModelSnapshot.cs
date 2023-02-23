﻿// <auto-generated />
using System;
using Doctrack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Doctrack.Migrations
{
    [DbContext(typeof(DoctrackContext))]
    partial class DoctrackContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Doctrack.Models.Document", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CommandOrder")
                        .HasColumnType("TEXT");

                    b.Property<int>("DocType_Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Doc_Title")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Operation")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("OperationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReceiptDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RemarkAll")
                        .HasColumnType("TEXT");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DocType_Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Doctrack.Models.DocumentDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Doc_Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Emp_Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Doc_Id");

                    b.HasIndex("Emp_Id");

                    b.ToTable("DocumentDetails");
                });

            modelBuilder.Entity("Doctrack.Models.DocumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Period")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("Doctrack.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Job_Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rank_Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Job_Id");

                    b.HasIndex("Rank_Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Doctrack.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("Doctrack.Models.Rank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Ranks");
                });

            modelBuilder.Entity("Doctrack.Models.Document", b =>
                {
                    b.HasOne("Doctrack.Models.DocumentType", "DocumentType")
                        .WithMany("Documents")
                        .HasForeignKey("DocType_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentType");
                });

            modelBuilder.Entity("Doctrack.Models.DocumentDetail", b =>
                {
                    b.HasOne("Doctrack.Models.Document", "Document")
                        .WithMany("DocumentDetails")
                        .HasForeignKey("Doc_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Doctrack.Models.Employee", "Employee")
                        .WithMany("DocumentDetails")
                        .HasForeignKey("Emp_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Doctrack.Models.Employee", b =>
                {
                    b.HasOne("Doctrack.Models.Job", "Job")
                        .WithMany("Employees")
                        .HasForeignKey("Job_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Doctrack.Models.Rank", "Rank")
                        .WithMany("Employees")
                        .HasForeignKey("Rank_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("Rank");
                });

            modelBuilder.Entity("Doctrack.Models.Document", b =>
                {
                    b.Navigation("DocumentDetails");
                });

            modelBuilder.Entity("Doctrack.Models.DocumentType", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("Doctrack.Models.Employee", b =>
                {
                    b.Navigation("DocumentDetails");
                });

            modelBuilder.Entity("Doctrack.Models.Job", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Doctrack.Models.Rank", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
