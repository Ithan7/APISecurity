using Demo.BL;
using Demo.ORM;

namespace Demo.MSTest
{
    [TestClass]
    public sealed class TestBL
    {
        [TestMethod]
        public void TestMethod2()
        {

            Data data = new Data();
            Service service = new Service(data);
            var result = service.GetCustomer(1);
            Assert.IsNotNull(result);
        }
    }
}
