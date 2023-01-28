using Microsoft.EntityFrameworkCore;

using Detours.Data.Entities;

namespace Detours.Data;

public sealed class DetoursDbContext : DbContext
{
	public DbSet<User> Users { get; init; } = default!;
	public DbSet<RefreshToken> RefreshTokens { get; init; } = default!;

	public DbSet<Tour> Tours { get; init; } = default!;
	public DbSet<Review> Reviews { get; init; } = default!;

	public DbSet<Booking> Bookings { get; init; } = default!;

	public DetoursDbContext(DbContextOptions<DetoursDbContext> options)
		: base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TourGuide>(e =>
		{
			e.ToTable("TourGuides");

			e.HasOne(x => x.Tour)
				.WithMany(x => x.Guides)
				.HasForeignKey(x => x.TourId);

			e.HasOne(x => x.Guide)
				.WithMany()
				.HasForeignKey(x => x.GuideId);

			e.HasIndex(x => new { x.TourId, x.GuideId })
				.IsUnique();
		});

		modelBuilder.Entity<TourStartDate>(e =>
		{
			e.ToTable("TourStartDates");

			e.HasOne(x => x.Tour)
				.WithMany(x => x.StartDates)
				.HasForeignKey(x => x.TourId);

			e.HasIndex(x => new { x.TourId, x.Date })
				.IsUnique();
		});

		modelBuilder.Entity<TourLocation>(e =>
		{
			e.ToTable("TourLocations");
		});

		modelBuilder.Entity<TourLocationByDay>(e =>
		{
			e.ToTable("TourLocationsByDay");

			e.HasOne(x => x.Tour)
				.WithMany(x => x.Locations)
				.HasForeignKey(x => x.TourId);
		});

		modelBuilder.Entity<StartTourLocation>(e =>
		{
			e.ToTable("StartTourLocations");

			e.HasOne(x => x.Tour)
				.WithOne(x => x.StartLocation)
				.HasForeignKey<StartTourLocation>(x => x.TourId);
		});

		modelBuilder.Entity<TourImage>(e =>
		{
			e.ToTable("TourImages");

			e.HasOne(x => x.Tour)
				.WithMany(x => x.Images)
				.HasForeignKey(x => x.TourId);
		});

		modelBuilder.Entity<TourImageCover>(e =>
		{
			e.ToTable("TourImageCovers");

			e.HasOne(x => x.Tour)
				.WithOne(x => x.ImageCover)
				.HasForeignKey<TourImageCover>(x => x.TourId);
		});

		modelBuilder.Entity<Review>(e =>
		{
			e.Property(x => x.Description)
				.HasMaxLength(255);

			e.HasOne(x => x.Tour)
				.WithMany(x => x.Reviews)
				.HasForeignKey(x => x.TourId);

			e.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId);
		});

		modelBuilder.Entity<Tour>(e =>
		{
			e.Property(x => x.Name)
				.HasMaxLength(40);

			e.HasIndex(x => x.Name)
				.IsUnique();

			e.HasIndex(x => x.Slug)
				.IsUnique();

			e.Property(x => x.Price)
				.HasColumnType("money");
		});

		modelBuilder.Entity<Booking>(e =>
		{
			e.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId);

			e.HasOne(x => x.Tour)
				.WithMany()
				.HasForeignKey(x => x.TourId);

			e.Property(x => x.Price)
				.HasColumnType("money");
		});

		modelBuilder.Entity<RefreshToken>(e =>
		{
			e.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId);
		});
	}
}
