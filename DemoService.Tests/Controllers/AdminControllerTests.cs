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
    public class AdminControllerTests : BaseTests
    {

        [Test]
        public void Ping_WithValidInput_ReturnsCorrectEcho()
        {
            AdminController controller = new AdminController(new CouchbaseProcessor(new CouchbaseDataClient()));

            string echo = "HAHAHAHAHAHA";
            string result = controller.Ping(echo);

            Assert.IsTrue(result.Contains(echo));
        }

        [Test]
        public void Populate_WithInvalidInputs_ReturnsBadRequest()
        {
            AdminController controller = new AdminController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.Populate(0, 0, 0);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public void Populate_WithInvalidAccountInput_ReturnsBadRequest()
        {
            AdminController controller = new AdminController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.Populate(12, 0, 12);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }


        [Test]
        public void Populate_WithInvalidUsersInput_ReturnsBadRequest()
        {
            AdminController controller = new AdminController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.Populate(12, 12, 0);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public void Populate_WithInvalidPortfolioInput_ReturnsBadRequest()
        {
            AdminController controller = new AdminController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.Populate(0, 12, 12);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public void Populate_WithValidInputs_ReturnsSuccess()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.Populate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns("done");

            AdminController controller = new AdminController(mock.Object);

            object result = controller.Populate(3, 4, 3);
            string tmp = "failed";
            if (result is OkObjectResult)
            {
                tmp = ((OkObjectResult)result).Value.ToString();
            }

            Assert.IsTrue(tmp == "done");
        }

        [Test]
        public void Populate_EncountersCouchbaseException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.Populate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new CouchbaseException("dogs and cats"));

            AdminController controller = new AdminController(mock.Object);
            object result = controller.Populate(3, 4, 3);
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }

        [Test]
        public void Populate_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.Populate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("hahahahaha"));

            AdminController controller = new AdminController(mock.Object);
            object result = controller.Populate(3, 4, 3);
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.Unknown);
        }
    }

}
