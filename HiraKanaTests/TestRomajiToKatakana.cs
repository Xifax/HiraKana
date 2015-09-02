using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiraKana;

namespace HiraKanaTests
{
    [TestClass]
    public class TestRomajiToKana
    {
        [TestMethod]
        public void ConvertSimpleStringsToKatakana()
        {
            Assert.AreEqual(
                new Romaji("monono").ToKatakana(),
                "モノノ");

            Assert.AreEqual(
                new Romaji("kyokutannazankyou").ToKatakana(),
                "キョクタンナザンキョウ");
        }

        [TestMethod]
        public void ConvertTrickyStringsToKatakana()
        {
            Assert.AreEqual(
                new Romaji("teppennowebbutyotto").ToKatakana(),
                "テッペンノウェッブチョット");

            Assert.AreEqual(
                new Romaji("tunaototsunadiji").ToKatakana(),
                "ツナオトツナヂジ");

            Assert.AreEqual(
                new Romaji("bizinessu").ToKatakana(),
                "ビジネッス");

            Assert.AreEqual(
                new Romaji("pyottonn").ToKatakana(),
                "ピョットン");
        }

    }
}
