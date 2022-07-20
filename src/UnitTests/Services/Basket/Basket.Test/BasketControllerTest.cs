using AutoMapper;
using Basket.API.Controllers;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using FluentAssertions;
using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;

namespace Basket.Test
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
            if(userName == "")
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

        // TODO!! Can't get this to work
        [Fact]
        public void UpdateBasketTest()
        {
            // Arrange
            var discountProtoServiceMock = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
            var discountGrpcServiceMock = new Mock<DiscountGrpcService>(discountProtoServiceMock.Object);

            var basketController = new BasketController(_basketRepositoryMock.Object,
                discountGrpcServiceMock.Object, _publishEndpointMock.Object, _mapperMock.Object);

            var discountRequest = new GetDiscountRequest()
            {
                ProductName = "Iphone X"
            };

            //discountProtoServiceMock.Setup(m => m.GetDiscountAsync(It.IsAny<GetDiscountRequest>())).Returns((AsyncUnaryCall<CouponModel> coupon) =>
            //{
            //    coupon.Amount = 20;
            //    return coupon;
            //}); 

            var shoppingCart = new ShoppingCart();

            var cartItem = new ShoppingCartItem()
            {
                Quantity = 1,
                Color = "Red",
                Price = 799,
                ProductId = "EE1E53EDD6AEDE591AB72322",
                ProductName = "Iphone X"
            };

            shoppingCart.Items.Add(cartItem);

            // Act
            var basket = basketController.UpdateBasket(shoppingCart);

            // Assert
            basket.Should().NotBeNull();
            basket.Result.Should().NotBeNull();
           
           
        }
    }
}