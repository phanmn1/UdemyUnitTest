using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    class FizzBuzzTests
    {
        [Test]
        public void GetOutput_NumberOnlyDivByThree_ShouldReturnFizz()
        {
            var result = FizzBuzz.GetOutput(6);

            Assert.That(result, Is.EqualTo("Fizz"));
        }

        [Test]
        public void GetOutput_NumberOnlyDivByFive_ShouldReturnBuzz()
        {
            var result = FizzBuzz.GetOutput(10);

            Assert.That(result, Is.EqualTo("Buzz"));
        }

        [Test]
        public void GetOutput_NumberDivByFiveAndThree_ShouldReturnFizzBuzz()
        {
            var result = FizzBuzz.GetOutput(15);

            Assert.That(result, Is.EqualTo("FizzBuzz")); 
        }

        [Test]
        public void GetOutput_NumberNotDivByFiveOrThree_ShouldReturnNumber()
        {
            var result = FizzBuzz.GetOutput(7);

            Assert.That(result, Is.EqualTo("7"));
        }

    }
}
