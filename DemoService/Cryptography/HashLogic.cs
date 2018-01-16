using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DemoService.Cryptography
{
    internal class HashLogic
    {
        internal byte[] ComputeHash(string toHash)
        {
            byte[] hashed = null;

            using (SHA512 hasher = SHA512.Create())
            {
                hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(toHash));
            }

            return hashed;
        }
    }
}
