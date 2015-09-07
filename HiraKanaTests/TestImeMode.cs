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
                new Romaji("monono").UseImeMode(true).ToHiragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("もnono").UseImeMode(true).ToHiragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("もnoの").UseImeMode(true).ToHiragana(),
                "ものの");

            Assert.AreEqual(
                new Romaji("kyokuたんなzankyou").UseImeMode(true).ToKatakana(),
                "キョクタンナザンキョウ" );
        }
    }
}
