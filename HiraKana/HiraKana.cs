using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace HiraKana
{
    /* This is an entry point, in case someone wants to run an example. */
    static class Example
    {
        static void Main()
        {
            Debug.WriteLine(new Romaji("korehahirakananodesu").ToHiaragana());
        }
    }

    /* Shorthands */
    public class Romaji
    {
        String romaji;

        public Romaji(String romaji)
        {
            this.romaji = romaji;
        }

        public String ToHiaragana()
        {
            return new KanaTools().toHiragana(romaji);
        }

    }

    public class Kana
    {
        String kana;

        public Kana(String kana)
        {
            this.kana = kana;
        }

        public String ToRomaji()
        {
            throw new NotImplementedException();
        }
    }

    /* Conversion class */
    class KanaTools
    {
        // Codes for kana symbols
        private static readonly int UPPERCASE_START = 0x41;
        private static readonly int UPPERCASE_END = 0x5A;
        private static readonly int HIRAGANA_START = 0x3041;
        private static readonly int HIRAGANA_END = 0x3096;
        private static readonly int KATAKANA_START = 0x30A1;
        private static readonly int KATAKANA_END = 0x30FA;

        // Romaji-to-hiragana table
        private static readonly IDictionary<string, string> romajiKana =
            new Dictionary<string, string>
        {
            {"a", "あ"}, {"i", "い"}, {"u", "う"}, {"e", "え"}, {"o", "お"}, {"yi", "い"}, {"wu", "う"},
            {"whu", "う"}, {"xa", "ぁ"}, {"xi", "ぃ"}, {"xu", "ぅ"}, {"xe", "ぇ"}, {"xo", "ぉ"}, {"xyi", "ぃ"},
            {"xye", "ぇ"}, {"ye", "いぇ"}, {"wha", "うぁ"}, {"whi", "うぃ"}, {"whe", "うぇ"}, {"who", "うぉ"},
            {"wi", "うぃ"}, {"we", "うぇ"}, {"va", "ゔぁ"}, {"vi", "ゔぃ"}, {"vu", "ゔ"}, {"ve", "ゔぇ"},
            {"vo", "ゔぉ"}, {"vya", "ゔゃ"}, {"vyi", "ゔぃ"}, {"vyu", "ゔゅ"}, {"vye", "ゔぇ"}, {"vyo", "ゔょ"},
            {"ka", "か"}, {"ki", "き"}, {"ku", "く"}, {"ke", "け"}, {"ko", "こ"}, {"lka", "ヵ"}, {"lke", "ヶ"},
            {"xka", "ヵ"}, {"xke", "ヶ"}, {"kya", "きゃ"}, {"kyi", "きぃ"}, {"kyu", "きゅ"}, {"kye", "きぇ"},
            {"kyo", "きょ"}, {"qya", "くゃ"}, {"qyu", "くゅ"}, {"qyo", "くょ"}, {"qwa", "くぁ"}, {"qwi", "くぃ"},
            {"qwu", "くぅ"}, {"qwe", "くぇ"}, {"qwo", "くぉ"}, {"qa", "くぁ"}, {"qi", "くぃ"}, {"qe", "くぇ"},
            {"qo", "くぉ"}, {"kwa", "くぁ"}, {"qyi", "くぃ"}, {"qye", "くぇ"}, {"ga", "が"}, {"gi", "ぎ"}, {"gu", "ぐ"},
            {"ge", "げ"}, {"go", "ご"}, {"gya", "ぎゃ"}, {"gyi", "ぎぃ"}, {"gyu", "ぎゅ"}, {"gye", "ぎぇ"}, {"gyo", "ぎょ"},
            {"gwa", "ぐぁ"}, {"gwi", "ぐぃ"}, {"gwu", "ぐぅ"}, {"gwe", "ぐぇ"}, {"gwo", "ぐぉ"}, {"sa", "さ"}, {"si", "し"},
            {"shi", "し"}, {"su", "す"}, {"se", "せ"}, {"so", "そ"}, {"za", "ざ"}, {"zi", "じ"}, {"zu", "ず"}, {"ze", "ぜ"},
            {"zo", "ぞ"}, {"ji", "じ"}, {"sya", "しゃ"}, {"syi", "しぃ"}, {"syu", "しゅ"}, {"sye", "しぇ"}, {"syo", "しょ"},
            {"sha", "しゃ"}, {"shu", "しゅ"}, {"she", "しぇ"}, {"sho", "しょ"}, {"swa", "すぁ"}, {"swi", "すぃ"},
            {"swu", "すぅ"}, {"swe", "すぇ"}, {"swo", "すぉ"}, {"zya", "じゃ"}, {"zyi", "じぃ"}, {"zyu", "じゅ"},
            {"zye", "じぇ"}, {"zyo", "じょ"}, {"ja", "じゃ"}, {"ju", "じゅ"}, {"je", "じぇ"}, {"jo", "じょ"}, {"jya", "じゃ"},
            {"jyi", "じぃ"}, {"jyu", "じゅ"}, {"jye", "じぇ"}, {"jyo", "じょ"}, {"ta", "た"}, {"ti", "ち"}, {"tu", "つ"},
            {"te", "て"}, {"to", "と"}, {"chi", "ち"}, {"tsu", "つ"}, {"ltu", "っ"}, {"xtu", "っ"}, {"tya", "ちゃ"},
            {"tyi", "ちぃ"}, {"tyu", "ちゅ"}, {"tye", "ちぇ"}, {"tyo", "ちょ"}, {"cha", "ちゃ"}, {"chu", "ちゅ"},
            {"che", "ちぇ"}, {"cho", "ちょ"}, {"cya", "ちゃ"}, {"cyi", "ちぃ"}, {"cyu", "ちゅ"}, {"cye", "ちぇ"},
            {"cyo", "ちょ"}, {"tsa", "つぁ"}, {"tsi", "つぃ"}, {"tse", "つぇ"}, {"tso", "つぉ"}, {"tha", "てゃ"},
            {"thi", "てぃ"}, {"thu", "てゅ"}, {"the", "てぇ"}, {"tho", "てょ"}, {"twa", "とぁ"}, {"twi", "とぃ"},
            {"twu", "とぅ"}, {"twe", "とぇ"}, {"two", "とぉ"}, {"da", "だ"}, {"di", "ぢ"}, {"du", "づ"}, {"de", "で"},
            {"do", "ど"}, {"dya", "ぢゃ"}, {"dyi", "ぢぃ"}, {"dyu", "ぢゅ"}, {"dye", "ぢぇ"}, {"dyo", "ぢょ"}, {"dha", "でゃ"},
            {"dhi", "でぃ"}, {"dhu", "でゅ"}, {"dhe", "でぇ"}, {"dho", "でょ"}, {"dwa", "どぁ"}, {"dwi", "どぃ"}, {"dwu", "どぅ"},
            {"dwe", "どぇ"}, {"dwo", "どぉ"}, {"na", "な"}, {"ni", "に"}, {"nu", "ぬ"}, {"ne", "ね"}, {"no", "の"},
            {"nya", "にゃ"}, {"nyi", "にぃ"}, {"nyu", "にゅ"}, {"nye", "にぇ"}, {"nyo", "にょ"}, {"ha", "は"}, {"hi", "ひ"},
            {"hu", "ふ"}, {"he", "へ"}, {"ho", "ほ"}, {"fu", "ふ"}, {"hya", "ひゃ"}, {"hyi", "ひぃ"}, {"hyu", "ひゅ"},
            {"hye", "ひぇ"}, {"hyo", "ひょ"}, {"fya", "ふゃ"}, {"fyu", "ふゅ"}, {"fyo", "ふょ"}, {"fwa", "ふぁ"}, {"fwi", "ふぃ"},
            {"fwu", "ふぅ"}, {"fwe", "ふぇ"}, {"fwo", "ふぉ"}, {"fa", "ふぁ"}, {"fi", "ふぃ"}, {"fe", "ふぇ"}, {"fo", "ふぉ"},
            {"fyi", "ふぃ"}, {"fye", "ふぇ"}, {"ba", "ば"}, {"bi", "び"}, {"bu", "ぶ"}, {"be", "べ"}, {"bo", "ぼ"}, {"bya", "びゃ"},
            {"byi", "びぃ"}, {"byu", "びゅ"}, {"bye", "びぇ"}, {"byo", "びょ"}, {"pa", "ぱ"}, {"pi", "ぴ"}, {"pu", "ぷ"}, {"pe", "ぺ"},
            {"po", "ぽ"}, {"pya", "ぴゃ"}, {"pyi", "ぴぃ"}, {"pyu", "ぴゅ"}, {"pye", "ぴぇ"}, {"pyo", "ぴょ"}, {"ma", "ま"},
            {"mi", "み"}, {"mu", "む"}, {"me", "め"}, {"mo", "も"}, {"mya", "みゃ"}, {"myi", "みぃ"}, {"myu", "みゅ"}, {"mye", "みぇ"},
            {"myo", "みょ"}, {"ya", "や"}, {"yu", "ゆ"}, {"yo", "よ"}, {"xya", "ゃ"}, {"xyu", "ゅ"}, {"xyo", "ょ"}, {"ra", "ら"},
            {"ri", "り"}, {"ru", "る"}, {"re", "れ"}, {"ro", "ろ"}, {"rya", "りゃ"}, {"ryi", "りぃ"}, {"ryu", "りゅ"}, {"rye", "りぇ"},
            {"ryo", "りょ"}, {"la", "ら"}, {"li", "り"}, {"lu", "る"}, {"le", "れ"}, {"lo", "ろ"}, {"lya", "りゃ"}, {"lyi", "りぃ"},
            {"lyu", "りゅ"}, {"lye", "りぇ"}, {"lyo", "りょ"}, {"wa", "わ"}, {"wo", "を"}, {"lwe", "ゎ"}, {"xwa", "ゎ"}, {"nn", "ん"},
            {"'n '", "ん"}, {"xn", "ん"}, {"ltsu", "っ"}, {"xtsu", "っ"},
        };

        // Options
        static Boolean IME_MODE = false;
        static Boolean USE_OBSOLETE_KANA = false;

        public KanaTools() { }

        public KanaTools enableIme(Boolean mode)
        {
            IME_MODE = mode;
            return this;
        }

        public KanaTools useObsoleteKana(Boolean flag)
        {
            USE_OBSOLETE_KANA = flag;
            return this;
        }

        /* Character check methods */

        private Boolean isCharInRange(char chr, int start, int end)
        {
            int code = (int)chr;
            return (start <= code && code <= end);
        }

        private Boolean isCharVowel(char chr, Boolean includeY)
        {
            Regex regexp = includeY ? new Regex(@"[aeiouy]") : new Regex(@"[aeiou]");
            return regexp.IsMatch(chr.ToString());
        }

        private Boolean isCharConsonant(char chr, Boolean includeY)
        {
            Regex regexp = includeY ? new Regex(@"[bcdfghjklmnpqrstvwxyz]") : new Regex(@"[bcdfghjklmnpqrstvwxz]");
            return regexp.IsMatch(chr.ToString());
        }

        /* KanaTools character check methods */

        private Boolean isCharHiragana(char chr)
        {
            return isCharInRange(chr, HIRAGANA_START, HIRAGANA_END);
        }

        private Boolean isCharKatakana(char chr)
        {
            return isCharInRange(chr, KATAKANA_START, KATAKANA_END);
        }

        private Boolean isCharKana(char chr)
        {
            return isCharHiragana(chr) || isCharKatakana(chr);
        }

        /* KanaTools methods */

        public Boolean isHiragana(String input)
        {
            return allTrue(input, delegate (String str)
            {
                return isCharHiragana(str[0]);
            });
        }

        public Boolean isKatakana(String input)
        {
            return allTrue(input, delegate (String str)
            {
                return isCharKatakana(str[0]);
            });
        }

        public Boolean isKana(String input)
        {
            return allTrue(input, delegate (String str)
            {
                return (isKatakana(str) || isKatakana(str));
            });
        }

        public Boolean isRomaji(String input)
        {
            return allTrue(input, delegate (String str)
            {
                return (!isKatakana(str) || (!isKatakana(str)));
            });
        }


        /* Utility methods */

        private Boolean allTrue(String stringToCheck, Func<String, Boolean> method)
        {
            for (int i = 0; i < stringToCheck.Length; i++)
            {
                if (!method(Convert.ToString(stringToCheck[i])))
                {
                    return false;
                }
            }
            return true;
        }

        /* Conversions */

        public String toHiragana(String input)
        {
            if (isRomaji(input))
            {
                return romajiToHiragana(input);
            }

            /*
            if (isKatakana(input))
            {
                return katakanaToHiragana(input);
            }
            */

            return input;
        }

        private String romajiToHiragana(String romaji)
        {
            return romajiToKana(romaji, true);
        }

        private String romajiToKana(String romaji, Boolean ignoreCase = true)
        {
            String chunk = "";
            String chunkLC = "";
            int chunkSize;
            int position = 0;
            int len = romaji.Length;
            int maxChunk = 3;
            String kana = "";
            String kanaChar = "";

            while (position < len)
            {
                chunkSize = Math.Min(maxChunk, len - position);

                while (chunkSize > 0)
                {
                    // NB: this is (start, end - start) equivalent to Java's (start, end)
                    chunk = romaji.Substring(position, chunkSize);
                    chunkLC = chunk.ToLower();

                    if ((chunkLC.Equals("lts") || chunkLC.Equals("xts")) && (len - position) >= 4)
                    {
                        chunkSize++;
                        // The second parameter in substring() is an end point, not a length!
                        chunk = romaji.Substring(position, chunkSize);
                        chunkLC = chunk.ToLower();
                    }

                    if (Convert.ToString(chunkLC[0]).Equals("n"))
                    {
                        // Convert n' to ん
                        if (IME_MODE && chunk.Length == 2 && Convert.ToString(chunkLC[1]).Equals("'"))
                        {
                            chunkSize = 2;
                            chunk = "nn";
                            chunkLC = chunk.ToLower();
                        }
                        // If the user types "nto", automatically convert "n" to "ん" first
                        // "y" is excluded from the list of consonants so we can still get にゃ, にゅ, and にょ
                        if (chunk.Length > 2 && isCharConsonant(chunkLC[1], false) && isCharVowel(chunkLC[2], true))
                        {
                            chunkSize = 1;
                            // I removed the "n"->"ん" mapping because the IME wouldn't let me type "na" for "な" without returning "んあ",
                            // so the chunk needs to be manually set to a value that will map to "ん"
                            chunk = "nn";
                            chunkLC = chunk.ToLower();
                        }
                    }

                    // Prepare to return a small-つ because we're looking at double-consonants.
                    if (chunk.Length > 1 && !Convert.ToString(chunkLC[0]).Equals("n")
                        && isCharConsonant(chunkLC[0], true)
                        && chunk[0] == chunk[1])
                    {
                        chunkSize = 1;
                        // Return a small katakana ツ when typing in uppercase
                        if (isCharInRange(chunk[0], UPPERCASE_START, UPPERCASE_END))
                        {
                            chunkLC = chunk = "ッ";
                        }
                        else
                        {
                            chunkLC = chunk = "っ";
                        }
                    }

                    // Try to parse the chunk
                    try {
                        kanaChar = romajiKana[chunkLC];
                    // If could not find key, then try again!
                    } catch(Exception) {
                        kanaChar = null;
                    }

                    // If successfully parsed - continue
                    if (kanaChar != null)
                    {
                        break;
                    }

                    chunkSize--;
                }

                if (kanaChar == null)
                {
                    chunk = convertPunctuation(Convert.ToString(chunk[0]));
                    kanaChar = chunk;
                }

                if (USE_OBSOLETE_KANA)
                {
                    if (chunkLC.Equals("wi"))
                    {
                        kanaChar = "ゐ";
                    }
                    if (chunkLC.Equals("we"))
                    {
                        kanaChar = "ゑ";
                    }
                }

                if (romaji.Length > (position + 1) && IME_MODE && Convert.ToString(chunkLC[0]).Equals("n"))
                {
                    if ((Convert.ToString(romaji[position + 1]).ToLower().Equals("y") && position == (len - 2)) || position == (len - 1))
                    {
                        kanaChar = Convert.ToString(chunk[0]);
                    }
                }

                /*
                if (!ignoreCase)
                {
                    if (isCharInRange(chunk[0], UPPERCASE_START, UPPERCASE_END))
                    {
                        kanaChar = hiraganaToKatakana(kanaChar);
                    }
                }
                */

                kana += kanaChar;

                position += chunkSize > 0 ? chunkSize : 1;
            }

            return kana;
        }


        // Convert punctuations: long space and dash
        private String convertPunctuation(String input)
        {
            if (input.Equals(Convert.ToString(('　'))))
            {
                return Convert.ToString(' ');
            }

            if (input.Equals(Convert.ToString('-')))
            {
                return Convert.ToString('ー');
            }

            return input;
        }

    }
}

