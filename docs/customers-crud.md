# Customers CRUD

27 October 2026

A customer is the person an order belongs to. Listing and reading one customer already existed. Create, update, and delete follow the same flow as [items-crud.md](items-crud.md): `CustomersController` sends a MediatR command, `ValidationBehavior` runs FluentValidation, and the handler calls `ICustomerRepository`. Writes use `CustomerMapper`. The existing list and get-by-id queries still use AutoMapper.

## Fields

First name, last name, email, and phone are required. Names are at most 100 characters, email at most 200 and must be an email address, and phone at most 30. `isActive` is a flag. `createdAt` is set when the customer is created and is not changed by an update.

## Endpoints

| Method | Path | Result |
| --- | --- | --- |
| GET | `/api/customers` | Every customer |
| GET | `/api/customers/{id}` | One customer |
| POST | `/api/customers` | Created customer, or 400 when validation fails |
| PUT | `/api/customers/{id}` | Updated customer, 404 when missing, or 400 when validation fails |
| DELETE | `/api/customers/{id}` | 204, 404 when missing, or 409 when the customer has orders |

Orders reference a customer with `DeleteBehavior.Restrict`. `DeleteCustomerHandler` checks `IsUsedInOrdersAsync` and returns 409 instead of letting SQLite reject the delete. Orders themselves are unchanged.

## Angular page

The Resources page shows a Customers card under the Items card. The table paginates at 5 rows and offers 5, 10, 15, and 20. Each row has edit and delete buttons. Add and edit share one dialog.

## Where the pieces live

- Entity: `SpooLLedger.Domain/Entities/Customer.cs`
- Commands and mapper: `SpooLLedger.Application/CQRS/Customers`
- Controller: `SpooLLedger/Controllers/CustomersController.cs`
- Tests: `SpooLLedger.Tests/CustomerTests.cs`
- Angular card: `client/src/app/customers`, shown from `client/src/app/items/items.html`
