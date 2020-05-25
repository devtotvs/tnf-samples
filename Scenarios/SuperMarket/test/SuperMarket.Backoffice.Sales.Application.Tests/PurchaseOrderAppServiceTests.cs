﻿using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SuperMarket.Backoffice.Sales.Application.Services.Interfaces;
using SuperMarket.Backoffice.Sales.Application.Tests.Mocks;
using SuperMarket.Backoffice.Sales.Domain;
using SuperMarket.Backoffice.Sales.Domain.Entities;
using SuperMarket.Backoffice.Sales.Domain.Interfaces;
using SuperMarket.Backoffice.Sales.Dto;
using SuperMarket.Backoffice.Sales.Infra.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Tnf.Application.Services;
using Tnf.Localization;
using Tnf.TestBase;
using Xunit;
using static SuperMarket.Backoffice.Sales.Dto.PurchaseOrderDto;

namespace SuperMarket.Backoffice.Sales.Application.Tests
{
    public class PurchaseOrderAppServiceTests : TnfIntegratedTestBase
    {
        private ILocalizationSource _localizationSource;
        private readonly IPurchaseOrderAppService _appService;
        private readonly CultureInfo _culture;

        public PurchaseOrderAppServiceTests()
        {
            _appService = Resolve<IPurchaseOrderAppService>();

            _culture = CultureInfo.GetCultureInfo("pt-BR");
        }

        protected override void PreInitialize(IServiceCollection services)
        {
            base.PreInitialize(services);

            // Configura o mesmo Mapper para ser testado
            services.AddSalesApplicationDependency();

            services.ConfigureTnf(builder => builder.ConfigureSalesDomain());

            // Registro dos serviços de Mock
            services.AddScoped<PurchaseOrderServiceMockManager>();
            services.ReplaceTransient<IPurchaseOrderService, PurchaseOrderServiceMock>();
            services.ReplaceTransient<IPurchaseOrderReadRepository, PurchaseOrderReadRepositoryMock>();
            services.ReplaceTransient<IPriceTableRepository, PriceTableRepositoryMock>();
            services.ReplaceTransient<IPurchaseOrderRepository, PurchaseOrderRepositoryMock>();
        }

        protected override void PostInitialize(IServiceProvider provider)
        {
            base.PostInitialize(provider);

            _localizationSource = provider.GetService<ILocalizationManager>().GetSource(Constants.LocalizationSourceName);
        }

        [Fact]
        public void Should_Resolve_All()
        {
            TnfSession.ShouldNotBeNull();
            ServiceProvider.GetService<PurchaseOrderServiceMockManager>().ShouldNotBeNull();
            ServiceProvider.GetService<IPurchaseOrderService>().ShouldNotBeNull();
            ServiceProvider.GetService<IPurchaseOrderReadRepository>().ShouldNotBeNull();
            ServiceProvider.GetService<IPriceTableRepository>().ShouldNotBeNull();
            ServiceProvider.GetService<IPurchaseOrderRepository>().ShouldNotBeNull();
            ServiceProvider.GetService<IPurchaseOrderAppService>().ShouldNotBeNull();
        }


        [Fact]
        public async Task Should_GetAll()
        {
            // Act
            var response = await _appService.GetAllPurchaseOrderAsync(new PurchaseOrderRequestAllDto());

            // Assert
            Assert.False(LocalNotification.HasNotification());
            Assert.False(response.HasNext);
            Assert.Equal(4, response.Items.Count);
        }


        [Fact]
        public async Task Should_Get_PurchaseOrder()
        {
            // Arrange
            var guid = Resolve<PurchaseOrderServiceMockManager>().PurchaseOrderGuid;

            // Act
            var purchaseOrder = await _appService.GetPurchaseOrderAsync(new DefaultRequestDto(guid));

            // Assert
            Assert.False(LocalNotification.HasNotification());
            Assert.Equal(guid, purchaseOrder.Id);
            Assert.Equal(0, purchaseOrder.Discount);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Get_Null()
        {
            // Act
            var purchaseOrder = await _appService.GetPurchaseOrderAsync(null);

            // Assert
            Assert.Null(purchaseOrder);
            Assert.True(LocalNotification.HasNotification());
            var message = string.Format(LocalLocalizationSource.GetString(ApplicationService.Error.ApplicationServiceOnInvalidIdError, _culture), "request");
            Assert.Contains(LocalNotification.GetAll(), n => n.Message == message);

            // Act
            purchaseOrder = await _appService.GetPurchaseOrderAsync(new DefaultRequestDto());

            // Assert
            Assert.Null(purchaseOrder);
            Assert.True(LocalNotification.HasNotification());
            message = string.Format(LocalLocalizationSource.GetString(ApplicationService.Error.ApplicationServiceOnInvalidIdError, _culture), "request");
            Assert.Contains(LocalNotification.GetAll(), n => n.Message == message);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Get_Not_Found()
        {
            // Act
            var purchaseOrder = await _appService.GetPurchaseOrderAsync(new DefaultRequestDto(Guid.NewGuid()));

            // Assert
            Assert.Null(purchaseOrder);
        }


        [Fact]
        public async Task Should_Create_PurchaseOrder()
        {
            // Arrange
            var customerGuid = Guid.NewGuid();
            var validProductId = Resolve<PurchaseOrderServiceMockManager>().ValidProduct;

            // Act
            var purchaseOrder = await _appService.CreatePurchaseOrderAsync(new PurchaseOrderDto
            {
                CustomerId = customerGuid,
                Discount = 5,
                Products = new List<ProductDto>()
                {
                    new ProductDto(validProductId, 1)
                }
            });

            // Assert
            Assert.False(LocalNotification.HasNotification());
            Assert.NotEqual(Guid.Empty, purchaseOrder.Id);
            Assert.Equal(5, purchaseOrder.Discount);
            Assert.Equal(customerGuid, purchaseOrder.CustomerId);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Create()
        {
            // Act
            var purchaseOrder = await _appService.CreatePurchaseOrderAsync(null);

            // Assert
            Assert.Null(purchaseOrder);
            Assert.True(LocalNotification.HasNotification());
            var message = string.Format(LocalLocalizationSource.GetString(ApplicationService.Error.ApplicationServiceOnInvalidDtoError, _culture), "dto");
            Assert.Contains(LocalNotification.GetAll(), n => n.Message == message);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Create_With_Specifications()
        {
            // Act
            var purchaseOrder = await _appService.CreatePurchaseOrderAsync(new PurchaseOrderDto());

            // Assert
            Assert.Null(purchaseOrder);
            Assert.True(LocalNotification.HasNotification());
            var message = _localizationSource.GetString(PurchaseOrder.Error.PurchaseOrderMustHaveCustomer, _culture);
            Assert.Contains(LocalNotification.GetAll(), n => n.Message == message);
        }


        [Fact]
        public async Task Should_Update_PurchaseOrder()
        {
            // Arrange
            var guid = Resolve<PurchaseOrderServiceMockManager>().PurchaseOrderGuid;

            // Act
            var purchaseOrder = await _appService.UpdatePurchaseOrderAsync(guid, new PurchaseOrderDto
            {
                Discount = 5
            });

            // Assert
            Assert.False(LocalNotification.HasNotification());
            Assert.Equal(purchaseOrder.Id, guid);
            Assert.Equal(5, purchaseOrder.Discount);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Update()
        {
            // Act
            var purchaseOrder = await _appService.UpdatePurchaseOrderAsync(Guid.Empty, null);

            // Assert
            Assert.Null(purchaseOrder);
            Assert.True(LocalNotification.HasNotification());
            Assert.Equal(2, LocalNotification.GetAll().Count());
            var message = string.Format(LocalLocalizationSource.GetString(ApplicationService.Error.ApplicationServiceOnInvalidDtoError, _culture), "dto");
            Assert.Contains(LocalNotification.GetAll(), n => n.Message == message);
            message = string.Format(LocalLocalizationSource.GetString(ApplicationService.Error.ApplicationServiceOnInvalidIdError, _culture), "id");
            Assert.Contains(LocalNotification.GetAll(), n => n.Message == message);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Update_With_Specifications()
        {
            // Arrange
            var guid = Resolve<PurchaseOrderServiceMockManager>().PurchaseOrderGuid;
            var invalidProductId = Guid.NewGuid();
            var dto = new PurchaseOrderDto();
            dto.Products.Add(new PurchaseOrderDto.ProductDto(invalidProductId, 1));

            // Act
            var purchaseOrder = await _appService.UpdatePurchaseOrderAsync(guid, dto);

            // Assert
            Assert.NotNull(purchaseOrder);
            Assert.True(LocalNotification.HasNotification());
            var message = string.Format(_localizationSource.GetString(PurchaseOrder.Error.ProductsThatAreNotInThePriceTable, _culture), invalidProductId);
            Assert.Contains(LocalNotification.GetAll(), n => n.Message == message);
        }
    }
}
