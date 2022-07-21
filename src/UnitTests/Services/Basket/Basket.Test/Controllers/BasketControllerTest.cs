using AutoMapper;
using Basket.API.Controllers;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.Tests.CallHelpers;
using Basket.Tests.TestData.Controllers;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Basket.Test.Controller
{
    // Test class for the basket controller
    public class BasketControllerTest
    {
        // Initialising the main mock objects
        public readonly Mock<IBasketRepository> _basketRepositoryMock = new();
        public readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
        public readonly Mock<IMapper> _mapperMock = new();

        // Testing the get basket method
        [Theory]
        [InlineData("Michal")]
        [InlineData("")]
        public void GetBasketTest(string? userName)
        {
            // Arrange
            var discountProtoServiceMock = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
            var discountGrpcServiceMock = new Mock<DiscountGrpcService>(discountProtoServiceMock.Object);

            var basketController = new BasketController(_basketRepositoryMock.Object,
                discountGrpcServiceMock.Object, _publishEndpointMock.Object, _mapperMock.Object);

            // Act
            var basket = basketController.GetBasket("Michal");

            // Assert
            if (userName == "")
            {
                _basketRepositoryMock.Verify(r => r.GetBasket(userName), Times.Never);
            }
            else
            {
                _basketRepositoryMock.Verify(r => r.GetBasket(userName), Times.Once);
            }

            basket.Should().NotBeNull();
            basket.Result.Should().NotBeNull();
        }

        // Testing the update basket method
        [Theory]
        [ClassData(typeof(UpdateBasketTestData))]
        public void UpdateBasketTest(ShoppingCart shoppingCart, CouponModel coupon)
        {
            // Arrange
            var discountProtoServiceMock = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
            var discountGrpcServiceMock = new Mock<DiscountGrpcService>(discountProtoServiceMock.Object);

            var basketController = new BasketController(_basketRepositoryMock.Object,
                discountGrpcServiceMock.Object, _publishEndpointMock.Object, _mapperMock.Object);

            var mockCall = CallHelpers.CreateAsyncUnaryCall(coupon);

            discountProtoServiceMock
                .Setup(m => m.GetDiscountAsync(
                    It.IsAny<GetDiscountRequest>(), null, null, CancellationToken.None))
                .Returns(mockCall);

            // Act
            var basket = basketController.UpdateBasket(shoppingCart);

            // Assert
            if (shoppingCart == null)
            {
                _basketRepositoryMock.Verify(r => r.UpdateBasket(shoppingCart), Times.Never);
            }
            else
            {
                _basketRepositoryMock.Verify(r => r.UpdateBasket(shoppingCart), Times.Once);
            }
            basket.Should().NotBeNull();
            basket.Result.Should().NotBeNull();

        }

        // Testing the delete basket method
        [Theory]
        [InlineData("Michal")]
        [InlineData("")]
        [InlineData(null)]
        public async void DeleteBasketTest(string? userName)
        {
            // Arrange
            var discountProtoServiceMock = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
            var discountGrpcServiceMock = new Mock<DiscountGrpcService>(discountProtoServiceMock.Object);

            var basketController = new BasketController(_basketRepositoryMock.Object,
                discountGrpcServiceMock.Object, _publishEndpointMock.Object, _mapperMock.Object);

            Task completedTask = Task.CompletedTask;

            _basketRepositoryMock
                .Setup(m => m.DeleteBasket(It.IsAny<string>()))
                .Returns(completedTask);

            // Act
            var result = await basketController.DeleteBasket(userName);

            // Assert
            result.Should().NotBeNull();
            if (userName == "")
            {
                result.Should().BeOfType<BadRequestObjectResult>();
                _basketRepositoryMock.Verify(r => r.DeleteBasket(userName), Times.Never);
            }
            if (userName == null)
            {
                result.Should().BeOfType<BadRequestObjectResult>();
                _basketRepositoryMock.Verify(r => r.DeleteBasket(userName), Times.Never);
            }
            if (userName == "Michal")
            {
                result.Should().BeOfType<OkResult>();
                _basketRepositoryMock.Verify(r => r.DeleteBasket(userName), Times.Once);
            }
        }

        // Testing the Basket Checkout method
        [Theory]
        [ClassData(typeof(CheckoutBasketTestData))]
        public async void CheckoutBasketTest(BasketCheckout basketCheckout, ShoppingCartItem cartItem, BasketCheckoutEvent checkoutEvent)
        {
            // Arrange
            var discountProtoServiceMock = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
            var discountGrpcServiceMock = new Mock<DiscountGrpcService>(discountProtoServiceMock.Object);

            var basketController = new BasketController(_basketRepositoryMock.Object,
                discountGrpcServiceMock.Object, _publishEndpointMock.Object, _mapperMock.Object);

            var itemList = new List<ShoppingCartItem>();
            itemList.Add(cartItem);

            _basketRepositoryMock
                .Setup(m => m.GetBasket(It.IsAny<string>()))
                .Returns(Task.FromResult(
                 new ShoppingCart()
                 {
                     UserName = "Michal",
                     Items = itemList
                 }));

            Task completedTask = Task.CompletedTask;
            _basketRepositoryMock
                .Setup(m => m.DeleteBasket(It.IsAny<string>()))
                .Returns(completedTask);

            _mapperMock.Setup(m =>
                m.Map<BasketCheckoutEvent>(It.IsAny<BasketCheckout>()))
                .Returns(checkoutEvent);

            // Act
            var result = await basketController.Checkout(basketCheckout);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AcceptedResult>();
            _mapperMock.Verify(m =>
                m.Map<BasketCheckoutEvent>(basketCheckout), Times.Once);
            _basketRepositoryMock.Verify(m =>
                m.DeleteBasket(basketCheckout.UserName), Times.Once);
        }
    }
}