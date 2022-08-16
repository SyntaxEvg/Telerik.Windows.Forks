using System;
using System.Collections.Generic;
using System.Globalization;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	static class LocaleCodeToCultureInfoResolver
	{
		public static Dictionary<string, CultureInfo> LocaleCodeToCultureInfo
		{
			get
			{
				return LocaleCodeToCultureInfoResolver.keyToCultureInfo;
			}
		}

		static LocaleCodeToCultureInfoResolver()
		{
			LocaleCodeToCultureInfoResolver.AddCulture("$-409", "en-US");
			LocaleCodeToCultureInfoResolver.AddCulture("$-436", "af");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10484", "gsw");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1045E", "am");
			LocaleCodeToCultureInfoResolver.AddCulture("$-2010000", "ar-DZ");
			LocaleCodeToCultureInfoResolver.AddCulture("$-2170000", "ar-SA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-42B", "hy");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1044D", "as");
			LocaleCodeToCultureInfoResolver.AddCulture("$-82C", "az-Cyrl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-42C", "az-Latn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1046D", "ba");
			LocaleCodeToCultureInfoResolver.AddCulture("$-42D", "eu");
			LocaleCodeToCultureInfoResolver.AddCulture("$-FC23", "be");
			LocaleCodeToCultureInfoResolver.AddCulture("$-845", "bn-BD");
			LocaleCodeToCultureInfoResolver.AddCulture("$-445", "bn-IN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1201A", "bs-Cyrl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1141A", "bs-Latn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1047E", "br");
			LocaleCodeToCultureInfoResolver.AddCulture("$-402", "bg");
			LocaleCodeToCultureInfoResolver.AddCulture("$-403", "ca");
			LocaleCodeToCultureInfoResolver.AddCulture("$-C04", "zh-HK");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1404", "zh-MO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-804", "zh-CN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1004", "zh-SG");
			LocaleCodeToCultureInfoResolver.AddCulture("$-404", "zh-TW");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10483", "co");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1101A", "hr-BA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-41A", "hr-HR");
			LocaleCodeToCultureInfoResolver.AddCulture("$-405", "cs");
			LocaleCodeToCultureInfoResolver.AddCulture("$-406", "da");
			LocaleCodeToCultureInfoResolver.AddCulture("$-6048C", "prs-AF");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1060465", "dv");
			LocaleCodeToCultureInfoResolver.AddCulture("$-813", "nl-BE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-413", "nl-NL");
			LocaleCodeToCultureInfoResolver.AddCulture("$-C09", "en-AU");
			LocaleCodeToCultureInfoResolver.AddCulture("$-2809", "en-BZ");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1009", "en-CA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-2409", "en-029");
			LocaleCodeToCultureInfoResolver.AddCulture("$-14009", "en-IN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1809", "en-IE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-2009", "en-JM");
			LocaleCodeToCultureInfoResolver.AddCulture("$-14409", "en-MY");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1409", "en-NZ");
			LocaleCodeToCultureInfoResolver.AddCulture("$-3409", "en-PH");
			LocaleCodeToCultureInfoResolver.AddCulture("$-14809", "en-SG");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1C09", "en-ZA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-2C09", "en-TT");
			LocaleCodeToCultureInfoResolver.AddCulture("$-809", "en-GB");
			LocaleCodeToCultureInfoResolver.AddCulture("$-3009", "en-ZW");
			LocaleCodeToCultureInfoResolver.AddCulture("$-425", "et");
			LocaleCodeToCultureInfoResolver.AddCulture("$-438", "fo");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10464", "fil");
			LocaleCodeToCultureInfoResolver.AddCulture("$-40B", "fi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-80C", "fr-BE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-C0C", "fr-CA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-40C", "fr-FR");
			LocaleCodeToCultureInfoResolver.AddCulture("$-140C", "fr-LU");
			LocaleCodeToCultureInfoResolver.AddCulture("$-180C", "fr-MC");
			LocaleCodeToCultureInfoResolver.AddCulture("$-100C", "fr-CH");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10462", "fy");
			LocaleCodeToCultureInfoResolver.AddCulture("$-456", "gl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-437", "ka");
			LocaleCodeToCultureInfoResolver.AddCulture("$-C07", "de-AT");
			LocaleCodeToCultureInfoResolver.AddCulture("$-407", "de-DE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1407", "de-LI");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1007", "de-LU");
			LocaleCodeToCultureInfoResolver.AddCulture("$-807", "de-CH");
			LocaleCodeToCultureInfoResolver.AddCulture("$-408", "el");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1046F", "kl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-447", "gu");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10468", "ha");
			LocaleCodeToCultureInfoResolver.AddCulture("$-101040D", "he");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1010439", "hi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-4010000", "hi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-4010439", "hi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1010409", "hi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-40E", "hu");
			LocaleCodeToCultureInfoResolver.AddCulture("$-40F", "is");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10470", "ig");
			LocaleCodeToCultureInfoResolver.AddCulture("$-421", "id");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1085D", "iu-Latn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1045D", "iu-Cans");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1083C", "ga");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10434", "xh");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10435", "zu");
			LocaleCodeToCultureInfoResolver.AddCulture("$-410", "it-IT");
			LocaleCodeToCultureInfoResolver.AddCulture("$-810", "it-CH");
			LocaleCodeToCultureInfoResolver.AddCulture("$-411", "ja");
			LocaleCodeToCultureInfoResolver.AddCulture("$-44B", "kn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-43F", "kk");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10453", "km");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10486", "qut");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10487", "rw");
			LocaleCodeToCultureInfoResolver.AddCulture("$-457", "kok");
			LocaleCodeToCultureInfoResolver.AddCulture("$-412", "ko");
			LocaleCodeToCultureInfoResolver.AddCulture("$-440", "ky");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10454", "lo");
			LocaleCodeToCultureInfoResolver.AddCulture("$-426", "lv");
			LocaleCodeToCultureInfoResolver.AddCulture("$-427", "lt");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1082E", "dsb");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1046E", "lb");
			LocaleCodeToCultureInfoResolver.AddCulture("$-42F", "mk");
			LocaleCodeToCultureInfoResolver.AddCulture("$-83E", "ms-BN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-43E", "ms-MY");
			LocaleCodeToCultureInfoResolver.AddCulture("$-44C", "ml");
			LocaleCodeToCultureInfoResolver.AddCulture("$-C00044C", "ml");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1043A", "mt");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10481", "mi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1047A", "arn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-44E", "mr");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1047C", "moh");
			LocaleCodeToCultureInfoResolver.AddCulture("$-450", "mn-Cyrl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10850", "mn-Mong-CN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10461", "ne");
			LocaleCodeToCultureInfoResolver.AddCulture("$-414", "nb");
			LocaleCodeToCultureInfoResolver.AddCulture("$-814", "nn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10482", "oc");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10448", "or");
			LocaleCodeToCultureInfoResolver.AddCulture("$-60463", "ps");
			LocaleCodeToCultureInfoResolver.AddCulture("$-30C0000", "fa");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10C0000", "fa");
			LocaleCodeToCultureInfoResolver.AddCulture("$-415", "pl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-416", "pt-BR");
			LocaleCodeToCultureInfoResolver.AddCulture("$-816", "pt-PT");
			LocaleCodeToCultureInfoResolver.AddCulture("$-6000446", "pa");
			LocaleCodeToCultureInfoResolver.AddCulture("$-446", "pa");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1046B", "quz-BO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1086B", "quz-EC");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10C6B", "quz-PE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-418", "ro");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10417", "rm");
			LocaleCodeToCultureInfoResolver.AddCulture("$-419", "ru");
			LocaleCodeToCultureInfoResolver.AddCulture("$-FC19", "ru");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10485", "sah-RU");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1243B", "smn-FI");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1103B", "smj-NO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1143B", "smj-SE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10C3B", "se-FI");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1043B", "se-NO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1083B", "se-SE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1203B", "sms-FI");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1183B", "sma-NO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-11C3B", "sma-SE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-44F", "sa");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10491", "gd-GB");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1C1A", "sr-Cyrl-BA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-C1A", "sr-Cyrl-ME");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1181A", "sr-Latn-BA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-81A", "sr-Latn-ME");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1046C", "nso");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10432", "tn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1045B", "si");
			LocaleCodeToCultureInfoResolver.AddCulture("$-41B", "sk");
			LocaleCodeToCultureInfoResolver.AddCulture("$-424", "sl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-2C0A", "es-AR");
			LocaleCodeToCultureInfoResolver.AddCulture("$-400A", "es-BO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-340A", "es-CL");
			LocaleCodeToCultureInfoResolver.AddCulture("$-240A", "es-CO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-140A", "es-CR");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1C0A", "es-DO");
			LocaleCodeToCultureInfoResolver.AddCulture("$-300A", "es-EC");
			LocaleCodeToCultureInfoResolver.AddCulture("$-440A", "es-SV");
			LocaleCodeToCultureInfoResolver.AddCulture("$-100A", "es-GT");
			LocaleCodeToCultureInfoResolver.AddCulture("$-480A", "es-HN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-80A", "es-MX");
			LocaleCodeToCultureInfoResolver.AddCulture("$-4C0A", "es-NI");
			LocaleCodeToCultureInfoResolver.AddCulture("$-180A", "es-PA");
			LocaleCodeToCultureInfoResolver.AddCulture("$-3C0A", "es-PY");
			LocaleCodeToCultureInfoResolver.AddCulture("$-280A", "es-PE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-500A", "es-PR");
			LocaleCodeToCultureInfoResolver.AddCulture("$-40A", "es-ES_tradnl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1540A", "es-US");
			LocaleCodeToCultureInfoResolver.AddCulture("$-380A", "es-UY");
			LocaleCodeToCultureInfoResolver.AddCulture("$-200A", "es-VE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-441", "sw");
			LocaleCodeToCultureInfoResolver.AddCulture("$-81D", "sv-FI");
			LocaleCodeToCultureInfoResolver.AddCulture("$-41D", "sv-SE");
			LocaleCodeToCultureInfoResolver.AddCulture("$-45A", "syr");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10428", "tg");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1085F", "tzm-Latn-DZ");
			LocaleCodeToCultureInfoResolver.AddCulture("$-9000449", "ta");
			LocaleCodeToCultureInfoResolver.AddCulture("$-449", "ta");
			LocaleCodeToCultureInfoResolver.AddCulture("$-444", "tt");
			LocaleCodeToCultureInfoResolver.AddCulture("$-A00044A", "te");
			LocaleCodeToCultureInfoResolver.AddCulture("$-44A", "te");
			LocaleCodeToCultureInfoResolver.AddCulture("$-107041E", "th");
			LocaleCodeToCultureInfoResolver.AddCulture("$-D07041E", "th");
			LocaleCodeToCultureInfoResolver.AddCulture("$-D070000", "th");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1070000", "th");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1070409", "th");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10451", "bo-CN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-41F", "tr");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10442", "tk");
			LocaleCodeToCultureInfoResolver.AddCulture("$-FC22", "uk");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1042E", "hsb");
			LocaleCodeToCultureInfoResolver.AddCulture("$-420", "ur");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10480", "ug");
			LocaleCodeToCultureInfoResolver.AddCulture("$-843", "uz-Cyrl");
			LocaleCodeToCultureInfoResolver.AddCulture("$-443", "uz-Latn");
			LocaleCodeToCultureInfoResolver.AddCulture("$-101040C", "vi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-101042A", "vi");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10452", "cy");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10488", "wo");
			LocaleCodeToCultureInfoResolver.AddCulture("$-10478", "ii-CN");
			LocaleCodeToCultureInfoResolver.AddCulture("$-1046A", "yo");
		}

		public static CultureInfo GetCulture(string key)
		{
			CultureInfo result;
			if (LocaleCodeToCultureInfoResolver.keyToCultureInfo.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}

		public static CultureInfo GetCultureOrDefault(string key)
		{
			CultureInfo result;
			if (LocaleCodeToCultureInfoResolver.keyToCultureInfo.TryGetValue(key, out result))
			{
				return result;
			}
			return new CultureInfo("en-US");
		}

		static void AddCulture(string key, string cultureCode)
		{
			try
			{
				LocaleCodeToCultureInfoResolver.keyToCultureInfo.Add(key, new CultureInfo(cultureCode));
			}
			catch (CultureNotFoundException)
			{
			}
		}

		static readonly Dictionary<string, CultureInfo> keyToCultureInfo = new Dictionary<string, CultureInfo>();
	}
}
