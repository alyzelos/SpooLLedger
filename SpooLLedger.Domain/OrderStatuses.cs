namespace SpooLLedger.Domain;

public static class OrderStatuses
{
    public const string Pending = "Pending";
    public const string Printing = "Printing";
    public const string Shipped = "Shipped";
    public const string ConfirmedPayment = "Confirmed payment";
    public const string Cancelled = "Cancelled";

    public static readonly string[] All =
    [
        Pending,
        Printing,
        Shipped,
        ConfirmedPayment,
        Cancelled
    ];
}
