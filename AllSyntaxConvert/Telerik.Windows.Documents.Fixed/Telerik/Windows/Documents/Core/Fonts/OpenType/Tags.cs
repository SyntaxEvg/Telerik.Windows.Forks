using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType
{
	static class Tags
	{
		internal static string GetStringFromTag(uint tag)
		{
			return new string(new char[]
			{
				(char)(255U & (tag >> 24)),
				(char)(255U & (tag >> 16)),
				(char)(255U & (tag >> 8)),
				(char)(255U & tag)
			});
		}

		internal static uint GetTagFromString(string str)
		{
			if (str.Length != 4)
			{
				Guard.ThrowArgumentException(str);
			}
			return (uint)(((uint)str[0] << 24) | ((uint)str[1] << 16) | ((uint)str[2] << 8) | str[3]);
		}

		internal static readonly uint DEFAULT_TABLE_SCRIPT_TAG = Tags.GetTagFromString("DFLT");

		internal static readonly uint CFF_TABLE = Tags.GetTagFromString("CFF ");

		internal static readonly uint CMAP_TABLE = Tags.GetTagFromString("cmap");

		internal static readonly uint GLYF_TABLE = Tags.GetTagFromString("glyf");

		internal static readonly uint MAXP_TABLE = Tags.GetTagFromString("maxp");

		internal static readonly uint LOCA_TABLE = Tags.GetTagFromString("loca");

		internal static readonly uint HEAD_TABLE = Tags.GetTagFromString("head");

		internal static readonly uint HHEA_TABLE = Tags.GetTagFromString("hhea");

		internal static readonly uint HMTX_TABLE = Tags.GetTagFromString("hmtx");

		internal static readonly uint KERN_TABLE = Tags.GetTagFromString("kern");

		internal static readonly uint GSUB_TABLE = Tags.GetTagFromString("GSUB");

		internal static readonly uint NAME_TABLE = Tags.GetTagFromString("name");

		internal static readonly uint OS_2_TABLE = Tags.GetTagFromString("OS/2");

		internal static readonly uint POST_TABLE = Tags.GetTagFromString("post");

		internal static readonly uint OTTO_TAG = Tags.GetTagFromString("OTTO");

		internal static readonly uint NULL_TAG = Tags.GetTagFromString("NULL");

		internal static readonly ushort NULL_TYPE = 255;

		internal static readonly uint TRUE_TYPE_COLLECTION = Tags.GetTagFromString("ttcf");

		internal static readonly uint FEATURE_ACCESS_ALL_ALTERNATES = Tags.GetTagFromString("aalt");

		internal static readonly uint FEATURE_ABOVE_BASE_FORMS = Tags.GetTagFromString("abvf");

		internal static readonly uint FEATURE_ABOVE_BASE_MARK_POSITIONING = Tags.GetTagFromString("abvm");

		internal static readonly uint FEATURE_ABOVE_BASE_SUBSTITUTIONS = Tags.GetTagFromString("abvs");

		internal static readonly uint FEATURE_ALTERNATIVE_FRACTIONS = Tags.GetTagFromString("afrc");

		internal static readonly uint FEATURE_AKHANDS = Tags.GetTagFromString("akhn");

		internal static readonly uint FEATURE_BELOW_BASE_FORMS = Tags.GetTagFromString("blwf");

		internal static readonly uint FEATURE_BELOW_BASE_MARK_POSITIONING = Tags.GetTagFromString("blwm");

		internal static readonly uint FEATURE_BELOW_BASE_SUBSTITUTIONS = Tags.GetTagFromString("blws");

		internal static readonly uint FEATURE_CONTEXTUAL_ALTERNATES = Tags.GetTagFromString("calt");

		internal static readonly uint FEATURE_CASE_SENSITIVE_FORMS = Tags.GetTagFromString("case");

		internal static readonly uint FEATURE_GLYPH_COMPOSITION_DECOMPOSITION = Tags.GetTagFromString("ccmp");

		internal static readonly uint FEATURE_CONJUNCT_FORM_AFTER_RO = Tags.GetTagFromString("cfar");

		internal static readonly uint FEATURE_CONJUNCT_FORMS = Tags.GetTagFromString("cjct");

		internal static readonly uint FEATURE_CONTEXTUAL_LIGATURES = Tags.GetTagFromString("clig");

		internal static readonly uint FEATURE_CENTERED_CJK_PUNCTUATION = Tags.GetTagFromString("cpct");

		internal static readonly uint FEATURE_CAPITAL_SPACING = Tags.GetTagFromString("cpsp");

		internal static readonly uint FEATURE_CONTEXTUAL_SWASH = Tags.GetTagFromString("cswh");

		internal static readonly uint FEATURE_CURSIVE_POSITIONING = Tags.GetTagFromString("curs");

		internal static readonly uint FEATURE_PETITE_CAPITALS_FROM_CAPITALS = Tags.GetTagFromString("c2pc");

		internal static readonly uint FEATURE_SMALL_CAPITALS_FROM_CAPITALS = Tags.GetTagFromString("c2sc");

		internal static readonly uint FEATURE_DISTANCES = Tags.GetTagFromString("dist");

		internal static readonly uint FEATURE_DISCRETIONARY_LIGATURES = Tags.GetTagFromString("dlig");

		internal static readonly uint FEATURE_DENOMINATORS = Tags.GetTagFromString("dnom");

		internal static readonly uint FEATURE_EXPERT_FORMS = Tags.GetTagFromString("expt");

		internal static readonly uint FEATURE_FINAL_GLYPH_ON_LINE_ALTERNATES = Tags.GetTagFromString("falt");

		internal static readonly uint FEATURE_TERMINAL_FORMS_2 = Tags.GetTagFromString("fin2");

		internal static readonly uint FEATURE_TERMINAL_FORMS_3 = Tags.GetTagFromString("fin3");

		internal static readonly uint FEATURE_TERMINAL_FORMS = Tags.GetTagFromString("fina");

		internal static readonly uint FEATURE_FRACTIONS = Tags.GetTagFromString("frac");

		internal static readonly uint FEATURE_FULL_WIDTHS = Tags.GetTagFromString("fwid");

		internal static readonly uint FEATURE_HALF_FORMS = Tags.GetTagFromString("half");

		internal static readonly uint FEATURE_HALANT_FORMS = Tags.GetTagFromString("haln");

		internal static readonly uint FEATURE_ALTERNATE_HALF_WIDTHS = Tags.GetTagFromString("halt");

		internal static readonly uint FEATURE_HISTORICAL_FORMS = Tags.GetTagFromString("hist");

		internal static readonly uint FEATURE_HORIZONTAL_KANA_ALTERNATES = Tags.GetTagFromString("hkna");

		internal static readonly uint FEATURE_HISTORICAL_LIGATURES = Tags.GetTagFromString("hlig");

		internal static readonly uint FEATURE_HANGUL = Tags.GetTagFromString("hngl");

		internal static readonly uint FEATURE_HOJO_KANJI_FORMS = Tags.GetTagFromString("hojo");

		internal static readonly uint FEATURE_HALF_WIDTHS = Tags.GetTagFromString("hwid");

		internal static readonly uint FEATURE_INITIAL_FORMS = Tags.GetTagFromString("init");

		internal static readonly uint FEATURE_ISOLATED_FORMS = Tags.GetTagFromString("isol");

		internal static readonly uint FEATURE_ITALICS = Tags.GetTagFromString("ital");

		internal static readonly uint FEATURE_JUSTIFICATION_ALTERNATES = Tags.GetTagFromString("jalt");

		internal static readonly uint FEATURE_JIS78_FORMS = Tags.GetTagFromString("jp78");

		internal static readonly uint FEATURE_JIS83_FORMS = Tags.GetTagFromString("jp83");

		internal static readonly uint FEATURE_JIS90_FORMS = Tags.GetTagFromString("jp90");

		internal static readonly uint FEATURE_JIS2004_FORMS = Tags.GetTagFromString("jp04");

		internal static readonly uint FEATURE_KERNING = Tags.GetTagFromString("kern");

		internal static readonly uint FEATURE_LEFT_BOUNDS = Tags.GetTagFromString("lfbd");

		internal static readonly uint FEATURE_STANDARD_LIGATURES = Tags.GetTagFromString("liga");

		internal static readonly uint FEATURE_LEADING_JAMO_FORMS = Tags.GetTagFromString("ljmo");

		internal static readonly uint FEATURE_LINING_FIGURES = Tags.GetTagFromString("lnum");

		internal static readonly uint FEATURE_LOCALIZED_FORMS = Tags.GetTagFromString("locl");

		internal static readonly uint FEATURE_LEFT_TO_RIGHT_ALTERNATES = Tags.GetTagFromString("ltra");

		internal static readonly uint FEATURE_LEFT_TO_RIGHT_MIRRORED_FORMS = Tags.GetTagFromString("ltrm");

		internal static readonly uint FEATURE_MARK_POSITIONING = Tags.GetTagFromString("mark");

		internal static readonly uint FEATURE_MEDIAL_FORMS_2 = Tags.GetTagFromString("med2");

		internal static readonly uint FEATURE_MEDIAL_FORMS = Tags.GetTagFromString("medi");

		internal static readonly uint FEATURE_MATHEMATICAL_GREEK = Tags.GetTagFromString("mgrk");

		internal static readonly uint FEATURE_MARK_TO_MARK_POSITIONING = Tags.GetTagFromString("mkmk");

		internal static readonly uint FEATURE_MARK_POSITIONING_VIA_SUBSTITUTION = Tags.GetTagFromString("mset");

		internal static readonly uint FEATURE_ALTERNATE_ANNOTATION_FORMS = Tags.GetTagFromString("nalt");

		internal static readonly uint FEATURE_NLC_KANJI_FORMS = Tags.GetTagFromString("nlck");

		internal static readonly uint FEATURE_NUKTA_FORMS = Tags.GetTagFromString("nukt");

		internal static readonly uint FEATURE_NUMERATORS = Tags.GetTagFromString("numr");

		internal static readonly uint FEATURE_OLDSTYLE_FIGURES = Tags.GetTagFromString("onum");

		internal static readonly uint FEATURE_OPTICAL_BOUNDS = Tags.GetTagFromString("opbd");

		internal static readonly uint FEATURE_ORDINALS = Tags.GetTagFromString("ordn");

		internal static readonly uint FEATURE_ORNAMENTS = Tags.GetTagFromString("ornm");

		internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_WIDTHS = Tags.GetTagFromString("palt");

		internal static readonly uint FEATURE_PETITE_CAPITALS = Tags.GetTagFromString("pcap");

		internal static readonly uint FEATURE_PROPORTIONAL_KANA = Tags.GetTagFromString("pkna");

		internal static readonly uint FEATURE_PROPORTIONAL_FIGURES = Tags.GetTagFromString("pnum");

		internal static readonly uint FEATURE_PRE_BASE_FORMS = Tags.GetTagFromString("pref");

		internal static readonly uint FEATURE_PRE_BASE_SUBSTITUTIONS = Tags.GetTagFromString("pres");

		internal static readonly uint FEATURE_POST_BASE_FORMS = Tags.GetTagFromString("pstf");

		internal static readonly uint FEATURE_POST_BASE_SUBSTITUTIONS = Tags.GetTagFromString("psts");

		internal static readonly uint FEATURE_PROPORTIONAL_WIDTHS = Tags.GetTagFromString("pwid");

		internal static readonly uint FEATURE_QUARTER_WIDTHS = Tags.GetTagFromString("qwid");

		internal static readonly uint FEATURE_RANDOMIZE = Tags.GetTagFromString("rand");

		internal static readonly uint FEATURE_RAKAR_FORMS = Tags.GetTagFromString("rkrf");

		internal static readonly uint FEATURE_REQUIRED_LIGATURES = Tags.GetTagFromString("rlig");

		internal static readonly uint FEATURE_REPH_FORMS = Tags.GetTagFromString("rphf");

		internal static readonly uint FEATURE_RIGHT_BOUNDS = Tags.GetTagFromString("rtbd");

		internal static readonly uint FEATURE_RIGHT_TO_LEFT_ALTERNATES = Tags.GetTagFromString("rtla");

		internal static readonly uint FEATURE_RIGHT_TO_LEFT_MIRRORED_FORMS = Tags.GetTagFromString("rtlm");

		internal static readonly uint FEATURE_RUBY_NOTATION_FORMS = Tags.GetTagFromString("ruby");

		internal static readonly uint FEATURE_STYLISTIC_ALTERNATES = Tags.GetTagFromString("salt");

		internal static readonly uint FEATURE_SCIENTIFIC_INFERIORS = Tags.GetTagFromString("sinf");

		internal static readonly uint FEATURE_OPTICAL_SIZE = Tags.GetTagFromString("size");

		internal static readonly uint FEATURE_SMALL_CAPITALS = Tags.GetTagFromString("smcp");

		internal static readonly uint FEATURE_SIMPLIFIED_FORMS = Tags.GetTagFromString("smpl");

		internal static readonly uint FEATURE_STYLISTIC_SET_1 = Tags.GetTagFromString("ss01");

		internal static readonly uint FEATURE_STYLISTIC_SET_2 = Tags.GetTagFromString("ss02");

		internal static readonly uint FEATURE_STYLISTIC_SET_3 = Tags.GetTagFromString("ss03");

		internal static readonly uint FEATURE_STYLISTIC_SET_4 = Tags.GetTagFromString("ss04");

		internal static readonly uint FEATURE_STYLISTIC_SET_5 = Tags.GetTagFromString("ss05");

		internal static readonly uint FEATURE_STYLISTIC_SET_6 = Tags.GetTagFromString("ss06");

		internal static readonly uint FEATURE_STYLISTIC_SET_7 = Tags.GetTagFromString("ss07");

		internal static readonly uint FEATURE_STYLISTIC_SET_8 = Tags.GetTagFromString("ss08");

		internal static readonly uint FEATURE_STYLISTIC_SET_9 = Tags.GetTagFromString("ss09");

		internal static readonly uint FEATURE_STYLISTIC_SET_10 = Tags.GetTagFromString("ss10");

		internal static readonly uint FEATURE_STYLISTIC_SET_11 = Tags.GetTagFromString("ss11");

		internal static readonly uint FEATURE_STYLISTIC_SET_12 = Tags.GetTagFromString("ss12");

		internal static readonly uint FEATURE_STYLISTIC_SET_13 = Tags.GetTagFromString("ss13");

		internal static readonly uint FEATURE_STYLISTIC_SET_14 = Tags.GetTagFromString("ss14");

		internal static readonly uint FEATURE_STYLISTIC_SET_15 = Tags.GetTagFromString("ss15");

		internal static readonly uint FEATURE_STYLISTIC_SET_16 = Tags.GetTagFromString("ss16");

		internal static readonly uint FEATURE_STYLISTIC_SET_17 = Tags.GetTagFromString("ss17");

		internal static readonly uint FEATURE_STYLISTIC_SET_18 = Tags.GetTagFromString("ss18");

		internal static readonly uint FEATURE_STYLISTIC_SET_19 = Tags.GetTagFromString("ss19");

		internal static readonly uint FEATURE_STYLISTIC_SET_20 = Tags.GetTagFromString("ss20");

		internal static readonly uint FEATURE_SUBSCRIPT = Tags.GetTagFromString("subs");

		internal static readonly uint FEATURE_SUPERSCRIPT = Tags.GetTagFromString("sups");

		internal static readonly uint FEATURE_SWASH = Tags.GetTagFromString("swsh");

		internal static readonly uint FEATURE_TITLING = Tags.GetTagFromString("titl");

		internal static readonly uint FEATURE_TRAILING_JAMO_FORMS = Tags.GetTagFromString("tjmo");

		internal static readonly uint FEATURE_TRADITIONAL_NAME_FORMS = Tags.GetTagFromString("tnam");

		internal static readonly uint FEATURE_TABULAR_FIGURES = Tags.GetTagFromString("tnum");

		internal static readonly uint FEATURE_TRADITIONAL_FORMS = Tags.GetTagFromString("trad");

		internal static readonly uint FEATURE_THIRD_WIDTHS = Tags.GetTagFromString("twid");

		internal static readonly uint FEATURE_UNICASE = Tags.GetTagFromString("unic");

		internal static readonly uint FEATURE_ALTERNATE_VERTICAL_METRICS = Tags.GetTagFromString("valt");

		internal static readonly uint FEATURE_VATTU_VARIANTS = Tags.GetTagFromString("vatu");

		internal static readonly uint FEATURE_VERTICAL_WRITING = Tags.GetTagFromString("vert");

		internal static readonly uint FEATURE_ALTERNATE_VERTICAL_HALF_METRICS = Tags.GetTagFromString("vhal");

		internal static readonly uint FEATURE_VOWEL_JAMO_FORMS = Tags.GetTagFromString("vjmo");

		internal static readonly uint FEATURE_VERTICAL_KANA_ALTERNATES = Tags.GetTagFromString("vkna");

		internal static readonly uint FEATURE_VERTICAL_KERNING = Tags.GetTagFromString("vkrn");

		internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_VERTICAL_METRICS = Tags.GetTagFromString("vpal");

		internal static readonly uint FEATURE_VERTICAL_ALTERNATES_AND_ROTATION = Tags.GetTagFromString("vrt2");

		internal static readonly uint FEATURE_SLASHED_ZERO = Tags.GetTagFromString("zero");
	}
}
