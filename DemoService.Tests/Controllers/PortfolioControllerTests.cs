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
                Number = "1",
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
        
    }
}
