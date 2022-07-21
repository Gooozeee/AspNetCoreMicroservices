using Discount.Grpc.Protos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Tests.TestData.GrpcServices
{
    public class GrpcServiceTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new CouponModel
                {
                    Amount = 20,
                    Description = "Red Phone",
                    Id = 1,
                    ProductName = "IPhone X"
                },
                "Michal"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
