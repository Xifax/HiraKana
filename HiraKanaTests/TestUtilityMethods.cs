using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiraKana;

namespace HiraKanaTests
{
    [TestClass]
    public class TestUtilityMethods
    {
        [TestMethod]
        public void TestBooleanChecks()
        {
            var tools = new KanaTools();
            Assert.IsTrue(tools.IsHiragana("ものの"));
            Assert.IsFalse(tools.IsHiragana("キョクタン"));
            Assert.IsFalse(tools.IsHiragana("romaji"));
            Assert.IsTrue(tools.IsKatakana("キョクタン"));
            Assert.IsTrue(tools.IsKana("キョクタンものの"));
            Assert.IsTrue(tools.IsRomaji("romaji"));
        }
    }
}
