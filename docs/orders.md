# Orders

15 October 2026

An order belongs to one customer and has many order lines. Each line stores a quantity and points at a catalog `Item`. Product name, weight, print time, and price live on `Item` (see [items-crud.md](items-crud.md)). An item's price can be empty; an order line then reports a null unit price and a total price of zero. An item used by an order line cannot be deleted. `Customer`, `Item`, and `OrderItem` use integer ids. `Order.Id` stays a Guid.

## Endpoints

- `GET api/orders` returns every order with customer name and lines.
- `GET api/orders/{orderNumber}` returns one order.
- `POST api/orders/create-order` creates an order. Body: `orderDate`, `status` (`Pending`), `customerId`, and `items` (`itemId`, `quantity`).
- `POST api/orders/create-order-soap` creates an order from a SOAP 1.1 envelope. `CreateOrderSoapHandler` maps that command onto `CreateOrderCommand` and uses the same repository. The response is a SOAP envelope.
- `PUT api/orders/update` replaces order fields and the full line list. Body adds `orderNumber`.

`OrdersController` sends MediatR requests. Handlers call `IOrderRepository`. The repository assigns the next `OrderNumber` and loads `Customer` and `Items.Item` before mapping to `OrderLineResponse`.

## Seeding

`Program` migrates, then seeds filament stock, customers, items (`Item.json`), orders, and order lines (`OrderItem.json`). Each seed step skips a table that already has rows. Order lines reference item ids from `Item.json`.
