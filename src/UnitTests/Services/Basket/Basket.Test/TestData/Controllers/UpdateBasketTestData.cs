using Basket.API.Entities;
using Discount.Grpc.Protos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Tests.TestData.Controllers
{
    // Test data for the update basket test
    public class UpdateBasketTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ShoppingCart
                {
                    UserName = "Michal",
                    Items = new List<ShoppingCartItem>()
                    {
                        new ShoppingCartItem
                        {
                            Price = 799,
                            ProductId = "hdh",
                            Color = "Red"
                        }
                    }
                },
                new CouponModel
                {
                    Amount = 20,
                    Description = "Red Phone",
                    Id = 1,
                    ProductName = "IPhone X"
                }
            };
            yield return new object[]
            {
                null,
                new CouponModel
                {
                    Amount = 20,
                    Description = "Red Phone",
                    Id = 1,
                    ProductName = "IPhone X"
                }
            };
            yield return new object[]
            {
                new ShoppingCart
                {
                    UserName = "Michal",
                    Items = new List<ShoppingCartItem>()
                    {
                        new ShoppingCartItem
                        {
                            Price = 799,
                            ProductId = "hdh",
                            Color = "Red",
                            ProductName = "IPhone X"
                        }
                    }
                },
                new CouponModel
                {
                    Amount = 20,
                    Description = "Red Phone",
                    Id = 1,
                    ProductName = "IPhone X"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
