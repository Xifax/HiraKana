using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HiraKana
{
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

        // Options
        static Boolean IME_MODE = false;
        static Boolean USE_OBSOLETE_KANA = false;

        public KanaTools() { }

        /* Options */

        public KanaTools useIme(Boolean mode)
        {
            IME_MODE = mode;
            return this;
        }

        public KanaTools useObsoleteKana(Boolean flag)
        {
            USE_OBSOLETE_KANA = flag;
            return this;
        }

        /* Public API */

        public String toHiragana(String input)
        {
            if (isRomaji(input))
            {
                return romajiToHiragana(input);
            }

            if (isKatakana(input))
            {
                return katakanaToHiragana(input);
            }

            return input;
        }

        public String toKatakana(String input)
        {
            if (isHiragana(input))
            {
                return hiraganaToKatakana(input);
            }

            if (isRomaji(input))
            {
                return hiraganaToKatakana(romajiToHiragana(input));
            }

            return input;
        }

        public String toRomaji(String input)
        {
            return hiraganaToRomaji(input);
        }

        public String toKana(String input)
        {
            return romajiToKana(input, false);
        }


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
                        kanaChar = RomajiToKana.table[chunkLC];
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

                if (!ignoreCase)
                {
                    if (isCharInRange(chunk[0], UPPERCASE_START, UPPERCASE_END))
                    {
                        kanaChar = hiraganaToKatakana(kanaChar);
                    }
                }

                kana += kanaChar;

                position += chunkSize > 0 ? chunkSize : 1;
            }

            return kana;
        }

        public String hiraganaToRomaji(String hira)
        {
            if (isRomaji(hira))
            {
                return hira;
            }

            String chunk = "";
            int chunkSize;
            int cursor = 0;
            int len = hira.Length;
            int maxChunk = 2;
            Boolean nextCharIsDoubleConsonant = false;
            String roma = "";
            String romaChar = null;

            while (cursor < len)
            {
                chunkSize = Math.Min(maxChunk, len - cursor);
                while (chunkSize > 0)
                {
                    chunk = hira.Substring(cursor, chunkSize);

                    if (isKatakana(chunk))
                    {
                        chunk = katakanaToHiragana(chunk);
                    }

                    if (Convert.ToString(chunk[0]).Equals("っ") && chunkSize == 1 && cursor < (len - 1))
                    {
                        nextCharIsDoubleConsonant = true;
                        romaChar = "";
                        break;
                    }

                    try {
                        romaChar = RomajiToKana.table[chunk];
                    } catch(Exception)
                    {
                        romaChar = null;
                    }

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

        private String hiraganaToKatakana(String hira)
        {
            int code;
            String kata = "";

            for (int i = 0; i < hira.Length; i++)
            {
                char hiraChar = hira[i];

                if (isCharHiragana(hiraChar))
                {
                    code = (int)hiraChar;
                    code += KATAKANA_START - HIRAGANA_START;
                    kata += Convert.ToString(Convert.ToChar(code));
                }
                else
                {
                    kata += hiraChar;
                }
            }

            return kata;
        }

        private String katakanaToHiragana(String kata)
        {
            int code;
            String hira = "";

            for (int i = 0; i < kata.Length; i++)
            {
                char kataChar = kata[i];

                if (isCharKatakana(kataChar))
                {
                    code = (int)kataChar;
                    code += HIRAGANA_START - KATAKANA_START;
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

