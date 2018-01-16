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
        private AccountState FakeAccount
        {
            get
            {
                return new AccountState
                {
                    AccountStatus = AccountStatus.Active,
                    AsOfDate = DateTime.Now,
                    CurrentBalance = 100.0M,
                    ID = 1,
                    PortfolioID = 1,
                    LastPaymentAmount = 100.0M,
                    LastPaymentDate = DateTime.Now.Subtract(new TimeSpan(5,0,0,0)),
                    DaysDelinquent = 2
                };
            }
        }


        [Test]
        public void GetAccountsByPortfolioId_InvalidInput_ReturnsBadRequest()
        {
            AccountController controller = new AccountController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.GetAccountsByPortfolioId(null);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }


        [Test]
        public void GetAccountsByPortfolioId_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByPortfolioId(It.IsAny<string>())).Throws(new Exception("dogs and cats, living together!"));

            AccountController controller = new AccountController(mock.Object);

            object result = controller.GetAccountsByPortfolioId("1");
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }

        [Test]
        public void GetAccountsByPortfolioId_WithValidInputs_ReturnsAccounts()
        {
            List<AccountState> list = new List<AccountState> { FakeAccount };

            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByPortfolioId(It.IsAny<string>())).Returns(list as object);

            AccountController controller = new AccountController(mock.Object);
            object result = controller.GetAccountsByPortfolioId("1");

            object accounts = null;
            if (result is OkObjectResult)
            {
                accounts = ((OkObjectResult)result).Value;
            }

            Assert.AreEqual(accounts, list as object);
        }


        [Test]
        public void GetAccountsByUserId_InvalidInput_ReturnsBadRequest()
        {
            AccountController controller = new AccountController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.GetAccountsByUserId(0);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public void GetAccountsByUserId_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByUserId(It.IsAny<int>())).Throws(new Exception("i'm you're huckleberry"));

            AccountController controller = new AccountController(mock.Object);

            object result = controller.GetAccountsByUserId(1);
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }

        [Test]
        public void GetAccountsByUserId_WithValidInputs_ReturnsAccounts()
        {
            List<AccountState> list = new List<AccountState> { FakeAccount };

            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetAccountsByUserId(It.IsAny<int>())).Returns(list as object);

            AccountController controller = new AccountController(mock.Object);
            object result = controller.GetAccountsByUserId(1);

            object accounts = null;
            if (result is OkObjectResult)
            {
                accounts = ((OkObjectResult)result).Value;
            }

            Assert.AreEqual(accounts, list as object);
        }

    }
}
