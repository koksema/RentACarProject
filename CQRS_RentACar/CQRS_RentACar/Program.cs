using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.AirportsHandler;
using CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.EmployeeHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.EmployeesHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.FuelHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.LocationHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.SliderHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.SlidersHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.StatisticHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DemoContext>();

builder.Services.AddScoped<GetAboutQueryHandler>();
builder.Services.AddScoped<GetAboutByIdQueryHandler>();
builder.Services.AddScoped<CreateAboutCommandHandler>();
builder.Services.AddScoped<UpdateAboutCommandHandler>();
builder.Services.AddScoped<RemoveAboutCommandHandler>();

builder.Services.AddScoped<GetAirportsQueryHandler>();

builder.Services.AddScoped<GetBookingQueryHandler>();
builder.Services.AddScoped<GetBookingByIdQueryHandler>();
builder.Services.AddScoped<CreateBookingCommandHandler>();
builder.Services.AddScoped<UpdateBookingCommandHandler>();
builder.Services.AddScoped<RemoveBookingCommandHandler>();
builder.Services.AddScoped<GetAvailableCarsQueryHandler>();

builder.Services.AddScoped<GetCarsQueryHandler>();
builder.Services.AddScoped<GetCarsByIdQueryHandler>();
builder.Services.AddScoped<CreateCarsCommandHandler>();
builder.Services.AddScoped<UpdateCarsCommandHandler>();
builder.Services.AddScoped<RemoveCarsCommandHandler>();

builder.Services.AddScoped<GetEmployeesQueryHandler>();
builder.Services.AddScoped<GetEmployeesByIdQueryHandler>();
builder.Services.AddScoped<CreateEmployeesCommandHandler>();
builder.Services.AddScoped<UpdateEmployeesCommandHandler>();
builder.Services.AddScoped<RemoveEmployeesCommandHandler>();

builder.Services.AddScoped<GetFeaturesQueryHandler>();
builder.Services.AddScoped<GetFeaturesByIdQueryHandler>();
builder.Services.AddScoped<CreateFeaturesCommandHandler>();
builder.Services.AddScoped<UpdateFeaturesCommandHandler>();
builder.Services.AddScoped<RemoveFeaturesCommandHandler>();

builder.Services.AddScoped<GetFuelPricesQueryHandler>();


builder.Services.AddScoped<GetLocationQueryHandler>();
builder.Services.AddScoped<GetLocationByIdQueryHandler>();
builder.Services.AddScoped<CreateLocationCommandHandler>();
builder.Services.AddScoped<UpdateLocationCommandHandler>();
builder.Services.AddScoped<RemoveLocationCommandHandler>();

builder.Services.AddScoped<GetMessageQueryHandler>();
builder.Services.AddScoped<GetMessageByIdQueryHandler>();
builder.Services.AddScoped<CreateMessageCommandHandler>();
builder.Services.AddScoped<UpdateMessageCommandHandler>();
builder.Services.AddScoped<RemoveMessageCommandHandler>();

builder.Services.AddScoped<GetServicesQueryHandler>();
builder.Services.AddScoped<GetServicesByIdQueryHandler>();
builder.Services.AddScoped<CreateServicesCommandHandler>();
builder.Services.AddScoped<UpdateServicesCommandHandler>();
builder.Services.AddScoped<RemoveServicesCommandHandler>();

builder.Services.AddScoped<GetSliderQueryHandler>();
builder.Services.AddScoped<GetSliderByIdQueryHandler>();
builder.Services.AddScoped<CreateSliderCommandHandler>();
builder.Services.AddScoped<UpdateSliderCommandHandler>();
builder.Services.AddScoped<RemoveSliderCommandHandler>();


builder.Services.AddScoped<GetTestimonialQueryHandler>();
builder.Services.AddScoped<GetTestimonialByIdQueryHandler>();
builder.Services.AddScoped<CreateTestimonialCommandHandler>();
builder.Services.AddScoped<UpdateTestimonialCommandHandler>();
builder.Services.AddScoped<RemoveTestimonialCommandHandler>();











builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
