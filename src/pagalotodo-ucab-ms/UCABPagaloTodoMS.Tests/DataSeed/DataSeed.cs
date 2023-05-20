using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Core.Enums;

namespace UCABPagaloTodoMS.Tests.DataSeed
{
    public static class DataSeed
    {
        public static Mock<DbSet<ProviderEntity>> mockSetProviderEntity = new();
        public static Mock<DbSet<ServiceEntity>> mockSetServiceEntity = new();
        public static Mock<DbSet<FieldEntity>> mockSetFieldEntity = new();
        public static Mock<DbSet<DebtorsEntity>> mockSetDebtorsEntity = new();
        public static Mock<DbSet<ConsumerEntity>> mockSetConsumerEntity = new();
        public static Mock<DbSet<PaymentEntity>> mockSetPaymentEntity = new();

        public static void SetupDbContextData(this Mock<IUCABPagaloTodoDbContext> mockContext)
        {
            //Providers data
            var providers = new List<ProviderEntity>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Username = "prueba",
                    PasswordHash = "Password.",
                    Email = "prueba@prueba.com",
                    Name = "Jhonny",
                    LastName = "Test",
                    Status = true,
                    Rif = "V123456789",
                    AccountNumber = "12345678909876543212",
                    Services = new List<ServiceEntity>()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Username = "test",
                    PasswordHash = "Password!",
                    Email = "test@test.com",
                    Name = "Juan",
                    LastName = "Parcial",
                    Status = true,
                    Rif = "V123456729",
                    AccountNumber = "09876543211234567890",
                    Services = new List<ServiceEntity>()
                }
            };

            //Services data
            var services = new List<ServiceEntity>()
            {
                new()
                {
                    Id = new Guid("12345678-1234-1234-1234-1234567890AC"),
                    Name = "ferreLeon",
                    Description = "Ferreteria a domicilio",
                    ServiceStatus = ServiceStatusEnum.Inactivo,
                    ServiceType = ServiceTypeEnum.Directo,
                    Provider = providers[0],
                    Payments = new List<PaymentEntity>(),
                    ConciliationFormat = new List<FieldEntity>()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "directv Colombia",
                    Description = "television satelital en toda latinoamerica",
                    ServiceStatus = ServiceStatusEnum.Activo,
                    ServiceType = ServiceTypeEnum.PorConfirmacion,
                    Provider = providers[1],
                    Payments = new List<PaymentEntity>(),
                    ConfirmationList = new List<DebtorsEntity>(),
                    ConciliationFormat = new List<FieldEntity>()
                }
            };
            providers[0].Services!.Add(services[0]);
            providers[1].Services!.Add(services[1]);

            //Fields data
            var fields = new List<FieldEntity>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Field 1",
                    Format = "XXXXXXXXXX",
                    Length = 10,
                    AttrReference = "Payment.Id",
                    Type = "int",
                    Service = services[0]
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Field 2",
                    Format = "XXXXX.XX",
                    Length = 8,
                    AttrReference = "Payment.Amount",
                    Type = "float",
                    Service = services[0]
                }    
            };
            services[0].ConciliationFormat!.AddRange(fields);

            //Debtors data
            var debtors = new List<DebtorsEntity>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 10,
                    Identifier = "V27670971",
                    Status = false,
                    Service = services[1]
                }
            };
            services[1].ConfirmationList!.AddRange(debtors);
            
            //Consumers data
            var consumers = new List<ConsumerEntity>()
            {
                new ConsumerEntity()
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Username = "prueba",
                    PasswordHash = "Password",
                    Email = "prueba@prueba.com",
                    Name = "Jhonny",
                    LastName = "Test",
                    Status = true,
                    ConsumerId = "V123456789",
                    Payments = new List<PaymentEntity>()
                },
                new ConsumerEntity()
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Username = "test",
                    PasswordHash = "Password",
                    Email = "test@test.com",
                    Name = "Juan",
                    LastName = "Parcial",
                    Status = true,
                    ConsumerId = "V123456789",
                    Payments = new List<PaymentEntity>()
                }
            };
            
            //Payments data
            var payments = new List<PaymentEntity>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 25,
                    Identifier = "1234",
                    OriginAccount = "12345678900987654321",
                    PaymentStatus = PaymentStatusEnum.Aprovado,
                    Service = services[0],
                    Consumer = consumers[0]
                },
                new()
                {
                Id = Guid.NewGuid(),
                Amount = 50,
                Identifier = "5678",
                OriginAccount = "12346678900947654321",
                PaymentStatus = PaymentStatusEnum.Aprovado,
                Service = services[0],
                Consumer = consumers[1]
                }
            };
            services[0].Payments!.AddRange(payments);
            consumers[0].Payments!.Add(payments[0]);
            consumers[1].Payments!.Add(payments[1]);
            
            //Providers setup
            mockSetProviderEntity.As<IQueryable<ProviderEntity>>().Setup(m => m.Provider)
                .Returns(providers.AsQueryable().Provider);
            mockSetProviderEntity.As<IQueryable<ProviderEntity>>().Setup(m => m.Expression)
                .Returns(providers.AsQueryable().Expression);
            mockSetProviderEntity.As<IQueryable<ProviderEntity>>().Setup(m => m.ElementType)
                .Returns(providers.AsQueryable().ElementType);
            mockSetProviderEntity.As<IQueryable<ProviderEntity>>().Setup(m => m.GetEnumerator())
                .Returns(providers.GetEnumerator());
            mockContext.Setup(c => c.Providers).Returns(providers.AsQueryable().BuildMockDbSet().Object);
            
            //Services setup
            mockSetServiceEntity.As<IQueryable<ServiceEntity>>().Setup(m => m.Provider)
                .Returns(services.AsQueryable().Provider);
            mockSetServiceEntity.As<IQueryable<ServiceEntity>>().Setup(m => m.Expression)
                .Returns(services.AsQueryable().Expression);
            mockSetServiceEntity.As<IQueryable<ServiceEntity>>().Setup(m => m.ElementType)
                .Returns(services.AsQueryable().ElementType);
            mockSetServiceEntity.As<IQueryable<ServiceEntity>>().Setup(m => m.GetEnumerator())
                .Returns(services.GetEnumerator());
            mockContext.Setup(c => c.Services).Returns(services.AsQueryable().BuildMockDbSet().Object);

            //Fields setup
            mockSetFieldEntity.As<IQueryable<FieldEntity>>().Setup(m => m.Provider)
                .Returns(fields.AsQueryable().Provider);
            mockSetFieldEntity.As<IQueryable<FieldEntity>>().Setup(m => m.Expression)
                .Returns(fields.AsQueryable().Expression);
            mockSetFieldEntity.As<IQueryable<FieldEntity>>().Setup(m => m.ElementType)
                .Returns(fields.AsQueryable().ElementType);
            mockSetFieldEntity.As<IQueryable<FieldEntity>>().Setup(m => m.GetEnumerator())
                .Returns(fields.GetEnumerator());
            mockContext.Setup(c => c.Fields).Returns(fields.AsQueryable().BuildMockDbSet().Object);

            //Debtors setup
            mockSetDebtorsEntity.As<IQueryable<DebtorsEntity>>().Setup(m => m.Provider)
                .Returns(debtors.AsQueryable().Provider);
            mockSetDebtorsEntity.As<IQueryable<DebtorsEntity>>().Setup(m => m.Expression)
                .Returns(debtors.AsQueryable().Expression);
            mockSetDebtorsEntity.As<IQueryable<DebtorsEntity>>().Setup(m => m.ElementType)
                .Returns(debtors.AsQueryable().ElementType);
            mockSetDebtorsEntity.As<IQueryable<DebtorsEntity>>().Setup(m => m.GetEnumerator())
                .Returns(debtors.GetEnumerator());
            mockContext.Setup(c => c.Debtors).Returns(debtors.AsQueryable().BuildMockDbSet().Object);

            //Consumers setup
            mockSetConsumerEntity.As<IQueryable<ConsumerEntity>>().Setup(m => m.Provider)
                .Returns(consumers.AsQueryable().Provider);
            mockSetConsumerEntity.As<IQueryable<ConsumerEntity>>().Setup(m => m.Expression)
                .Returns(consumers.AsQueryable().Expression);
            mockSetConsumerEntity.As<IQueryable<ConsumerEntity>>().Setup(m => m.ElementType)
                .Returns(consumers.AsQueryable().ElementType);
            mockSetConsumerEntity.As<IQueryable<ConsumerEntity>>().Setup(m => m.GetEnumerator())
                .Returns(consumers.GetEnumerator());
            mockContext.Setup(c => c.Consumers).Returns(consumers.AsQueryable().BuildMockDbSet().Object);
            
            //Payments setup
            mockSetPaymentEntity.As<IQueryable<PaymentEntity>>().Setup(m => m.Provider)
                .Returns(payments.AsQueryable().Provider);
            mockSetPaymentEntity.As<IQueryable<PaymentEntity>>().Setup(m => m.Expression)
                .Returns(payments.AsQueryable().Expression);
            mockSetPaymentEntity.As<IQueryable<PaymentEntity>>().Setup(m => m.ElementType)
                .Returns(payments.AsQueryable().ElementType);
            mockSetPaymentEntity.As<IQueryable<PaymentEntity>>().Setup(m => m.GetEnumerator())
                .Returns(payments.GetEnumerator());
            mockContext.Setup(c => c.Payments).Returns(payments.AsQueryable().BuildMockDbSet().Object);
        }
    }
}