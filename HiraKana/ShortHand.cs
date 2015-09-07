using System.Diagnostics;

namespace HiraKana
{
    /* This is an entry point, in case someone wants to run an example. */

    internal static class Example
    {
        private static void Main()
        {
            Debug.WriteLine(new Romaji("korehahirakananodesu").ToHiragana());
            Debug.WriteLine(new Romaji("wawiwewo").UseObsoleteKana(true).ToKatakana());
            Debug.WriteLine(new KanaTools().ToHiragana("sorehaZOMUBIEdesuka"));
            Debug.WriteLine(new KanaTools().IsKana("あいうえお"));
        }
    }

    /* Romaji methods */

    public class Romaji
    {
        private bool _ime;
        private readonly string _romaji;
        private readonly KanaTools _tools;

        /* Constructor and settings */

        public Romaji(string romaji)
        {
            _romaji = romaji;
            _tools = new KanaTools();
        }

        public Romaji UseImeMode(bool mode)
        {
            _tools.UseIme(mode);
            _ime = true;
            return this;
        }

        public Romaji UseObsoleteKana(bool mode)
        {
            _tools.UseObsoleteKana(mode);
            return this;
        }

        /* Conversions */

        public string ToHiragana()
        {
            return _ime ? _tools.OnTheFlyToKana(_romaji) : _tools.ToHiragana(_romaji);
        }

        public string ToKatakana()
        {
            return _ime ? _tools.OnTheFlyToKana(_romaji, katakana: true) : _tools.ToKatakana(_romaji);
        }
    }

    /* Kana methods */

    public class Kana
    {
        private readonly string _kana;
        private readonly KanaTools _tools;

        /* Constructor and settings */

        public Kana(string kana)
        {
            _kana = kana;
            _tools = new KanaTools();
        }


        /* Conversions */

        public string ToRomaji()
        {
            return _tools.ToRomaji(_kana);
        }

        public string ToHiragana()
        {
            return _tools.ToHiragana(_kana);
        }

        public string ToKatakana()
        {
            return _tools.ToKatakana(_kana);
        }
    }
}