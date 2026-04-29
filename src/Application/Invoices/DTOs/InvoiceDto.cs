using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.DTOs
{
    public record InvoiceDto(
        Guid Id,
        DateTime IssuedAt,
        DateTime DueDate,
        decimal TotalAmount,
        InvoiceStatus Status,
        Guid ContractId,
        IEnumerable<InvoiceLineItemDto> LineItems,
        DateTime CreatedAt
    );

    public record InvoiceLineItemDto(
        Guid Id,
        string Description,
        decimal Amount,
        Guid? MilestoneId
    );


}
