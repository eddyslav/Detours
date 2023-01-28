﻿// <auto-generated />
using System;
using Detours.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Detours.Data.Migrations
{
	[DbContext(typeof(DetoursDbContext))]
	partial class DetoursDbContextModelSnapshot : ModelSnapshot
	{
		protected override void BuildModel(ModelBuilder modelBuilder)
		{
#pragma warning disable 612, 618
			modelBuilder
				.HasAnnotation("ProductVersion", "7.0.1")
				.HasAnnotation("Relational:MaxIdentifierLength", 128);

			SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

			modelBuilder.Entity("Detours.Data.Entities.Booking", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<DateTimeOffset>("CreatedAt")
						.HasColumnType("datetimeoffset");

					b.Property<decimal>("Price")
						.HasColumnType("money");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.Property<Guid>("UserId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("TourId");

					b.HasIndex("UserId");

					b.ToTable("Bookings");
				});

			modelBuilder.Entity("Detours.Data.Entities.RefreshToken", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<DateTimeOffset>("CreatedAt")
						.HasColumnType("datetimeoffset");

					b.Property<DateTimeOffset>("ExpiresAt")
						.HasColumnType("datetimeoffset");

					b.Property<bool>("Invalidated")
						.HasColumnType("bit");

					b.Property<Guid>("JwtId")
						.HasColumnType("uniqueidentifier");

					b.Property<bool>("Used")
						.HasColumnType("bit");

					b.Property<Guid>("UserId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("UserId");

					b.ToTable("RefreshTokens");
				});

			modelBuilder.Entity("Detours.Data.Entities.Review", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<DateTimeOffset>("CreatedAt")
						.HasColumnType("datetimeoffset");

					b.Property<string>("Description")
						.IsRequired()
						.HasMaxLength(255)
						.HasColumnType("nvarchar(255)");

					b.Property<byte>("Rating")
						.HasColumnType("tinyint");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.Property<Guid>("UserId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("TourId");

					b.HasIndex("UserId");

					b.ToTable("Reviews");
				});

			modelBuilder.Entity("Detours.Data.Entities.Tour", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<string>("Description")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<int>("Difficulty")
						.HasColumnType("int");

					b.Property<int>("Duration")
						.HasColumnType("int");

					b.Property<int>("MaxGroupSize")
						.HasColumnType("int");

					b.Property<string>("Name")
						.IsRequired()
						.HasMaxLength(40)
						.HasColumnType("nvarchar(40)");

					b.Property<decimal>("Price")
						.HasColumnType("money");

					b.Property<string>("Slug")
						.IsRequired()
						.HasColumnType("nvarchar(450)");

					b.Property<string>("Summary")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("Name")
						.IsUnique();

					b.HasIndex("Slug")
						.IsUnique();

					b.ToTable("Tours");
				});

			modelBuilder.Entity("Detours.Data.Entities.TourGuide", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<Guid>("GuideId")
						.HasColumnType("uniqueidentifier");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("GuideId");

					b.HasIndex("TourId", "GuideId")
						.IsUnique();

					b.ToTable("TourGuides", (string)null);
				});

			modelBuilder.Entity("Detours.Data.Entities.TourImage", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<string>("Image")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("TourId");

					b.ToTable("TourImages", (string)null);
				});

			modelBuilder.Entity("Detours.Data.Entities.TourImageCover", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<string>("Image")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("TourId")
						.IsUnique();

					b.ToTable("TourImageCovers", (string)null);
				});

			modelBuilder.Entity("Detours.Data.Entities.TourLocation", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<string>("Description")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<float>("X")
						.HasColumnType("real");

					b.Property<float>("Y")
						.HasColumnType("real");

					b.HasKey("Id");

					b.ToTable("TourLocations", (string)null);

					b.UseTptMappingStrategy();
				});

			modelBuilder.Entity("Detours.Data.Entities.TourStartDate", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<DateTimeOffset>("Date")
						.HasColumnType("datetimeoffset");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("TourId", "Date")
						.IsUnique();

					b.ToTable("TourStartDates", (string)null);
				});

			modelBuilder.Entity("Detours.Data.Entities.User", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<string>("Email")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Name")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Password")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Photo")
						.HasColumnType("nvarchar(max)");

					b.Property<int>("Role")
						.HasColumnType("int");

					b.HasKey("Id");

					b.ToTable("Users");
				});

			modelBuilder.Entity("Detours.Data.Entities.StartTourLocation", b =>
				{
					b.HasBaseType("Detours.Data.Entities.TourLocation");

					b.Property<string>("Address")
						.IsRequired()
						.HasColumnType("nvarchar(max)");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.HasIndex("TourId")
						.IsUnique()
						.HasFilter("[TourId] IS NOT NULL");

					b.ToTable("StartTourLocations", (string)null);
				});

			modelBuilder.Entity("Detours.Data.Entities.TourLocationByDay", b =>
				{
					b.HasBaseType("Detours.Data.Entities.TourLocation");

					b.Property<byte>("Day")
						.HasColumnType("tinyint");

					b.Property<Guid>("TourId")
						.HasColumnType("uniqueidentifier");

					b.HasIndex("TourId");

					b.ToTable("TourLocationsByDay", (string)null);
				});

			modelBuilder.Entity("Detours.Data.Entities.Booking", b =>
				{
					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithMany()
						.HasForeignKey("TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.HasOne("Detours.Data.Entities.User", "User")
						.WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Tour");

					b.Navigation("User");
				});

			modelBuilder.Entity("Detours.Data.Entities.RefreshToken", b =>
				{
					b.HasOne("Detours.Data.Entities.User", "User")
						.WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("User");
				});

			modelBuilder.Entity("Detours.Data.Entities.Review", b =>
				{
					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithMany("Reviews")
						.HasForeignKey("TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.HasOne("Detours.Data.Entities.User", "User")
						.WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Tour");

					b.Navigation("User");
				});

			modelBuilder.Entity("Detours.Data.Entities.TourGuide", b =>
				{
					b.HasOne("Detours.Data.Entities.User", "Guide")
						.WithMany()
						.HasForeignKey("GuideId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithMany("Guides")
						.HasForeignKey("TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Guide");

					b.Navigation("Tour");
				});

			modelBuilder.Entity("Detours.Data.Entities.TourImage", b =>
				{
					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithMany("Images")
						.HasForeignKey("TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Tour");
				});

			modelBuilder.Entity("Detours.Data.Entities.TourImageCover", b =>
				{
					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithOne("ImageCover")
						.HasForeignKey("Detours.Data.Entities.TourImageCover", "TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Tour");
				});

			modelBuilder.Entity("Detours.Data.Entities.TourStartDate", b =>
				{
					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithMany("StartDates")
						.HasForeignKey("TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Tour");
				});

			modelBuilder.Entity("Detours.Data.Entities.StartTourLocation", b =>
				{
					b.HasOne("Detours.Data.Entities.TourLocation", null)
						.WithOne()
						.HasForeignKey("Detours.Data.Entities.StartTourLocation", "Id")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithOne("StartLocation")
						.HasForeignKey("Detours.Data.Entities.StartTourLocation", "TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Tour");
				});

			modelBuilder.Entity("Detours.Data.Entities.TourLocationByDay", b =>
				{
					b.HasOne("Detours.Data.Entities.TourLocation", null)
						.WithOne()
						.HasForeignKey("Detours.Data.Entities.TourLocationByDay", "Id")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.HasOne("Detours.Data.Entities.Tour", "Tour")
						.WithMany("Locations")
						.HasForeignKey("TourId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Tour");
				});

			modelBuilder.Entity("Detours.Data.Entities.Tour", b =>
				{
					b.Navigation("Guides");

					b.Navigation("ImageCover")
						.IsRequired();

					b.Navigation("Images");

					b.Navigation("Locations");

					b.Navigation("Reviews");

					b.Navigation("StartDates");

					b.Navigation("StartLocation")
						.IsRequired();
				});
#pragma warning restore 612, 618
		}
	}
}
