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
                new Romaji("monono").ToHiragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("kyokutannazankyou").ToHiragana(),
                "きょくたんなざんきょう");
        }

        [TestMethod]
        public void ConvertTrickyStrings()
        {
            Assert.AreEqual(
                new Romaji("teppennowebbutyotto").ToHiragana(),
                "てっぺんのうぇっぶちょっと");

            Assert.AreEqual(
                new Romaji("tunaototsunadiji").ToHiragana(),
                "つなおとつなぢじ");

            Assert.AreEqual(
                new Romaji("bizinessu").ToHiragana(),
                "びじねっす");

            Assert.AreEqual(
                new Romaji("pyottonn").ToHiragana(),
                "ぴょっとん");
        }

        [TestMethod]
        public void TestOptions()
        {
            Assert.AreEqual(
                new Romaji("wiwewo").ToHiragana(),
                "うぃうぇを"
            );

            Assert.AreEqual(
                new Romaji("wiwewo").useObsoleteKana(true).ToHiragana(),
                "ゐゑを"
            );
        }
    }
}
