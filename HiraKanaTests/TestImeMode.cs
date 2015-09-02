using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiraKana;

namespace HiraKanaTests
{
    [TestClass]
    public class TestImeMode
    {
        [TestMethod]
        public void ConvertOnTheFly()
        {
            Assert.AreEqual(
                new Romaji("monono").useImeMode(true).ToHiragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("もnono").useImeMode(true).ToHiragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("もnoの").useImeMode(true).ToHiragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("kyokuたんなzankyou").useImeMode(true).ToKatakana(),
                "キョクタンナザンキョウ" );
        }
    }
}
