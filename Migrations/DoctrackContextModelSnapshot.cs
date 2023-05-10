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

            modelBuilder.Entity("Doctrack.Models.Account", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEmailConfirm")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("KeyToken")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Role_Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Username");

                    b.HasIndex("Role_Id");

                    b.ToTable("Accounts");
                });

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

                    b.Property<int>("Job_Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rank_Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Doc_Id");

                    b.HasIndex("Emp_Id");

                    b.HasIndex("Job_Id");

                    b.HasIndex("Rank_Id");

                    b.ToTable("DocumentDetails");
                });

            modelBuilder.Entity("Doctrack.Models.DocumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PeriodEnd")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PeriodWarning")
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

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

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

            modelBuilder.Entity("Doctrack.Models.JobRankDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Job_Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rank_Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Job_Id");

                    b.HasIndex("Rank_Id");

                    b.ToTable("JobRankDetails");
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

            modelBuilder.Entity("Doctrack.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Doctrack.Models.Account", b =>
                {
                    b.HasOne("Doctrack.Models.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
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

                    b.HasOne("Doctrack.Models.Job", "Job")
                        .WithMany("DocumentDetails")
                        .HasForeignKey("Job_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Doctrack.Models.Rank", "Rank")
                        .WithMany("DocumentDetails")
                        .HasForeignKey("Rank_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("Employee");

                    b.Navigation("Job");

                    b.Navigation("Rank");
                });

            modelBuilder.Entity("Doctrack.Models.JobRankDetail", b =>
                {
                    b.HasOne("Doctrack.Models.Job", "Job")
                        .WithMany("JobRankDetails")
                        .HasForeignKey("Job_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Doctrack.Models.Rank", "Rank")
                        .WithMany("JobRankDetails")
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
                    b.Navigation("DocumentDetails");

                    b.Navigation("JobRankDetails");
                });

            modelBuilder.Entity("Doctrack.Models.Rank", b =>
                {
                    b.Navigation("DocumentDetails");

                    b.Navigation("JobRankDetails");
                });

            modelBuilder.Entity("Doctrack.Models.Role", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
