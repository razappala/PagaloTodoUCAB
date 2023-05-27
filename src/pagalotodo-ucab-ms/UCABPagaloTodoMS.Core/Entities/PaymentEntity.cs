﻿using System.ComponentModel.DataAnnotations;
using UCABPagaloTodoMS.Core.Enums;

namespace UCABPagaloTodoMS.Core.Entities;

public class PaymentEntity : BaseEntity
{
    public float? Amount { get; set; }
    public string? CardNumber { get; set; }
    public int? ExpirationMonth { get; set; }
    public int? ExpirationYear { get; set; }
    public string? CardholderName { get; set; }
    public string? CardSecurityCode { get; set; }
    public string? TransactionId { get; set; }
    public PaymentStatusEnum? PaymentStatus { get; set; }
    [Required]
    public ConsumerEntity? Consumer { get; set; }
    [Required]
    public ServiceEntity? Service { get; set; }
    //Para los pagos por confirmacion
    public string? Identifier { get; set; }
}