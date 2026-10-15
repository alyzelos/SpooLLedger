using System;
using MediatR;

namespace SpooLLedger.Application.CQRS.Customer.Responses;

public class CustomerQueryResponse 
{
    public int Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; } 

    public string? FullName { get; set; }

    public string? Email  { get; set; }
    public string? PhoneNumber  { get; set; }

    public bool IsActive  { get; set; }
    public DateTime CreatedAt  { get; set; }
}
