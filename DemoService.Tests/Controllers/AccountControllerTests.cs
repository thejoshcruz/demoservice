using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;
using Moq;

using DemoService.Controllers;
using DemoService.Data;
using DemoService.Models;

namespace DemoService.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests : BaseTests
    {
        private readonly string FakeAccountNumber = "8675309";

        private AccountState FakeAccount
        {
            get
            {
                return new AccountState
                {
                    AccountStatus = AccountStatus.Active,
                    AsOfDate = DateTime.Now,
                    CurrentBalance = 100.0M,
                    AccountNumber = FakeAccountNumber,
                    PortfolioNumber = "Portfolio01",
                    AccountInventory = "Inventory01",
                    LastPaymentAmount = 100.0M,
                    LastPaymentDate = DateTime.Now.Subtract(new TimeSpan(5,0,0,0)),
                    DaysDelinquent = 2
                };
            }
        }


        [Test]
        public void GetAccountsByPortfolioNumber_InvalidInput_ReturnsBadRequest()
        {
            AccountController controller = new AccountController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.GetAccountsByPortfolioNumber(null);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }


        [Test]
        public void GetAccountsByPortfolioNumber_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByPortfolioNumber(It.IsAny<string>())).Throws(new Exception("dogs and cats, living together!"));

            AccountController controller = new AccountController(mock.Object);

            object result = controller.GetAccountsByPortfolioNumber("1");
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }

        [Test]
        public void GetAccountsByPortfolioNumber_WithValidInputs_ReturnsAccounts()
        {
            List<AccountState> list = new List<AccountState> { FakeAccount };

            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByPortfolioNumber(It.IsAny<string>())).Returns(list as object);

            AccountController controller = new AccountController(mock.Object);
            object result = controller.GetAccountsByPortfolioNumber("1");

            object accounts = null;
            if (result is OkObjectResult)
            {
                accounts = ((OkObjectResult)result).Value;
            }

            Assert.AreEqual(accounts, list as object);
        }


        [Test]
        public void GetAccountsByUsername_InvalidInput_ReturnsBadRequest()
        {
            AccountController controller = new AccountController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.GetAccountsByUsername(null);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public void GetAccountsByUsername_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByUsername(It.IsAny<string>())).Throws(new Exception("i'm you're huckleberry"));

            AccountController controller = new AccountController(mock.Object);

            object result = controller.GetAccountsByUsername("user1");
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }

        [Test]
        public void GetAccountsByUsername_WithValidInputs_ReturnsAccounts()
        {
            List<AccountState> list = new List<AccountState> { FakeAccount };

            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByUsername(It.IsAny<string>())).Returns(list as object);

            AccountController controller = new AccountController(mock.Object);
            object result = controller.GetAccountsByUsername("user1");

            object accounts = null;
            if (result is OkObjectResult)
            {
                accounts = ((OkObjectResult)result).Value;
            }

            Assert.AreEqual(accounts, list as object);
        }

    }
}
