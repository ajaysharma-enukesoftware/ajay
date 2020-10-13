using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoapCore;
using System.ServiceModel;
using IVPD.Web_Services;
using Microsoft.AspNetCore.Mvc;
using java.awt;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IVPD
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
     
  
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );
            });

            services.AddDbContext<IVPDContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IVPDDB")), ServiceLifetime.Transient);
            services.AddDbContext<IVPDSBaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IVPDSBase")), ServiceLifetime.Transient);
            services.AddDbContext<VindimaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("VINDIMA")), ServiceLifetime.Transient);
            services.AddDbContext<RevenueContext>(options => options.UseSqlServer(Configuration.GetConnectionString("REVENUEDB")), ServiceLifetime.Transient);

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<IParcelService, ParcelService>();
            services.AddScoped<IPermisssionService, PermissionsService>();
            services.AddScoped<IGroupPermisssionService, GroupPermisssionService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IDesignationService, DesignationService>();
            services.AddScoped<IAllService, AllService>();
            services.AddScoped<ILanguageKeysService, LanguageKeysService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IEstatutoService, EstatutoService>();
            services.AddScoped<IMatrixListService, MatrixListService>();
            services.AddScoped<IBusEntidadeEstatutoService, BusEntidadeEstatutoService>();
            services.AddScoped<IBusEntFreguesiaService, BusEntFreguesiaService>();
            services.AddScoped<IBusEntConcelhoService, BusEntConcelhoService>();
            services.AddScoped<IBusEntDistritoService, BusEntDistritoService>();
            services.AddScoped<IBusEntidadeService, BusEntidadeService>();
            services.AddScoped<IRecocileProducerService, RecocileProducerService>();
            services.AddScoped<ITransferProducerService, TransferProducerService>();
            services.AddScoped<ILogFilesService, LogFilesService>();
            services.AddScoped<IPaymentMadeService, PaymentMadeService>();
            services.AddScoped<IEntitiesDifferenceService, EntitiesDifferenceService>();
            services.AddScoped<IReconciliationCarriedService, ReconciliationCarriedService>();
            services.AddScoped<IPaymentNotSentService, PaymentNotSentService>();
            services.AddScoped<IOutstandingPaymentService, OutstandingPaymentService>();
            services.AddScoped<IPaymentHeldService, PaymentHeldService>();
            services.AddScoped<IPaymentReturnedService, PaymentReturnedService>();
            services.AddScoped<IPendingFilesService, PendingFilesService>();
            services.AddScoped<IConsultationPledgesService, ConsultationPledgesService>();
            services.AddScoped<IPaymentEntitiesService, PaymentEntitiesService>();
            services.AddScoped<IPaymentDicoFreService, PaymentDicoFreService>();
            services.AddScoped<IReceiptsService, ReceiptsService>();
            services.AddScoped<IPendingPaymentService, PendingPaymentService>();
            services.AddScoped<IIncomeStatementsService, IncomeStatementsService>();
            services.AddScoped<IFileDetailService, FileDetailService>();
            services.AddScoped<IPaymentFileDetailService, PaymentFileDetailService>();
            services.AddScoped<IPaymentDetailsService, PaymentDetailsService>();
            services.AddScoped<IRegisterMovementsService, RegisterMovementsService>();
            services.AddScoped<IRegistrationImpressionService, RegistrationImpressionService>();
            services.AddScoped<IPaymentConfirmationService, PaymentConfirmationService>();
            services.AddScoped<ILQBaseService, LQBaseService>();
            services.AddScoped<IMGBalanceService, MGBalanceService>();
            services.AddScoped<ISinonimoService, SinonimoService>();
            services.AddScoped<ICastaService, CastaService>();
            services.AddScoped<IImportedPaymentFilesService, ImportedPaymentFilesService>();
            services.AddScoped<IFilesCreateBankService, FilesCreateBankService>();
            services.AddScoped<IInformationYearService, InformationYearService>();
            services.AddScoped<IProducerAccountService, ProducerAccountService>();
            services.AddScoped<IRegisterPrintingService, RegisterPrintingService>();
            services.AddScoped<IProductionAuthorizationService, ProductionAuthorizationService>();
            services.AddScoped<IConsultCurrentAccountService, ConsultCurrentAccountService>();
            services.AddScoped<IPendingInvoicesServices, PendingInvoicesServices>();
            services.AddScoped<IDetailDocumentInfoService, DetailDocumentInfoService>();
            services.AddScoped<IBoxOpeningService, BoxOpeningService>();
            services.AddScoped<ICollectionRevenueService, CollectionRevenueService>();
            services.AddScoped<IBoxClaspService, BoxClaspService>();
            services.AddScoped<IBoxDetailsServices, BoxDetailsServices>();
            services.AddScoped<ICashValuesServices, CashValuesServices>();
            services.AddScoped<IEntityTransactionService, EntityTransactionService>();
            services.AddScoped<IAllTransactionService, AllTransactionService>();
            services.AddScoped<ILIBTESOUR_TSARTIFService, LIBTESOUR_TSARTIFService>();
            services.AddScoped<IAlottedServicesService, AlottedServicesService>();
            services.AddScoped<IAddTransactionService, AddTransactionService>();
            services.AddScoped<IInvoiceCreationService, InvoiceCreationService>();
            services.AddScoped<ILevantamentoService, LevantamentoService>();
            services.AddScoped<IFatoresPontucaoService, FatoresPontucaoService>();

            
            services.TryAddSingleton<ISample, Sample>();
            services.AddTransient<IBillingEntityWebService, BillingEntityWebService>();
            services.AddTransient<IDeleteTransactionWebService, DeleteTransactionWebService>();
            services.AddTransient<IUpdatePaymentMethodService, UpdatePaymentMethodService>();
            services.AddTransient<ICreditWebService, CreditWebService>();
            services.AddTransient<IInvoiceWebService, InvoiceWebService>();
            services.AddTransient<IUpdateBillingByEntityWebService, UpdateBillingByEntityWebService>();
            services.AddTransient<IExcelUploadWebService, ExcelUploadWebService>();


            // Enable the use of an [Authorize("Bearer")] attribute on methods and
            // classes to protect.
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPilicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().Build());
            });

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme).AddAzureADBearer(options => Configuration.Bind("AppSettings", options));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problems = new CustomBadRequest(context);

                return new BadRequestObjectResult(problems);
            };
        }); 
                
            services.AddSoapCore();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //    app.UseSoapEndpoint<ISample>("/Service.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<IBillingEntityWebService>("/BillingEntityWebService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<IDeleteTransactionWebService>("/DeleteTransactionWebService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<IUpdatePaymentMethodService>("/UpdatePaymentMethodService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<ICreditWebService>("/CreditWebService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<IInvoiceWebService>("/InvoiceWebService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<IUpdateBillingByEntityWebService>("/UpdateBillingByEntityWebService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<IExcelUploadWebService>("/ExcelUploadWebService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFileServer();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/StaticFiles"
                
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
