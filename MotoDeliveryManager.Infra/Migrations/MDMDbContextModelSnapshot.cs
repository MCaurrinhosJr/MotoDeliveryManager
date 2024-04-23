﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MotoDeliveryManager.Infra.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotoDeliveryManager.Infra.Migrations
{
    [DbContext(typeof(MDMDbContext))]
    partial class MDMDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Entregador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FotoCNHUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumeroCNH")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TipoCNH")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.HasIndex("NumeroCNH")
                        .IsUnique();

                    b.ToTable("Entregador", (string)null);
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Locacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataTerminoPrevista")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DataTerminoReal")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EntregadorId")
                        .HasColumnType("integer");

                    b.Property<int>("MotoId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal?>("ValorTotal")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ValorTotalPrevisto")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("EntregadorId");

                    b.HasIndex("MotoId");

                    b.ToTable("Locacao", (string)null);
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Moto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Ano")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Placa")
                        .IsUnique();

                    b.ToTable("Moto", (string)null);
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Notificacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataEnvio")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EntregadorId")
                        .HasColumnType("integer");

                    b.Property<string>("Mensagem")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PedidoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EntregadorId");

                    b.HasIndex("PedidoId");

                    b.ToTable("Notificacao", (string)null);
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("EntregadorId")
                        .HasColumnType("integer");

                    b.Property<int>("StatusPedido")
                        .HasColumnType("integer");

                    b.Property<decimal>("ValorCorrida")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("EntregadorId");

                    b.ToTable("Pedido", (string)null);
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Locacao", b =>
                {
                    b.HasOne("MotoDeliveryManager.Domain.Models.Entregador", "Entregador")
                        .WithMany()
                        .HasForeignKey("EntregadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotoDeliveryManager.Domain.Models.Moto", "Moto")
                        .WithMany("Locacoes")
                        .HasForeignKey("MotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entregador");

                    b.Navigation("Moto");
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Notificacao", b =>
                {
                    b.HasOne("MotoDeliveryManager.Domain.Models.Entregador", "Entregador")
                        .WithMany()
                        .HasForeignKey("EntregadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotoDeliveryManager.Domain.Models.Pedido", "Pedido")
                        .WithMany()
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entregador");

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Pedido", b =>
                {
                    b.HasOne("MotoDeliveryManager.Domain.Models.Entregador", "Entregador")
                        .WithMany("Pedidos")
                        .HasForeignKey("EntregadorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Entregador");
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Entregador", b =>
                {
                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("MotoDeliveryManager.Domain.Models.Moto", b =>
                {
                    b.Navigation("Locacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
