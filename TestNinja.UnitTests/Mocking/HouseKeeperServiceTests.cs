using NUnit.Framework;
using TestNinja.Mocking;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    class HouseKeeperServiceTests
    {
        private HousekeeperService _service;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<HousekeeperService.IXtraMessageBox> _messageBox;
        private System.DateTime _statementDate = new System.DateTime(2017, 1, 1);
        private HousekeeperService.Housekeeper _houseKeeper;
        private readonly string _statementFileName = "filename";

        [SetUp]
        public void SetUp()
        {
            _houseKeeper = new HousekeeperService.Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };
            var unitOfWork = new Mock<IUnitOfWork>();           
            unitOfWork.Setup(uow => uow.Query<HousekeeperService.Housekeeper>()).Returns(new List<HousekeeperService.Housekeeper>
            {
                _houseKeeper
            }.AsQueryable());

            _statementGenerator = new Mock<IStatementGenerator>();
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<HousekeeperService.IXtraMessageBox>();

            _service = new HousekeeperService(
                unitOfWork.Object,
                _statementGenerator.Object,
                _emailSender.Object,
                _messageBox.Object);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {

            _service.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate));
        }

        

        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_HouseKeepersEmailNullEmptyWhitespace_ShouldNotGenerateStatements(string email)
        {
            _houseKeeper.Email = email;

            _service.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg =>
                sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, (_statementDate)),
                Times.Never);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement()
        {
            _statementGenerator.Setup(sg =>
                sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, (_statementDate)))
                  .Returns(_statementFileName);
                
            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                _houseKeeper.Email,
                _houseKeeper.StatementEmailBody,
                _statementFileName,
                It.IsAny<string>()));

        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_StatementFileNameIsEmptyNullWhitespace_ShouldNotEmailTheStatement(string emailStatement)
        {
            _statementGenerator.Setup(sg =>
                sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, (_statementDate)))
                  .Returns(() => emailStatement);

            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), 
                Times.Never);

        }

        [Test]
        public void SendStatementEmails_EmailSendingFails_ShouldDisplayMessageBox()
        {
            _emailSender.Setup(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
                )).Throws<Exception>();

            _service.SendStatementEmails(_statementDate);

            _messageBox.Verify(mb => mb.Show(
                It.IsAny<string>(),
                It.IsAny<string>(),
                HousekeeperService.MessageBoxButtons.OK
                ));

        }


    }
}
