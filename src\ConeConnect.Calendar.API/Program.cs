using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.Services;
using ConeConnect.Calendar.API.Filters;
using ConeConnect.Calendar.API.Models;
using ConeConnect.Calendar.API.Middleware;
using Common.SharedKernel.Helpers.Storage;

var builder = WebApplication.CreateBuilder(args);
var CORS_POLICY = "CorsPolicy";
var key = Encoding.ASCII.GetBytes("db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
  options.Authority = "https://securetoken.google.com/coneconnect-ap";
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidIssuer = "https://securetoken.google.com/coneconnect-ap",
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidAudience = "coneconnect-ap",
    ValidateLifetime = true
  };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ValidationFilter>();
// builder.Services.AddScoped<LogFilter>();

builder.Services.AddScoped<IDispatchService, DispatchService>();
builder.Services.AddScoped<IWorkSitePhotosService, WorkSitePhotosService>();
builder.Services.AddScoped<IWorkPerformedService, WorkPerformedService>();
builder.Services.AddScoped<IJobSchedulesService, JobSchedulesService>();
builder.Services.AddScoped<IDayOffRequestService, DayOffRequestService>();
builder.Services.AddScoped<IWorkReceiptService, WorkReceiptService>();
builder.Services.AddScoped<IDapperService, DapperService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IUserDetailService, UserDetailService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IJSAService, JSAService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDeviceTrackingService, DeviceTrackingService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

FirebaseModel firebaseModel = new FirebaseModel();
firebaseModel.type = builder.Configuration.GetSection("FireBaseSettings").GetSection("type").Value;
firebaseModel.project_id = builder.Configuration.GetSection("FireBaseSettings").GetSection("project_id").Value;
firebaseModel.private_key_id = builder.Configuration.GetSection("FireBaseSettings").GetSection("private_key_id").Value;
firebaseModel.private_key = builder.Configuration.GetSection("FireBaseSettings").GetSection("private_key").Value;
firebaseModel.client_email = builder.Configuration.GetSection("FireBaseSettings").GetSection("client_email").Value;
firebaseModel.client_id = builder.Configuration.GetSection("FireBaseSettings").GetSection("client_id").Value;
firebaseModel.auth_uri = builder.Configuration.GetSection("FireBaseSettings").GetSection("auth_uri").Value;
firebaseModel.token_uri = builder.Configuration.GetSection("FireBaseSettings").GetSection("token_uri").Value;
firebaseModel.auth_provider_x509_cert_url = builder.Configuration.GetSection("FireBaseSettings").GetSection("auth_provider_x509_cert_url").Value;
firebaseModel.client_x509_cert_url = builder.Configuration.GetSection("FireBaseSettings").GetSection("client_x509_cert_url").Value;

FirebaseApp.Create(new AppOptions
{
  Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseModel))
});


builder.Services.AddCors(options =>
{
  options.AddPolicy(
                    name: CORS_POLICY,
                    builder =>
                    {
                      // builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'));
                      builder.AllowAnyOrigin();
                      builder.AllowAnyMethod();
                      builder.AllowAnyHeader();
                    });
});

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cone Connect Calendar API V1", Version = "v1", Description = string.Format("Build Number:{0}", builder.Configuration.GetSection("BuildId").Value ?? string.Empty) });
  c.AddSecurityDefinition(
 "Bearer",
 new OpenApiSecurityScheme
 {
   Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
   Name = "Authorization",
   In = ParameterLocation.Header,
   Type = SecuritySchemeType.Http,
   BearerFormat = "JWT",
   Scheme = "Bearer"
 });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

  // Added this for adding the custom Header Parameter.
});

builder.WebHost.UseSentry(options =>
{
  options.Dsn = builder.Configuration.GetSection("SentryDsnURL").Value ?? string.Empty;
  options.TracesSampleRate = 1.0;
  options.Environment = builder.Configuration.GetSection("SentryEnvironment").Value ?? "development";
  options.SetBeforeSend((@event, hint) =>
  {    // Never report server names
    @event.ServerName = null;
    return @event;
  });
});// Initialize Sentry

builder.Services.AddHealthChecks();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.MapHealthChecks("/health/status");
app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();
app.UseCors(CORS_POLICY);
app.Run();
