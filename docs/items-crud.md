# Items CRUD

27 October 2026

An item is a printable product from the catalog. Order lines point at items, so an item carries the weight, print time, and price that orders read. Items follow the same flow as [filament-stock-crud.md](filament-stock-crud.md): `ItemsController` sends a MediatR command or query, `ValidationBehavior` runs FluentValidation, and the handler calls `IItemsRepository`. Fields are copied by `ItemMapper`; items no longer use AutoMapper.

## Fields

| Field | Meaning |
| --- | --- |
| `productName` | Required, at most 100 characters |
| `weight` | Grams of filament, greater than zero |
| `printDurationMinutes` | Print time in minutes, greater than zero. The Angular dialog edits it as hours and minutes |
| `details` | Optional notes, at most 1000 characters |
| `pricePerUnit` | Read-only through CRUD. Null until a price is calculated |

Create and update never set the price, and update keeps the existing price. The price will be filled by a separate calculate action that uses the `Pricing` table (see [pricing-crud.md](pricing-crud.md)). That action is not built yet; the Angular calculate button is a placeholder.

## Endpoints

| Method | Path | Result |
| --- | --- | --- |
| GET | `/api/items` | List of items, sorted by product name |
| GET | `/api/items/{id}` | One item, or 404 |
| POST | `/api/items` | Created item, or 400 when validation fails. `/api/items/create-item` is kept as an alias |
| PUT | `/api/items/{id}` | Updated item, 404 when missing, or 400 when validation fails |
| DELETE | `/api/items/{id}` | 204, 404 when missing, or 409 when an order line uses the item |

## Effect on orders

`OrderItem` references `Item` with `DeleteBehavior.Restrict`. `DeleteItemHandler` checks `IItemsRepository.IsUsedInOrdersAsync` first and returns `UsedInOrders`, which the controller turns into 409, instead of letting SQLite reject the delete.

Because `PricePerUnit` is nullable, `OrderLineResponse.PricePerUnit` is nullable too, and `OrderItem.TotalPrice` counts a missing price as zero.

## Angular page

The toolbar **Resources** button opens `/resources`, which shows `ItemsPage`: a PrimeNG card with an Add button and a table of items. Each row has edit (pencil) and delete (bin) buttons. Rows without a price also show a calculate (calculator icon) button. Deleting an item used in orders shows a message instead of removing it.

## Where the pieces live

- Entity and configuration: `SpooLLedger.Domain/Entities/Item.cs`, `SpooLLedger.Infrastructure/Data/Configurations/ItemConfiguration.cs`
- Repository: `SpooLLedger.Application/Abstractions/IItemsRepository.cs`, `SpooLLedger.Infrastructure/Repositories/ItemsRepository.cs`
- Commands, queries, validators, and mapper: `SpooLLedger.Application/CQRS/Items`
- Controller: `SpooLLedger/Controllers/ItemsController.cs`
- Migration: `AddItemDurationAndDetails`, which also fills print time and details for the five seeded items
- Tests: `SpooLLedger.Tests/ItemTests.cs`
- Angular page and service: `client/src/app/items`
