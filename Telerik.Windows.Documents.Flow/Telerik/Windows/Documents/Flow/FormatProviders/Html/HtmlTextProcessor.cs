using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	class HtmlTextProcessor
	{
		HtmlTextProcessor()
		{
			this.htmlEscape = new Dictionary<int, string>();
			this.RegisterHtmlEscape(34, "&quot;");
			this.RegisterHtmlEscape(38, "&amp;");
			this.RegisterHtmlEscape(60, "&lt;");
			this.RegisterHtmlEscape(62, "&gt;");
			this.RegisterHtmlEscape(153, "&trade;");
			this.RegisterHtmlEscape(8482, "&trade;");
			this.RegisterHtmlEscape(160, "&nbsp;");
			this.RegisterHtmlEscape(9, "&nbsp; &nbsp; ");
			this.RegisterHtmlEscape(161, "&iexcl;");
			this.RegisterHtmlEscape(162, "&cent;");
			this.RegisterHtmlEscape(163, "&pound;");
			this.RegisterHtmlEscape(8364, "&euro;");
			this.RegisterHtmlEscape(164, "&curren;");
			this.RegisterHtmlEscape(165, "&yen;");
			this.RegisterHtmlEscape(166, "&brvbar;");
			this.RegisterHtmlEscape(167, "&sect;");
			this.RegisterHtmlEscape(168, "&uml;");
			this.RegisterHtmlEscape(169, "&copy;");
			this.RegisterHtmlEscape(170, "&ordf;");
			this.RegisterHtmlEscape(171, "&laquo;");
			this.RegisterHtmlEscape(172, "&not;");
			this.RegisterHtmlEscape(173, "&shy;");
			this.RegisterHtmlEscape(174, "&reg;");
			this.RegisterHtmlEscape(175, "&macr;");
			this.RegisterHtmlEscape(176, "&deg;");
			this.RegisterHtmlEscape(177, "&plusmn;");
			this.RegisterHtmlEscape(178, "&sup2;");
			this.RegisterHtmlEscape(179, "&sup3;");
			this.RegisterHtmlEscape(180, "&acute;");
			this.RegisterHtmlEscape(181, "&micro;");
			this.RegisterHtmlEscape(182, "&para;");
			this.RegisterHtmlEscape(183, "&middot;");
			this.RegisterHtmlEscape(184, "&cedil;");
			this.RegisterHtmlEscape(185, "&sup1;");
			this.RegisterHtmlEscape(186, "&ordm;");
			this.RegisterHtmlEscape(187, "&raquo;");
			this.RegisterHtmlEscape(188, "&frac14;");
			this.RegisterHtmlEscape(189, "&frac12;");
			this.RegisterHtmlEscape(190, "&frac34;");
			this.RegisterHtmlEscape(191, "&iquest;");
			this.RegisterHtmlEscape(215, "&times;");
			this.RegisterHtmlEscape(247, "&divide;");
			this.RegisterHtmlEscape(192, "&Agrave;");
			this.RegisterHtmlEscape(193, "&Aacute;");
			this.RegisterHtmlEscape(194, "&Acirc;");
			this.RegisterHtmlEscape(195, "&Atilde;");
			this.RegisterHtmlEscape(196, "&Auml;");
			this.RegisterHtmlEscape(197, "&Aring;");
			this.RegisterHtmlEscape(198, "&AElig;");
			this.RegisterHtmlEscape(199, "&Ccedil;");
			this.RegisterHtmlEscape(200, "&Egrave;");
			this.RegisterHtmlEscape(201, "&Eacute;");
			this.RegisterHtmlEscape(202, "&Ecirc;");
			this.RegisterHtmlEscape(203, "&Euml;");
			this.RegisterHtmlEscape(204, "&Igrave;");
			this.RegisterHtmlEscape(205, "&Iacute;");
			this.RegisterHtmlEscape(206, "&Icirc;");
			this.RegisterHtmlEscape(207, "&Iuml;");
			this.RegisterHtmlEscape(208, "&ETH;");
			this.RegisterHtmlEscape(209, "&Ntilde;");
			this.RegisterHtmlEscape(210, "&Ograve;");
			this.RegisterHtmlEscape(211, "&Oacute;");
			this.RegisterHtmlEscape(212, "&Ocirc;");
			this.RegisterHtmlEscape(213, "&Otilde;");
			this.RegisterHtmlEscape(214, "&Ouml;");
			this.RegisterHtmlEscape(216, "&Oslash;");
			this.RegisterHtmlEscape(217, "&Ugrave;");
			this.RegisterHtmlEscape(218, "&Uacute;");
			this.RegisterHtmlEscape(219, "&Ucirc;");
			this.RegisterHtmlEscape(220, "&Uuml;");
			this.RegisterHtmlEscape(221, "&Yacute;");
			this.RegisterHtmlEscape(222, "&THORN;");
			this.RegisterHtmlEscape(223, "&szlig;");
			this.RegisterHtmlEscape(224, "&agrave;");
			this.RegisterHtmlEscape(225, "&aacute;");
			this.RegisterHtmlEscape(226, "&acirc;");
			this.RegisterHtmlEscape(227, "&atilde;");
			this.RegisterHtmlEscape(228, "&auml;");
			this.RegisterHtmlEscape(229, "&aring;");
			this.RegisterHtmlEscape(230, "&aelig;");
			this.RegisterHtmlEscape(231, "&ccedil;");
			this.RegisterHtmlEscape(232, "&egrave;");
			this.RegisterHtmlEscape(233, "&eacute;");
			this.RegisterHtmlEscape(234, "&ecirc;");
			this.RegisterHtmlEscape(235, "&euml;");
			this.RegisterHtmlEscape(236, "&igrave;");
			this.RegisterHtmlEscape(237, "&iacute;");
			this.RegisterHtmlEscape(238, "&icirc;");
			this.RegisterHtmlEscape(239, "&iuml;");
			this.RegisterHtmlEscape(240, "&eth;");
			this.RegisterHtmlEscape(241, "&ntilde;");
			this.RegisterHtmlEscape(242, "&ograve;");
			this.RegisterHtmlEscape(243, "&oacute;");
			this.RegisterHtmlEscape(244, "&ocirc;");
			this.RegisterHtmlEscape(245, "&otilde;");
			this.RegisterHtmlEscape(246, "&ouml;");
			this.RegisterHtmlEscape(248, "&oslash;");
			this.RegisterHtmlEscape(249, "&ugrave;");
			this.RegisterHtmlEscape(250, "&uacute;");
			this.RegisterHtmlEscape(251, "&ucirc;");
			this.RegisterHtmlEscape(252, "&uuml;");
			this.RegisterHtmlEscape(253, "&yacute;");
			this.RegisterHtmlEscape(254, "&thorn;");
			this.RegisterHtmlEscape(255, "&yuml;");
			this.RegisterHtmlEscape(338, "&OElig;");
			this.RegisterHtmlEscape(339, "&oelig;");
			this.RegisterHtmlEscape(352, "&Scaron;");
			this.RegisterHtmlEscape(353, "&scaron;");
			this.RegisterHtmlEscape(376, "&Yuml;");
			this.RegisterHtmlEscape(710, "&circ;");
			this.RegisterHtmlEscape(732, "&tilde;");
			this.RegisterHtmlEscape(8211, "&ndash;");
			this.RegisterHtmlEscape(8212, "&mdash;");
			this.RegisterHtmlEscape(8216, "&lsquo;");
			this.RegisterHtmlEscape(8217, "&rsquo;");
			this.RegisterHtmlEscape(8218, "&sbquo;");
			this.RegisterHtmlEscape(8220, "&ldquo;");
			this.RegisterHtmlEscape(8221, "&rdquo;");
			this.RegisterHtmlEscape(8222, "&bdquo;");
			this.RegisterHtmlEscape(8224, "&dagger;");
			this.RegisterHtmlEscape(8225, "&Dagger;");
			this.RegisterHtmlEscape(8240, "&permil;");
			this.RegisterHtmlEscape(8249, "&lsaquo;");
			this.RegisterHtmlEscape(8250, "&rsaquo;");
			this.RegisterHtmlEscape(402, "&fnof;");
			this.RegisterHtmlEscape(8226, "&bull;");
			this.RegisterHtmlEscape(8230, "&hellip;");
			this.RegisterHtmlEscape(8242, "&prime;");
			this.RegisterHtmlEscape(8243, "&Prime;");
			this.RegisterHtmlEscape(8254, "&oline;");
			this.RegisterHtmlEscape(8260, "&frasl;");
			this.RegisterHtmlEscape(8704, "&forall;");
			this.RegisterHtmlEscape(8706, "&part;");
			this.RegisterHtmlEscape(8707, "&exist;");
			this.RegisterHtmlEscape(8709, "&empty;");
			this.RegisterHtmlEscape(8711, "&nabla;");
			this.RegisterHtmlEscape(8712, "&isin;");
			this.RegisterHtmlEscape(8713, "&notin;");
			this.RegisterHtmlEscape(8715, "&ni;");
			this.RegisterHtmlEscape(8719, "&prod;");
			this.RegisterHtmlEscape(8721, "&sum;");
			this.RegisterHtmlEscape(8722, "&minus;");
			this.RegisterHtmlEscape(8727, "&lowast;");
			this.RegisterHtmlEscape(8730, "&radic;");
			this.RegisterHtmlEscape(8733, "&prop;");
			this.RegisterHtmlEscape(8734, "&infin;");
			this.RegisterHtmlEscape(8736, "&ang;");
			this.RegisterHtmlEscape(8743, "&and;");
			this.RegisterHtmlEscape(8744, "&or;");
			this.RegisterHtmlEscape(8745, "&cap;");
			this.RegisterHtmlEscape(8746, "&cup;");
			this.RegisterHtmlEscape(8747, "&int;");
			this.RegisterHtmlEscape(8756, "&there4;");
			this.RegisterHtmlEscape(8764, "&sim;");
			this.RegisterHtmlEscape(8773, "&cong;");
			this.RegisterHtmlEscape(8776, "&asymp;");
			this.RegisterHtmlEscape(8800, "&ne;");
			this.RegisterHtmlEscape(8801, "&equiv;");
			this.RegisterHtmlEscape(8804, "&le;");
			this.RegisterHtmlEscape(8805, "&ge;");
			this.RegisterHtmlEscape(8834, "&sub;");
			this.RegisterHtmlEscape(8835, "&sup;");
			this.RegisterHtmlEscape(8836, "&nsub;");
			this.RegisterHtmlEscape(8838, "&sube;");
			this.RegisterHtmlEscape(8839, "&supe;");
			this.RegisterHtmlEscape(8853, "&oplus;");
			this.RegisterHtmlEscape(8855, "&otimes;");
			this.RegisterHtmlEscape(8869, "&perp;");
			this.RegisterHtmlEscape(8901, "&sdot;");
			this.RegisterHtmlEscape(913, "&Alpha;");
			this.RegisterHtmlEscape(914, "&Beta;");
			this.RegisterHtmlEscape(915, "&Gamma;");
			this.RegisterHtmlEscape(916, "&Delta;");
			this.RegisterHtmlEscape(917, "&Epsilon;");
			this.RegisterHtmlEscape(918, "&Zeta;");
			this.RegisterHtmlEscape(919, "&Eta;");
			this.RegisterHtmlEscape(920, "&Theta;");
			this.RegisterHtmlEscape(921, "&Iota;");
			this.RegisterHtmlEscape(922, "&Kappa;");
			this.RegisterHtmlEscape(923, "&Lambda;");
			this.RegisterHtmlEscape(924, "&Mu;");
			this.RegisterHtmlEscape(925, "&Nu;");
			this.RegisterHtmlEscape(926, "&Xi;");
			this.RegisterHtmlEscape(927, "&Omicron;");
			this.RegisterHtmlEscape(928, "&Pi;");
			this.RegisterHtmlEscape(929, "&Rho;");
			this.RegisterHtmlEscape(931, "&Sigma;");
			this.RegisterHtmlEscape(932, "&Tau;");
			this.RegisterHtmlEscape(933, "&Upsilon;");
			this.RegisterHtmlEscape(934, "&Phi;");
			this.RegisterHtmlEscape(935, "&Chi;");
			this.RegisterHtmlEscape(936, "&Psi;");
			this.RegisterHtmlEscape(937, "&Omega;");
			this.RegisterHtmlEscape(945, "&alpha;");
			this.RegisterHtmlEscape(946, "&beta;");
			this.RegisterHtmlEscape(947, "&gamma;");
			this.RegisterHtmlEscape(948, "&delta;");
			this.RegisterHtmlEscape(949, "&epsilon;");
			this.RegisterHtmlEscape(950, "&zeta;");
			this.RegisterHtmlEscape(951, "&eta;");
			this.RegisterHtmlEscape(952, "&theta;");
			this.RegisterHtmlEscape(953, "&iota;");
			this.RegisterHtmlEscape(954, "&kappa;");
			this.RegisterHtmlEscape(955, "&lambda;");
			this.RegisterHtmlEscape(956, "&mu;");
			this.RegisterHtmlEscape(957, "&nu;");
			this.RegisterHtmlEscape(958, "&xi;");
			this.RegisterHtmlEscape(959, "&omicron;");
			this.RegisterHtmlEscape(960, "&pi;");
			this.RegisterHtmlEscape(961, "&rho;");
			this.RegisterHtmlEscape(962, "&sigmaf;");
			this.RegisterHtmlEscape(963, "&sigma;");
			this.RegisterHtmlEscape(964, "&tau;");
			this.RegisterHtmlEscape(965, "&upsilon;");
			this.RegisterHtmlEscape(966, "&phi;");
			this.RegisterHtmlEscape(967, "&chi;");
			this.RegisterHtmlEscape(968, "&psi;");
			this.RegisterHtmlEscape(969, "&omega;");
			this.RegisterHtmlEscape(977, "&thetasym;");
			this.RegisterHtmlEscape(978, "&upsih;");
			this.RegisterHtmlEscape(982, "&piv;");
		}

		public static HtmlTextProcessor TextProcessor
		{
			get
			{
				return HtmlTextProcessor.textProcessor;
			}
		}

		public static string RemoveMultipleWhiteSpaces(string text)
		{
			return HtmlTextProcessor.whiteSpacesPattern.Replace(text, ' '.ToString());
		}

		public static string TrimStart(string text)
		{
			return text.TrimStart(new char[] { ' ' });
		}

		public static string TrimEnd(string text)
		{
			return text.TrimEnd(new char[] { ' ' });
		}

		public static string Normalize(string text)
		{
			return text.Replace('\u00a0', ' ');
		}

		public static string StripText(string currentText, string lastText)
		{
			if (HtmlTextProcessor.IsSpace(currentText.First<char>()))
			{
				currentText = HtmlTextProcessor.TrimStart(currentText);
				if (!string.IsNullOrEmpty(lastText) && !HtmlTextProcessor.IsSpace(lastText.Last<char>()))
				{
					currentText = ' ' + currentText;
				}
			}
			if (!string.IsNullOrEmpty(currentText) && HtmlTextProcessor.IsSpace(currentText.Last<char>()))
			{
				currentText = HtmlTextProcessor.TrimEnd(currentText) + ' ';
			}
			return currentText;
		}

		public static bool IsSpace(char ch)
		{
			return ch == ' ';
		}

		public static string PreserveWhiteSpaces(string text)
		{
			return text.Replace('\t'.ToString(), "&nbsp; &nbsp; ").Replace(HtmlTextProcessor.doubleSpace, HtmlTextProcessor.doubleSpaceHtml);
		}

		public string EscapeHtml(string text)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				string value;
				if (this.htmlEscape.TryGetValue((int)text[i], out value))
				{
					stringBuilder.Append(value);
				}
				else if (text[i] >= ' ')
				{
					stringBuilder.Append(text[i]);
				}
			}
			return stringBuilder.ToString();
		}

		void RegisterHtmlEscape(int ch, string escape)
		{
			Guard.ThrowExceptionIfNullOrEmpty(escape, "escape");
			this.htmlEscape.Add(ch, escape);
		}

		public const char Space = ' ';

		const string NonBreakingSpaceHtml = "&nbsp;";

		const string NonBreakingTabHtml = "&nbsp; &nbsp; ";

		const char NonBreakingSpace = '\u00a0';

		const char Tab = '\t';

		static readonly string doubleSpace = string.Format("{0}{0}", ' ');

		static readonly string doubleSpaceHtml = string.Format("{0}{1}", ' ', "&nbsp;");

		static readonly Regex whiteSpacesPattern = new Regex("[ \\f\\n\\r\\t\\v]+", RegexOptions.Compiled);

		static readonly HtmlTextProcessor textProcessor = new HtmlTextProcessor();

		readonly Dictionary<int, string> htmlEscape;
	}
}
