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
        private readonly string FakeAccountNumber = "8675309";

        private List<dynamic> FakePortfolioList
        {
            get
            {
                dynamic d = new PortfolioState
                {
                    AccountCount = 21,
                    AsOfDate = DateTime.Now,
                    Number = "1",
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
                    AccountNumber = FakeAccountNumber,
                    PortfolioNumber = "Portfolio01",
                    AccountInventory = "Inventory01"
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
        public void GetAccountsByPortfolioNumber_WithInvalidPortfolioNumber_ThrowsArgumentException()
        {
            CouchbaseProcessor proc = new CouchbaseProcessor(new CouchbaseDataClient());
            bool result = false;

            try
            {
                object tmp = proc.GetAccountsByPortfolioNumber(null);
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
        public void GetAccountsByPortfolioNumber_WithValidInputs_ReturnsPortfolioList()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakeAccountList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            object result = proc.GetAccountsByPortfolioNumber("1");

            string accountNumber = string.Empty;
            try
            {
                accountNumber = ((AccountState)((List<dynamic>)result)[0]).AccountNumber;
            }
            catch
            {
            }

            Assert.AreEqual(accountNumber, FakeAccountNumber);
        }

        [Test]
        public void GetAccountsByUsername_WithValidInputs_ReturnsPortfolioList()
        {
            Mock<IDataClient> mock = new Mock<IDataClient>();
            mock.Setup(m => m.ExecuteQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(FakeAccountList);

            CouchbaseProcessor proc = new CouchbaseProcessor(mock.Object);
            object result = proc.GetAccountsByUsername("user1");

            string accountNumber = string.Empty;
            try
            {
                accountNumber = ((AccountState)((List<dynamic>)result)[0]).AccountNumber;
            }
            catch
            {
            }

            Assert.AreEqual(accountNumber, FakeAccountNumber);
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
