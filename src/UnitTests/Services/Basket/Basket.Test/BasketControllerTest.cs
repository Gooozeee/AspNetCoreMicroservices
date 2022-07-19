using AutoMapper;
using Basket.API.Controllers;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Basket.Test
{
    public class BasketControllerTest
    {
        private const string UserName = "Michal";
        public readonly Mock<IBasketRepository> _basketRepositoryMock = new();
        public readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
        public readonly Mock<IMapper> _mapperMock = new();

        [Theory]
        [InlineData("Michal")]
        [InlineData("")]
        public void GetBasketTest(string? userName)
        {
            // Given
            var discountProtoServiceMock = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
            var discountGrpcServiceMock = new Mock<DiscountGrpcService>(discountProtoServiceMock.Object);

            var basketController = new BasketController(_basketRepositoryMock.Object,
                discountGrpcServiceMock.Object, _publishEndpointMock.Object, _mapperMock.Object);

            // When
            var basket = basketController.GetBasket("Michal");

            // Then
            _basketRepositoryMock.Verify(r => r.GetBasket(userName), Times.Never);
            basket.Should().NotBeNull();
            basket.Should().BeOfType<OkResult>();

            Console.WriteLine(basket.Result);
        }
    }
}