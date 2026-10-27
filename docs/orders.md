# Orders

15 October 2026

An order belongs to one customer and has many order lines. Each line stores a quantity and points at a catalog `Item`. Product name, weight, print time, and price live on `Item` (see [items-crud.md](items-crud.md)). An item's price can be empty; an order line then reports a null unit price and a total price of zero. An item used by an order line cannot be deleted. `Customer`, `Item`, and `OrderItem` use integer ids. `Order.Id` stays a Guid.

## Endpoints

- `GET api/orders` returns every order with customer name and lines.
- `GET api/orders/{orderNumber}` returns one order.
- `POST api/orders/create-order` creates an order. Body: `orderDate`, `status` (`Pending`), `pricingName` (`Standard` or `Urgent`, or any name in the pricing table), `customerId`, and `items` (`itemId`, `quantity`).
- `POST api/orders/create-order-soap` creates an order from a SOAP 1.1 envelope. `CreateOrderSoapHandler` maps that command onto `CreateOrderCommand` and uses the same repository. The response is a SOAP envelope.
- `PUT api/orders/update` replaces order fields and the full line list. Body adds `orderNumber` and `pricingName`.

`GET api/orders` returns orders newest first. Each line stores its own filament type and color, separate from the catalog item, plus the item's print time. The color must be one of the names from `FilamentColors`. The same item can be added again when the filament type or the color is different. The Angular Orders page, opened from the toolbar, lists those orders. A row expands to the lines, the total print time (each item's time multiplied by how many instances were ordered), and the total price. Each order stores a pricing name. Line prices are calculated from that pricing row (filament, electricity, and maintenance), so Standard and Urgent totals differ. Adding an order picks a customer and a pricing from searchable lists, or creates the customer in the same dialog. Edit changes the customer, the pricing, and the status, and keeps the lines. Existing orders without a stored pricing are treated as Standard. While an order is not cancelled, its filament needed (item weight times the number of instances, grouped by filament type and color) is removed from filament stock. Changing the lines updates that amount. Cancelling an order keeps the filament for finished pieces out of stock and returns the rest. When nothing was finished, the whole amount goes back. Deleting an order returns whatever that order still holds. Each order line stores `completedQuantity`, how many pieces are already printed. The order completion percentage is completed pieces divided by ordered pieces. While the status is Printing, the status tag shows that fraction, for example Printing 1/2. Statuses are Pending (grey), Printing (yellow), Shipped (green), Confirmed payment (blue), and Cancelled (red). Confirmed payment replaces the old Delivered status. The order number cell has Edit and Delete. Edit changes only the customer and the status, and keeps the lines. Delete calls `DELETE api/orders/{orderNumber}` and removes the order with its lines. The expanded row adds a product that is not already on the order, with a number of pieces. The order response includes `customerId` so the dialog can select the current customer.

## Seeding

`Program` migrates, then seeds filament stock, customers, items (`Item.json`), orders, and order lines (`OrderItem.json`). Each seed step skips a table that already has rows. Order lines reference item ids from `Item.json`.
