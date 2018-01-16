using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;
using Moq;

using DemoService.Controllers;
using DemoService.Data;
using DemoService.Exceptions;
using DemoService.Models;

namespace DemoService.Tests.Controllers
{
    [TestFixture]
    public class PortfolioControllerTests : BaseTests
    {
        [Test]
        public void GetPortfolios_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetPortfolios()).Throws(new Exception("I'm Henry the Eighth, I am. Henry the Eighth, I am, I am"));

            PortfolioController controller = new PortfolioController(mock.Object);

            object result = controller.GetPortfolios();
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }

        [Test]
        public void GetPortfolios_WithValidInputs_ReturnsPortfolios()
        {
            PortfolioState portfolio = new PortfolioState
            {
                AccountCount = 2,
                AsOfDate = DateTime.Now,
                ID = 1,
                Name = "Henry!",
                TotalBalance = 100.0M
            };
            List<PortfolioState> list = new List<PortfolioState> { portfolio };

            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetPortfolios()).Returns(list as object);

            PortfolioController controller = new PortfolioController(mock.Object);
            object result = controller.GetPortfolios();
            object portfolios = null;
            if (result is OkObjectResult)
            {
                portfolios = ((OkObjectResult)result).Value;
            }

            Assert.AreEqual(portfolios, list as object);
        }

        [Test]
        public void GetPortfoliosByAggregate_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetPortfoliosByAggregate()).Throws(new Exception("You keep using that word. I do not think it means what you think it means."));

            PortfolioController controller = new PortfolioController(mock.Object);

            object result = controller.GetPortfoliosByAggregate();
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }

        [Test]
        public void GetPortfoliosByAggregate_WithValidInputs_ReturnsPortfolios()
        {
            PortfolioState portfolio = new PortfolioState
            {
                AccountCount = 2,
                AsOfDate = DateTime.Now,
                ID = 1,
                Name = "Inigo Montoya",
                TotalBalance = 100.0M
            };
            List<PortfolioState> list = new List<PortfolioState> { portfolio };

            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.GetPortfoliosByAggregate()).Returns(list as object);

            PortfolioController controller = new PortfolioController(mock.Object);
            object result = controller.GetPortfoliosByAggregate();
            object portfolios = null;
            if (result is OkObjectResult)
            {
                portfolios = ((OkObjectResult)result).Value;
            }

            Assert.AreEqual(portfolios, list as object);
        }
    }
}
