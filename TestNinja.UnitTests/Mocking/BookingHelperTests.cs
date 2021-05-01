﻿using NUnit.Framework;
using TestNinja.Mocking;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    class BookingHelper_OverlappingBookingExistTests
    {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _existingBooking = new Booking
            {
                ArrivalDate = ArriveOn(2017, 1, 15),
                DepartureDate = DepartOn(2017, 1, 20),
                Reference = "a"
            };

            _repository = new Mock<IBookingRepository>();
            _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking>
            {
                _existingBooking
            }.AsQueryable());
        }

        [Test]
        public void BookingStartsAndFinishesBeforeExistingBooking_ReturnEmptyString()
        {
            
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate, days: 2),
                DepartureDate = Before(_existingBooking.ArrivalDate)
            }, _repository.Object);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsAndFinishesInTheMiddleOfExistingBooking_ReturnExistingBookingsReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.ArrivalDate)
            }, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesAfterExistingBooking_ReturnExistingBookingsReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate)
            }, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesInTheMiddleExistingBooking_ReturnExistingBookingsReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate)
            }, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsInTheMiddleOfExistingBookingButFinishesAfter_ReturnExistingBookingsReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate)
            }, _repository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesAfterExistingBooking_ReturnEmptyString()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.DepartureDate),
                DepartureDate = After(_existingBooking.DepartureDate, days: 2)
            }, _repository.Object);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingsOverlapbutNewBookingIsCancelled_ReturnEmptyString()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate),
                Status = "Cancelled"
                
            }, _repository.Object);

            Assert.That(result, Is.Empty);
        }

        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }

        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }

        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}
