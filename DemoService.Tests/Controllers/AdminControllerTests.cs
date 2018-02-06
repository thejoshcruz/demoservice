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

    }

}
