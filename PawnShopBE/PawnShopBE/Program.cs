using Microsoft.EntityFrameworkCore;
using PawnShopBE;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.ServiceExtension;
using Services.Services;
using Services.Services.IServices;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PawnShopBE.Helpers;
using System.Configuration;
using Quartz;
using PawnShopBE.Core.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using PawnShopBE.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using PawnShopBE.Core.Validation;

var builder = WebApplication.CreateBuilder(args);
//Add Authentication

var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    // Just use the name of your job that you created in the Jobs folder.
    var scheduleJob = new JobKey("ScheduleJob");
    q.AddJob<ScheduleJob>(opts => opts.WithIdentity(scheduleJob));

    q.AddTrigger(opts => opts
        .ForJob(scheduleJob)
        .WithIdentity("ScheduleJob-trigger")
        //This Cron interval can be described as "run every minute" (when second is zero)
        .WithCronSchedule("0 * * ? * *")
    );

    var monthlyJob = new JobKey("MonthlyJob");
    q.AddJob<MonthlyJob>(opts => opts.WithIdentity(monthlyJob));

    q.AddTrigger(opts => opts
        .ForJob(monthlyJob)
        .WithIdentity("MonthlyJob-trigger")
        // Run at midnight on the 1st of every month
        .WithCronSchedule("0 0 0 1 * ?")
    );
    q.AddTrigger(opts => opts
        .ForJob(monthlyJob)
        .WithIdentity("MonthlyJob-trigger")
        // Run at 9 PM on the last day of every month
        .WithCronSchedule("0 0 21 L * ?")
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    otp =>
    {
        otp.TokenValidationParameters = new TokenValidationParameters
        {
            // tự cấp token
            ValidateIssuer = false,
            ValidateAudience = false,

            //ký vào token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.
builder.Services.Configure<Appsetting>(builder.Configuration.GetSection("AppSettings"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDIServices(builder.Configuration);

//builder.Services.AddIdentity<User, Role>(options =>
//{
//    options.User.RequireUniqueEmail= false;
//})
//   .AddEntityFrameworkStores<DbContextClass>()
//   .AddDefaultTokenProviders();
//builder.Services.AddSingleton<ILookupNormalizer, UpperInvariantLookupNormalizer>();
//builder.Services.AddScoped<IUserStore<User>, UserStore<User,Role,DbContextClass,Guid>>();
//builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//builder.Services.AddScoped<UserManager<User>, UserManager<User>>();
//builder.Services.AddScoped<SignInManager<User>, SignInManager<User>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IAuthentication, AuthenticationService>();
builder.Services.AddScoped<ILedgerService, LedgerService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IDependentService, DependentService>();
builder.Services.AddScoped<ICustomerRelativeService, CustomerRelativeService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IContractAssetService, ContractAssetService>();
builder.Services.AddScoped<IWareHouseService, WareHouseService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<ILiquidationService, LiquidationService>();
builder.Services.AddScoped<IPawnableProductService, PawnableProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IKycService, KycService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IAttributeService, AttributeService>();
builder.Services.AddScoped<IInteresDiaryService, InterestDiaryService>();
builder.Services.AddScoped<IRansomService, RansomService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ILogContractService, LogContractService>();
builder.Services.AddScoped<IMoneyService, MoneyService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Setting fluent validation
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BranchValidation>());
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    // hiển thị khung authorize điền token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \\n\\n
                      Enter your token in the text input below.
                      \\n\\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
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
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
    c.OperationFilter<RemovePrefixFromAuthorizationHeaderFilter>();
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowReact");
app.MapControllers();
app.Run();
