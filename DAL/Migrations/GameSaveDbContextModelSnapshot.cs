﻿// <auto-generated />
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(GameSaveDbContext))]
    partial class GameSaveDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("Domain.GameSave", b =>
                {
                    b.Property<int>("GameSaveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardHeight")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BoardState")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("BoardWidth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfMines")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SaveName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GameSaveId");

                    b.ToTable("GameSave");
                });
#pragma warning restore 612, 618
        }
    }
}
