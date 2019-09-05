using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.Request;
using Aliyun.FunctionCompute.SDK.Response;
using Xunit;

namespace Aliyun.FunctionCompute.SDK.Unittests
{
    [Collection("fcDotnet.Unittests")]
    public class ReservedCapacityUnitTests : IDisposable
    {
        readonly TestConfig tf = new TestConfig();

        public void Dispose()
        {
            Console.WriteLine("ReservedCapacityUnitTests TearDownBase .....");
        }

        [Fact]
        public void TestListReservedCapacities()
        {
            var response = tf.Client.ListReservedCapacities(new ListReservedCapacitiesRequest(5));
            Assert.True(response.Data.ReservedCapacities.GetLength(0)<=5);

            for (int i = 0; i < response.Data.ReservedCapacities.GetLength(0); i++)
            {
                Assert.Equal(22, response.Data.ReservedCapacities[i].InstanceId.Length);
                Assert.True(response.Data.ReservedCapacities[i].CU > 0);
                Assert.NotNull(response.Data.ReservedCapacities[i].LastModifiedTime);
                Assert.NotNull(response.Data.ReservedCapacities[i].IsRefunded);
                Assert.True(response.Data.ReservedCapacities[i].Deadline.CompareTo(response.Data.ReservedCapacities[i].CreatedTime) > 0);
            }
        }
    }
}
