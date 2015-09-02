# HiraKana
Romaji and kana converter.

Utility class with all the usual methods. Based on Wanakana.js.
Usage examples (also included in ShortHand::Main):

```c#
using HiraKana;

/* Shorthand */

Debug.WriteLine(
	new Romaji("korehahirakananodesu").ToHiragana()
);

Debug.WriteLine(
	new Romaji("korehahirakananodesu").useObsoleteKana(true).ToKatakana()
);

/* Direct */

Debug.WriteLine(
	new KanaTools().toHiragana("sorehaZOMUBIEdesuka")
);

Debug.WriteLine(
	new KanaTools().isKana("あいうえお")
);
```