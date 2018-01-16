using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;
using Moq;

using DemoService.Data;
using DemoService.Exceptions;
using DemoService.Models;

namespace DemoService.Tests.Data
{
    [TestFixture]
    public class CouchbaseProcessorTests
    {
        private readonly string FakeUsername = "InigoMontoya";
        private readonly string FakePortfolioName = "Portfolio!";
        private readonly int FakeAccountId = 8675309;

        private List<dynamic> FakePortfolioList
        {
            get
            {
                dynamic d = new PortfolioState
                {
                    AccountCount = 21,
                    AsOfDate = DateTime.Now,
                    ID = 1,
                    Name = FakePortfolioName,
                    TotalBalance = 100.0M
                };

                return new List<dynamic> { d };
            }
        }

        private List<dynamic> FakeAccountList
        {
            get
            {
                dynamic d = new AccountState
                {
                    AccountStatus = AccountStatus.Active,
                    AsOfDate = DateTime.Now,
                    CurrentBalance = 100.0M,
                    ID = FakeAccountId,
                    PortfolioID = 1
                };

                return new List<dynamic> { d };
            }
        }

        private List<dynamic> FakeUserList
        {
            get
            {
                dynamic d = new User {
                    ID = 0,
                    LastLogin = DateTime.Now,
                    Password = "password",
                    Username = FakeUsername
                };

                return new List<dynamic> { d };
            }
        }

        private List<dynamic> FakeEmptyUserList
        {
            get { return new List<dynamic>(); }
        }

        [Test]
        public void Populate_EncountersDataClientException_ThrowsException()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.Upsert(It.IsAny<string>(), It.IsAny<object>())).Throws(new Exception("failure"));

            bool result = false;
            try
            {
                CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
                string response = proc.Populate(1, 1, 1);
            }
            catch (CouchbaseException)
            {
                result = true;
            }
            catch
            {
            }

            Assert.IsTrue(result);
        }

        [Test]
        public void Populate_WithValidInputs_CallsUpsertTheRightNumberOfTimes()
        {
            int portfolioCount = 5;
            int accountCount = 2;
            int userCount = 4;

            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.Upsert(It.IsAny<string>(), It.IsAny<object>()));

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            string response = proc.Populate(portfolioCount, accountCount, userCount);

            mock.Verify(m => m.Upsert(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(portfolioCount + accountCount + userCount + 1));  // + 1 because we add admin user
        }


        [Test]
        public void GetPortfolios_WithValidInputs_ReturnsPortfolioList()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakePortfolioList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            object result = proc.GetPortfolios();

            string name = string.Empty;
            try
            {
                name = ((PortfolioState)((List<dynamic>)result)[0]).Name;
            }
            catch
            {
            }

            Assert.AreEqual(name, FakePortfolioName);
        }

        [Test]
        public void GetPortfoliosByAggregate_WithValidInputs_ReturnsPortfolioList()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakePortfolioList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            object result = proc.GetPortfoliosByAggregate();

            string name = string.Empty;
            try
            {
                name = ((PortfolioState)((List<dynamic>)result)[0]).Name;
            }
            catch
            {
            }

            Assert.AreEqual(name, FakePortfolioName);
        }

        [Test]
        public void GetAccountsByPortfolioId_WithInvalidPortfolioId_ThrowsArgumentException()
        {
            CouchbaseProcessor proc = new CouchbaseProcessor(new CouchbaseDataClient());
            bool result = false;

            try
            {
                object tmp = proc.GetAccountsByPortfolioId(null);
            }
            catch (ArgumentException)
            {
                result = true;
            }
            catch
            {
                result = false;
            }

            Assert.IsTrue(result);
        } 

        [Test]
        public void GetAccountsByPortfolioId_WithValidInputs_ReturnsPortfolioList()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakeAccountList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            object result = proc.GetAccountsByPortfolioId("1");

            int id = 0;
            try
            {
                id = ((AccountState)((List<dynamic>)result)[0]).ID;
            }
            catch
            {
            }

            Assert.AreEqual(id, FakeAccountId);
        }

        [Test]
        public void GetAccountsByUserId_WithValidInputs_ReturnsPortfolioList()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakeAccountList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            object result = proc.GetAccountsByUserId(1);

            int id = 0;
            try
            {
                id = ((AccountState)((List<dynamic>)result)[0]).ID;
            }
            catch
            {
            }

            Assert.AreEqual(id, FakeAccountId);
        }

        [Test]
        public void AuthenticateUser_WithInvalidUsername_ThrowsArgumentException()
        {
            CouchbaseProcessor proc = new CouchbaseProcessor(new CouchbaseDataClient());
            bool result = false;

            try
            {
                object tmp = proc.AuthenticateUser(null, "password");
            }
            catch (ArgumentException)
            {
                result = true;
            }
            catch
            {
                result = false;
            }

            Assert.IsTrue(result);
        }

        [Test]
        public void AuthenticateUser_WithInvalidPassword_ThrowsArgumentException()
        {
            CouchbaseProcessor proc = new CouchbaseProcessor(new CouchbaseDataClient());
            bool result = false;

            try
            {
                object tmp = proc.AuthenticateUser("username", null);
            }
            catch (ArgumentException)
            {
                result = true;
            }
            catch
            {
                result = false;
            }

            Assert.IsTrue(result);
        }

        [Test]
        public void AuthenticateUser_WithValidInputsAndExistingUser_ReturnsUserDetails()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakeUserList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            object result = proc.AuthenticateUser(FakeUsername, "asecurepassword");

            string username = string.Empty;
            try
            {
                username = ((User)result).Username;
            }
            catch
            {
            }

            Assert.AreEqual(username, FakeUsername);
        }

        [Test]
        public void AuthenticateUser_WithValidInputsAndNoUser_ThrowsCouchbaseException()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakeEmptyUserList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);

            bool result = false;
            try
            {
                object tmp = proc.AuthenticateUser(FakeUsername, "asecurepassword");
            }
            catch (CouchbaseException)
            {
                result = true;
            }
            catch
            {
                result = false;
            }

            Assert.IsTrue(result);
        }
    }
}
