﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;

namespace MyMemory.Tests
{
    [TestClass]
    public class ParseWords : BaseTest
    {
        [TestMethod]
        public void ParseWordsStart()
        {
            var t = MemoryDBInitializer.ReadResource("MyMemory.Domain.Data.EnglishVerbs.txt");
        }
    }
}