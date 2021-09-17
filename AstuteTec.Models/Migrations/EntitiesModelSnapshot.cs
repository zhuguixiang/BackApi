﻿// <auto-generated />
using System;
using AstuteTec.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

namespace AstuteTec.Models.Migrations
{
    [DbContext(typeof(Entities))]
    partial class EntitiesModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("NJQHWATER")
                .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.Entity("AstuteTec.Models.Dictionary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Dict");
                });

            modelBuilder.Entity("AstuteTec.Models.DictionaryItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("DictionaryId");

                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<int>("NumericalOrder");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.ToTable("DictItem");
                });

            modelBuilder.Entity("AstuteTec.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Cellphone")
                        .HasMaxLength(11);

                    b.Property<DateTime>("CreateTime");

                    b.Property<Guid>("CreateUserId");

                    b.Property<string>("Email")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("Removed");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("AstuteTec.Models.DictionaryItem", b =>
                {
                    b.HasOne("AstuteTec.Models.Dictionary", "Dictionary")
                        .WithMany("DictionaryItem")
                        .HasForeignKey("DictionaryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}