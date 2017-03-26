﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;
using System.Text;
using System.Security.Cryptography;

namespace MyMemory.Tests
{
    [TestClass]
    public class FastTest : BaseTest
    {
        [TestMethod]
        public void TestMD5()
        {
            byte[] hash = Encoding.ASCII.GetBytes("194253766748fTanppCrNSeuYPbA4ENCo");
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
            {
                result += b.ToString("x2");
            }
            
        }
    }
}
