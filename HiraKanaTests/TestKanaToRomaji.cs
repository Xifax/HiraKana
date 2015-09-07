using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiraKana;

namespace HiraKanaTests
{
    [TestClass]
    public class TestKanaToRomaji
    {
        [TestMethod]
        public void ConverHiraganaToRomaji()
        {
            Assert.AreEqual(
                new Kana("きょくたんなざんきょう").ToRomaji(),
                "kyokutannazankyou" );

        }

        [TestMethod]
        public void ConverKatakanaToRomaji()
        {
            Assert.AreEqual(
                new Kana("キョクタンナザンキョウ").ToRomaji(),
                "kyokutannazankyou" );
        }

    }
}
