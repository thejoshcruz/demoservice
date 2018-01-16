using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Moq;

using DemoService.Controllers;
using DemoService.Data;

namespace DemoService.Tests.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        [Test]
        public void Constructor_WithDataProcessorInput_SetsDataProcessor()
        {
            CouchbaseProcessor proc = new CouchbaseProcessor(new CouchbaseDataClient());
            BaseController controller = new BaseController(proc);

            Assert.AreEqual(controller.DataProcessor, proc);
        }

    }
}
