# HiraKana
Romaji and kana converter.

Utility class with all the usual methods. Based on Wanakana.js (wanakana.com).
Usage examples (also included in ShortHand::Main):

```c#
using HiraKana;

/* Shorthand */

Debug.WriteLine(
	new Romaji("korehahirakananodesu").ToHiragana()
); // これはひらかなのです

Debug.WriteLine(
	new Romaji("wawiwewo").useObsoleteKana(true).ToKatakana()
); // ワヰヱヲ

/* Direct */

Debug.WriteLine(
	new KanaTools().toHiragana("sorehaZOMUBIEdesuka")
); // それはぞむびえですか

Debug.WriteLine(
	new KanaTools().isKana("あいうえお")
); // False
```