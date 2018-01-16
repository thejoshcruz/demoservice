using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Authentication;

using DemoService.Exceptions;

namespace DemoService.Configuration
{
    /// <summary>
    /// configuration of the couchbase client; offers a singleton instance
    /// </summary>
    public class CouchbaseConfig
    {
        /// <summary>
        /// the uris to the couchbase server
        /// </summary>
        private List<Uri> ServerUris { get; set; }

        /// <summary>
        /// the couchbase buckets to configure with the ClusterHelper
        /// </summary>
        private List<string> Buckets { get; set; }

        /// <summary>
        /// username to access couchbase
        /// </summary>
        private string Username { get; set; }

        /// <summary>
        /// password to access couchbase
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// the couchbase cluster instance
        /// </summary>
        private Cluster Cluster { get; set; }


        /// <summary>
        /// the name of the account bucket
        /// </summary>
        public string AccountBucketName { get; set; }

        /// <summary>
        /// the name of the portfolio bucket
        /// </summary>
        public string PortfolioBucketName { get; set; }

        /// <summary>
        /// the name of the user bucket
        /// </summary>
        public string UserBucketName { get; set; }

        /// <summary>
        /// initialize the couchbase configuration values (for use when processing actions)
        /// </summary>
        public void Initialize()
        {
            ParseEnvironmentVariables();
            VerifyEnvironmentVariables();

            ClientConfiguration config = new ClientConfiguration();
            config.BucketConfigs.Clear();
            config.Servers = ServerUris;
            
            // add all the buckets to the config
            Buckets.ForEach(bucket => config.BucketConfigs.Add(bucket, new BucketConfiguration { BucketName = bucket, Username = Username, Password = Password }));

            // set up cluster
            Cluster = new Cluster(config);

            PasswordAuthenticator authenticator = new PasswordAuthenticator(Username, Password);
            Cluster.Authenticate(authenticator);

            ClusterHelper.Initialize(Cluster.Configuration);
        }

        /// <summary>
        /// close the ClusterHelper
        /// </summary>
        public void Close()
        {
            if (Cluster != null)
            {
                Cluster.Dispose();
            }

            ClusterHelper.Close();
        }

        /// <summary>
        /// parse and import the env variables into the config for use while processing
        /// </summary>
        private void ParseEnvironmentVariables()
        {
            // get the couchbase details from the environment vars passed in
            Username = Environment.GetEnvironmentVariable("COUCHBASE_USER");
            Password = Environment.GetEnvironmentVariable("COUCHBASE_PWD");
            
            // get servers from the env variables
            string servers = Environment.GetEnvironmentVariable("COUCHBASE_SERVERS");
            if (!String.IsNullOrEmpty(servers))
            {
                string[] list = servers.Split(";", StringSplitOptions.RemoveEmptyEntries);
                List<Uri> uris = new List<Uri>();
                foreach (string uri in list)
                {
                    string tmp = "http://" + uri;
                    uris.Add(new Uri(tmp));
                }
                ServerUris = uris;
            }

            // get the buckets to configure as well
            PortfolioBucketName = Environment.GetEnvironmentVariable("COUCHBASE_PORTFOLIO");
            AccountBucketName = Environment.GetEnvironmentVariable("COUCHBASE_ACCOUNT");
            UserBucketName = Environment.GetEnvironmentVariable("COUCHBASE_USERS");

            List<string> buckets = new List<string> { PortfolioBucketName, AccountBucketName, UserBucketName };
            Buckets = buckets;

            // debug. prove to me you're working
            Console.WriteLine($"Starting: {servers} : {Username} : {Password}");
            Console.WriteLine($"Buckets: {PortfolioBucketName}, {AccountBucketName}, {UserBucketName}");
        }

        private void VerifyEnvironmentVariables()
        {
            if (String.IsNullOrEmpty(Username))
            {
                throw new ConfigurationException("invalid username");
            }

            if (String.IsNullOrEmpty(Password))
            {
                throw new ConfigurationException("invalid password");
            }

            if (Buckets == null
                || Buckets.Count == 0
                || !Buckets.Exists(bucket => !String.IsNullOrEmpty(bucket)))
            {
                throw new ConfigurationException("bucket list cannot be null or empty");
            }

            if (String.IsNullOrEmpty(PortfolioBucketName))
            {
                throw new ConfigurationException("invalid portfolio bucket name");
            }

            if (String.IsNullOrEmpty(AccountBucketName))
            {
                throw new ConfigurationException("invalid account bucket name");
            }

            if (String.IsNullOrEmpty(UserBucketName))
            {
                throw new ConfigurationException("invalid user bucket name");
            }

            if (ServerUris == null || ServerUris.Count == 0)
            {
                throw new ConfigurationException("server list cannot be null or empty");
            }
        }
    }
}
