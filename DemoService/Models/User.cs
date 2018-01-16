using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Models
{
    /// <summary>
    /// authorized user info
    /// </summary>
    public class User
    {
        /// <summary>
        /// identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// user name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// date and time of last successful login
        /// </summary>
        public DateTime LastLogin { get; set; }
    }
}
