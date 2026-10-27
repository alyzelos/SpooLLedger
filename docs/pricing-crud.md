# Pricing CRUD

27 October 2026

Pricing holds the cost inputs used to price a print. It follows the same flow as [filament-stock-crud.md](filament-stock-crud.md): `PricingController` sends a MediatR command or query, `ValidationBehavior` runs the FluentValidation rules, and the handler calls `IPricingRepository`. `PricingRepository` talks to SQLite through `DataContext`. Fields are copied by `PricingMapper`.

## Fields

| Field | Meaning |
| --- | --- |
| `name` | Label for the price set, for example "Default" or a printer name. Unique |
| `electricityCostPerKwh` | Price of one kWh of electricity |
| `filamentSpoolPrice` | Price of one 1 kg spool |
| `filamentCostPerGram` | Read-only. `filamentSpoolPrice / 1000`, because stock is counted in grams |
| `printerMaintenanceCostPerHour` | Wear and maintenance cost per printing hour |

`filamentCostPerGram` is computed on the `Pricing` entity and is not stored. Amounts are in euros.

## Seed

`DataSeeder.SeedPricing` fills the table from `Pricing.json` on startup when it is empty:

| Name | Electricity / kWh | Spool (1 kg) | Per gram | Maintenance / hour |
| --- | --- | --- | --- | --- |
| Standard | €1.50 | €30.00 | €0.03 | €0.50 |
| Urgent | €1.50 | €30.00 | €0.03 | €1.00 |

## Endpoints

| Method | Path | Result |
| --- | --- | --- |
| GET | `/api/pricing` | List of pricing rows, sorted by name |
| GET | `/api/pricing/{id}` | One row, or 404 |
| POST | `/api/pricing` | Created row, or 400 when validation fails |
| PUT | `/api/pricing/{id}` | Updated row, 404 when missing, or 400 when validation fails |
| DELETE | `/api/pricing/{id}` | 204, or 404 when missing |

The name is required, at most 100 characters, and must not belong to another row. The three costs must be zero or greater. An update also requires an id greater than zero.

## Price calculation

`ItemPriceCalculator` in `SpooLLedger.Application/Pricings` turns an item and a pricing row into a breakdown. It does no database work. Filament is `weight × filamentCostPerGram`. Maintenance is the print time in hours times `printerMaintenanceCostPerHour`. Electricity uses a fixed printer draw: 120 W for the first 10 minutes, then 80 W. Energy in kWh is that power times the hours, and the cost is the energy times `electricityCostPerKwh`. `CalculateItemPriceHandler` calls it and stores the rounded total on the item.

## Angular page

`/pricing` shows `PricingPage`: a PrimeNG card with an Add button and a table. Each row has edit (pencil) and delete (bin) buttons. Add and edit share one dialog, which shows the price per gram while the spool price is typed. Delete asks for confirmation.

## Where the pieces live

- Entity and configuration: `SpooLLedger.Domain/Entities/Pricing.cs`, `SpooLLedger.Infrastructure/Data/Configurations/PricingConfiguration.cs`
- Repository contract and implementation: `SpooLLedger.Application/Abstractions/IPricingRepository.cs`, `SpooLLedger.Infrastructure/Repositories/PricingRepository.cs`
- Commands, queries, validators, and mapper: `SpooLLedger.Application/Pricings`
- Controller: `SpooLLedger/Controllers/PricingController.cs`
- Tests: `SpooLLedger.Tests/PricingTests.cs`
- Angular page and service: `client/src/app/pricing`
