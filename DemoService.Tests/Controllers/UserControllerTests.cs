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
    public class UserControllerTests : BaseTests
    {
        [Test]
        public void Authenticate_WithInvalidUsername_ReturnsBadRequest()
        {
            UserController controller = new UserController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.Authenticate(string.Empty, "not empty");

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public void Authenticate_WithInvalidPassword_ReturnsBadRequest()
        {
            UserController controller = new UserController(new CouchbaseProcessor(new CouchbaseDataClient()));

            object result = controller.Authenticate("not empty", string.Empty);

            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }


        [Test]
        public void Authenticate_EncountersException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("dogs and cats, living together!"));

            UserController controller = new UserController(mock.Object);

            object result = controller.Authenticate("user", "pwd");
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.Unknown);
        }

        [Test]
        public void Authenticate_EncountersCouchbaseException_ReturnsBadRequest()
        {
            Mock<IDataProcessor> mock = new Mock<IDataProcessor>();
            mock.Setup(m => m.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new CouchbaseException("You keep using that word. I do not think it means what you think it means."));

            UserController controller = new UserController(mock.Object);

            object result = controller.Authenticate("user", "pwd");
            int code = ParseBadRequestForErrorCode(result);

            Assert.AreEqual(code, (int)ErrorCodes.CouchbaseProcessing);
        }
    }
}
