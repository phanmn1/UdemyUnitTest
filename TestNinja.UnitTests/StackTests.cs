using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    class StackTests
    {
        private Stack<User> _stack;

        [SetUp]
        public void SetUp()
        {
            _stack = new Stack<User>();
        }

        [Test]
        public void Push_WhenNUll_ShouldThrowArgumentNullException()
        {
            User _user = null;
            
            Assert.That(() => _stack.Push(_user), Throws.ArgumentNullException);
        }

        [Test]
        public void Push_WhenCalled_ShouldAddToStack()
        {
            var initialCount = _stack.Count;
            User _user = new User() { IsAdmin = false };

            _stack.Push(_user);

            Assert.That(_stack.Count, Is.EqualTo(initialCount + 1));
        }

        [Test]
        public void Pop_WhenCountIs0_ShouldThrowInvalidOperationException()
        {
            Assert.That(() => _stack.Pop(), Throws.InvalidOperationException);
        }

        [Test]
        public void Pop_WhenCalled_ShouldReturnRecentValue()
        {
            User _user1 = new User();
            User _user2 = new User();

            _stack.Push(_user1);
            _stack.Push(_user2);

            var result = _stack.Pop();

            Assert.That(result, Is.EqualTo(_user2));
        }

        [Test]
        public void Peek_WhenCountIs0_ShouldReturnInvalidOperationException()
        {
            Assert.That(() => _stack.Peek(), Throws.InvalidOperationException);
        }

        [Test]
        public void Peek_WhenCalled_ShouldShowRecentValue()
        {
            User _user1 = new User();
            User _user2 = new User();

            _stack.Push(_user1);
            _stack.Push(_user2);

            var result = _stack.Peek();

            Assert.That(result, Is.EqualTo(_user2));
        }

        [Test]
        public void Peek_WhenCalled_ShouldNotChangeCount()
        {
            User _user1 = new User();
            User _user2 = new User();

            _stack.Push(_user1);
            _stack.Push(_user2);

            var count = _stack.Count;

            _stack.Peek();

            Assert.That(count, Is.EqualTo(count));
        }
    }
}
