# HiraKana
Romaji and kana converter.

Utility class with all the usual methods. Based on Wanakana.js.
Usage examples (also included in ShortHand::Main):

```c#
using HiraKana;

/* Shorthand */

Debug.WriteLine(
	new Romaji("korehahirakananodesu").ToHiragana()
); // これはひらかなのです

Debug.WriteLine(
	new Romaji("wawiwo").useObsoleteKana(true).ToKatakana()
); // ワヰヲ

/* Direct */

Debug.WriteLine(
	new KanaTools().toHiragana("sorehaZOMUBIEdesuka")
); // それはぞむびえですか

Debug.WriteLine(
	new KanaTools().isKana("あいうえお")
); // False
```