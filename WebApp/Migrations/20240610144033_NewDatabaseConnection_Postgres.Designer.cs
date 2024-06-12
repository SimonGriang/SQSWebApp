﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApp.Data;

#nullable disable

namespace WebApp.Migrations
{
    [DbContext(typeof(WebAppContext))]
    [Migration("20240610144033_NewDatabaseConnection_Postgres")]
    partial class NewDatabaseConnection_Postgres
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApp.Models.Language", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Abbreviation")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isOriginLanguage")
                        .HasColumnType("boolean");

                    b.Property<bool>("isTargetLanguage")
                        .HasColumnType("boolean");

                    b.HasKey("ID");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("WebApp.Models.Translation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int?>("OriginalLanguageID")
                        .HasColumnType("integer");

                    b.Property<string>("OriginalText")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("TranslatedLanguageID")
                        .HasColumnType("integer");

                    b.Property<string>("TranslatedText")
                        .HasColumnType("text");

                    b.Property<DateTime>("translated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ID");

                    b.HasIndex("OriginalLanguageID");

                    b.HasIndex("TranslatedLanguageID");

                    b.ToTable("Translation");
                });

            modelBuilder.Entity("WebApp.Models.Translation", b =>
                {
                    b.HasOne("WebApp.Models.Language", "OriginalLanguage")
                        .WithMany()
                        .HasForeignKey("OriginalLanguageID");

                    b.HasOne("WebApp.Models.Language", "TranslatedLanguage")
                        .WithMany()
                        .HasForeignKey("TranslatedLanguageID");

                    b.Navigation("OriginalLanguage");

                    b.Navigation("TranslatedLanguage");
                });
#pragma warning restore 612, 618
        }
    }
}
