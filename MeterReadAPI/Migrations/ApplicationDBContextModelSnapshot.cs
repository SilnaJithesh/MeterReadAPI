// <auto-generated />
using System;
using MeterReadAPI.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeterReadAPI.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("MeterReadAPI.Models.CustomerAccount", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fname")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Lname")
                        .HasColumnType("TEXT");

                    b.HasKey("AccountId");

                    b.ToTable("CustomerAccount");
                });

            modelBuilder.Entity("MeterReadAPI.Models.MeterReadings", b =>
                {
                    b.Property<int>("AccountId")                  
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ReadingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MeterReadings");
                });
#pragma warning restore 612, 618
        }
    }
}
