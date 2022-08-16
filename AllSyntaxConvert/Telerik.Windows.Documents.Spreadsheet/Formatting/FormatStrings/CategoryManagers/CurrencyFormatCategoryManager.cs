using System;
using System.Collections.Generic;
using System.Globalization;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers
{
	public static class CurrencyFormatCategoryManager
	{
		public static Dictionary<CultureInfo, string> CultureInfoToCurrencyCode
		{
			get
			{
				return CurrencyFormatCategoryManager.cultureInfoToCurrencyCode;
			}
		}

		public static IEnumerable<CurrencyInfo> CurrencyInfos
		{
			get
			{
				if (!CurrencyFormatCategoryManager.currencyInfos[1].Equals(CurrencyInfo.Symbol))
				{
					CurrencyFormatCategoryManager.currencyInfos[1] = CurrencyInfo.Symbol;
				}
				return CurrencyFormatCategoryManager.currencyInfos;
			}
		}

		static CurrencyFormatCategoryManager()
		{
			CurrencyFormatCategoryManager.InitCultureInfoToCurrencyCode();
			CurrencyFormatCategoryManager.currencyInfos = new List<CurrencyInfo>();
			CurrencyFormatCategoryManager.InitCurrencyInfos();
		}

		static void InitCurrencyInfos()
		{
			CurrencyFormatCategoryManager.AddCurrencyInfo("None", null);
			CurrencyFormatCategoryManager.AddCurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null);
			foreach (CultureInfo cultureInfo in CurrencyFormatCategoryManager.CultureInfoToCurrencyCode.Keys)
			{
				CurrencyFormatCategoryManager.AddCurrencyInfo(cultureInfo.NumberFormat.CurrencySymbol, cultureInfo);
			}
		}

		static void AddCurrencyInfo(string currencySymbol, CultureInfo culture)
		{
			CurrencyFormatCategoryManager.currencyInfos.Add(new CurrencyInfo(currencySymbol, culture));
		}

		static void InitCultureInfoToCurrencyCode()
		{
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("af", "[$R-436]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("gsw", "[$€-484]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("am", "[$ETB-45E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ar-DZ", "[$د.ج.\u200f-1401]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ar-SA", "[$ر.س.\u200f-401]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("hy", "[$դր.-42B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("as", "[$ট-44D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("az-Cyrl", "[$ман.-82C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("az-Latn", "[$man.-42C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ba", "[$һ.-46D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("eu", "[$€-42D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("be", "[$р.-423]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("bn-BD", "[$৳-845]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("bn-IN", "[$ট\u09be-445]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("bs-Cyrl", "[$КМ-201A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("bs-Latn", "[$KM-141A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("br", "[$€-47E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("bg", "[$лв.-402]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ca", "[$€-403]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("zh-HK", "[$HK$-C04]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("zh-MO", "[$MOP-1404]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("zh-CN", "[$¥-804]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("zh-SG", "[$$-1004]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("zh-TW", "[$NT$-404]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("co", "[$€-483]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("hr-BA", "[$KM-101A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("hr-HR", "[$kn-41A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("cs", "[$Kč-405]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("da", "[$kr.-406]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("prs-AF", "[$؋-48C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("dv", "[$ރ.-465]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("nl-BE", "[$€-813]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("nl-NL", "[$€-413]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-AU", "[$$-C09]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-BZ", "[$BZ$-2809]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-CA", "[$$-1009]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-029", "[$$-2409]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-IN", "[$Rs.-4009]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-IE", "[$€-1809]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-JM", "[$J$-2009]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-MY", "[$RM-4409]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-NZ", "[$$-1409]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-PH", "[$Php-3409]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-SG", "[$$-4809]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-US", "[$$-409]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-ZA", "[$R-1C09]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-TT", "[$TT$-2C09]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-GB", "[$£-809]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("en-ZW", "[$Z$-3009]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("et", "[$kr-425]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fo", "[$kr.-438]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fil", "[$PhP-464]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fi", "[$€-40B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fr-BE", "[$€-80C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fr-CA", "[$$-C0C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fr-FR", "[$€-40C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fr-LU", "[$€-140C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fr-MC", "[$€-180C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fr-CH", "[$fr.-100C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fy", "[$€-462]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("gl", "[$€-456]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ka", "[$Lari-437]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("de-AT", "[$€-C07]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("de-DE", "[$€-407]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("de-LI", "[$CHF-1407]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("de-LU", "[$€-1007]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("de-CH", "[$Fr.-807]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("el", "[$€-408]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("kl", "[$kr.-46F]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("gu", "[$ર\u0ac2-447]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ha", "[$N-468]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("he", "[$₪-40D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("hi", "[$र\u0941-439]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("hu", "[$Ft-40E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("is", "[$kr.-40F]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ig", "[$N-470]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("id", "[$Rp-421]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("iu-Latn", "[$$-85D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("iu-Cans", "[$$-45D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ga", "[$€-83C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("xh", "[$R-434]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("zu", "[$R-435]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("it-IT", "[$€-410]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("it-CH", "[$fr.-810]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ja", "[$¥-411]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("kn", "[$ರ\u0cc2-44B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("kk", "[$Т-43F]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("km", "[$៛-453]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("qut", "[$Q-486]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("rw", "[$RWF-487]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("kok", "[$र\u0941-457]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ko", "[$₩-412]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ky", "[$сом-440]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("lo", "[$₭-454]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("lv", "[$Ls-426]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("lt", "[$Lt-427]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("dsb", "[$€-82E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("lb", "[$€-46E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("mk", "[$ден.-42F]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ms-BN", "[$$-83E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ms-MY", "[$RM-43E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ml", "[$ക-44C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("mt", "[$€-43A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("mi", "[$$-481]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("arn", "[$$-47A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("mr", "[$र\u0941-44E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("moh", "[$$-47C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("mn-Cyrl", "[$₮-450]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("mn-Mong-CN", "[$¥-850]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ne", "[$र\u0941-461]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("nb", "[$kr-414]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("nn", "[$kr-814]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("oc", "[$€-482]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("or", "[$ଟ-448]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ps", "[$؋-463]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("fa", "[$ريال-429]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("pl", "[$zł-415]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("pt-BR", "[$R$-416]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("pt-PT", "[$€-816]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("pa", "[$ਰ\u0a41-446]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("quz-BO", "[$$b-46B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("quz-EC", "[$$-86B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("quz-PE", "[$S/.-C6B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ro", "[$lei-418]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("rm", "[$fr.-417]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ru", "[$р.-419]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("smn-FI", "[$€-243B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("smj-NO", "[$kr-103B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("smj-SE", "[$kr-143B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("se-FI", "[$€-C3B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("se-NO", "[$kr-43B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("se-SE", "[$kr-83B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sms-FI", "[$€-203B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sma-NO", "[$kr-183B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sma-SE", "[$kr-1C3B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sa", "[$र\u0941-44F]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("gd-GB", "[$£-491]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sr-Cyrl-BA", "[$КМ-1C1A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sr-Cyrl-ME", "[$€-301A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sr-Latn-BA", "[$KM-181A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sr-Latn-ME", "[$€-2C1A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("nso", "[$R-46C]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("tn", "[$R-432]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("si", "[$ර\u0dd4.-45B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sk", "[$€-41B]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sl", "[$€-424]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-AR", "[$$-2C0A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-BO", "[$$b-400A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-CL", "[$$-340A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-CO", "[$$-240A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-CR", "[$₡-140A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-DO", "[$RD$-1C0A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-EC", "[$$-300A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-SV", "[$$-440A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-GT", "[$Q-100A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-HN", "[$L.-480A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-MX", "[$$-80A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-NI", "[$C$-4C0A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-PA", "[$B/.-180A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-PY", "[$Gs-3C0A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-PE", "[$S/.-280A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-PR", "[$$-500A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-ES_tradnl", "[$€-40A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-US", "[$$-540A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("es-UY", "[$$U-380A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sw", "[$S-441]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sv-FI", "[$€-81D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("sv-SE", "[$kr-41D]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("syr", "[$ل.س.\u200f-45A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("tg", "[$т.р.-428]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("tzm-Latn-DZ", "[$DZD-85F]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ta", "[$ர\u0bc2-449]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("tt", "[$р.-444]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("te", "[$ర\u0c42-44A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("th", "[$฿-41E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("bo-CN", "[$¥-451]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("tr", "[$TL-41F]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("tk", "[$m.-442]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("uk", "[$₴-422]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("hsb", "[$€-42E]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ur", "[$Rs-420]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ug", "[$¥-480]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("uz-Cyrl", "[$сўм-843]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("uz-Latn", "[$so'm-443]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("vi", "[$₫-42A]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("cy", "[$£-452]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("wo", "[$XOF-488]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("ii-CN", "[$¥-478]");
			CurrencyFormatCategoryManager.AddCultureInfoToCurrencyCode("yo", "[$N-46A]");
		}

		static void AddCultureInfoToCurrencyCode(string cultureCode, string currencyCode)
		{
			try
			{
				CurrencyFormatCategoryManager.cultureInfoToCurrencyCode.Add(new CultureInfo(cultureCode), currencyCode);
			}
			catch (CultureNotFoundException)
			{
			}
		}

		const int CurrencyInfosSymbolIndex = 1;

		internal const int CurrencyInfosNoneIndex = 0;

		static readonly Dictionary<CultureInfo, string> cultureInfoToCurrencyCode = new Dictionary<CultureInfo, string>();

		static readonly List<CurrencyInfo> currencyInfos;
	}
}
