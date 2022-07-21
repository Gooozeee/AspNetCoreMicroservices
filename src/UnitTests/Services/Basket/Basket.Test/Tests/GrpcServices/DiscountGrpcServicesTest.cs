using Basket.API.GrpcServices;
using Basket.Tests.CallHelpers;
using Basket.Tests.TestData.GrpcServices;
using Discount.Grpc.Protos;
using FluentAssertions;
using Moq;
using System.Threading;

namespace Basket.Test.GrpcServices
{
    // Test class for the DiscountGrpcService Consumer class
    public class DiscountGrpcServicesTest
    {
        // Test method for the GetDiscount method
        [Theory]
        [ClassData(typeof(GrpcServiceTestData))]
        public async void GetDiscountTest(CouponModel coupon, string? productName)
        {
            // Arrange
            var discountProtoServiceMock = new Mock<DiscountProtoService.DiscountProtoServiceClient>();
            var discountGrpcService = new DiscountGrpcService(discountProtoServiceMock.Object);

            var mockCall = CallHelpers.CreateAsyncUnaryCall(coupon);

            discountProtoServiceMock
                .Setup(m => m.GetDiscountAsync(
                    It.IsAny<GetDiscountRequest>(), null, null, CancellationToken.None))
                .Returns(mockCall);

            // Act
            var result = await discountGrpcService.GetDiscount(productName);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CouponModel>();

            discountProtoServiceMock
                .Verify(m => m.GetDiscountAsync(
                    It.IsAny<GetDiscountRequest>(), 
                    null, 
                    null, 
                    CancellationToken.None), 
                    Times.Once);
        }
    }
}
