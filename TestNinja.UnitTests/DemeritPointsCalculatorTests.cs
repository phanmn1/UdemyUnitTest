using System;
using NUnit.Framework;
using TestNinja.Fundamentals;
namespace TestNinja.UnitTests
{
    [TestFixture]
    public class DemeritPointsCalculatorTests
    {
        private DemeritPointsCalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new DemeritPointsCalculator();
        }

        // SpeedLimit = 65;
        // MaxSpeed = 300;
        [Test]
        [TestCase(-1)]
        [TestCase(350)]
        public void CalculateDemeritPoints_WhenSpeedIsNegativeOrGreaterThanMax_ShouldThrowError(int speed)
        {
            Assert.That(() => _calculator.CalculateDemeritPoints(speed), Throws.TypeOf<ArgumentOutOfRangeException>()); 
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(64, 0)]
        [TestCase(65, 0)]
        [TestCase(66, 0)]
        [TestCase(70, 1)]
        [TestCase(75, 2)]
        public void CalculateDemeritPoints_WhenOverSpeedLimit_ShouldReturnResult(int speed, int expected)
        {
            var result = _calculator.CalculateDemeritPoints(speed);

            Assert.That(result, Is.EqualTo(expected));
        }


    }
}
