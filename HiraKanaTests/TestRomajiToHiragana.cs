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
            Assert.AreEqual(
                new Romaji("monono").ToHiaragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("kyokutannazankyou").ToHiaragana(),
                "きょくたんなざんきょう");
        }

        [TestMethod]
        public void ConvertTrickyStrings()
        {
            Assert.AreEqual(
                new Romaji("teppennowebbutyotto").ToHiaragana(),
                "てっぺんのうぇっぶちょっと");

            Assert.AreEqual(
                new Romaji("tunaototsunadiji").ToHiaragana(),
                "つなおとつなぢじ");

            Assert.AreEqual(
                new Romaji("bizinessu").ToHiaragana(),
                "びじねっす");

            Assert.AreEqual(
                new Romaji("pyottonn").ToHiaragana(),
                "ぴょっとん");
        }
    }
}
