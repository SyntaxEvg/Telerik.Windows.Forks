using System;
using System.Collections.Generic;
using System.Globalization;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers
{
	public static class DateTimeFormatCategoryManager
	{
		public static Dictionary<CultureInfo, IList<string>> CultureInfoToDateFormats
		{
			get
			{
				return DateTimeFormatCategoryManager.cultureInfoToDateFormats;
			}
		}

		public static Dictionary<CultureInfo, IList<string>> CultureInfoToTimeFormats
		{
			get
			{
				return DateTimeFormatCategoryManager.cultureInfoToTimeFormats;
			}
		}

		static DateTimeFormatCategoryManager()
		{
			DateTimeFormatCategoryManager.InitCultureInfoToDateTimeFormats();
		}

		static void InitCultureInfoToDateTimeFormats()
		{
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-US", new List<string>
			{
				"m/d/yyyy", "[$-F800]dddd, mmmm dd, yyyy", "m/d;@", "m/d/yy;@", "mm/dd/yy;@", "[$-409]d-mmm;@", "[$-409]d-mmm-yy;@", "[$-409]dd-mmm-yy;@", "[$-409]mmm-yy;@", "[$-409]mmmm-yy;@",
				"[$-409]mmmm d, yyyy;@", "[$-409]m/d/yy h:mm AM/PM;@", "m/d/yy h:mm", "m/d/yyyy h:mm", "[$-409]mmmmm;@", "[$-409]mmmmm-yy;@", "[$-409]d-mmm-yyyy;@"
			}, new List<string> { "h:mm:ss AM/PM", "h:mm", "[$-409]h:mm AM/PM;@", "h:mm:ss", "[$-409]h:mm:ss AM/PM;@", "mm:ss.0", "[h]:mm:ss;@", "[$-409]m/d/yy h:mm AM/PM;@", "m/d/yy h:mm" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("af", new List<string> { "yyyy/mm/dd;@", "yy/mm/dd;@", "yyyy-mm-dd;@", "[$-436]dd mmmm yyyy;@" }, new List<string> { "[$-409]hh:mm:ss AM/PM;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("gsw", new List<string> { "[$-10484]dd/mm/yyyy;@", "[$-10484]dd/mm/yy;@", "[$-10484]dd.mm.yy;@", "[$-10484]dd-mm-yy;@", "[$-10484]yyyy-mm-dd;@", "[$-10484]dddd d mmmm yyyy;@", "[$-10484]d mmm yy;@", "[$-10484]d mmmm yyyy;@" }, new List<string> { "[$-10484]hh:mm:ss;@", "[$-10484]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("am", new List<string> { "[$-1045E]d/m/yyyy;@", "[$-1045E]yyyy-mm-dd;@", "[$-1045E]dddd \"፣\" mmmm d \"ቀን\" yyyy;@" }, new List<string> { "[$-1045E]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ar-DZ", new List<string> { "[$-1010000]d/m/yyyy;@", "[$-1010000]yyyy/mm/dd;@", "[$-1010401]d/m/yyyy h:mm AM/PM;@", "[$-1010409]d/m/yyyy h:mm AM/PM;@", "[$-2010000]d/mm/yyyy;@", "[$-2010000]yyyy/mm/dd;@", "[$-2010401]d/mm/yyyy h:mm AM/PM;@" }, new List<string> { "[$-1000000]h:mm:ss;@", "[$-1000401]h:mm AM/PM;@", "[$-1000409]h:mm AM/PM;@", "[$-2000000]h:mm:ss;@", "[$-2000401]h:mm AM/PM;@", "[$-2000409]h:mm AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("hy", new List<string>
			{
				"dd.mm.yyyy;@", "dd.mm.yy;@", "d/mm/yyyy;@", "dd/mm/yyyy;@", "[$-42B]d/mmm/yyyy;@", "[$-42B]dd/mmm/yyyy;@", "yyyy-mm-dd;@", "[$-42B]d mmmm, yyyy;@", "[$-42B]dddd, d mmmm yyyy;@", "[$-42B]dddd, dd mmmm yyyy;@",
				"[$-42B]dd mmmm yyyy;@", "[$-42B]d-mmm-yyyy;@", "[$-42B]dd-mmm-yyyy;@", "[$-42B]ddd, d-mmmm-yyyy;@", "[$-42B]ddd, dd-mmmm-yyyy;@"
			}, new List<string> { "h:mm:ss;@", "hh:mm:ss;@", "[$-409]h:mm:ss AM/PM;@", "[$-409]hh:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("as", new List<string> { "[$-1044D]dd-mm-yyyy;@", "[$-1044D]yyyy,mmmm dd, dddd;@" }, new List<string> { "[$-1044D]AM/PM h:mm:ss;@", "[$-1044D]AM/PM hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("az-Cyrl", new List<string> { "dd.mm.yyyy;@", "dd.mm.yy;@", "d.m.yy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "[$-82C]d mmmm yyyy;@", "[$-82C]dd mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("az-Latn", new List<string> { "dd.mm.yyyy;@", "dd.mm.yy;@", "d.m.yy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "[$-42C]d mmmm yyyy;@", "[$-42C]dd mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ba", new List<string> { "[$-1046D]dd.mm.yy;@", "[$-1046D]yyyy-mm-dd;@", "[$-1046D]d mmmm yyyy \"й\";@" }, new List<string> { "[$-1046D]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("eu", new List<string>
			{
				"m/d;@", "yy/m/d;@", "yy/mm/dd;@", "[$-42D]mmm-d;@", "[$-42D]yy-mmm-d;@", "[$-42D]yy-mmm-dd;@", "[$-42D]yy-mmm;@", "[$-42D]yy-mmmm;@", "[$-42D]yyyy\"(e)ko\" mmmm\"ren\" d\"a\";@", "[$-42D]yy/m/d/ h:mm AM/PM;@",
				"yy/m/d/ h:mm;@", "[$-42D]mmmmm;@", "[$-42D]yy-mmmmm;@", "yyyy/m/d;@", "[$-42D]yyyy-mmm-d;@", "yyyy.mm.dd;@", "[$-42D]yyyy\"ko\" mmmm\"ren\" d\"a\";@", "[$-42D]yyyy\"ko\" mmmm\"k\" d\"a\";@", "[$-42D]yyyy\"ko\" mmmm;@"
			}, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("be", new List<string> { "dd.mm.yyyy;@", "dd.mm.yy;@", "yyyy-mm-dd;@", "[$-FC23]d mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("bn-BD", new List<string> { "[$-445]dd mmmm yyyy;@", "[$-445]d mmmm yyyy;@" }, new List<string> { "[$-10445]hh.mm.ss;@", "[$-10445]h.mm.ss;@", "[$-10445]AM/PM hh.mm.ss;@", "[$-10445]AM/PM h.mm.ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("bn-IN", new List<string> { "[$-845]dd mmmm yyyy;@", "[$-845]d mmmm yyyy;@" }, new List<string> { "[$-10845]hh.mm.ss;@", "[$-10845]h.mm.ss;@", "[$-10845]AM/PM hh.mm.ss;@", "[$-10845]AM/PM h.mm.ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("bs-Cyrl", new List<string>
			{
				"[$-1201A]d.m.yyyy;@", "[$-1201A]d.m.yy;@", "[$-1201A]d. m. yyyy;@", "[$-1201A]dd.mm.yyyy;@", "[$-1201A]d. m. yy;@", "[$-1201A]dd.mm.yy;@", "[$-1201A]dd. mm. yy;@", "[$-1201A]yyyy-mm-dd;@", "[$-1201A]d. mmmm yyyy;@", "[$-1201A]dd. mmmm yyyy;@",
				"[$-1201A]dddd, d. mmmm yyyy;@"
			}, new List<string> { "[$-1201A]h:mm:ss;@", "[$-1201A]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("bs-Latn", new List<string>
			{
				"[$-1141A]d.m.yyyy;@", "[$-1141A]d.m.yy;@", "[$-1141A]d. m. yyyy;@", "[$-1141A]dd.mm.yyyy;@", "[$-1141A]d. m. yy;@", "[$-1141A]dd.mm.yy;@", "[$-1141A]dd. mm. yy;@", "[$-1141A]yyyy-mm-dd;@", "[$-1141A]d. mmmm yyyy;@", "[$-1141A]dd. mmmm yyyy;@",
				"[$-1141A]dddd, d. mmmm yyyy;@"
			}, new List<string> { "[$-1141A]h:mm:ss;@", "[$-1141A]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("br", new List<string> { "[$-1047E]dd/mm/yyyy;@", "[$-1047E]dd/mm/yy;@", "[$-1047E]dd.mm.yy;@", "[$-1047E]dd-mm-yy;@", "[$-1047E]yyyy-mm-dd;@", "[$-1047E]dddd d mmmm yyyy;@", "[$-1047E]d mmm yy;@", "[$-1047E]d mmmm yyyy;@" }, new List<string> { "[$-1047E]hh:mm:ss;@", "[$-1047E]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("bg-BG", new List<string> { "[$-1047E]dd/mm/yyyy;@", "[$-1047E]dd/mm/yy;@", "[$-1047E]dd.mm.yy;@", "[$-1047E]dd-mm-yy;@", "[$-1047E]yyyy-mm-dd;@", "[$-1047E]dddd d mmmm yyyy;@", "[$-1047E]d mmm yy;@", "[$-1047E]d mmmm yyyy;@" }, new List<string> { "[$-1047E]hh:mm:ss;@", "[$-1047E]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ca", new List<string> { "[$-403]dddd, d\" / \"mmmm\" / \"yyyy;@", "[$-403]d]\"/\"mmmm\"/\"yyyy;@", "[$-403]d\" \"mmmm\" \"yyyy;@" }, new List<string> { "hh:mm:ss;@", "hh:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("zh-HK", new List<string> { "[$-C04]dddd, d mmmm, yyyy;@", "[$-C04]d mmmm, yyyy;@", "[$-C04]dddd yyyy mm dd;@", "" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("zh-MO", new List<string> { "[$-1404]dddd, d mmmm, yyyy;@", "[$-1404]d mmmm, yyyy;@", "[$-1404]dddd yyyy mm dd;@", "" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("zh-CN", new List<string> { "[DBNum1][$-804]yyyy\"年\"m\"月\"d\"日\";@", "[DBNum1][$-804]yyyy]\"年\"m\"月\";@", "[DBNum1][$-804]m\"月\"d\"日\";@", "yyyy\"年\"m\"月\"d\"日\";@", "yyyy\"年\"m\"月\";@", "m\"月\"d\"日\";@", "[$-804]aaaa;@", "[$-804]aaa;@" }, new List<string> { "h\"时\"mm\"分\";@", "h\"时\"mm\"分\"ss\"秒\";@", "上午/下午h\"时\"mm\"分\";@", "上午/下午h\"时\"mm\"分\"ss\"秒\";@", "[DBNum1][$-804]h\"时\"mm\"分\";@", "[DBNum1][$-804]上午/下午h\"时\"mm\"分\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("zh-SG", new List<string> { "[$-1004]dddd, d mmmm, yyyy;@", "[$-1004]d mmmm, yyyy;@", "[$-1004]dddd yyyy mm dd;@", "yyyy mm dd;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("zh-TW", new List<string> { "yyyy\"年\"m\"月\"d\"日\";@", "m\"月\"d\"日\";@", "[DBNum1][$-404]yyyy\"年\"m\"月\"d\"日\";@", "[DBNum1][$-404]m\"月\"d\"日\";@", "[$-404]aaaa;@", "[$-404]aaa;@" }, new List<string> { "h\"時\"mm\"分\";@", "h\"時\"mm\"分\"ss\"秒\";@", "上午/下午h\"時\"mm\"分\";@", "上午/下午h\"時\"mm\"分\"ss\"秒\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("co", new List<string> { "[$-10483]dd/mm/yyyy;@", "[$-10483]dd/mm/yy;@", "[$-10483]dd.mm.yy;@", "[$-10483]dd-mm-yy;@", "[$-10483]yyyy-mm-dd;@", "[$-10483]dddd d mmmm yyyy;@", "[$-10483]d mmm yy;@", "[$-10483]d mmmm yyyy;@" }, new List<string> { "[$-10483]hh:mm:ss;@", "[$-10483]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("hr-BA", new List<string>
			{
				"[$-1101A]d.m.yyyy.;@", "[$-1101A]d.m.yy.;@", "[$-1101A]d. m. yyyy.;@", "[$-1101A]dd.mm.yyyy.;@", "[$-1101A]d. m. yy.;@", "[$-1101A]dd.mm.yy.;@", "[$-1101A]dd. mm. yy.;@", "[$-1101A]yyyy-mm-dd;@", "[$-1101A]d. mmmm yyyy.;@", "[$-1101A]dd. mmmm yyyy.;@",
				"[$-1101A]dddd, d. mmmm yyyy.;@"
			}, new List<string> { "[$-1101A]h:mm:ss;@", "[$-1101A]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("hr-HR", new List<string>
			{
				"d.m.;@", "d.m.yy.;@", "dd.mm.yy.;@", "[$-41A]d-mmm;@", "[$-41A]d-mmm-yy;@", "[$-41A]dd-mmm-yy;@", "[$-41A]mmm-yy;@", "[$-41A]mmmm-yy;@", "[$-41A]d. mmmm yyyy.;@", "[$-409]d.m.yy. h:mm AM/PM;@",
				"d.m.yy. h:mm;@", "[$-41A]mmmmm;@", "[$-41A]mmmmm-yy.;@", "d.m.yyyy.;@", "[$-41A]d-mmm-yyyy.;@"
			}, new List<string> { "d.m.yy. h:mm AM/PM;@", "d.m.yy. h:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("cs", new List<string> { "[$-405]mmmm yy;@", "[$-405]d. mmmm yyyy;@", "[$-405]mmmmm;@", "[$-405]mmmmm-yy;@" }, new List<string> { "d.m.yy. h:mm AM/PM;@", "d.m.yy. h:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("da", new List<string> { "[$-406]d. mmmm yyyy;@", "[$-406]mmmm yyyy;@", "[$-406]mmmm yy;@", "[$-406]mmmmm;@", "[$-406]mmmmm-yy;@" }, new List<string> { "yyyy-mm-dd hh:mm;@", "mm-dd-yy hh:mm:ss;@", "dd-mm-yy hh:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("prs-AF", new List<string> { "[$-6048C]dd/mm/yy;@", "[$-6048C]dd/mm/yyyy;@", "[$-6048C]dd/mmmm/yyyy;@", "[$-6048C]dddd, dd mmmm, yyyy;@", "[$-6048C]dd/mm/yyyy \"هـ\";@" }, new List<string> { "[$-6048C]h:mm:ss AM/PM;@", "[$-6048C]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("dv", new List<string> { "[$-1060000]dd/mm/yy;@", "[$-1060000]dd/mm/yyyy;@", "[$-1060465]dd/mmmm/yyyy;@", "[$-1060465]dddd, dd mmmm, yyyy;@" }, new List<string> { "[$-465]hh:mm AM/PM;@", "hh:mm:ss;@", "[$-465]hh:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("nl-BE", new List<string> { "[$-813]dddd d mmmm yyyy;@", "[$-813]dd-mmm-yy;@", "[$-813]d mmmm yyyy;@", "[$-813]dd mmm yy;@" }, new List<string> { "h.mm\" u.\";@", "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("nl-NL", new List<string> { "[$-413]d-mmm;@", "[$-413]d-mmm-yy;@", "[$-413]dd-mmm-yy;@", "[$-413]mmm-yy;@", "[$-413]mmmm-yy;@", "[$-413]d mmmm yyyy;@", "[$-413]mmmmm;@", "[$-413]mmmmm-yy;@", "[$-413]d-mmm-yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-AU", new List<string> { "[$-C09]dd-mmm-yy;@", "[$-C09]dd-mmmm-yyyy;@", "[$-C09]dddd, d mmmm yyyy;@", "[$-C09]d mmmm yyyy;@" }, new List<string> { "[$-409]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-BZ", new List<string> { "[$-2809]dddd, dd mmmm yyyy;@" }, new List<string> { "[$-409]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-CA", new List<string> { "[$-1009]mmmm d, yyyy;@", "[$-1009]d-mmm-yy;@" }, new List<string> { "[$-409]h:mm:ss AM/PM;@", "[$-409]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-029", new List<string> { "[$-2409]dddd, mmmm dd, yyyy;@", "[$-2409]mmmm dd, yyyy;@", "[$-2409]dddd, dd mmmm, yyyy;@", "[$-2409]dd mmmm, yyyy;@" }, new List<string> { "[$-409]h:mm:ss AM/PM;@", "[$-409]hh:mm:ss AM/PM;@", "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-IN", new List<string> { "[$-14009]d mmmm yyyy;@", "[$-14009]dd mmmm yyyy;@", "[$-14009]yyyy-mm-dd;@", "[$-14009]d.m.yy;@", "[$-14009]d-m-yy;@", "[$-14009]dd-mm-yy;@", "[$-14009]dd-mm-yyyy;@" }, new List<string> { "[$-14009]hh:mm:ss;@", "[$-14009]h:mm:ss;@", "[$-10409]AM/PM hh:mm:ss;@", "[$-10409]AM/PM h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-IE", new List<string> { "[$-1809]dd mmmm yyyy;@", "[$-1809]d mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-JM", new List<string> { "[$-2009]dddd, mmmm dd, yyyy;@", "[$-2009]mmmm dd, yyyy;@", "[$-2009]dddd, dd mmmm, yyyy;@", "[$-2009]dd mmmm, yyyy;@" }, new List<string> { "[$-409]hh:mm:ss AM/PM;@", "[$-409]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-MY", new List<string> { "[$-14409]d/m/yyyy;@", "[$-14409]d/m/yy;@", "[$-14409]dd/mm/yyyy;@", "[$-14409]dd/mm/yy;@", "[$-14409]yyyy-mm-dd;@", "[$-14409]dddd, d mmmm, yyyy;@", "[$-14409]d mmmm, yyyy;@" }, new List<string> { "[$-10409]h:mm:ss AM/PM;@", "[$-10409]hh:mm:ss AM/PM;@", "[$-14409]h:mm:ss;@", "[$-14409]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-NZ", new List<string> { "[$-1409]dddd, d mmmm yyyy;@", "[$-1409]d mmmm yyyy;@" }, new List<string> { "[$-1409]h:mm:ss AM/PM;@", "[$-1409]hh:mm:ss AM/PM;@", "hh:mm:ss;@", "[$-1409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-PH", new List<string> { "[$-3409]dd-mmm-yy;@", "[$-3409]dddd, mmmm dd, yyyy;@", "[$-3409]mmmm dd, yyyy;@", "[$-3409]dddd, dd mmmm, yyyy;@", "[$-3409]dd mmmm, yyyy;@" }, new List<string> { "[$-1409]h:mm:ss AM/PM;@", "[$-1409]hh:mm:ss AM/PM;@", "hh:mm:ss;@", "[$-1409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-SG", new List<string> { "[$-14809]d/m/yyyy;@", "[$-14809]d/m/yy;@", "[$-14809]dd/mm/yyyy;@", "[$-14809]dd/mm/yy;@", "[$-14809]yyyy-mm-dd;@", "[$-14809]dddd, d mmmm, yyyy;@", "[$-14809]d mmmm, yyyy;@" }, new List<string> { "[$-10409]h:mm:ss AM/PM;@", "[$-10409]hh:mm:ss AM/PM;@", "[$-14809]h:mm:ss;@", "[$-14809]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-ZA", new List<string> { "[$-1C09]dd mmmm yyyy;@" }, new List<string> { "[$-409]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-TT", new List<string> { "[$-2C09]dddd, dd mmmm yyyy;@" }, new List<string> { "[$-409]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-GB", new List<string> { "[$-809]dd mmmm yyyy;@", "[$-809]d mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "[$-409]hh:mm:ss AM/PM;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("en-ZW", new List<string> { "[$-3009]dd-mmm-yy;@", "[$-3009]dddd, mmmm dd, yyyy;@", "[$-3009]mmmm dd, yyyy;@", "[$-3009]dddd, dd mmmm, yyyy;@", "[$-3009]dd mmmm, yyyy;@" }, new List<string> { "[$-409]h:mm:ss AM/PM;@", "[$-409]hh:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("et", new List<string> { "[$-425]d. mmmm yyyy\". a.\";@", "[$-425]dd. mmmm yyyy\". a.\";@", "[$-425]dddd, d. mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fo", new List<string> { "[$-438]d. mmmm yyyy;@" }, new List<string> { "hh.mm.ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fil", new List<string>
			{
				"[$-10464]m/d/yyyy;@", "[$-10464]m/d/yy;@", "[$-10464]mm/dd/yy;@", "[$-10464]mm/dd/yyyy;@", "[$-10464]yy/mm/dd;@", "[$-10464]yyyy-mm-dd;@", "[$-10464]dd-mmm-yy;@", "[$-10464]dddd, mmmm dd, yyyy;@", "[$-10464]mmmm dd, yyyy;@", "[$-10464]dddd, dd mmmm, yyyy;@",
				"[$-10464]dd mmmm, yyyy;@"
			}, new List<string> { "[$-10409]h:mm:ss AM/PM;@", "[$-10409]hh:mm:ss AM/PM;@", "[$-10464]h:mm:ss;@", "[$-10464]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fi", new List<string>
			{
				"[$-40B]d. mmmmt\a;@", "[$-40B]d. mmmmt\a yy;@", "[$-40B]d. mmmmt\a yyyy;@", "[$-40B]mmmm yy;@", "[$-40B]mmmm yyyy;@", "[$-40B]d. mmmmt\a yyyy h:mm;@", "d.m.yyyy h:mm;@", "d.m.yy h:mm;@", "[$-40B]mmmmm;@", "[$-40B]mmmmm yy;@",
				"yyyy-mm-dd;@"
			}, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fr-BE", new List<string> { "[$-80C]dddd d mmmm yyyy;@", "[$-80C]d mmmm yyyy;@", "[$-80C]dd-mmm-yy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@", "h.mm;@", "h\" h \"mm;@", "h\" h \"m\" min \"s\" s \";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fr-CA", new List<string> { "[$-C0C]d mmmm, yyyy;@", "[$-C0C]d mmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "h\" h \"mm;@", "h:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fr-FR", new List<string> { "[$-40C]d-mmm;@", "[$-40C]d-mmm-yy;@", "[$-40C]dd-mmm-yy;@", "[$-40C]mmm-yy;@", "[$-40C]mmmm-yy;@", "[$-40C]d mmmm yyyy;@", "[$-40C]mmmmm;@", "[$-40C]mmmmm-yy;@", "[$-40C]d-mmm-yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fr-LU", new List<string> { "[$-140C]dddd d mmmm yyyy;@", "[$-140C]d mmm yy;@", "[$-140C]d mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "hh.mm;@", "hh\" h \"mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fr-MC", new List<string> { "[$-180C]dddd d mmmm yyyy;@", "[$-180C]d mmm yy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "hh.mm;@", "hh\" h \"mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fr-CH", new List<string> { "[$-100C]dddd, d. mmmm yyyy;@", "[$-100C]d. mmmm yyyy;@", "[$-100C]d mmm yy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "hh.mm\" h\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fy", new List<string> { "[$-10462]d-m-yyyy;@", "[$-10462]d-m-yy;@", "[$-10462]dd-mm-yy;@", "[$-10462]dd/mm/yy;@", "[$-10462]dd.mm.yy;@", "[$-10462]yyyy-mm-dd;@", "[$-10462]dddd d mmmm yyyy;@", "[$-10462]d-mmm-yy;@", "[$-10462]d mmmm yyyy;@", "[$-10462]d mmm yy;@" }, new List<string> { "[$-10462]h:mm:ss;@", "[$-10462]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("gl", new List<string> { "[$-456]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-456]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-456]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@", "hh:mm;@", "[$-456]hh:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ka", new List<string> { "[$-437]yyyy \"წლის\" dd mm, dddd;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("de-AT", new List<string> { "[$-C07]dddd, dd. mmmm yyyy;@", "[$-C07]d.mmmm yyyy;@", "[$-C07]d.mmmyyyy;@", "[$-C07]d mmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "hh:mm;@", "hh:mm\" Uhr\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("de-DE", new List<string> { "[$-407]d. mmm.;@", "[$-407]d. mmm. yy;@", "[$-407]d. mmm yy;@", "[$-407]mmm. yy;@", "[$-407]mmmm yy;@", "[$-407]d. mmmm yyyy;@", "[$-407]mmmmm;@", "[$-407]mmmmm yy;@", "d.m.yyyy;@", "[$-407]d. mmm. yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("de-LI", new List<string> { "[$-1407]dddd, d. mmmm yyyy;@", "[$-1407]d. mmmm yyyy;@", "[$-1407]d. mmm yy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "h.mm\" h\";@", "hh.mm\" h\";@", "h.mm\" Uhr\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("de-LU", new List<string> { "[$-1007]dddd, d. mmmm yyyy;@", "[$-1007]d. mmmm yyyy;@", "[$-1007]d. mmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "h.mm;@", "h.mm\" Uhr \";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("de-CH", new List<string> { "[$-807]dddd, d. mmmm yyyy;@", "[$-807]d. mmmm yyyy;@", "[$-807]d. mmm yy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "h.mm\" h\";@", "hh.mm\" h\";@", "h.mm\" Uhr\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("el", new List<string> { "[$-408]d-mmm;@", "[$-408]d-mmm-yy;@", "[$-408]dd-mmm-yy;@", "[$-408]mmm-yy;@", "[$-408]d mmmm yyyy;@", "[$-408]d/m/yy h:mm AM/PM;@", "d/m/yy h:mm;@", "[$-408]mmmmm;@", "[$-408]mmmmm-yy;@", "[$-408]d-mmm-yyyy;@" }, new List<string> { "h:mm;@", "[$-408]h:mm AM/PM;@", "h:mm:ss;@", "[$-408]h:mm:ss AM/PM;@", "[$-408]d/m/yy h:mm AM/PM;@", "d/m/yy h:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("kl", new List<string> { "[$-1046F]dd-mm-yyyy;@", "[$-1046F]dd-mm-yy;@", "[$-1046F]yyyy-mm-dd;@", "[$-1046F]yyyy mm dd;@", "[$-1046F]d. mmmm yyyy;@", "[$-1046F]dd. mmmm yyyy;@" }, new List<string> { "[$-1046F]hh:mm:ss;@", "[$-1046F]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("gu", new List<string> { "[$-447]dd mmmm yyyy;@", "[$-447]d mmmm yyyy;@", "[$-7000447]dd-mm-yy;@", "[$-7000447]d-m-yy;@", "[$-7000447]d.m.yy;@", "[$-7000447]dd-mm-yyyy;@", "[$-7000447]yyyy-mm-dd;@", "[$-7000447]dd mmmm yyyy;@", "[$-7000447]d mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "[$-447]AM/PM hh:mm:ss;@", "[$-447]AM/PM h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ha", new List<string> { "[$-10468]d/m/yyyy;@", "[$-10468]d/m/yy;@", "[$-10468]dd/mm/yy;@", "[$-10468]dd/mm/yyyy;@", "[$-10468]yy/mm/dd;@", "[$-10468]yyyy-mm-dd;@", "[$-10468]dd-mmm-yy;@", "[$-10468]dddd, mmmm dd, yyyy;@", "[$-10468]dd mmmm, yyyy;@", "[$-10468]dddd, dd mmmm, yyyy;@" }, new List<string> { "[$-10468]h:mm:ss AM/PM;@", "[$-10468]hh:mm:ss AM/PM;@", "[$-10468]h:mm:ss;@", "[$-10468]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("he", new List<string> { "[$-1010409]d/m/yyyy h:mm AM/PM;@", "[$-1010409]d/m/yyyy h:mm;@", "[$-101040D]d mmm yy;@", "[$-101040D]d mmmm yyyy;@", "[$-1010409]d mmm yy;@", "[$-1010409]d mmmm yyyy;@" }, new List<string> { "[$-1000409]h:mm:ss AM/PM;@", "[$-1010409]d/m/yyyy h:mm AM/PM;@", "[$-1010409]d/m/yyyy h:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("hu", new List<string> { "[$-40E]yyyy/ mmm/ d.;@", "[$-40E]yy/ mmmm d.;@", "[$-40E]mmmm d.;@", "[$-40E]yyyy/ mmm.;@", "[$-40E]yyyy/ mmmm;@", "[$-40E]yyyy/ mmmm d.;@", "[$-40E]yyyy/ m/ d. h:mm AM/PM;@", "yyyy/ m/ d. h:mm;@", "[$-40E]mmm/ d.;@" }, new List<string> { "[$-40E]h:mm AM/PM;@", "[$-40E]h:mm:ss AM/PM;@", "[$-40E]yyyy. m. d. h:mm AM/PM;@", "[$-40E]h \"óra\" m \"perc\" AM/PM;@", "h \"óra\" m \"perc\";@", "[$-40E]h \"óra\" m \"perckor\" AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("is", new List<string> { "[$-40F]d. mmmm yyyy;@", "[$-40F]dd. mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "hh:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ig", new List<string> { "[$-10470]d/m/yyyy;@", "[$-10470]d/m/yy;@", "[$-10470]dd/mm/yy;@", "[$-10470]dd/mm/yyyy;@", "[$-10470]yy/mm/dd;@", "[$-10470]yyyy-mm-dd;@", "[$-10470]dd-mmm-yy;@", "[$-10470]dddd, mmmm dd, yyyy;@", "[$-10470]mmmm dd, yyyy;@", "[$-10470]dddd, dd mmmm, yyyy;@" }, new List<string> { "[$-10470]h:mm:ss AM/PM;@", "[$-10470]hh:mm:ss AM/PM;@", "[$-10470]h:mm:ss;@", "[$-10470]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("id", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "[$-421]dd mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("iu-Latn", new List<string> { "[$-1085D]d/mm/yyyy;@", "[$-1085D]d/m/yy;@", "[$-1085D]yy-mm-dd;@", "[$-1085D]dd/mm/yyyy;@", "[$-1085D]yyyy-mm-dd;@", "[$-1085D]dd-mmm-yy;@", "[$-1085D]ddd, mmmm dd,yyyy;@", "[$-1085D]mmmm dd,yyyy;@", "[$-1085D]dddd, dd mmmm, yyyy;@", "[$-1085D]dd mmmm, yyyy;@" }, new List<string> { "[$-10409]h:mm:ss AM/PM;@", "[$-10409]hh:mm:ss AM/PM;@", "[$-1085D]hh:mm:ss;@", "[$-1085D]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("iu-Cans", new List<string> { "[$-1045D]d/m/yyyy;@", "[$-1045D]d/m/yy;@", "[$-1045D]dd/mm/yy;@", "[$-1045D]yy/mm/dd;@", "[$-1045D]yyyy-mm-dd;@", "[$-1045D]dd-mmm-yy;@", "[$-1045D]dddd,mmmm dd,yyyy;@", "[$-1045D]mmmm dd,yyyy;@", "[$-1045D]dddd, dd mmmm, yyyy;@", "[$-1045D]dd mmmm, yyyy;@" }, new List<string> { "[$-10409]h:mm:ss AM/PM;@", "[$-10409]hh:mm:ss AM/PM;@", "[$-1045D]h:mm:ss;@", "[$-1045D]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ga", new List<string> { "[$-1083C]dd/mm/yyyy;@", "[$-1083C]dd/mm/yy;@", "[$-1083C]d/m/yy;@", "[$-1083C]d.m.yy;@", "[$-1083C]yyyy-mm-dd;@", "[$-1083C]d mmmm yyyy;@", "[$-1083C]dd mmmm yyyy;@", "[$-1083C]dd mmm yyyy;@", "[$-1083C]d mmm yyyy;@" }, new List<string> { "[$-1083C]hh:mm:ss;@", "[$-1083C]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("xh", new List<string> { "[$-10434]yyyy/mm/dd;@", "[$-10434]yy/mm/dd;@", "[$-10434]yyyy-mm-dd;@", "[$-10434]dd mmmm yyyy;@" }, new List<string> { "[$-10409]hh:mm:ss AM/PM;@", "[$-10409]h:mm:ss AM/PM;@", "[$-10434]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("zu", new List<string> { "[$-10435]yyyy/mm/dd;@", "[$-10435]yy/mm/dd;@", "[$-10435]yyyy-mm-dd;@", "[$-10435]dd mmmm yyyy;@" }, new List<string> { "[$-10409]hh:mm:ss AM/PM;@", "[$-10409]h:mm:ss AM/PM;@", "[$-10435]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("it-IT", new List<string> { "[$-410]d-mmm;@", "[$-410]d-mmm-yy;@", "[$-410]dd-mmm-yy;@", "[$-410]mmm-yy;@", "[$-410]mmmm-yy;@", "[$-410]d mmmm yyyy;@", "[$-410]mmmmm;@", "[$-410]mmmmm-yy;@", "[$-410]d-mmm-yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@", "mm:ss,0;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("it-CH", new List<string> { "[$-810]dddd, d. mmmm yyyy;@", "[$-810]d-mmm-yy;@", "[$-810]d mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "h.mm\" h\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ja", new List<string> { "yyyy\"年\"m\"月\"d\"日\";@", "yyyy\"年\"m\"月\";@", "m\"月\"d\"日\";@", "yyyy/m/d;@", "[$-409]yyyy/m/d h:mm AM/PM;@", "yyyy/m/d h:mm;@", "m/d;@" }, new List<string> { "h\"時\"mm\"分\";@", "h\"時\"mm\"分\"ss\"秒\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("kn", new List<string> { "[$-44B]dd mmmm yyyy;@", "[$-44B]d mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "[$-44B]AM/PM hh:mm:ss;@", "[$-44B]AM/PM h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("kk", new List<string> { "[$-43F]d mmmm yyyy \"ж.\";@", "[$-43F]dd mmmm yyyy \"ж.\";@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("km", new List<string> { "[$-10453]yyyy-mm-dd;@", "[$-10453]d mmmm yyyy;@", "[$-10453]ddd d mmmm yyyy;@" }, new List<string> { "[$-10453]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("qut", new List<string> { "[$-10486]dd/mm/yyyy;@", "[$-10486]dd/mm/yy;@", "[$-10486]d/mm/yyyy;@", "[$-10486]d/m/yy;@", "[$-10486]dd-mm-yy;@", "[$-10486]yyyy-mm-dd;@", "[$-10486]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-10486]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-10486]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-10486]hh:mm:ss AM/PM;@", "[$-10486]h:mm:ss AM/PM;@", "[$-10486]h:mm:ss;@", "[$-10486]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("rw", new List<string>
			{
				"[$-10487]m/d/yyyy;@", "[$-10487]m/d/yy;@", "[$-10487]mm/dd/yy;@", "[$-10487]mm/dd/yyyy;@", "[$-10487]yy/mm/dd;@", "[$-10487]yyyy-mm-dd;@", "[$-10487]dd-mmm-yy;@", "[$-10487]dddd, mmmm dd, yyyy;@", "[$-10487]mmmm dd, yyyy;@", "[$-10487]dddd, dd mmmm, yyyy;@",
				"[$-10487]dd mmmm, yyyy;@"
			}, new List<string> { "[$-10487]h:mm:ss AM/PM;@", "[$-10487]hh:mm:ss AM/PM;@", "[$-10487]h:mm:ss;@", "[$-10487]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("kok", new List<string> { "[$-457]dd mmmm yyyy;@", "[$-457]d mmmm yyyy;@" }, new List<string> { "[$-457]hh:mm:ss AM/PM;@", "[$-457]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ko", new List<string>
			{
				"yyyy\"년\" m\"월\" d\"일\";@", "yy\"年\" m\"月\" d\"日\";@", "yyyy\"년\" m\"월\";@", "m\"월\" d\"일\";@", "yy\"-\"m\"-\"d;@", "yy\"-\"m\"-\"d h:mm;@", "[$-412]yy\"-\"m\"-\"d AM/PM h:mm;@", "[$-409]yy\"-\"m\"-\"d h:mm AM/PM;@", "yy\"/\"m\"/\"d;@", "yyyy\"-\"m\"-\"d;@",
				"yyyy\"/\"m\"/\"d;@"
			}, new List<string> { "[$-412]AM/PM h:mm;@", "[$-412]AM/PM h:mm:ss;@", "yyyy\"-\"m\"-\"d h:mm;@", "[$-412]yyyy\"-\"m\"-\"d AM/PM h:mm;@", "[$-409]yyyy\"-\"m\"-\"d h:mm AM/PM;@", "h\"시\" mm\"분\";@", "h\"시\" mm\"분\" ss\"초\";@", "[$-412]AM/PM h\"시\" mm\"분\";@", "[$-412]AM/PM h\"시\" mm\"분\" ss\"초\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ky", new List<string> { "dd.mm.yy;@", "[$-440]d\"-\"mmmm yyyy\"-ж.\";@" }, new List<string> { "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("lo", new List<string> { "[$-10454]dd/mm/yyyy;@", "[$-10454]yyyy-mm-dd;@", "[$-10454]dd mmmm yyyy;@" }, new List<string> { "[$-10454]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("lv", new List<string> { "yyyy.mm.dd.;@", "[$-426]dddd, yyyy\". gada \"d. mmmm;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("lt", new List<string> { "[$-FC27]yyyy \"m.\" mmmm d \"d.\";@", "[$-427]yyyy \"m.\" mmmm d \"d.\";@" }, new List<string> { "hh:mm:ss;@", "hh:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("dsb", new List<string> { "[$-1082E]d. m. yyyy;@", "[$-1082E]d. m. yy;@", "[$-1082E]dd.mm.yyyy;@", "[$-1082E]dd.mm.yy;@", "[$-1082E]yyyy-mm-dd;@", "[$-1082E]dddd, \"dnja\" d. mmmm yyyy;@", "[$-1082E]dddd, d. mmmm yyyy;@", "[$-1082E]d. mmmm yyyy;@" }, new List<string> { "[$-1082E]h:mm:ss;@", "[$-1082E]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("lb", new List<string> { "[$-1046E]dd/mm/yyyy;@", "[$-1046E] dd/mm/yy;@", "[$-1046E]dd.mm.yy;@", "[$-1046E]dd/mm/yy;@", "[$-1046E]dd-mm-yy;@", "[$-1046E]yyyy-mm-dd;@", "[$-1046E]dddd d mmmm yyyy;@", "[$-1046E]d mmm yy;@", "[$-1046E]d mmmm yyyy;@" }, new List<string> { "[$-1046E]hh:mm:ss;@", "[$-1046E]h:mm:ss\" Auer\";@", "[$-1046E]hh:mm:ss\" Auer\";@", "[$-1046E]hhmmss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("mk", new List<string> { "[$-42F]dddd, dd mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ms-BN", new List<string> { "[$-83E]dd mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ms-MY", new List<string> { "[$-43E]dd mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ml", new List<string> { "[$-44C]dd mmmm yyyy;@", "[$-44C]d mmmm yyyy;@", "[$-C00044C]dd-mm-yyyy;@", "[$-C00044C]dd-mm-yy;@", "[$-C00044C]d-m-yy;@", "[$-C00044C]d.m.yy;@", "[$-C00044C]yyyy-mm-dd;@", "[$-C00044C]dd mmmm yyyy;@", "[$-C00044C]d mmmm yyyy;@" }, new List<string> { "[$-1044C]hh.mm.ss;@", "[$-1044C]h.mm.ss;@", "[$-10409]AM/PM hh.mm.ss;@", "[$-10409]AM/PM h.mm.ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("mt", new List<string> { "[$-1043A]dd/mm/yyyy;@", "[$-1043A]yyyy-mm-dd;@", "[$-1043A]ddmmyyyy;@", "[$-1043A]d-m-yyyy;@", "[$-1043A]d/m/yyyy;@", "[$-1043A]dddd, d\" ta\"\" \"mmmm yyyy;@", "[$-1043A]d\" ta\"\" \"mmmm yyyy;@", "[$-1043A]dd mmm yyyy;@" }, new List<string> { "[$-1043A]hh:mm:ss;@", "[$-10409]h:mm:ss AM/PM;@", "[$-1043A]h:mm:ss;@", "[$-10409]hh:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("mi", new List<string> { "[$-10481]dd/mm/yyyy;@", "[$-10481]d/mm/yy;@", "[$-10481]dd/mm/yy;@", "[$-10481]d.mm.yy;@", "[$-10481]yyyy-mm-dd;@", "[$-10481]dddd, dd mmmm, yyyy;@", "[$-10481]d mmmm yyyy;@" }, new List<string> { "[$-10481]h:mm:ss AM/PM;@", "[$-10481]hh:mm:ss AM/PM;@", "[$-10481]hh:mm:ss;@", "[$-10481]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("arn", new List<string> { "[$-1047A]dd-mm-yyyy;@", "[$-1047A]dd-mm-yy;@", "[$-1047A]dd/mm/yy;@", "[$-1047A]d/m/yy;@", "[$-1047A]yyyy-mm-dd;@", "[$-1047A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-1047A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-1047A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-1047A]h:mm:ss;@", "[$-1047A]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("mr", new List<string> { "[$-44E]dd mmmm yyyy;@", "[$-44E]d mmmm yyyy;@" }, new List<string> { "[$-44E]hh:mm:ss AM/PM;@", "[$-44E]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("moh", new List<string>
			{
				"[$-1047C]m/d/yyyy;@", "[$-1047C]m/d/yy;@", "[$-1047C]mm/dd/yy;@", "[$-1047C]mm/dd/yyyy;@", "[$-1047C]yy/mm/dd;@", "[$-1047C]yyyy-mm-dd;@", "[$-1047C]dd-mmm-yy;@", "[$-1047C]dddd, mmmm dd, yyyy;@", "[$-1047C]mmmm dd, yyyy;@", "[$-1047C]dddd, dd mmmm, yyyy;@",
				"[$-1047C]dd mmmm, yyyy;@"
			}, new List<string> { "[$-10409]h:mm:ss AM/PM;@", "[$-10409]hh:mm:ss AM/PM;@", "[$-1047C]hh:mm:ss;@", "[$-1047C]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("mn-Cyrl", new List<string> { "yy.mm.dd;@", "[$-450]yyyy \"оны\" mmmm d;@" }, new List<string> { "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("mn-Mong-CN", new List<string>
			{
				"[$-10850]yyyy/m/d;@", "[$-10850]yyyy-m-d;@", "[$-10850]yyyy.m.d;@", "[$-10850]yyyy.mm.dd;@", "[$-10850]yyyy-mm-dd;@", "[$-10850]yyyy/mm/dd;@", "[$-10850]yy-m-d;@", "[$-10850]yy/m/d;@", "[$-10850]yy.m.d;@", "[$-10850]yy/mm/dd;@",
				"[$-10850]yyyy\"ᠣᠨ ᠤ\u180b\" m\"ᠰᠠᠷ\u180eᠠ \u202fᠢᠢᠨ \"d\" ᠤ\u180b ᠡᠳᠦᠷ\";@", "[$-10850]yyyy\"ᠣᠨ ᠤ\u180b\" m\"ᠰᠠᠷ\u180eᠠ \u202fᠢᠢᠨ \"d\" ᠤ\u180b ᠡᠳᠦᠷ᠂\" dddd;@"
			}, new List<string> { "[$-10850]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ne", new List<string>
			{
				"[$-10461]m/d/yyyy;@", "[$-10461]m/d/yy;@", "[$-10461]mm/dd/yy;@", "[$-10461]mm/dd/yyyy;@", "[$-10461]yy/mm/dd;@", "[$-10461]yyyy-mm-dd;@", "[$-10461]dd-mmm-yy;@", "[$-10461]dddd, mmmm dd, yyyy;@", "[$-10461]mmmm dd, yyyy;@", "[$-10461]dddd, dd mmmm, yyyy;@",
				"[$-10461]dd mmmm, yyyy;@"
			}, new List<string> { "[$-10461]h:mm:ss AM/PM;@", "[$-10461]hh:mm:ss AM/PM;@", "[$-10461]h:mm:ss;@", "[$-10461]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("nb", new List<string> { "[$-414]d/ mmm.;@", "[$-414]d/ mmmm;@", "[$-414]d/ mmm. yyyy;@", "[$-414]d/ mmmm yyyy;@", "[$-414]mmm. yy;@", "[$-414]mmmm yy;@", "[$-414]mmmm yyyy;@", "yyyy-mm-dd;@" }, new List<string> { "'kl 'hh.mm;@", "[$-409]m/d/yy h:mm AM/PM;@", "m/d/yy hh:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("nn", new List<string> { "[$-814]d. mmmm yyyy;@", "[$-814]dd. mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "\"kl \"hh.mm;@", "hh.mm.ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("oc", new List<string> { "[$-10482]dd/mm/yyyy;@", "[$-10482]dd/mm/yy;@", "[$-10482]dd.mm.yy;@", "[$-10482]dd-mm-yy;@", "[$-10482]yyyy-mm-dd;@", "[$-10482]dddd,\" lo \"d mmmm\" de \"yyyy;@" }, new List<string> { "[$-10482]hh:mm:ss;@", "[$-10482]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("or", new List<string> { "[$-10448]dd-mm-yy;@", "[$-10448]d-m-yy;@", "[$-10448]d.m.yy;@", "[$-10448]dd-mm-yyyy;@", "[$-10448]yyyy-mm-dd;@", "[$-10448]dd mmmm yyyy;@", "[$-10448]d mmmm yyyy;@" }, new List<string> { "[$-10448]hh:mm:ss;@", "[$-10448]h:mm:ss;@", "[$-10409]AM/PM hh:mm:ss;@", "[$-10409]AM/PM h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ps", new List<string> { "[$-60463]dd/mm/yy;@", "[$-60463]dd/mm/yyyy;@", "[$-60463]dd/mmmm/yyyy;@", "[$-60463]dddd, dd mmmm, yyyy;@", "[$-60463]dd/mm/yyyy \"هـ\";@" }, new List<string> { "[$-60463]h:mm:ss AM/PM;@", "[$-60463]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("fa", new List<string> { "[$-10C0000]d mmmm yyyy;@", "[$-30C0000]d mmmm yyyy;@" }, new List<string> { "[$-1000000]h:mm:ss;@", "[$-1000429]h:mm AM/PM;@", "[$-1000409]h:mm AM/PM;@", "[$-3000000]h:mm:ss;@", "[$-3000429]h:mm AM/PM;@", "[$-3000409]h:mm AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("pl", new List<string> { "[$-415]d mmm;@", "[$-415]d mmm yy;@", "[$-415]dd mmm yy;@", "[$-415]mmm yy;@", "[$-415]mmmm yy;@", "[$-415]d mmmm yyyy;@", "[$-415]mmmmm;@", "[$-415]mmmmm.yy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("pt-BR", new List<string> { "[$-416]d-mmm;@", "[$-416]d-mmm-yy;@", "[$-416]dd-mmm-yy;@", "[$-416]mmm-yy;@", "[$-416]mmmm-yy;@", "[$-416]d  mmmm, yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("pt-PT", new List<string> { "[$-816]d/mmm;@", "[$-816]d-mmm-yy;@", "[$-816]dd-mmm-yy;@", "[$-816]mmm/yy;@", "[$-816]mmmm yy;@", "[$-816]d \"de\" mmmm \"de\" yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("pa", new List<string> { "[$-446]dd mmmm yyyy dddd;@", "[$-446]d mmmm yyyy;@", "[$-6000446]dd-mm-yy;@", "[$-6000446]d-m-yy;@", "[$-6000446]d.m.yy;@", "[$-6000446]dd-mm-yyyy;@", "[$-6000446]yyyy-mm-dd;@", "[$-6000446]dd mmmm yyyy dddd;@", "[$-6000446]d mmmm yyyy;@" }, new List<string> { "[$-446]AM/PM hh:mm:ss;@", "[$-446]AM/PM h:mm:ss;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("quz-BO", new List<string> { "[$-1046B]dd/mm/yyyy;@", "[$-1046B]dd/mm/yy;@", "[$-1046B]d/m/yy;@", "[$-1046B]dd-mm-yy;@", "[$-1046B]yyyy-mm-dd;@", "[$-1046B]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-1046B]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-1046B]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-1046B]hh:mm:ss AM/PM;@", "[$-1046B]h:mm:ss AM/PM;@", "[$-1046B]h:mm:ss;@", "[$-1046B]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("quz-EC", new List<string> { "[$-1086B]dd/mm/yyyy;@", "[$-1086B]dd/mm/yy;@", "[$-1086B]d/m/yy;@", "[$-1086B]dd-mm-yy;@", "[$-1086B]yyyy-mm-dd;@", "[$-1086B]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-1086B]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-1086B]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-1086B]h:mm:ss;@", "[$-1086B]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("quz-PE", new List<string> { "[$-10C6B]dd/mm/yyyy;@", "[$-10C6B]dd/mm/yy;@", "[$-10C6B]d/m/yy;@", "[$-10C6B]dd-mm-yy;@", "[$-10C6B]yyyy-mm-dd;@", "[$-10C6B]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-10C6B]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-10C6B]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-10C6B]hh:mm:ss AM/PM;@", "[$-10C6B]h:mm:ss AM/PM;@", "[$-10C6B]h:mm:ss;@", "[$-10C6B]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ro", new List<string> { "[$-418]d-mmm;@", "[$-418]d-mmm-yy;@", "[$-418]dd-mmm-yy;@", "[$-418]mmm-yy;@", "[$-418]mmmm-yy;@", "[$-418]d mmmm yyyy;@", "[$-418]mmmmm;@", "[$-418]mmmmm-yy;@", "d/m/yyyy;@", "[$-418]d-mmm-yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("rm", new List<string> { "[$-10417]dd/mm/yyyy;@", "[$-10417]dd/mm/yy;@", "[$-10417]d/mm/yy;@", "[$-10417]dd/m/yy;@", "[$-10417]d/m/yy;@", "[$-10417]yyyy-mm-dd;@", "[$-10417]dddd, d mmmm yyyy;@", "[$-10417]d mmmm yyyy;@", "[$-10417]d mmm yy;@" }, new List<string> { "[$-10417]hh:mm:ss;@", "[$-10417]h:mm:ss;@", "[$-10417]hh:mm:ss\" Ura\";@", "[$-10417]h:mm:ss\" Ura\";@", "[$-10417]hh:mm:ss\" Uhr\";@", "[$-10417]h:mm:ss\" Uhr\";@", "[$-10417]hh:mm:ss\" h\";@", "[$-10417]h:mm:ss\" h\";@", "[$-10417]hhmmss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ru", new List<string> { "[$-419]d mmm;@", "[$-419]d mmm yy;@", "[$-419]dd mmm yy;@", "[$-F419]yyyy, mmmm;@", "[$-419]mmmm yyyy;@", "[$-FC19]dd mmmm yyyy г.;@", "[$-419]mmmm;@", "[$-FC19]yyyy, dd mmmm;@", "[$-419]d-mmm-yyyy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sah-RU", new List<string> { "[$-10485]mm.dd.yyyy;@", "[$-10485]mm.dd.yy;@", "[$-10485]d.m.yy;@", "[$-10485]mm/dd/yy;@", "[$-10485]mm-dd-yyyy;@", "[$-10485]mmmm d yyyy \"с.\";@", "[$-10485]mmmm dd yyyy \"с.\";@" }, new List<string> { "[$-10485]h:mm:ss;@", "[$-10485]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("smn-FI", new List<string> { "[$-1243B]d.m.yyyy;@", "[$-1243B]dd.mm.yyyy;@", "[$-1243B]d.m.yy;@", "[$-1243B]yyyy-mm-dd;@", "[$-1243B]mmmm d\". p. \"yyyy;@" }, new List<string> { "[$-1243B]h:mm:ss;@", "[$-1243B]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("smj-NO", new List<string> { "[$-1103B]dd.mm.yyyy;@", "[$-1103B]dd.mm.yy;@", "[$-1103B]d.m.yy;@", "[$-1103B]yyyy-mm-dd;@", "[$-1103B]mmmm d\". b. \"yyyy;@" }, new List<string> { "[$-1103B]hh:mm:ss;@", "[$-1103B]h:mm:ss;@", "[$-1103B]hh.mm.ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("smj-SE", new List<string> { "[$-1143B]yyyy-mm-dd;@", "[$-1143B]yy-mm-dd;@", "[$-1143B]mmmm d\". b. \"yyyy;@" }, new List<string> { "[$-1143B]hh:mm:ss;@", "[$-1143B]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("se-FI", new List<string> { "[$-10C3B]d.m.yyyy;@", "[$-10C3B]dd.mm.yyyy;@", "[$-10C3B]d.m.yy;@", "[$-10C3B]yyyy-mm-dd;@", "[$-10C3B]mmmm d\". b. \"yyyy;@" }, new List<string> { "[$-10C3B]h:mm:ss;@", "[$-10C3B]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("se-NO", new List<string> { "[$-1043B]dd.mm.yyyy;@", "[$-1043B]dd.mm.yy;@", "[$-1043B]d.m.yy;@", "[$-1043B]yyyy-mm-dd;@", "[$-1043B]mmmm d\". b. \"yyyy;@" }, new List<string> { "[$-1043B]hh:mm:ss;@", "[$-1043B]h:mm:ss;@", "[$-1043B]hh.mm.ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("se-SE", new List<string> { "[$-1083B]yyyy-mm-dd;@", "[$-1083B]yy-mm-dd;@", "[$-1083B]mmmm d\". b. \"yyyy;@" }, new List<string> { "[$-1083B]hh:mm:ss;@", "[$-1083B]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sms-FI", new List<string> { "[$-1203B]d.m.yyyy;@", "[$-1203B]dd.mm.yyyy;@", "[$-1203B]d.m.yy;@", "[$-1203B]yyyy-mm-dd;@", "[$-1203B]mmmm d\". p. \"yyyy;@" }, new List<string> { "[$-1203B]h:mm:ss;@", "[$-1203B]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sma-NO", new List<string> { "[$-1183B]dd.mm.yyyy;@", "[$-1183B]dd.mm.yy;@", "[$-1183B]d.m.yy;@", "[$-1183B]yyyy-mm-dd;@", "[$-1183B]mmmm d\". b. \"yyyy;@" }, new List<string> { "[$-1183B]hh:mm:ss;@", "[$-1183B]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sma-SE", new List<string> { "[$-11C3B]yyyy-mm-dd;@", "[$-11C3B]yy-mm-dd;@", "[$-11C3B]mmmm d\". b. \"yyyy;@" }, new List<string> { "[$-11C3B]hh:mm:ss;@", "[$-11C3B]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sa", new List<string> { "[$-44F]dd mmmm yyyy dddd;@", "[$-44F]d mmmm yyyy;@" }, new List<string> { "[$-44F]hh:mm:ss AM/PM;@", "[$-44F]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("gd-GB", new List<string> { "[$-10491]dd/mm/yyyy;@", "[$-10491]dd/mm/yy;@", "[$-10491]d/m/yy;@", "[$-10491]d.m.yy;@", "[$-10491]yyyy-mm-dd;@", "[$-10491]dd mmmm yyyy;@", "[$-10491]d mmmm yyyy;@" }, new List<string> { "[$-10491]hh:mm:ss;@", "[$-10491]h:mm:ss;@", "[$-10491]hh:mm:ss AM/PM;@", "[$-10491]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sr-Cyrl-BA", new List<string> { "[$-1C1A]d. mmmm yyyy;@", "[$-1C1A]dd. mmmm yyyy;@", "[$-1C1A]dddd, d. mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sr-Cyrl-ME", new List<string> { "[$-C1A]d. mmmm yyyy;@", "[$-C1A]dd. mmmm yyyy;@", "[$-C1A]dddd, d. mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sr-Latn-BA", new List<string>
			{
				"[$-1181A]d.m.yy;@", "[$-1181A]d.m.yyyy;@", "[$-1181A]d. m. yyyy;@", "[$-1181A]dd.mm.yyyy;@", "[$-1181A]d. m. yy;@", "[$-1181A]dd.mm.yy;@", "[$-1181A]dd. mm. yy;@", "[$-1181A]yyyy-mm-dd;@", "[$-1181A]d. mmmm yyyy;@", "[$-1181A]dd. mmmm yyyy;@",
				"[$-1181A]dddd, d. mmmm yyyy;@"
			}, new List<string> { "[$-1181A]h:mm:ss;@", "[$-1181A]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sr-Latn-ME", new List<string> { "[$-81A]d. mmmm yyyy;@", "[$-81A]dd. mmmm yyyy;@", "[$-81A]dddd, d. mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("nso", new List<string> { "[$-1046C]yyyy/mm/dd;@", "[$-1046C]yy/mm/dd;@", "[$-1046C]yyyy-mm-dd;@", "[$-1046C]dd mmmm yyyy;@" }, new List<string> { "[$-10409]hh:mm:ss AM/PM;@", "[$-10409]h:mm:ss AM/PM;@", "[$-1046C]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("tn", new List<string> { "[$-10432]yyyy/mm/dd;@", "[$-10432]yy/mm/dd;@", "[$-10432]yyyy-mm-dd;@", "[$-10432]dd mmmm yyyy;@" }, new List<string> { "[$-10409]hh:mm:ss AM/PM;@", "[$-10409]h:mm:ss AM/PM;@", "[$-10432]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("si", new List<string> { "[$-1045B]yyyy-mm-dd;@", "[$-1045B]yyyy/mm/dd;@", "[$-1045B]yy/mm/dd;@", "[$-1045B]dd/mm/yyyy;@", "[$-1045B]dd/mm/yy;@", "[$-1045B]yyyy mmmm\" මස \"dd\" ව\u0dd0න\u0dd2ද\u0dcf \"dddd;@" }, new List<string> { "[$-1045B]h:mm:ss AM/PM;@", "[$-1045B]hh:mm:ss AM/PM;@", "[$-1045B]h:mm:ss;@", "[$-1045B]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sk", new List<string> { "[$-41B]d-mmm.;@", "[$-41B]d/mmm/yy;@", "[$-41B]dd-mmm-yy;@", "[$-41B]mmm-yy;@", "[$-41B]mmmm yy;@", "[$-41B]d. mmmm yyyy;@", "[$-41B]mmmmm;@", "[$-41B]mmmmm-yy;@" }, new List<string> { "h:mm;@", "[$-409]h:mm AM/PM;@", "h:mm:ss;@", "[$-409]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sl", new List<string> { "[$-424]d. mmmm yyyy;@", "[$-424]dd. mmmm yyyy;@", "[$-424]dddd, d. mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-AR", new List<string> { "[$-2C0A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-2C0A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-2C0A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-2C0A]hh:mm:ss AM/PM;@", "[$-2C0A]h:mm:ss AM/PM;@", "hh:mm:ss;@", "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-BO", new List<string> { "[$-400A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-400A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-400A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-400A]hh:mm:ss AM/PM;@", "[$-400A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-CL", new List<string> { "[$-340A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-340A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-340A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-CO", new List<string> { "[$-240A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-240A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-240A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-240A]hh:mm:ss AM/PM;@", "[$-240A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-CR", new List<string> { "[$-140A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-140A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-140A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-140A]hh:mm:ss AM/PM;@", "[$-140A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-DO", new List<string> { "[$-1C0A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-1C0A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-1C0A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-1C0A]hh:mm:ss AM/PM;@", "[$-1C0A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-EC", new List<string> { "[$-300A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-300A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-300A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-SV", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "mm-dd-yyyy;@", "yyyy-mm-dd;@", "[$-440A]dddd, dd\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-440A]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-GT", new List<string> { "[$-100A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-100A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-100A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-100A]hh:mm:ss AM/PM;@", "[$-100A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-HN", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "mm-dd-yyyy;@", "yyyy-mm-dd;@", "[$-480A]dddd, dd\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-480A]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-MX", new List<string> { "[$-80A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-80A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-80A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-80A]hh:mm:ss AM/PM;@", "[$-80A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-NI", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "mm-dd-yyyy;@", "yyyy-mm-dd;@", "[$-4C0A]dddd, dd\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-4C0A]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-PA", new List<string> { "mm/dd/yyyy;@", "mm/dd/yy;@", "d/m/yy;@", "dd/mm/yy;@", "dd-mm-yy;@", "yyyy-mm-dd;@", "[$-180A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-180A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-180A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-180A]hh:mm:ss AM/PM;@", "[$-180A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-PY", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "d/m/yy;@", "dd-mm-yy;@", "yyyy-mm-dd;@", "[$-3C0A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-3C0A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-3C0A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-3C0A]hh:mm:ss AM/PM;@", "[$-3C0A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-PE", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "d/m/yy;@", "dd-mm-yy;@", "yyyy-mm-dd;@", "[$-280A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-280A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-280A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-280A]hh:mm:ss AM/PM;@", "[$-280A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-PR", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "mm-dd-yyyy;@", "yyyy-mm-dd;@", "[$-500A]dddd, dd\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-500A]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-ES_tradnl", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "d/mm/yy;@", "d/m/yy;@", "dd-mm-yy;@", "dd.mm.yy;@", "yyyy-mm-dd;@", "[$-40A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-40A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-40A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@", "hh:mm;@", "hh\"H\"mm\"'\";@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-US", new List<string>
			{
				"[$-1540A]m/d/yyyy;@", "[$-1540A]m/d/yy;@", "[$-1540A]mm/dd/yy;@", "[$-1540A]mm/dd/yyyy;@", "[$-1540A]yy/mm/dd;@", "[$-1540A]yyyy-mm-dd;@", "[$-1540A]dd-mmm-yy;@", "[$-1540A]dddd, mmmm dd, yyyy;@", "[$-1540A]mmmm dd, yyyy;@", "[$-1540A]dddd, dd mmmm, yyyy;@",
				"[$-1540A]dd mmmm, yyyy;@"
			}, new List<string> { "[$-10409]h:mm:ss AM/PM;@", "[$-10409]hh:mm:ss AM/PM;@", "[$-1540A]h:mm:ss;@", "[$-1540A]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-UY", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "d/m/yy;@", "dd-mm-yy;@", "yyyy-mm-dd;@", "[$-380A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-380A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-380A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-380A]hh:mm:ss AM/PM;@", "[$-380A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("es-VE", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "d/m/yy;@", "dd-mm-yy;@", "yyyy-mm-dd;@", "[$-200A]dddd, dd\" de \"mmmm\" de \"yyyy;@", "[$-200A]dddd d\" de \"mmmm\" de \"yyyy;@", "[$-200A]d\" de \"mmmm\" de \"yyyy;@" }, new List<string> { "[$-200A]hh:mm:ss AM/PM;@", "[$-200A]h:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sw", new List<string>
			{
				"m/d/yyyy;@", "m/d/yy;@", "mm/dd/yy;@", "mm/dd/yyyy;@", "yy/mm/dd;@", "yyyy-mm-dd;@", "[$-441]dd-mmm-yy;@", "[$-441]dddd, mmmm dd, yyyy;@", "[$-441]mmmm dd, yyyy;@", "[$-441]dddd, dd mmmm, yyyy;@",
				"[$-441]dd mmmm, yyyy;@"
			}, new List<string> { "[$-409]h:mm:ss AM/PM;@", "[$-409]hh:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sv-FI", new List<string> { "d.m.yyyy;@", "dd.mm.yyyy;@", "d.m.yy;@", "yyyy-mm-dd;@", "[$-81D]\"den \"d mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "\"kl \"h:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("sv-SE", new List<string>
			{
				"yyyy-mm-dd;@", "yyyy-mm-dd hh:mm;@", "yy-mm-dd;@", "yy-mm-dd hh:mm;@", "d/m yyyy;@", "d/m -yy;@", "d/m yy;@", "d/m/yy;@", "[$-41D]\"den \" d mmmm yyyy;@", "[$-41D]d mmmm yyyy;@",
				"[$-41D]d mmmm -yy;@", "[$-41D]mmmmm;@", "[$-41D]mmmmm-yy;@", "yyyy mm dd;@", "[$-41D]mmmm;@", "[$-41D]dd-mmm;@", "[$-41D]mmmm yyyy;@", "[$-41D]mmmm -yy;@", "[$-41D]mmm-yy;@", "yyyy;@"
			}, new List<string> { "hh:mm;@", "hh:mm:ss;@", "\"kl \"hh:mm;@", "\"kl \"hh:mm:ss;@", "[$-409]yyyy-mm-dd h:mm AM/PM;@", "[h]:mm:ss;@", "yyyy-mm-dd hh:mm;@", "[$-409]yyyy-mm-dd h:mm AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("syr", new List<string> { "dd/mm/yyyy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "[$-45A]dd mmmm, yyyy;@", "[$-45A]dddd, dd mmmm, yyyy;@" }, new List<string> { "[$-45A]hh:mm:ss AM/PM;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("tg", new List<string> { "[$-10428]dd.mm.yy;@", "[$-10428]yyyy-mm-dd;@", "[$-10428]d mmmm yyyy;@" }, new List<string> { "[$-10428]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("tzm-Latn-DZ", new List<string> { "[$-1085F]dd-mm-yyyy;@", "[$-1085F]dd-mm-yy;@", "[$-1085F]yyyy-mm-dd;@", "[$-1085F]dd mmmm, yyyy;@", "[$-1085F]dddd, dd mmmm, yyyy;@" }, new List<string> { "[$-1085F]h:mm:ss;@", "[$-1085F]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ta", new List<string>
			{
				"dd-mm-yyyy;@", "dd-mm-yy;@", "d-m-yy;@", "d.m.yy;@", "yyyy-mm-dd;@", "[$-449]dd mmmm yyyy;@", "[$-449]d mmmm yyyy;@", "[$-9000449]dd-mm-yyyy;@", "[$-9000449]dd-mm-yy;@", "[$-9000449]d-m-yy;@",
				"[$-9000449]d.m.yy;@", "[$-9000449]yyyy-mm-dd;@", "[$-9000449]dd mmmm yyyy;@", "[$-9000449]d mmmm yyyy;@"
			}, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "[$-449]hh:mm:ss AM/PM;@", "[$-449]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("tt", new List<string> { "dd.mm.yyyy;@", "dd.mm.yy;@", "d.m.yy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "[$-444]d mmmm yyyy;@", "[$-444]dd mmmm yyyy;@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("te", new List<string>
			{
				"dd-mm-yy;@", "d-m-yy;@", "d.m.yy;@", "dd-mm-yyyy;@", "yyyy-mm-dd;@", "[$-44A]dd mmmm yyyy;@", "[$-44A]d mmmm yyyy;@", "[$-A00044A]dd-mm-yy;@", "[$-A00044A]d-m-yy;@", "[$-A00044A]d.m.yy;@",
				"[$-A00044A]dd-mm-yyyy;@", "[$-A00044A]yyyy-mm-dd;@", "[$-A00044A]dd mmmm yyyy;@", "[$-A00044A]d mmmm yyyy;@"
			}, new List<string> { "hh:mm:ss;@", "h:mm:ss;@", "[$-44A]AM/PM hh:mm:ss;@", "[$-44A]AM/PM h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("th", new List<string>
			{
				"[$-1070000]d/m/yy;@", "[$-1070000]d/mm/yyyy;@", "[$-1070000]d/mm/yyyy h:mm \"น.\";@", "[$-1070409]d/mm/yyyy h:mm AM/PM;@", "[$-D070000]d/m/yy;@", "[$-D070000]d/mm/yyyy;@", "[$-D070000]d/mm/yyyy h:mm \"น.\";@", "[$-D07041E]d mmm yy;@", "[$-D07041E]d mmmm yyyy;@", "[$-107041E]d mmm yy;@",
				"[$-107041E]d mmmm yyyy;@"
			}, new List<string> { "[$-D000000]h:mm:ss AM/PM;@", "[$-D000000]h:mm:ss;@", "[$-D000000]h:mm \"น.\";@", "[$-D000409]h:mm AM/PM;@", "[$-1000000]h:mm:ss AM/PM;@", "[$-1000000]h:mm:ss;@", "[$-1000000]h:mm \"น\";@", "[$-1000409]h:mm AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("bo-CN", new List<string>
			{
				"[$-10451]yyyy/m/d;@", "[$-10451]yyyy-m-d;@", "[$-10451]yyyy.m.d;@", "[$-10451]yyyy.mm.dd;@", "[$-10451]yyyy-mm-dd;@", "[$-10451]yyyy/mm/dd;@", "[$-10451]yy-m-d;@", "[$-10451]yy/m/d;@", "[$-10451]yy.m.d;@", "[$-10451]yyyy\"ལ\u0f7cའ\u0f72་ཟ\u0fb3\" m\"ཚ\u0f7aས\" d;@",
				"[$-10451]yyyy\"ལ\u0f7cའ\u0f72་ཟ\u0fb3\" m\"ཚ\u0f7aས\" d dddd;@"
			}, new List<string> { "[$-10451]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("tr", new List<string>
			{
				"d/m;@", "d/m/yy;@", "dd/mm/yy;@", "[$-41F]d mmmm;@", "[$-41F]d mmmm yy;@", "[$-41F]dd mmmm yy;@", "dd/mm/yyyy;@", "[$-41F]mmmm yy;@", "[$-41F]d mmmm yyyy;@", "d/m/yy h:mm;@",
				"[$-41F]d mmmm yyyy h:mm;@", "[$-41F]mmmmm;@", "[$-41F]mmmmm yy;@", "m/d/yyyy;@", "[$-41F]d mmm yyyy;@"
			}, new List<string> { "hh:mm;@", "hh:mm:ss;@", "mm:ss.0;@", "d/m/yy hh:mm;@", "dd/mm/yy hh:mm;@", "d/m/yyyy hh:mm;@", "m/d/yy hh:mm;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("tk", new List<string> { "[$-10442]dd.mm.yy;@", "[$-10442]yyyy-mm-dd;@", "[$-10442]yyyy \"ý.\" mmmm d;@", "[$-10442]yyyy \"ý.\" mmmm dd;@" }, new List<string> { "[$-10442]h:mm:ss;@", "[$-10442]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("uk", new List<string> { "dd.mm.yyyy;@", "dd.mm.yy;@", "yyyy-mm-dd;@", "[$-FC22]d mmmm yyyy\" р.\";@" }, new List<string> { "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("hsb", new List<string> { "[$-1042E]d. m. yyyy;@", "[$-1042E]d. m. yy;@", "[$-1042E]dd.mm.yyyy;@", "[$-1042E]dd.mm.yy;@", "[$-1042E]yyyy-mm-dd;@", "[$-1042E]dddd, \"dnja\" d. mmmm yyyy;@", "[$-1042E]dddd, d. mmmm yyyy;@", "[$-1042E]d. mmmm yyyy;@" }, new List<string> { "[$-1042E]h:mm:ss;@", "[$-1042E]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ur", new List<string>
			{
				"dd/mm/yyyy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "m/d/yyyy;@", "m/d/yy;@", "mm/dd/yy;@", "mm/dd/yyyy;@", "[$-420]dd mmmm, yyyy;@", "[$-420]dddd, dd mmmm, yyyy;@", "[$-420]dddd, mmmm dd, yyyy;@",
				"[$-420]mmmm dd, yyyy;@", "[$-420]dd/mmmm/yyyy;@"
			}, new List<string> { "[$-409]h:mm:ss AM/PM;@", "[$-409]hh:mm:ss AM/PM;@", "h:mm:ss;@", "hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ug", new List<string> { "[$-10480]yyyy-m-d;@", "[$-10480]yyyy.m.d;@", "[$-10480]yyyy-mm-dd;@", "[$-10480]yyyy.mm.dd;@", "[$-10480]yyyy-\"يىلى\" mmmm d-\"كۈنى،\";@", "[$-10480]yyyy-\"يىلى\" mmmm d-\"كۈنى،\" dddd;@" }, new List<string> { "[$-10480]h:mm:ss;@", "[$-10480]hh:mm:ss;@", "[$-10480]AM/PM h:mm:ss;@", "[$-10480]AM/PM hh:mm:ss;@", "[$-10480]yyyy-\"يىلى\" mmmm d-\"كۈنى،\";@", "[$-10480]yyyy-\"يىلى\" mmmm d-\"كۈنى،\" dddd;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("uz-Cyrl", new List<string> { "dd.mm.yyyy;@", "dd.mm.yy;@", "dd/mm yyyy;@", "d.m.yy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "[$-843]yyyy \"йил\" d-mmmm;@", "[$-843]d mmmm yyyy;@", "[$-843]dd mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("uz-Latn", new List<string> { "dd/mm yyyy;@", "dd.mm.yy;@", "d.m.yy;@", "dd/mm/yy;@", "yyyy-mm-dd;@", "[$-443]yyyy \"yil\" d-mmmm;@", "[$-443]d mmmm yyyy;@", "[$-443]dd mmmm yyyy;@" }, new List<string> { "hh:mm:ss;@", "h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("vi", new List<string> { "[$-1010000]d/m/yy;@", "[$-1010000]d/m/yyyy;@", "[$-101042A]d/m/yyyy h:mm AM/PM;@", "[$-1010409]d/m/yyyy h:mm AM/PM;@", "[$-101042A]d mmm yy;@", "[$-101042A]d mmmm yyyy;@", "[$-101040C]d mmm yy;@", "[$-101040C]d mmmm yyyy;@", "[$-1010409]d mmm yy;@", "[$-1010409]d mmmm yyyy;@" }, new List<string> { "[$-1000000]h:mm;@", "[$-100042A]h:mm:ss AM/PM;@", "[$-1000409]h:mm:ss AM/PM;@", "[$-1000000]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("cy", new List<string> { "[$-10452]dd/mm/yyyy;@", "[$-10452]dd/mm/yy;@", "[$-10452]d/m/yy;@", "[$-10452]d.m.yy;@", "[$-10452]yyyy-mm-dd;@", "[$-10452]dd mmmm yyyy;@", "[$-10452]d mmmm yyyy;@" }, new List<string> { "[$-10452]hh:mm:ss;@", "[$-10452]h:mm:ss;@", "[$-10452]hh:mm:ss AM/PM;@", "[$-10452]h:mm:ss AM/PM;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("wo", new List<string> { "[$-10488]dd/mm/yyyy;@", "[$-10488]dd/mm/yy;@", "[$-10488]dd.mm.yy;@", "[$-10488]dd-mm-yy;@", "[$-10488]yyyy-mm-dd;@", "[$-10488]dddd d mmmm yyyy;@", "[$-10488]d mmm yy;@", "[$-10488]d mmmm yyyy;@" }, new List<string> { "[$-10488]hh:mm:ss;@", "[$-10488]h:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("ii-CN", new List<string> { "[$-10478]yyyy/m/d;@", "[$-10478]yyyy-m-d;@", "[$-10478]yyyy.m.d;@", "[$-10478]yyyy.mm.dd;@", "[$-10478]yyyy-mm-dd;@", "[$-10478]yyyy/mm/dd;@", "[$-10478]yyyy\"ꈎ\" m\"ꆪ\" d\"ꑍ\";@", "[$-10478]dddd, yyyy\"ꈎ\" m\"ꆪ\" d\"ꑍ\";@", "[$-10478]yyyy\"ꈎ\" m\"ꆪ\" d\"ꑍ\", dddd;@" }, new List<string> { "[$-10478]h:mm:ss;@", "[$-10478]hh:mm:ss;@" });
			DateTimeFormatCategoryManager.AddCultureInfoDateTimeFormats("yo", new List<string>
			{
				"[$-1046A]d/m/yyyy;@", "[$-1046A]d/m/yy;@", "[$-1046A]dd/mm/yy;@", "[$-1046A]dd/mm/yyyy;@", "[$-1046A]yy/mm/dd;@", "[$-1046A]yyyy-mm-dd;@", "[$-1046A]dd-mmm-yy;@", "[$-1046A]dddd, mmmm dd, yyyy;@", "[$-1046A]mmmm dd, yyyy;@", "[$-1046A]dddd, dd mmmm, yyyy;@",
				"[$-1046A]dd mmmm, yyyy;@"
			}, new List<string> { "[$-1046A]h:mm:ss AM/PM;@", "[$-1046A]hh:mm:ss AM/PM;@", "[$-1046A]h:mm:ss;@", "[$-1046A]hh:mm:ss;@" });
		}

		static void AddCultureInfoDateTimeFormats(string cultureCode, List<string> dateFormatStrings, List<string> timeFormatStrings)
		{
			try
			{
				CultureInfo key = new CultureInfo(cultureCode);
				DateTimeFormatCategoryManager.CultureInfoToDateFormats.Add(key, dateFormatStrings);
				DateTimeFormatCategoryManager.CultureInfoToTimeFormats.Add(key, timeFormatStrings);
			}
			catch (CultureNotFoundException)
			{
			}
		}

		static readonly Dictionary<CultureInfo, IList<string>> cultureInfoToDateFormats = new Dictionary<CultureInfo, IList<string>>();

		static readonly Dictionary<CultureInfo, IList<string>> cultureInfoToTimeFormats = new Dictionary<CultureInfo, IList<string>>();
	}
}
