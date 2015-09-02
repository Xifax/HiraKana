using System;
using System.Diagnostics;

namespace HiraKana
{
    /* This is an entry point, in case someone wants to run an example. */
    static class Example
    {
        static void Main()
        {
            Debug.WriteLine(new Romaji("korehahirakananodesu").ToHiragana());
            Debug.WriteLine(new Romaji("wawiwo").useObsoleteKana(true).ToKatakana());
            Debug.WriteLine(new KanaTools().toHiragana("sorehaZOMUBIEdesuka"));
            Debug.WriteLine(new KanaTools().isKana("あいうえお"));
        }
    }

    /* Romaji methods */
    public class Romaji
    {
        String romaji;
        KanaTools tools;

        /* Constructor and settings */
        public Romaji(String romaji)
        {
            this.romaji = romaji;
            this.tools = new KanaTools();
        }

        public Romaji useImeMode(Boolean mode)
        {
            tools.useIme(mode);
            return this;
        }

        public Romaji useObsoleteKana(Boolean mode)
        {
            tools.useObsoleteKana(mode);
            return this;
        }

        /* Conversions */
        public String ToHiragana()
        {
            return tools.toHiragana(romaji);
        }

        public String ToKatakana()
        {
            return tools.toKatakana(romaji);
        }

    }

    /* Kana methods */
    public class Kana
    {
        String kana;
        KanaTools tools;

        /* Constructor and settings */
        public Kana(String kana)
        {
            this.kana = kana;
            tools = new KanaTools();
        }
        

        /* Conversions */
        public String ToRomaji()
        {
            return tools.toRomaji(kana);
        }

        public String ToHiragana()
        {
            return tools.toHiragana(kana);
        }

        public String ToKatakana()
        {
            return tools.toKatakana(kana);
        }
    }
}
