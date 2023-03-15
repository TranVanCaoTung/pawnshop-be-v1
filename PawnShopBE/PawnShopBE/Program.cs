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

var builder = WebApplication.CreateBuilder(args);
//Add Authentication

var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    // Just use the name of your job that you created in the Jobs folder.
    var jobKey = new JobKey("ScheduleJob");
    q.AddJob<ScheduleJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ScheduleJob-trigger")
        //This Cron interval can be described as "run every minute" (when second is zero)
        .WithCronSchedule("0 * * ? * *")
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
builder.Services.AddScoped<ILiquidationService,LiquidationService>();
builder.Services.AddScoped<IPawnableProductService, PawnableProductService>();
builder.Services.AddScoped<ICustomerService,CustomerService>();
builder.Services.AddScoped<IKycService, KycService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IAttributeService, AttributeService>();
builder.Services.AddScoped<IInteresDiaryService, InterestDiaryService>();
builder.Services.AddScoped<IRansomService, RansomService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowReact");
app.MapControllers();
app.Run();
