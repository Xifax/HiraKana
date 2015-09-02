using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiraKana;

namespace HiraKanaTests
{
    [TestClass]
    public class TestRomajiToHiragana
    {
        [TestMethod]
        public void ConvertSimpleStrings()
        {
            String hiragana = new Romaji("monono").ToHiaragana();
            Assert.AreEqual(hiragana, "ものの");
        }
    }
}
