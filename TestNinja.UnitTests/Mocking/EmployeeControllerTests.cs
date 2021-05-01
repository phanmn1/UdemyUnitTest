using TestNinja.Mocking;
using NUnit.Framework;
using Moq;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    class EmployeeControllerTests
    {
        [Test]
        public void DeleteEmployee_WhenCalled_DeleteEmployeeFromDb()
        {
            var storage = new Mock<IEmployeeStorage>();
            var controller = new EmployeeController(storage.Object);

            var result = controller.DeleteEmployee(1);

            storage.Verify(s => s.DeleteEmployee(1));
            Assert.That(result, Is.TypeOf<RedirectResult>());
        }
    }
}
