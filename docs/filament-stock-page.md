# Filament stock page

11 October 2026

The filament stock card lives on its own Angular page. Home shows the photos from `client/public`. The page loads rows through `FilamentStockService`, which calls `StockController`.

## Routes

| Path | Component | What it shows |
| --- | --- | --- |
| `/` | `Home` | Photos `/017.jpg`, `/080.jpg`, and `/136.jpg` in one band under the toolbar, with an intro card 50px below the toolbar |
| `/filament-stock` | `FilamentStockPage` | PrimeNG card with the stock list |

The toolbar **Filament Stock** button navigates to `/filament-stock`.

## How the list is loaded

`FilamentStockPage` calls `FilamentStockService.getAll()` on init. The service sends `GET https://localhost:5101/api/stock`. That path is `StockController.GetFilament`, which returns `FilamentStockDto` rows as JSON. The card shows producer, type, color, and quantity. A failed request shows “Could not load filament stock.” An empty list shows “No filament records yet.”

## Home photos

The three `.jpg` files in `client/public` sit in a full-width row directly under the toolbar. The row is 550px tall. Each photo fills its third of the width with `object-fit: cover`. `spoolicon.png` and `avatar.png` stay on the toolbar.

A PrimeNG card sits on top of that row, 50px below the toolbar and centered. It holds a short description of the app, two tips, a rough note that filament is seed data only, and a note that orders are not implemented.

## Where the pieces live

- Page: `client/src/app/filament-stock/filament-stock.ts`
- Template: `client/src/app/filament-stock/filament-stock.html`
- HTTP service: `client/src/app/filament-stock/filament-stock.service.ts`
- Home photos: `client/src/app/home/home.html` and `client/src/app/home/home.css`
- Routes: `client/src/app/app.routes.ts`
- API: `GET /api/stock` on `StockController` (see [filament-stock-crud.md](filament-stock-crud.md))
