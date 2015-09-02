using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
