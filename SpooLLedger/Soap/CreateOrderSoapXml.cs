using System.Globalization;
using System.Xml.Linq;
using SpooLLedger.Application.CQRS.Order;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrderSoap;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Soap;

public static class CreateOrderSoapXml
{
    private static readonly XNamespace Soap = "http://schemas.xmlsoap.org/soap/envelope/";

    public static CreateOrderSoapCommand Read(string xml)
    {
        var document = XDocument.Parse(xml);
        var body = document.Descendants(Soap + "Body").FirstOrDefault()
            ?? throw new InvalidOperationException("SOAP Body is missing.");
        var order = body.Elements().FirstOrDefault()
            ?? throw new InvalidOperationException("CreateOrder element is missing.");

        return new CreateOrderSoapCommand
        {
            OrderDate = DateTime.Parse(Value(order, "orderDate"), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
            Status = Value(order, "status"),
            CustomerId = int.Parse(Value(order, "customerId"), CultureInfo.InvariantCulture),
            Items = order.Elements().FirstOrDefault(element => element.Name.LocalName == "items")?
                .Elements()
                .Select(item => new OrderItemRequest
                {
                    ItemId = int.Parse(Value(item, "itemId"), CultureInfo.InvariantCulture),
                    Quantity = int.Parse(Value(item, "quantity"), CultureInfo.InvariantCulture)
                })
                .ToList() ?? []
        };
    }

    public static string Write(CreateOrderSoapResponse response)
    {
        var items = new XElement("items",
            response.Items.Select(item => new XElement("item",
                new XElement("itemId", item.ItemId),
                new XElement("productName", item.ProductName),
                new XElement("weight", item.Weight),
                new XElement("pricePerUnit", item.PricePerUnit),
                new XElement("quantity", item.Quantity),
                new XElement("totalWeight", item.TotalWeight),
                new XElement("totalPrice", item.TotalPrice))));

        return Envelope(new XElement("CreateOrderResponse",
            new XElement("orderNumber", response.OrderNumber),
            new XElement("orderDate", response.OrderDate.ToString("o", CultureInfo.InvariantCulture)),
            new XElement("status", response.Status),
            new XElement("customerName", response.CustomerName),
            items));
    }

    public static string WriteFault(string message)
    {
        return Envelope(new XElement(Soap + "Fault",
            new XElement("faultcode", "soap:Client"),
            new XElement("faultstring", message)));
    }

    private static string Envelope(XElement bodyContent)
    {
        var document = new XDocument(
            new XElement(Soap + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soap", Soap),
                new XElement(Soap + "Body", bodyContent)));
        return document.ToString();
    }

    private static string Value(XElement parent, string name) =>
        parent.Elements().FirstOrDefault(element => element.Name.LocalName == name)?.Value
        ?? throw new InvalidOperationException($"{name} is missing.");
}
