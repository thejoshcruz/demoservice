using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using DemoService.Configuration;
using DemoService.Exceptions;

namespace DemoService.Tests.Configuration
{
    [TestFixture]
    public class CouchbaseConfigTests
    {
        private readonly string FAKEPORTFOLIO = "PortfolioState";
        private readonly string FAKEACCOUNT = "AccountState";
        private readonly string FAKEUSERS = "Users";

        [SetUp]
        public void SetTestEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_USER", "username");
            Environment.SetEnvironmentVariable("COUCHBASE_PWD", "password");
            Environment.SetEnvironmentVariable("COUCHBASE_SERVERS", "server01;server02");
            Environment.SetEnvironmentVariable("COUCHBASE_PORTFOLIO", FAKEPORTFOLIO);
            Environment.SetEnvironmentVariable("COUCHBASE_ACCOUNT", FAKEACCOUNT);
            Environment.SetEnvironmentVariable("COUCHBASE_USERS", FAKEUSERS);
        }

        [Test]
        public void Initialize_WithInvalidUsername_ThrowsConfigurationException()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_USER", string.Empty);
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();
            }
            catch (ConfigurationException)
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
        public void Initialize_WithInvalidPassword_ThrowsConfigurationException()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_PWD", string.Empty);
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();
            }
            catch (ConfigurationException)
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
        public void Initialize_WithInvalidServerList_ThrowsConfigurationException()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_SERVERS", string.Empty);
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();
            }
            catch (ConfigurationException)
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
        public void Initialize_WithInvalidPortfolioName_ThrowsConfigurationException()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_PORTFOLIO", string.Empty);
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();
            }
            catch (ConfigurationException)
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
        public void Initialize_WithInvalidAccountName_ThrowsConfigurationException()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_ACCOUNT", string.Empty);
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();
            }
            catch (ConfigurationException)
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
        public void Initialize_WithInvalidUsersBucket_ThrowsConfigurationException()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_USERS", string.Empty);
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();
            }
            catch (ConfigurationException)
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
        public void Initialize_WithAllInvalidBuckets_ThrowsConfigurationException()
        {
            Environment.SetEnvironmentVariable("COUCHBASE_PORTFOLIO", string.Empty);
            Environment.SetEnvironmentVariable("COUCHBASE_ACCOUNT", string.Empty);
            Environment.SetEnvironmentVariable("COUCHBASE_USERS", string.Empty);
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();
            }
            catch (ConfigurationException)
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
        public void Initialize_WithValidInputs_ConfiguresCorrectly()
        {
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();

                result = config.AccountBucketName == FAKEACCOUNT
                    && config.PortfolioBucketName == FAKEPORTFOLIO
                    && config.UserBucketName == FAKEUSERS;
            }
            catch
            {
                result = false;
            }

            Assert.IsTrue(result);
        }

        [Test]
        public void Initialize_WithValidInputs_InitializesClusterHelper()
        {
            CouchbaseConfig config = new CouchbaseConfig();

            bool result = false;
            try
            {
                config.Initialize();

                result = Couchbase.ClusterHelper.Initialized;
            }
            catch
            {
                result = false;
            }

            Assert.IsTrue(result);
        }
    }
}
