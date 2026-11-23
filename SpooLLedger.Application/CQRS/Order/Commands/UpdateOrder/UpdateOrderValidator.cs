using FluentValidation;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain;

namespace SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator(
        IFilamentTypeRepository filamentTypes,
        IFilamentColorRepository filamentColors,
        IPricingRepository pricings,
        IOrderRepository orders)
    {
        RuleFor(c => c.OrderNumber).GreaterThan(0);
        RuleFor(c => c.OrderDate).NotEmpty();
        RuleFor(c => c.Status).Must(status => OrderStatuses.All.Contains(status));
        RuleFor(c => c.PricingName).NotEmpty().MaximumLength(100);
        RuleFor(c => c.PricingName)
            .MustAsync((name, cancellationToken) => pricings.NameExistsAsync((name ?? "").Trim(), null, cancellationToken))
            .WithMessage("Choose a pricing that exists.");
        RuleFor(c => c.CustomerId).GreaterThan(0);
        RuleForEach(c => c.Items).ChildRules(line =>
        {
            line.RuleFor(item => item.ItemId).GreaterThan(0);
            line.RuleFor(item => item.FilamentType).NotEmpty().MaximumLength(100);
            line.RuleFor(item => item.Color).NotEmpty().MaximumLength(100);
            line.RuleFor(item => item.Color)
                .MustAsync((color, cancellationToken) => filamentColors.ExistsAsync((color ?? "").Trim(), cancellationToken))
                .WithMessage("Choose a filament color that exists.");
            line.RuleFor(item => item.Quantity).GreaterThan(0);
            line.RuleFor(item => item.CompletedQuantity).GreaterThanOrEqualTo(0);
            line.RuleFor(item => item.CompletedQuantity)
                .LessThanOrEqualTo(item => item.Quantity)
                .WithMessage("Completed pieces cannot exceed the number ordered.");
        });

        RuleFor(command => command.Items).CustomAsync(async (items, context, cancellationToken) =>
        {
            var command = (UpdateOrderCommand)context.InstanceToValidate;
            var order = await orders.GetByOrderNumber(command.OrderNumber, cancellationToken);
            var stored = order.Items
                .Select(line => (line.FilamentType ?? "").Trim())
                .ToHashSet();

            foreach (var item in items)
            {
                var type = (item.FilamentType ?? "").Trim();
                if (stored.Contains(type) || await filamentTypes.ExistsAsync(type, cancellationToken))
                    continue;

                context.AddFailure("Choose a filament type that exists.");
            }
        });
    }
}
