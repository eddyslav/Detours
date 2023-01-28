using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.FileProviders;

using Serilog;

using MediatR;

using Detours;
using Detours.Extensions;
using Detours.Middlewares;

using Detours.Data;

using Detours.Mediatr;

using Detours.Services;
using Detours.Services.Extensions;
using Detours.Services.Notifications;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDetoursAuthentication(configuration);
builder.Services.AddDetoursAuthorization();

builder.Services.AddDetoursControllers();

builder.AddDetoursLogging();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DetoursDbContext>(options => options
		.UseSqlServer(configuration.GetConnectionString(SettingNames.ConnectionStrings.DetoursDb)))
	.AddScoped<DetoursDbContext>();

builder.Services.AddMediatR(typeof(SignedUpUserNotificationHandler).Assembly);

builder.Services.AddSingleton<StrategiesPublisher>();

builder.Services.AddBookingService(configuration.GetRequiredSection(SettingNames.Services.Stripe).Bind);

if (environment.IsDevelopment())
{
	builder.Services
		.AddSmtpMailSenderService(configuration.GetRequiredSection(SettingNames.Services.SmtpMailSender).Bind);
}
else
{
	builder.Services
		.AddSendGridMailSenderService(configuration.GetRequiredSection(SettingNames.Services.SendGridMailSender).Bind);
}

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IClaimProvider, ClaimProvider>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
scope.ServiceProvider
	.GetRequiredService<DetoursDbContext>()
	.Database
	.Migrate();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "public")),
	RequestPath = "/public"
});

app.UseMiddleware<ErrorHandler>();

app.UseRouting();
app.UseCors(config =>
{
	config.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
