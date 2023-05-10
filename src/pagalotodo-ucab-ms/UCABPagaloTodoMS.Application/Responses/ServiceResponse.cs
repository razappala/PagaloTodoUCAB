﻿using UCABPagaloTodoMS.Core.Enums;

namespace UCABPagaloTodoMS.Application.Responses;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ServiceStatusEnum ServiceStatus { get; set; }
    public ServiceTypeEnum ServiceType { get; set; }
    public ProviderResponse? Provider { get; set; }
    public List<PaymentResponse>? Payments { get; set; }
    public List<DebtorsResponse>? ConfirmationList { get; set; }
    public List<FieldResponse>? ConciliationFormat { get; set; }
}