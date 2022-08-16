﻿using System;
using System.Text;

namespace HtmlParserSharp.Core
{
	sealed class NCName
	{
		public static bool IsNCNameStart(char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= 'À' && c <= 'Ö') || (c >= 'Ø' && c <= 'ö') || (c >= 'ø' && c <= 'ÿ') || (c >= 'Ā' && c <= 'ı') || (c >= 'Ĵ' && c <= 'ľ') || (c >= 'Ł' && c <= 'ň') || (c >= 'Ŋ' && c <= 'ž') || (c >= 'ƀ' && c <= 'ǃ') || (c >= 'Ǎ' && c <= 'ǰ') || (c >= 'Ǵ' && c <= 'ǵ') || (c >= 'Ǻ' && c <= 'ȗ') || (c >= 'ɐ' && c <= 'ʨ') || (c >= 'ʻ' && c <= 'ˁ') || c == 'Ά' || (c >= 'Έ' && c <= 'Ί') || c == 'Ό' || (c >= 'Ύ' && c <= 'Ρ') || (c >= 'Σ' && c <= 'ώ') || (c >= 'ϐ' && c <= 'ϖ') || c == 'Ϛ' || c == 'Ϝ' || c == 'Ϟ' || c == 'Ϡ' || (c >= 'Ϣ' && c <= 'ϳ') || (c >= 'Ё' && c <= 'Ќ') || (c >= 'Ў' && c <= 'я') || (c >= 'ё' && c <= 'ќ') || (c >= 'ў' && c <= 'ҁ') || (c >= 'Ґ' && c <= 'ӄ') || (c >= 'Ӈ' && c <= 'ӈ') || (c >= 'Ӌ' && c <= 'ӌ') || (c >= 'Ӑ' && c <= 'ӫ') || (c >= 'Ӯ' && c <= 'ӵ') || (c >= 'Ӹ' && c <= 'ӹ') || (c >= 'Ա' && c <= 'Ֆ') || c == 'ՙ' || (c >= 'ա' && c <= 'ֆ') || (c >= 'א' && c <= 'ת') || (c >= 'װ' && c <= 'ײ') || (c >= 'ء' && c <= 'غ') || (c >= 'ف' && c <= 'ي') || (c >= 'ٱ' && c <= 'ڷ') || (c >= 'ں' && c <= 'ھ') || (c >= 'ۀ' && c <= 'ێ') || (c >= 'ې' && c <= 'ۓ') || c == 'ە' || (c >= 'ۥ' && c <= 'ۦ') || (c >= 'अ' && c <= 'ह') || c == 'ऽ' || (c >= 'क़' && c <= 'ॡ') || (c >= 'অ' && c <= 'ঌ') || (c >= 'এ' && c <= 'ঐ') || (c >= 'ও' && c <= 'ন') || (c >= 'প' && c <= 'র') || c == 'ল' || (c >= 'শ' && c <= 'হ') || (c >= 'ড়' && c <= 'ঢ়') || (c >= 'য়' && c <= 'ৡ') || (c >= 'ৰ' && c <= 'ৱ') || (c >= 'ਅ' && c <= 'ਊ') || (c >= 'ਏ' && c <= 'ਐ') || (c >= 'ਓ' && c <= 'ਨ') || (c >= 'ਪ' && c <= 'ਰ') || (c >= 'ਲ' && c <= 'ਲ਼') || (c >= 'ਵ' && c <= 'ਸ਼') || (c >= 'ਸ' && c <= 'ਹ') || (c >= 'ਖ਼' && c <= 'ੜ') || c == 'ਫ਼' || (c >= 'ੲ' && c <= 'ੴ') || (c >= 'અ' && c <= 'ઋ') || c == 'ઍ' || (c >= 'એ' && c <= 'ઑ') || (c >= 'ઓ' && c <= 'ન') || (c >= 'પ' && c <= 'ર') || (c >= 'લ' && c <= 'ળ') || (c >= 'વ' && c <= 'હ') || c == 'ઽ' || c == 'ૠ' || (c >= 'ଅ' && c <= 'ଌ') || (c >= 'ଏ' && c <= 'ଐ') || (c >= 'ଓ' && c <= 'ନ') || (c >= 'ପ' && c <= 'ର') || (c >= 'ଲ' && c <= 'ଳ') || (c >= 'ଶ' && c <= 'ହ') || c == 'ଽ' || (c >= 'ଡ଼' && c <= 'ଢ଼') || (c >= 'ୟ' && c <= 'ୡ') || (c >= 'அ' && c <= 'ஊ') || (c >= 'எ' && c <= 'ஐ') || (c >= 'ஒ' && c <= 'க') || (c >= 'ங' && c <= 'ச') || c == 'ஜ' || (c >= 'ஞ' && c <= 'ட') || (c >= 'ண' && c <= 'த') || (c >= 'ந' && c <= 'ப') || (c >= 'ம' && c <= 'வ') || (c >= 'ஷ' && c <= 'ஹ') || (c >= 'అ' && c <= 'ఌ') || (c >= 'ఎ' && c <= 'ఐ') || (c >= 'ఒ' && c <= 'న') || (c >= 'ప' && c <= 'ళ') || (c >= 'వ' && c <= 'హ') || (c >= 'ౠ' && c <= 'ౡ') || (c >= 'ಅ' && c <= 'ಌ') || (c >= 'ಎ' && c <= 'ಐ') || (c >= 'ಒ' && c <= 'ನ') || (c >= 'ಪ' && c <= 'ಳ') || (c >= 'ವ' && c <= 'ಹ') || c == 'ೞ' || (c >= 'ೠ' && c <= 'ೡ') || (c >= 'അ' && c <= 'ഌ') || (c >= 'എ' && c <= 'ഐ') || (c >= 'ഒ' && c <= 'ന') || (c >= 'പ' && c <= 'ഹ') || (c >= 'ൠ' && c <= 'ൡ') || (c >= 'ก' && c <= 'ฮ') || c == 'ะ' || (c >= 'า' && c <= 'ำ') || (c >= 'เ' && c <= 'ๅ') || (c >= 'ກ' && c <= 'ຂ') || c == 'ຄ' || (c >= 'ງ' && c <= 'ຈ') || c == 'ຊ' || c == 'ຍ' || (c >= 'ດ' && c <= 'ທ') || (c >= 'ນ' && c <= 'ຟ') || (c >= 'ມ' && c <= 'ຣ') || c == 'ລ' || c == 'ວ' || (c >= 'ສ' && c <= 'ຫ') || (c >= 'ອ' && c <= 'ຮ') || c == 'ະ' || (c >= 'າ' && c <= 'ຳ') || c == 'ຽ' || (c >= 'ເ' && c <= 'ໄ') || (c >= 'ཀ' && c <= 'ཇ') || (c >= 'ཉ' && c <= 'ཀྵ') || (c >= 'Ⴀ' && c <= 'Ⴥ') || (c >= 'ა' && c <= 'ჶ') || c == 'ᄀ' || (c >= 'ᄂ' && c <= 'ᄃ') || (c >= 'ᄅ' && c <= 'ᄇ') || c == 'ᄉ' || (c >= 'ᄋ' && c <= 'ᄌ') || (c >= 'ᄎ' && c <= 'ᄒ') || c == 'ᄼ' || c == 'ᄾ' || c == 'ᅀ' || c == 'ᅌ' || c == 'ᅎ' || c == 'ᅐ' || (c >= 'ᅔ' && c <= 'ᅕ') || c == 'ᅙ' || (c >= 'ᅟ' && c <= 'ᅡ') || c == 'ᅣ' || c == 'ᅥ' || c == 'ᅧ' || c == 'ᅩ' || (c >= 'ᅭ' && c <= 'ᅮ') || (c >= 'ᅲ' && c <= 'ᅳ') || c == 'ᅵ' || c == 'ᆞ' || c == 'ᆨ' || c == 'ᆫ' || (c >= 'ᆮ' && c <= 'ᆯ') || (c >= 'ᆷ' && c <= 'ᆸ') || c == 'ᆺ' || (c >= 'ᆼ' && c <= 'ᇂ') || c == 'ᇫ' || c == 'ᇰ' || c == 'ᇹ' || (c >= 'Ḁ' && c <= 'ẛ') || (c >= 'Ạ' && c <= 'ỹ') || (c >= 'ἀ' && c <= 'ἕ') || (c >= 'Ἐ' && c <= 'Ἕ') || (c >= 'ἠ' && c <= 'ὅ') || (c >= 'Ὀ' && c <= 'Ὅ') || (c >= 'ὐ' && c <= 'ὗ') || c == 'Ὑ' || c == 'Ὓ' || c == 'Ὕ' || (c >= 'Ὗ' && c <= 'ώ') || (c >= 'ᾀ' && c <= 'ᾴ') || (c >= 'ᾶ' && c <= 'ᾼ') || c == 'ι' || (c >= 'ῂ' && c <= 'ῄ') || (c >= 'ῆ' && c <= 'ῌ') || (c >= 'ῐ' && c <= 'ΐ') || (c >= 'ῖ' && c <= 'Ί') || (c >= 'ῠ' && c <= 'Ῥ') || (c >= 'ῲ' && c <= 'ῴ') || (c >= 'ῶ' && c <= 'ῼ') || c == 'Ω' || (c >= 'K' && c <= 'Å') || c == '℮' || (c >= 'ↀ' && c <= 'ↂ') || (c >= 'ぁ' && c <= 'ゔ') || (c >= 'ァ' && c <= 'ヺ') || (c >= 'ㄅ' && c <= 'ㄬ') || (c >= '가' && c <= '힣') || (c >= '一' && c <= '龥') || c == '〇' || (c >= '〡' && c <= '〩') || c == '_';
		}

		public static bool IsNCNameTrail(char c)
		{
			return (c >= '0' && c <= '9') || (c >= '٠' && c <= '٩') || (c >= '۰' && c <= '۹') || (c >= '०' && c <= '९') || (c >= '০' && c <= '৯') || (c >= '੦' && c <= '੯') || (c >= '૦' && c <= '૯') || (c >= '୦' && c <= '୯') || (c >= '௧' && c <= '௯') || (c >= '౦' && c <= '౯') || (c >= '೦' && c <= '೯') || (c >= '൦' && c <= '൯') || (c >= '๐' && c <= '๙') || (c >= '໐' && c <= '໙') || (c >= '༠' && c <= '༩') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= 'À' && c <= 'Ö') || (c >= 'Ø' && c <= 'ö') || (c >= 'ø' && c <= 'ÿ') || (c >= 'Ā' && c <= 'ı') || (c >= 'Ĵ' && c <= 'ľ') || (c >= 'Ł' && c <= 'ň') || (c >= 'Ŋ' && c <= 'ž') || (c >= 'ƀ' && c <= 'ǃ') || (c >= 'Ǎ' && c <= 'ǰ') || (c >= 'Ǵ' && c <= 'ǵ') || (c >= 'Ǻ' && c <= 'ȗ') || (c >= 'ɐ' && c <= 'ʨ') || (c >= 'ʻ' && c <= 'ˁ') || c == 'Ά' || (c >= 'Έ' && c <= 'Ί') || c == 'Ό' || (c >= 'Ύ' && c <= 'Ρ') || (c >= 'Σ' && c <= 'ώ') || (c >= 'ϐ' && c <= 'ϖ') || c == 'Ϛ' || c == 'Ϝ' || c == 'Ϟ' || c == 'Ϡ' || (c >= 'Ϣ' && c <= 'ϳ') || (c >= 'Ё' && c <= 'Ќ') || (c >= 'Ў' && c <= 'я') || (c >= 'ё' && c <= 'ќ') || (c >= 'ў' && c <= 'ҁ') || (c >= 'Ґ' && c <= 'ӄ') || (c >= 'Ӈ' && c <= 'ӈ') || (c >= 'Ӌ' && c <= 'ӌ') || (c >= 'Ӑ' && c <= 'ӫ') || (c >= 'Ӯ' && c <= 'ӵ') || (c >= 'Ӹ' && c <= 'ӹ') || (c >= 'Ա' && c <= 'Ֆ') || c == 'ՙ' || (c >= 'ա' && c <= 'ֆ') || (c >= 'א' && c <= 'ת') || (c >= 'װ' && c <= 'ײ') || (c >= 'ء' && c <= 'غ') || (c >= 'ف' && c <= 'ي') || (c >= 'ٱ' && c <= 'ڷ') || (c >= 'ں' && c <= 'ھ') || (c >= 'ۀ' && c <= 'ێ') || (c >= 'ې' && c <= 'ۓ') || c == 'ە' || (c >= 'ۥ' && c <= 'ۦ') || (c >= 'अ' && c <= 'ह') || c == 'ऽ' || (c >= 'क़' && c <= 'ॡ') || (c >= 'অ' && c <= 'ঌ') || (c >= 'এ' && c <= 'ঐ') || (c >= 'ও' && c <= 'ন') || (c >= 'প' && c <= 'র') || c == 'ল' || (c >= 'শ' && c <= 'হ') || (c >= 'ড়' && c <= 'ঢ়') || (c >= 'য়' && c <= 'ৡ') || (c >= 'ৰ' && c <= 'ৱ') || (c >= 'ਅ' && c <= 'ਊ') || (c >= 'ਏ' && c <= 'ਐ') || (c >= 'ਓ' && c <= 'ਨ') || (c >= 'ਪ' && c <= 'ਰ') || (c >= 'ਲ' && c <= 'ਲ਼') || (c >= 'ਵ' && c <= 'ਸ਼') || (c >= 'ਸ' && c <= 'ਹ') || (c >= 'ਖ਼' && c <= 'ੜ') || c == 'ਫ਼' || (c >= 'ੲ' && c <= 'ੴ') || (c >= 'અ' && c <= 'ઋ') || c == 'ઍ' || (c >= 'એ' && c <= 'ઑ') || (c >= 'ઓ' && c <= 'ન') || (c >= 'પ' && c <= 'ર') || (c >= 'લ' && c <= 'ળ') || (c >= 'વ' && c <= 'હ') || c == 'ઽ' || c == 'ૠ' || (c >= 'ଅ' && c <= 'ଌ') || (c >= 'ଏ' && c <= 'ଐ') || (c >= 'ଓ' && c <= 'ନ') || (c >= 'ପ' && c <= 'ର') || (c >= 'ଲ' && c <= 'ଳ') || (c >= 'ଶ' && c <= 'ହ') || c == 'ଽ' || (c >= 'ଡ଼' && c <= 'ଢ଼') || (c >= 'ୟ' && c <= 'ୡ') || (c >= 'அ' && c <= 'ஊ') || (c >= 'எ' && c <= 'ஐ') || (c >= 'ஒ' && c <= 'க') || (c >= 'ங' && c <= 'ச') || c == 'ஜ' || (c >= 'ஞ' && c <= 'ட') || (c >= 'ண' && c <= 'த') || (c >= 'ந' && c <= 'ப') || (c >= 'ம' && c <= 'வ') || (c >= 'ஷ' && c <= 'ஹ') || (c >= 'అ' && c <= 'ఌ') || (c >= 'ఎ' && c <= 'ఐ') || (c >= 'ఒ' && c <= 'న') || (c >= 'ప' && c <= 'ళ') || (c >= 'వ' && c <= 'హ') || (c >= 'ౠ' && c <= 'ౡ') || (c >= 'ಅ' && c <= 'ಌ') || (c >= 'ಎ' && c <= 'ಐ') || (c >= 'ಒ' && c <= 'ನ') || (c >= 'ಪ' && c <= 'ಳ') || (c >= 'ವ' && c <= 'ಹ') || c == 'ೞ' || (c >= 'ೠ' && c <= 'ೡ') || (c >= 'അ' && c <= 'ഌ') || (c >= 'എ' && c <= 'ഐ') || (c >= 'ഒ' && c <= 'ന') || (c >= 'പ' && c <= 'ഹ') || (c >= 'ൠ' && c <= 'ൡ') || (c >= 'ก' && c <= 'ฮ') || c == 'ะ' || (c >= 'า' && c <= 'ำ') || (c >= 'เ' && c <= 'ๅ') || (c >= 'ກ' && c <= 'ຂ') || c == 'ຄ' || (c >= 'ງ' && c <= 'ຈ') || c == 'ຊ' || c == 'ຍ' || (c >= 'ດ' && c <= 'ທ') || (c >= 'ນ' && c <= 'ຟ') || (c >= 'ມ' && c <= 'ຣ') || c == 'ລ' || c == 'ວ' || (c >= 'ສ' && c <= 'ຫ') || (c >= 'ອ' && c <= 'ຮ') || c == 'ະ' || (c >= 'າ' && c <= 'ຳ') || c == 'ຽ' || (c >= 'ເ' && c <= 'ໄ') || (c >= 'ཀ' && c <= 'ཇ') || (c >= 'ཉ' && c <= 'ཀྵ') || (c >= 'Ⴀ' && c <= 'Ⴥ') || (c >= 'ა' && c <= 'ჶ') || c == 'ᄀ' || (c >= 'ᄂ' && c <= 'ᄃ') || (c >= 'ᄅ' && c <= 'ᄇ') || c == 'ᄉ' || (c >= 'ᄋ' && c <= 'ᄌ') || (c >= 'ᄎ' && c <= 'ᄒ') || c == 'ᄼ' || c == 'ᄾ' || c == 'ᅀ' || c == 'ᅌ' || c == 'ᅎ' || c == 'ᅐ' || (c >= 'ᅔ' && c <= 'ᅕ') || c == 'ᅙ' || (c >= 'ᅟ' && c <= 'ᅡ') || c == 'ᅣ' || c == 'ᅥ' || c == 'ᅧ' || c == 'ᅩ' || (c >= 'ᅭ' && c <= 'ᅮ') || (c >= 'ᅲ' && c <= 'ᅳ') || c == 'ᅵ' || c == 'ᆞ' || c == 'ᆨ' || c == 'ᆫ' || (c >= 'ᆮ' && c <= 'ᆯ') || (c >= 'ᆷ' && c <= 'ᆸ') || c == 'ᆺ' || (c >= 'ᆼ' && c <= 'ᇂ') || c == 'ᇫ' || c == 'ᇰ' || c == 'ᇹ' || (c >= 'Ḁ' && c <= 'ẛ') || (c >= 'Ạ' && c <= 'ỹ') || (c >= 'ἀ' && c <= 'ἕ') || (c >= 'Ἐ' && c <= 'Ἕ') || (c >= 'ἠ' && c <= 'ὅ') || (c >= 'Ὀ' && c <= 'Ὅ') || (c >= 'ὐ' && c <= 'ὗ') || c == 'Ὑ' || c == 'Ὓ' || c == 'Ὕ' || (c >= 'Ὗ' && c <= 'ώ') || (c >= 'ᾀ' && c <= 'ᾴ') || (c >= 'ᾶ' && c <= 'ᾼ') || c == 'ι' || (c >= 'ῂ' && c <= 'ῄ') || (c >= 'ῆ' && c <= 'ῌ') || (c >= 'ῐ' && c <= 'ΐ') || (c >= 'ῖ' && c <= 'Ί') || (c >= 'ῠ' && c <= 'Ῥ') || (c >= 'ῲ' && c <= 'ῴ') || (c >= 'ῶ' && c <= 'ῼ') || c == 'Ω' || (c >= 'K' && c <= 'Å') || c == '℮' || (c >= 'ↀ' && c <= 'ↂ') || (c >= 'ぁ' && c <= 'ゔ') || (c >= 'ァ' && c <= 'ヺ') || (c >= 'ㄅ' && c <= 'ㄬ') || (c >= '가' && c <= '힣') || (c >= '一' && c <= '龥') || c == '〇' || (c >= '〡' && c <= '〩') || c == '_' || c == '.' || c == '-' || (c >= '\u0300' && c <= '\u0345') || (c >= '\u0360' && c <= '\u0361') || (c >= '\u0483' && c <= '\u0486') || (c >= '\u0591' && c <= '\u05a1') || (c >= '\u05a3' && c <= '\u05b9') || (c >= '\u05bb' && c <= '\u05bd') || c == '\u05bf' || (c >= '\u05c1' && c <= '\u05c2') || c == '\u05c4' || (c >= '\u064b' && c <= '\u0652') || c == '\u0670' || (c >= '\u06d6' && c <= '\u06dc') || (c >= '\u06dd' && c <= '\u06df') || (c >= '\u06e0' && c <= '\u06e4') || (c >= '\u06e7' && c <= '\u06e8') || (c >= '\u06ea' && c <= '\u06ed') || (c >= '\u0901' && c <= '\u0903') || c == '\u093c' || (c >= '\u093e' && c <= '\u094c') || c == '\u094d' || (c >= '\u0951' && c <= '\u0954') || (c >= '\u0962' && c <= '\u0963') || (c >= '\u0981' && c <= '\u0983') || c == '\u09bc' || c == '\u09be' || c == '\u09bf' || (c >= '\u09c0' && c <= '\u09c4') || (c >= '\u09c7' && c <= '\u09c8') || (c >= '\u09cb' && c <= '\u09cd') || c == '\u09d7' || (c >= '\u09e2' && c <= '\u09e3') || c == '\u0a02' || c == '\u0a3c' || c == '\u0a3e' || c == '\u0a3f' || (c >= '\u0a40' && c <= '\u0a42') || (c >= '\u0a47' && c <= '\u0a48') || (c >= '\u0a4b' && c <= '\u0a4d') || (c >= '\u0a70' && c <= '\u0a71') || (c >= '\u0a81' && c <= '\u0a83') || c == '\u0abc' || (c >= '\u0abe' && c <= '\u0ac5') || (c >= '\u0ac7' && c <= '\u0ac9') || (c >= '\u0acb' && c <= '\u0acd') || (c >= '\u0b01' && c <= '\u0b03') || c == '\u0b3c' || (c >= '\u0b3e' && c <= '\u0b43') || (c >= '\u0b47' && c <= '\u0b48') || (c >= '\u0b4b' && c <= '\u0b4d') || (c >= '\u0b56' && c <= '\u0b57') || (c >= '\u0b82' && c <= 'ஃ') || (c >= '\u0bbe' && c <= '\u0bc2') || (c >= '\u0bc6' && c <= '\u0bc8') || (c >= '\u0bca' && c <= '\u0bcd') || c == '\u0bd7' || (c >= '\u0c01' && c <= '\u0c03') || (c >= '\u0c3e' && c <= '\u0c44') || (c >= '\u0c46' && c <= '\u0c48') || (c >= '\u0c4a' && c <= '\u0c4d') || (c >= '\u0c55' && c <= '\u0c56') || (c >= '\u0c82' && c <= '\u0c83') || (c >= '\u0cbe' && c <= '\u0cc4') || (c >= '\u0cc6' && c <= '\u0cc8') || (c >= '\u0cca' && c <= '\u0ccd') || (c >= '\u0cd5' && c <= '\u0cd6') || (c >= '\u0d02' && c <= '\u0d03') || (c >= '\u0d3e' && c <= '\u0d43') || (c >= '\u0d46' && c <= '\u0d48') || (c >= '\u0d4a' && c <= '\u0d4d') || c == '\u0d57' || c == '\u0e31' || (c >= '\u0e34' && c <= '\u0e3a') || (c >= '\u0e47' && c <= '\u0e4e') || c == '\u0eb1' || (c >= '\u0eb4' && c <= '\u0eb9') || (c >= '\u0ebb' && c <= '\u0ebc') || (c >= '\u0ec8' && c <= '\u0ecd') || (c >= '\u0f18' && c <= '\u0f19') || c == '\u0f35' || c == '\u0f37' || c == '\u0f39' || c == '\u0f3e' || c == '\u0f3f' || (c >= '\u0f71' && c <= '\u0f84') || (c >= '\u0f86' && c <= 'ྋ') || (c >= '\u0f90' && c <= '\u0f95') || c == '\u0f97' || (c >= '\u0f99' && c <= '\u0fad') || (c >= '\u0fb1' && c <= '\u0fb7') || c == '\u0fb9' || (c >= '\u20d0' && c <= '\u20dc') || c == '\u20e1' || (c >= '\u302a' && c <= '\u302f') || c == '\u3099' || c == '\u309a' || c == '·' || c == 'ː' || c == 'ˑ' || c == '·' || c == 'ـ' || c == 'ๆ' || c == 'ໆ' || c == '々' || (c >= '〱' && c <= '〵') || (c >= 'ゝ' && c <= 'ゞ') || (c >= 'ー' && c <= 'ヾ');
		}

		public static bool IsNCName(string str)
		{
			if (str == null)
			{
				return false;
			}
			int length = str.Length;
			switch (length)
			{
			case 0:
				return false;
			case 1:
				return NCName.IsNCNameStart(str[0]);
			default:
				if (!NCName.IsNCNameStart(str[0]))
				{
					return false;
				}
				for (int i = 1; i < length; i++)
				{
					if (!NCName.IsNCNameTrail(str[i]))
					{
						return false;
					}
				}
				return true;
			}
		}

		static void AppendUHexTo(StringBuilder sb, int c)
		{
			sb.Append('U');
			for (int i = 0; i < 6; i++)
			{
				sb.Append(NCName.HEX_TABLE[(c & 15728640) >> 20]);
				c <<= 4;
			}
		}

		public static string EscapeName(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if ((c & 'ﰀ') == '\ud800')
				{
					char c2 = str[++i];
					NCName.AppendUHexTo(stringBuilder, (int)(((int)c << 10) + c2) + -56613888);
				}
				else if (i == 0 && !NCName.IsNCNameStart(c))
				{
					NCName.AppendUHexTo(stringBuilder, (int)c);
				}
				else if (i != 0 && !NCName.IsNCNameTrail(c))
				{
					NCName.AppendUHexTo(stringBuilder, (int)c);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return string.Intern(stringBuilder.ToString());
		}

		const int SURROGATE_OFFSET = -56613888;

		static readonly char[] HEX_TABLE = "0123456789ABCDEF".ToCharArray();
	}
}
