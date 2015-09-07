using System;
using System.Linq;
using System.Text.RegularExpressions;
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantAssignment

namespace HiraKana
{
    /* Conversion class */
    public class KanaTools
    {
        // Codes for kana symbols
        private const int UppercaseStart = 0x41;
        private const int UppercaseEnd = 0x5A;
        private const int HiraganaStart = 0x3041;
        private const int HiraganaEnd = 0x3096;
        private const int KatakanaStart = 0x30A1;
        private const int KatakanaEnd = 0x30FA;

        // Options
        private bool _imeMode;
        private bool _useObsoleteKana;

        public KanaTools() { }

        /* Options */

        public KanaTools UseIme(bool mode)
        {
            _imeMode = mode;
            return this;
        }

        public KanaTools UseObsoleteKana(bool flag)
        {
            _useObsoleteKana = flag;
            return this;
        }

        /* Public API */
        public string OnTheFlyToKana(string input, bool hiragana = true, bool katakana = false)
        {
            var result = RomajiToHiragana(input);
            return katakana ? HiraganaToKatakana(result) : result;
        }

        public string ToHiragana(string input)
        {
            if (IsRomaji(input))
            {
                return RomajiToHiragana(input);
            }

            return IsKatakana(input) ? KatakanaToHiragana(input) : input;
        }

        public string ToKatakana(string input)
        {
            if (IsHiragana(input))
            {
                return HiraganaToKatakana(input);
            }

            return IsRomaji(input) ? HiraganaToKatakana(RomajiToHiragana(input)) : input;
        }

        public string ToRomaji(String input)
        {
            return HiraganaToRomaji(input);
        }

        public string ToKana(string input)
        {
            return RomajiToKana(input, false);
        }


        public bool IsHiragana(string input)
        {
            return AllTrue(input, str => IsCharHiragana(str[0]));
        }

        public bool IsKatakana(string input)
        {
            return AllTrue(input, str => IsCharKatakana(str[0]));
        }

        public bool IsKana(string input)
        {
            return AllTrue(input, str => (IsKatakana(str) || IsHiragana(str)));
        }

        public bool IsRomaji(string input)
        {
            return AllTrue(input, str => (!IsKatakana(str) && !IsHiragana(str)));
        }



        /* Character check methods */

        private static bool IsCharInRange(char chr, int start, int end)
        {
            var code = (int)chr;
            return (start <= code && code <= end);
        }

        private static bool IsCharVowel(char chr, bool includeY)
        {
            var regexp = includeY ? new Regex(@"[aeiouy]") : new Regex(@"[aeiou]");
            return regexp.IsMatch(chr.ToString());
        }

        private static bool IsCharConsonant(char chr, bool includeY)
        {
            var regexp = includeY ? new Regex(@"[bcdfghjklmnpqrstvwxyz]") : new Regex(@"[bcdfghjklmnpqrstvwxz]");
            return regexp.IsMatch(chr.ToString());
        }

        /* KanaTools character check methods */

        private static bool IsCharHiragana(char chr)
        {
            return IsCharInRange(chr, HiraganaStart, HiraganaEnd);
        }

        private static bool IsCharKatakana(char chr)
        {
            return IsCharInRange(chr, KatakanaStart, KatakanaEnd);
        }

        private bool IsCharKana(char chr)
        {
            return IsCharHiragana(chr) || IsCharKatakana(chr);
        }

        /* Utility methods */

        private static bool AllTrue(string stringToCheck, Func<string, bool> method)
        {
            return stringToCheck.All(t => method(Convert.ToString(t)));
        }


        /* Conversions */

        private string RomajiToHiragana(string romaji)
        {
            return RomajiToKana(romaji);
        }

        // TODO: move ignore case to options?
        private string RomajiToKana(string romaji, bool ignoreCase = true)
        {
            var chunk = "";
            var chunkLc = "";
            var position = 0;
            var len = romaji.Length;
            const int maxChunk = 3;
            var kana = "";
            var kanaChar = "";

            while (position < len)
            {
                var chunkSize = Math.Min(maxChunk, len - position);

                while (chunkSize > 0)
                {
                    // NB: this is (start, end - start) equivalent to Java's (start, end)
                    chunk = romaji.Substring(position, chunkSize);
                    chunkLc = chunk.ToLower();

                    if ((chunkLc.Equals("lts") || chunkLc.Equals("xts")) && (len - position) >= 4)
                    {
                        chunkSize++;
                        // The second parameter in substring() is an end point, not a length!
                        chunk = romaji.Substring(position, chunkSize);
                        chunkLc = chunk.ToLower();
                    }

                    if (Convert.ToString(chunkLc[0]).Equals("n"))
                    {
                        // Convert n' to ん
                        if (_imeMode && chunk.Length == 2 && Convert.ToString(chunkLc[1]).Equals("'"))
                        {
                            chunkSize = 2;
                            chunk = "nn";
                            chunkLc = chunk.ToLower();
                        }
                        // If the user types "nto", automatically convert "n" to "ん" first
                        // "y" is excluded from the list of consonants so we can still get にゃ, にゅ, and にょ
                        if (chunk.Length > 2 && IsCharConsonant(chunkLc[1], false) && IsCharVowel(chunkLc[2], true))
                        {
                            chunkSize = 1;
                            // I removed the "n"->"ん" mapping because the IME wouldn't let me type "na" for "な" without returning "んあ",
                            // so the chunk needs to be manually set to a value that will map to "ん"
                            chunk = "nn";
                            chunkLc = chunk.ToLower();
                        }
                    }

                    // Prepare to return a small-つ because we're looking at double-consonants.
                    if (chunk.Length > 1 && !Convert.ToString(chunkLc[0]).Equals("n")
                        && IsCharConsonant(chunkLc[0], true)
                        && chunk[0] == chunk[1])
                    {
                        chunkSize = 1;
                        // Return a small katakana ツ when typing in uppercase
                        if (IsCharInRange(chunk[0], UppercaseStart, UppercaseEnd))
                        {
                            chunkLc = chunk = "ッ";
                        }
                        else
                        {
                            chunkLc = chunk = "っ";
                        }
                    }

                    // Try to parse the chunk
                    kanaChar = null;
                    HiraKana.RomajiToKana.Table.TryGetValue(chunkLc, out kanaChar);

                    // Continue, if found this item in table
                    if (kanaChar != null)
                    {
                        break;
                    }

                    // If could not find key, then try again with the smaller chunk
                    chunkSize--;
                }

                if (kanaChar == null)
                {
                    chunk = ConvertPunctuation(Convert.ToString(chunk[0]));
                    kanaChar = chunk;
                }

                if (_useObsoleteKana)
                {
                    if (chunkLc.Equals("wi"))
                    {
                        kanaChar = "ゐ";
                    }
                    if (chunkLc.Equals("we"))
                    {
                        kanaChar = "ゑ";
                    }
                }

                if (romaji.Length > (position + 1) && _imeMode && Convert.ToString(chunkLc[0]).Equals("n"))
                {
                    if ((Convert.ToString(romaji[position + 1]).ToLower().Equals("y") && position == (len - 2)) || position == (len - 1))
                    {
                        kanaChar = Convert.ToString(chunk[0]);
                    }
                }

                if (!ignoreCase)
                {
                    if (IsCharInRange(chunk[0], UppercaseStart, UppercaseEnd))
                    {
                        kanaChar = HiraganaToKatakana(kanaChar);
                    }
                }

                kana += kanaChar;

                position += chunkSize > 0 ? chunkSize : 1;
            }

            return kana;
        }

        public string HiraganaToRomaji(string hira)
        {
            if (IsRomaji(hira))
            {
                return hira;
            }

            var chunk = "";
            var cursor = 0;
            var len = hira.Length;
            const int maxChunk = 2;
            var nextCharIsDoubleConsonant = false;
            var roma = "";
            string romaChar = null;

            while (cursor < len)
            {
                var chunkSize = Math.Min(maxChunk, len - cursor);
                while (chunkSize > 0)
                {
                    chunk = hira.Substring(cursor, chunkSize);

                    if (IsKatakana(chunk))
                    {
                        chunk = KatakanaToHiragana(chunk);
                    }

                    if (Convert.ToString(chunk[0]).Equals("っ") && chunkSize == 1 && cursor < (len - 1))
                    {
                        nextCharIsDoubleConsonant = true;
                        romaChar = "";
                        break;
                    }

                    // Try to parse
                    romaChar = null;
                    KanaToRomaji.Table.TryGetValue(chunk, out romaChar);

                    if ((romaChar != null) && nextCharIsDoubleConsonant)
                    {
                        romaChar = romaChar[0] + romaChar;
                        nextCharIsDoubleConsonant = false;
                    }

                    if (romaChar != null)
                    {
                        break;
                    }

                    chunkSize--;
                }
                if (romaChar == null)
                {
                    romaChar = chunk;
                }

                roma += romaChar;
                cursor += chunkSize > 0 ? chunkSize : 1;
            }
            return roma;
        }

        private static string HiraganaToKatakana(string hira)
        {
            var kata = "";

            foreach (var hiraChar in hira)
            {
                if (IsCharHiragana(hiraChar))
                {
                    var code = (int)hiraChar;
                    code += KatakanaStart - HiraganaStart;
                    kata += Convert.ToString(Convert.ToChar(code));
                }
                else
                {
                    kata += hiraChar;
                }
            }

            return kata;
        }

        private static string KatakanaToHiragana(string kata)
        {
            String hira = "";

            foreach (var kataChar in kata)
            {
                if (IsCharKatakana(kataChar))
                {
                    var code = (int)kataChar;
                    code += HiraganaStart - KatakanaStart;
                    hira += Convert.ToString(Convert.ToChar(code));
                }
                else
                {
                    hira += kataChar;
                }
            }

            return hira;
        }


        // Convert punctuations: long space and dash
        private static string ConvertPunctuation(string input)
        {
            if (input.Equals(Convert.ToString(('　'))))
            {
                return Convert.ToString(' ');
            }

            return input.Equals(Convert.ToString('-')) ? Convert.ToString('ー') : input;
        }

    }
}

