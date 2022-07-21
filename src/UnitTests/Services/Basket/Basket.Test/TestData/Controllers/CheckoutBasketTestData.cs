using Basket.API.Entities;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Tests.TestData.Controllers
{
    public class CheckoutBasketTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new BasketCheckout
                {
                    UserName = "Michal",
                    TotalPrice = 799
                },
                new ShoppingCartItem
                {
                    ProductName = "IPhone X",
                    Price = 499,
                    Quantity = 2
                },
                new BasketCheckoutEvent
                {
                    UserName = "Michal",
                    TotalPrice = 998
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
