# Orders

15 October 2026

An order belongs to one customer and has many order lines. Each line stores a quantity and points at a catalog `Item`. Product name, weight, and price live on `Item`. `Customer`, `Item`, and `OrderItem` use integer ids. `Order.Id` stays a Guid.

## Endpoints

- `GET api/orders` returns every order with customer name and lines.
- `GET api/orders/{orderNumber}` returns one order.
- `POST api/orders/create-order` creates an order. Body: `orderDate`, `status` (`Pending`), `customerId`, and `items` (`itemId`, `quantity`).
- `PUT api/orders/update` replaces order fields and the full line list. Body adds `orderNumber`.

`OrdersController` sends MediatR requests. Handlers call `IOrderRepository`. The repository assigns the next `OrderNumber` and loads `Customer` and `Items.Item` before mapping to `OrderLineResponse`.

## Seeding

`Program` migrates, then seeds filament stock, customers, items (`Item.json`), orders, and order lines (`OrderItem.json`). Each seed step skips a table that already has rows. Order lines reference item ids from `Item.json`.
