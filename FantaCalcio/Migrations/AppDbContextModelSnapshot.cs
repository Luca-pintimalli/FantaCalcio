﻿// <auto-generated />
using System;
using FantaCalcio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FantaCalcio.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FantaCalcio.Models.Asta", b =>
                {
                    b.Property<int>("ID_Asta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Asta"));

                    b.Property<int>("CreditiDisponibili")
                        .HasColumnType("int");

                    b.Property<int>("ID_Modalita")
                        .HasColumnType("int");

                    b.Property<int>("ID_TipoAsta")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<int>("ID_Utente")
                        .HasColumnType("int");

                    b.Property<int>("NumeroSquadre")
                        .HasColumnType("int");

                    b.HasKey("ID_Asta");

                    b.HasIndex("ID_Modalita");

                    b.HasIndex("ID_TipoAsta");

                    b.HasIndex("ID_Utente");

                    b.ToTable("Aste");
                });

            modelBuilder.Entity("FantaCalcio.Models.Giocatore", b =>
                {
                    b.Property<int>("ID_Giocatore")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Giocatore"));

                    b.Property<int>("Assist")
                        .HasColumnType("int");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Foto")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("GoalFatti")
                        .HasColumnType("int");

                    b.Property<int>("GoalSubiti")
                        .HasColumnType("int");

                    b.Property<int?>("ID_Squadra")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PartiteGiocate")
                        .HasColumnType("int");

                    b.Property<string>("RuoloClassic")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SquadraAttuale")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID_Giocatore");

                    b.HasIndex("ID_Squadra");

                    b.ToTable("Giocatori");
                });

            modelBuilder.Entity("FantaCalcio.Models.Modalita", b =>
                {
                    b.Property<int>("ID_Modalita")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Modalita"));

                    b.Property<string>("TipoModalita")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID_Modalita");

                    b.ToTable("Modalita");
                });

            modelBuilder.Entity("FantaCalcio.Models.Operazione", b =>
                {
                    b.Property<int>("ID_Operazione")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Operazione"));

                    b.Property<int>("CreditiSpesi")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataOperazione")
                        .HasColumnType("datetime2");

                    b.Property<int>("ID_Giocatore")
                        .HasColumnType("int");

                    b.Property<int>("ID_Squadra")
                        .HasColumnType("int");

                    b.Property<string>("StatoOperazione")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Operazione");

                    b.HasIndex("ID_Giocatore");

                    b.HasIndex("ID_Squadra");

                    b.ToTable("Operazioni");
                });

            modelBuilder.Entity("FantaCalcio.Models.Ruolo", b =>
                {
                    b.Property<int>("ID_Ruolo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Ruolo"));

                    b.Property<int>("ID_Modalita")
                        .HasColumnType("int");

                    b.Property<string>("NomeRuolo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID_Ruolo");

                    b.HasIndex("ID_Modalita");

                    b.ToTable("Ruoli");
                });

            modelBuilder.Entity("FantaCalcio.Models.RuoloMantra", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ID_Giocatore")
                        .HasColumnType("int");

                    b.Property<int>("ID_Ruolo")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ID_Giocatore");

                    b.HasIndex("ID_Ruolo");

                    b.ToTable("RuoloMantra");
                });

            modelBuilder.Entity("FantaCalcio.Models.Squadra", b =>
                {
                    b.Property<int>("ID_Squadra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Squadra"));

                    b.Property<int>("CreditiSpesi")
                        .HasColumnType("int");

                    b.Property<int>("CreditiTotali")
                        .HasColumnType("int");

                    b.Property<int>("ID_Asta")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Stemma")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID_Squadra");

                    b.HasIndex("ID_Asta");

                    b.ToTable("Squadre");
                });

            modelBuilder.Entity("FantaCalcio.Models.TipoAsta", b =>
                {
                    b.Property<int>("ID_TipoAsta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_TipoAsta"));

                    b.Property<string>("NomeTipoAsta")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID_TipoAsta");

                    b.ToTable("TipiAsta");
                });

            modelBuilder.Entity("FantaCalcio.Models.Utente", b =>
                {
                    b.Property<int>("ID_Utente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Utente"));

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("DataRegistrazione")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Foto")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID_Utente");

                    b.ToTable("Utenti");
                });

            modelBuilder.Entity("FantaCalcio.Models.Asta", b =>
                {
                    b.HasOne("FantaCalcio.Models.Modalita", "Modalita")
                        .WithMany("Aste")
                        .HasForeignKey("ID_Modalita")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FantaCalcio.Models.TipoAsta", "TipoAsta")
                        .WithMany("Aste")
                        .HasForeignKey("ID_TipoAsta")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FantaCalcio.Models.Utente", "Utente")
                        .WithMany("Aste")
                        .HasForeignKey("ID_Utente")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Modalita");

                    b.Navigation("TipoAsta");

                    b.Navigation("Utente");
                });

            modelBuilder.Entity("FantaCalcio.Models.Giocatore", b =>
                {
                    b.HasOne("FantaCalcio.Models.Squadra", "Squadra")
                        .WithMany("Giocatori")
                        .HasForeignKey("ID_Squadra");

                    b.Navigation("Squadra");
                });

            modelBuilder.Entity("FantaCalcio.Models.Operazione", b =>
                {
                    b.HasOne("FantaCalcio.Models.Giocatore", "Giocatore")
                        .WithMany("Operazioni")
                        .HasForeignKey("ID_Giocatore")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FantaCalcio.Models.Squadra", "Squadra")
                        .WithMany("Operazioni")
                        .HasForeignKey("ID_Squadra")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Giocatore");

                    b.Navigation("Squadra");
                });

            modelBuilder.Entity("FantaCalcio.Models.Ruolo", b =>
                {
                    b.HasOne("FantaCalcio.Models.Modalita", "Modalita")
                        .WithMany("Ruoli")
                        .HasForeignKey("ID_Modalita")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Modalita");
                });

            modelBuilder.Entity("FantaCalcio.Models.RuoloMantra", b =>
                {
                    b.HasOne("FantaCalcio.Models.Giocatore", "Giocatore")
                        .WithMany("RuoliMantra")
                        .HasForeignKey("ID_Giocatore")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FantaCalcio.Models.Ruolo", "Ruolo")
                        .WithMany("RuoliMantra")
                        .HasForeignKey("ID_Ruolo")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Giocatore");

                    b.Navigation("Ruolo");
                });

            modelBuilder.Entity("FantaCalcio.Models.Squadra", b =>
                {
                    b.HasOne("FantaCalcio.Models.Asta", "Asta")
                        .WithMany("Squadre")
                        .HasForeignKey("ID_Asta")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Asta");
                });

            modelBuilder.Entity("FantaCalcio.Models.Asta", b =>
                {
                    b.Navigation("Squadre");
                });

            modelBuilder.Entity("FantaCalcio.Models.Giocatore", b =>
                {
                    b.Navigation("Operazioni");

                    b.Navigation("RuoliMantra");
                });

            modelBuilder.Entity("FantaCalcio.Models.Modalita", b =>
                {
                    b.Navigation("Aste");

                    b.Navigation("Ruoli");
                });

            modelBuilder.Entity("FantaCalcio.Models.Ruolo", b =>
                {
                    b.Navigation("RuoliMantra");
                });

            modelBuilder.Entity("FantaCalcio.Models.Squadra", b =>
                {
                    b.Navigation("Giocatori");

                    b.Navigation("Operazioni");
                });

            modelBuilder.Entity("FantaCalcio.Models.TipoAsta", b =>
                {
                    b.Navigation("Aste");
                });

            modelBuilder.Entity("FantaCalcio.Models.Utente", b =>
                {
                    b.Navigation("Aste");
                });
#pragma warning restore 612, 618
        }
    }
}
