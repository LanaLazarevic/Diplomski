using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PFM.Application.Dto;
using PFM.Application.Interfaces;
using PFM.Application.UseCases.Analytics.Queries.GetSpendingAnalytics;
using PFM.Application.UseCases.Transaction.Commands.SplitTransaction;
using PFM.Application.UseCases.Transaction.Queries.GetAllTransactions;
using PFM.Application.Validation;
using PFM.Domain.Interfaces;
using PFM.Infrastructure.Persistence;
using PFM.Infrastructure.Persistence.Repositories;
using PFM.Infrastructure.Services;
using PFM.Infrastructure.Settings;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<EmailSettings>(configuration.GetSection("Email"));


            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IValidator<GetTransactionsQuery>, GetTransactionsQueryValidator>();
            services.AddScoped<IValidator<GetSpendingsAnalyticsQuery>, GetSpendingsAnalyticsQueryValidator>();
            services.AddScoped<IValidator<SplitTransactionCommand>, SplitTransactionCommandValidator>();
            services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
            services.AddScoped<IValidator<CreateCardDto>, CreateCardDtoValidator>();
            services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
            services.AddScoped<ITransactionImportLogger, FileTransactionImportLogger>();
            services.AddScoped<IAutoCategorizationService, AutoCategorizationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<MonthlyReportService>();
            services.AddHostedService(sp => sp.GetRequiredService<MonthlyReportService>());
            return services;
        }

       
    }
}
