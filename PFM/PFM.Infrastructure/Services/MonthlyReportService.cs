using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PFM.Application.Interfaces;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Formats.Asn1.AsnWriter;

namespace PFM.Infrastructure.Services
{
    public class MonthlyReportService : BackgroundService
    {
        private readonly ILogger<MonthlyReportService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CronExpression _cron;
        private readonly TaskCompletionSource<bool> _completed = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private Timer? _timer;

        public MonthlyReportService(ILogger<MonthlyReportService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _cron = CronExpression.Parse("0 0 0 1 * *", CronFormat.IncludeSeconds);
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
            {
                _timer?.Dispose();
                _completed.TrySetResult(true);
            });

            ScheduleNext();
            return _completed.Task;
        }

        private void ScheduleNext()
        {
            Console.WriteLine("Scheduling next monthly report job");
            var next = _cron.GetNextOccurrence(DateTime.UtcNow, TimeZoneInfo.Utc);
            if (next == null)
            {
                _logger.LogWarning("No next occurrence found for monthly report job");
                return;
            }

            var delay = next.Value - DateTime.UtcNow;
            _logger.LogInformation("Next monthly report scheduled for {Next}", next);

            _timer = new Timer(async _ => await RunScheduledAsync(), null, delay, Timeout.InfiniteTimeSpan);
        }

        private async Task RunScheduledAsync()
        {
            try
            {
                Console.WriteLine("Running monthly report job");
                await SendReportsAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send monthly reports");
            }
            finally
            {
                ScheduleNext();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }

        public async Task SendReportsAsync(CancellationToken ct)
        {
            Console.WriteLine("Generating monthly transaction reports");
            _logger.LogInformation("Generating monthly transaction reports");
            var start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddMonths(1).AddTicks(-1);
            using var scope = _scopeFactory.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var users = await uow.Users.GetAllWithCardsAndTransactionsAsync(start, end, ct);

            foreach (var user in users)
            {
                if (user.Cards == null || user.Cards.Count == 0)
                    continue;

                var sb = new StringBuilder();
                sb.AppendLine("CardNumber,Date,Amount,Description");

                foreach (var card in user.Cards!)
                {
                    if (card.Transactions == null)
                        continue;

                    foreach (var tx in card.Transactions.OrderBy(t => t.Date))
                    {
                        sb.AppendLine($"{card.CardNumber},{tx.Date:yyyy-MM-dd},{tx.Amount},{tx.Description}");
                    }
                }

                using var ms = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
                var subject = $"Transactions report for {start:yyyy-MM}";
                var body = "Please find attached your transactions report.";
                await emailService.SendEmailWithAttachmentAsync(user.Email, subject, body, ms, $"transactions_{start:yyyy_MM}.csv", ct);
            }
        }
    
    }
}
