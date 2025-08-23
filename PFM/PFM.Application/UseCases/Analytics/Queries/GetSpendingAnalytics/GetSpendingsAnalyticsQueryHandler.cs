using FluentValidation;
using MediatR;
using PFM.Application.Result;
using PFM.Domain.Dtos;
using PFM.Domain.Enums;
using PFM.Domain.Interfaces;

namespace PFM.Application.UseCases.Analytics.Queries.GetSpendingAnalytics
{
    public class GetSpendingsAnalyticsQueryHandler : IRequestHandler<GetSpendingsAnalyticsQuery, OperationResult<SpendingsGroupDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<GetSpendingsAnalyticsQuery> _validator;


        public GetSpendingsAnalyticsQueryHandler(IUnitOfWork uow, IValidator<GetSpendingsAnalyticsQuery> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<OperationResult<SpendingsGroupDto>> Handle(GetSpendingsAnalyticsQuery request, CancellationToken cancellationToken)
        {

            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e =>
                {
                    var raw = e.ErrorMessage ?? string.Empty;
                    var parts = raw.Split(':', 3);
                    var tag = parts.ElementAtOrDefault(0) ?? e.PropertyName;
                    var code = parts.ElementAtOrDefault(1) ?? e.ErrorCode;
                    var message = parts.ElementAtOrDefault(2) ?? raw;
                    return new ValidationError
                    {
                        Tag = tag,
                        Error = code,
                        Message = message
                    };
                }).ToList();
                return OperationResult<SpendingsGroupDto>.Fail(400, errors);
            }


            if (!string.IsNullOrWhiteSpace(request.CatCode))
            {
                var cats = await _uow.Categories.GetByCodesAsync(new[] { request.CatCode }, cancellationToken);
                var cat = cats.SingleOrDefault();
                if (cat == null)
                {
                    BusinessError error = new BusinessError
                    {
                        Problem = "provided-category-does-not-exists",
                        Details = $"Category '{request.CatCode}' not found.",
                        Message = "The provided category does not exist."
                    };
                    List<BusinessError> errors = new List<BusinessError> { error };
                    return OperationResult<SpendingsGroupDto>.Fail(440, errors);

                }
            }
           
            DirectionEnum? directionEnum = null;

            if (!string.IsNullOrWhiteSpace(request.Direction))
            {
                directionEnum = Enum.Parse<DirectionEnum>(request.Direction, true);
            }

            DateTime? startUtc = request.StartDate.HasValue
                        ? DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc)
                        : (DateTime?)null;
            DateTime? endUtc = request.EndDate.HasValue
                        ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc)
                        : (DateTime?)null;

            var spec = new AnalyticsTransactionQuerySpecification(startUtc, endUtc, directionEnum);

            var txs = await _uow.Transactions.GetForAnalyticsAsync(spec, cancellationToken);

            var flat = txs.SelectMany(t =>
            (t.Splits != null && t.Splits.Count > 0)
                ? t.Splits.Select(s => new
                        {
                            Cat = s.CatCode ?? string.Empty,
                            Parent = s.Category?.ParentCode,
                            Amount = s.Amount
                        })
                : [ new {
                            Cat = t.CatCode ?? string.Empty,
                            Parent = t.Category?.ParentCode,
                            Amount = t.Amount
                        } ]);


           
            List<SpendingGroupDto> groups;

            if (string.IsNullOrWhiteSpace(request.CatCode))
            {
                groups = flat
                    .GroupBy(x => string.IsNullOrEmpty(x.Parent) ? x.Cat : x.Parent)
                    .Select(g => new SpendingGroupDto
                    {
                        CatCode = g.Key,
                        Amount = g.Sum(x => x.Amount),
                        Count = g.Count()
                    })
                    .ToList();
            }
            else
            {
                
                var childGroups = flat
                    .Where(x => x.Parent == request.CatCode)
                    .GroupBy(x => x.Cat)
                    .Select(g => new SpendingGroupDto
                    {
                        CatCode = g.Key,
                        Amount = g.Sum(x => x.Amount),
                        Count = g.Count()
                    })
                    .ToList();

                if (childGroups.Any())
                {
                    
                    groups = childGroups;
                }
                else
                {
                    
                    groups = flat
                        .Where(x => x.Cat == request.CatCode)
                        .GroupBy(x => x.Cat)
                        .Select(g => new SpendingGroupDto
                        {
                            CatCode = g.Key,
                            Amount = g.Sum(x => x.Amount),
                            Count = g.Count()
                        })
                        .ToList();
                }
            }

            return OperationResult<SpendingsGroupDto>.Success(
                new SpendingsGroupDto { Groups = groups }, 200
            );

        }
    }
}
