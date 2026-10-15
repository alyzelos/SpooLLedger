using System;

namespace SpooLLedger.Domain.Entities;

public class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public int OrderNumber { get; set; }
    public DateTime OrderDate { get;  set; }
    public string Status { get; set; } = "Pending";

    // LINK TO CUSTOMER: Every order belongs to a specific Customer
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!; 

    // LINK TO ITEMS: An order contains multiple components/items
    public List<OrderItem> Items { get; set; } = new();

    // Computed Property to get the total cost of the order automatically
    public decimal TotalAmount 
    {
        get
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.TotalPrice;
            }
            return total;
        }
    }
}
