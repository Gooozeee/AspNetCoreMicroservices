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
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
