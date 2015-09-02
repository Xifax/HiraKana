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
            KanaTools tools = new KanaTools();
            Assert.IsTrue(tools.isHiragana("ものの"));
            Assert.IsFalse(tools.isHiragana("キョクタン"));
            Assert.IsFalse(tools.isHiragana("romaji"));
            Assert.IsTrue(tools.isKatakana("キョクタン"));
            Assert.IsTrue(tools.isKana("キョクタンものの"));
            Assert.IsTrue(tools.isRomaji("romaji"));

        }
    }
}
