using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    class CustomerControllerTests
    {
        private CustomerController _controller; 

        [SetUp]
        public void Setup()
        {
            _controller = new CustomerController();
        }

        [Test]
        public void GetCustomer_IdIsZero_ReturnNotFound()
        {

            var result = _controller.GetCustomer(0);

            // Exactly a NotFound object
            Assert.That(result, Is.TypeOf<NotFound>());

            // Not Found Object or Derivatives
            //Assert.That(result, Is.InstanceOf<NotFound>());
        }

        [Test]
        public void GetCustomer_IdIsNotZero_ReturnFound()
        {
            var result = _controller.GetCustomer(1);

            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}
