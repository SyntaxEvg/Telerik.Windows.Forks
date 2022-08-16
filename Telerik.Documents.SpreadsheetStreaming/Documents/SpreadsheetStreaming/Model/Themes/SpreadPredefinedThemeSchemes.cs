using System;
using System.Collections.ObjectModel;
using Telerik.Documents.SpreadsheetStreaming.ImportExport;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Themes
{
	static class SpreadPredefinedThemeSchemes
	{
		static SpreadPredefinedThemeSchemes()
		{
			SpreadPredefinedThemeSchemes.colorSchemes = new ReadOnlyCollection<SpreadThemeColorScheme>(new SpreadThemeColorScheme[]
			{
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Office", "#FFFFFF", "#000000", "#EEECE1", "#1F497D", "#4F81BD", "#C0504D", "#9BBB59", "#8064A2", "#4BACC6", "#F79646", "#0000FF", "#800080"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Grayscale", "#FFFFFF", "#000000", "#F8F8F8", "#000000", "#DDDDDD", "#B2B2B2", "#969696", "#808080", "#5F5F5F", "#4D4D4D", "#5F5F5F", "#919191"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Adjacency", "#FFFFFF", "#2F2B20", "#DFDCB7", "#675E47", "#A9A57C", "#9CBEBD", "#D2CB6C", "#95A39D", "#C89F5D", "#B1A089", "#D25814", "#849A0A"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Angels", "#FFFFFF", "#000000", "#CDD7D9", "#434342", "#797B7E", "#F96A1B", "#08A1D9", "#7C984A", "#C2AD8D", "#506E94", "#5F5F5F", "#969696"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Apex", "#FFFFFF", "#000000", "#C9C2D1", "#69676D", "#CEB966", "#9CB084", "#6BB1C9", "#6585CF", "#7E6BC9", "#A379BB", "#410082", "#932968"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Apothecary", "#FFFFFF", "#000000", "#ECEDD1", "#564B3C", "#93A299", "#CF543F", "#B5AE53", "#848058", "#E8B54D", "#786C71", "#CCCC00", "#B2B2B2"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Aspect", "#FFFFFF", "#000000", "#E3DED1", "#323232", "#F07F09", "#9F2936", "#1B587C", "#4E8542", "#604878", "#C19859", "#6B9F25", "#B26B02"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Austin", "#FFFFFF", "#000000", "#CAF278", "#3E3D2D", "#94C600", "#71685A", "#FF6700", "#909465", "#956B43", "#FEA022", "#E68200", "#FFA94A"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Black Tie", "#FFFFFF", "#000000", "#E3DCCF", "#46464A", "#6F6F74", "#A7B789", "#BEAE98", "#92A9B9", "#9C8265", "#8D6974", "#67AABF", "#B1B5AB"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Civic", "#FFFFFF", "#000000", "#C5D1D7", "#646B86", "#D16349", "#CCB400", "#8CADAE", "#8C7B70", "#8FB08C", "#D19049", "#00A3D6", "#694F07"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Clarity", "#FFFFFF", "#292934", "#F3F2DC", "#D2533C", "#93A299", "#AD8F67", "#726056", "#4C5A6A", "#808DA0", "#79463D", "#0000FF", "#800080"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Composite", "#FFFFFF", "#000000", "#E7ECED", "#5B6973", "#98C723", "#59B0B9", "#DEAE00", "#B77BB4", "#E0773C", "#A98D63", "#26CBEC", "#598C8C"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Concourse", "#FFFFFF", "#000000", "#DEF5FA", "#464646", "#2DA2BF", "#DA1F28", "#EB641B", "#39639D", "#474B78", "#7D3C4A", "#FF8119", "#44B9E8"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Couture", "#FFFFFF", "#000000", "#D0CCB9", "#37302A", "#9E8E5C", "#A09781", "#85776D", "#AEAFA9", "#8D878B", "#6B6149", "#B6A272", "#8A784F"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Elemental", "#FFFFFF", "#000000", "#ACCBF9", "#242852", "#629DD1", "#297FD5", "#7F8FA9", "#4A66AC", "#5AA2AE", "#9D90A0", "#9454C3", "#3EBBF0"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Equity", "#FFFFFF", "#000000", "#E9E5DC", "#696464", "#D34817", "#9B2D1F", "#A28E6A", "#956251", "#918485", "#855D5D", "#CC9900", "#96A9A9"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Essential", "#FFFFFF", "#000000", "#C8C8B1", "#D1282E", "#7A7A7A", "#F5C501", "#526DB0", "#989AAC", "#DC5924", "#B4B392", "#CC9900", "#969696"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Executive", "#FFFFFF", "#000000", "#E4E9EF", "#2F5897", "#6076B4", "#9C5252", "#E68422", "#846648", "#63891F", "#758085", "#3399FF", "#B2B2B2"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Flow", "#FFFFFF", "#000000", "#DBF5F9", "#04617B", "#0F6FC6", "#009DD9", "#0BD0D9", "#10CF9B", "#7CCA62", "#A5C249", "#F49100", "#85DFD0"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Foundry", "#FFFFFF", "#000000", "#EAEBDE", "#676A55", "#72A376", "#B0CCB0", "#A8CDD7", "#C0BEAF", "#CEC597", "#E8B7B7", "#DB5353", "#903638"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Grid", "#FFFFFF", "#000000", "#CCD1B9", "#534949", "#C66951", "#BF974D", "#928B70", "#87706B", "#94734E", "#6F777D", "#CC9900", "#C0C0C0"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Hardcover", "#FFFFFF", "#000000", "#ECE9C6", "#895D1D", "#873624", "#D6862D", "#D0BE40", "#877F6C", "#972109", "#AEB795", "#CC9900", "#B2B2B2"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Horizon", "#FFFFFF", "#000000", "#DC9E1F", "#1F2123", "#7E97AD", "#CC8E60", "#7A6A60", "#B4936D", "#67787B", "#9D936F", "#646464", "#969696"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Median", "#FFFFFF", "#000000", "#EBDDC3", "#775F55", "#94B6D2", "#DD8047", "#A5AB81", "#D8B25C", "#7BA79D", "#968C8C", "#F7B615", "#704404"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Metro", "#FFFFFF", "#000000", "#D6ECFF", "#4E5B6F", "#7FD13B", "#EA157A", "#FEB80A", "#00ADDC", "#738AC8", "#1AB39F", "#EB8803", "#5F7791"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Module", "#FFFFFF", "#000000", "#D4D4D6", "#5A6378", "#F0AD00", "#60B5CC", "#E66C7D", "#6BB76D", "#E88651", "#C64847", "#168BBA", "#680000"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Newsprint", "#FFFFFF", "#000000", "#DEDEE0", "#303030", "#AD0101", "#726056", "#AC956E", "#808DA9", "#424E5B", "#730E00", "#D26900", "#D89243"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Opulent", "#FFFFFF", "#000000", "#F4E7ED", "#B13F9A", "#B83D68", "#AC66BB", "#DE6C36", "#F9B639", "#CF6DA4", "#FA8D3D", "#FFDE66", "#D490C5"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Oriel", "#FFFFFF", "#000000", "#FFF39D", "#575F6D", "#FE8637", "#7598D9", "#B32C16", "#F5CD2D", "#AEBAD5", "#777C84", "#D2611C", "#3B435B"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Origin", "#FFFFFF", "#000000", "#DDE9EC", "#464653", "#727CA3", "#9FB8CD", "#D2DA7A", "#FADA7A", "#B88472", "#8E736A", "#B292CA", "#6B5680"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Paper", "#FFFFFF", "#000000", "#FEFAC9", "#444D26", "#A5B592", "#F3A447", "#E7BC29", "#D092A7", "#9C85C0", "#809EC2", "#8E58B6", "#7F6F6F"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Perspective", "#FFFFFF", "#000000", "#FF8600", "#283138", "#838D9B", "#D2610C", "#80716A", "#94147C", "#5D5AD2", "#6F6C7D", "#6187E3", "#7B8EB8"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Pushpin", "#FFFFFF", "#000000", "#CCDDEA", "#465E9C", "#FDA023", "#AA2B1E", "#71685C", "#64A73B", "#EB5605", "#B9CA1A", "#D83E2C", "#ED7D27"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Slipstream", "#FFFFFF", "#000000", "#B4DCFA", "#212745", "#4E67C8", "#5ECCF3", "#A7EA52", "#5DCEAF", "#FF8021", "#F14124", "#56C7AA", "#59A8D1"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Solstice", "#FFFFFF", "#000000", "#E7DEC9", "#4F271C", "#3891A7", "#FEB80A", "#C32D2E", "#84AA33", "#964305", "#475A8D", "#8DC765", "#AA8A14"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Technic", "#FFFFFF", "#000000", "#D4D2D0", "#3B3B3B", "#6EA0B0", "#CCAF0A", "#8D89A4", "#748560", "#9E9273", "#7E848D", "#00C8C3", "#A116E0"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Thatch", "#FFFFFF", "#000000", "#DFE6D0", "#1D3641", "#759AA5", "#CFC60D", "#99987F", "#90AC97", "#FFAD1C", "#B9AB6F", "#66AACD", "#809DB3"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Trek", "#FFFFFF", "#000000", "#FBEEC9", "#4E3B30", "#F0A22E", "#A5644E", "#B58B80", "#C3986D", "#A19574", "#C17529", "#AD1F1F", "#FFC42F"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Urban", "#FFFFFF", "#000000", "#DEDEDE", "#424456", "#53548A", "#438086", "#A04DA3", "#C4652D", "#8B5D3D", "#5C92B5", "#67AFBD", "#C2A874"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Verve", "#FFFFFF", "#000000", "#D2D2D2", "#666666", "#FF388C", "#E40059", "#9C007F", "#68007F", "#005BD3", "#00349E", "#17BBFD", "#FF79C2"),
				SpreadPredefinedThemeSchemes.CreateThemeColorScheme("Waveform", "#FFFFFF", "#000000", "#C6E7FC", "#073E87", "#31B6FD", "#4584D3", "#5BD078", "#A5D028", "#F5C040", "#05E0DB", "#0080FF", "#5EAEFF")
			});
			SpreadThemeColorScheme spreadThemeColorScheme = SpreadPredefinedThemeSchemes.ColorSchemes[0];
			SpreadThemeFontScheme fontScheme = SpreadPredefinedThemeSchemes.FontSchemes[0];
			SpreadPredefinedThemeSchemes.defaultTheme = new SpreadTheme(spreadThemeColorScheme.Name, spreadThemeColorScheme, fontScheme);
		}

		public static SpreadTheme DefaultTheme
		{
			get
			{
				return SpreadPredefinedThemeSchemes.defaultTheme;
			}
		}

		public static ReadOnlyCollection<SpreadThemeColorScheme> ColorSchemes
		{
			get
			{
				return SpreadPredefinedThemeSchemes.colorSchemes;
			}
		}

		public static ReadOnlyCollection<SpreadThemeFontScheme> FontSchemes
		{
			get
			{
				return SpreadPredefinedThemeSchemes.fontSchemes;
			}
		}

		static SpreadThemeColorScheme CreateThemeColorScheme(string name, string background1, string text1, string background2, string text2, string accent1, string accent2, string accent3, string accent4, string accent5, string accent6, string hyperlink, string followedHyperlink)
		{
			return new SpreadThemeColorScheme(name, ColorsHelper.HexStringToColor(background1), ColorsHelper.HexStringToColor(text1), ColorsHelper.HexStringToColor(background2), ColorsHelper.HexStringToColor(text2), ColorsHelper.HexStringToColor(accent1), ColorsHelper.HexStringToColor(accent2), ColorsHelper.HexStringToColor(accent3), ColorsHelper.HexStringToColor(accent4), ColorsHelper.HexStringToColor(accent5), ColorsHelper.HexStringToColor(accent6), ColorsHelper.HexStringToColor(hyperlink), ColorsHelper.HexStringToColor(followedHyperlink));
		}

		static readonly SpreadTheme defaultTheme;

		static readonly ReadOnlyCollection<SpreadThemeColorScheme> colorSchemes;

		static readonly ReadOnlyCollection<SpreadThemeFontScheme> fontSchemes = new ReadOnlyCollection<SpreadThemeFontScheme>(new SpreadThemeFontScheme[]
		{
			new SpreadThemeFontScheme("Office", "Cambria", "Calibri", null, null, null, null),
			new SpreadThemeFontScheme("Office 2", "Calibri", "Cambria", null, null, null, null),
			new SpreadThemeFontScheme("Office Classic", "Arial", "Times New Roman", null, null, null, null),
			new SpreadThemeFontScheme("Office Classic 2", "Arial", "Arial", null, null, null, null),
			new SpreadThemeFontScheme("Adjacency", "Cambria", "Calibri", null, null, null, null),
			new SpreadThemeFontScheme("Angels", "Franklin Gothic Medium", "Franklin Gothic Book", null, null, null, null),
			new SpreadThemeFontScheme("Apex", "Lucida Sans", "Book Antiqua", null, null, null, null),
			new SpreadThemeFontScheme("Apothecary", "Book Antiqua", "Century Gothic", null, null, null, null),
			new SpreadThemeFontScheme("Aspect", "Verdana", "Verdana", null, null, null, null),
			new SpreadThemeFontScheme("Austin", "Century Gothic", "Century Gothic", null, null, null, null),
			new SpreadThemeFontScheme("Black Tie", "Garamond", "Garamond", null, null, null, null),
			new SpreadThemeFontScheme("Civic", "Georgia", "Georgia", null, null, null, null),
			new SpreadThemeFontScheme("Clarity", "Arial", "Arial", null, null, null, null),
			new SpreadThemeFontScheme("Composite", "Calibri", "Calibri", null, null, null, null),
			new SpreadThemeFontScheme("Concourse", "Lucida Sans Unicode", "Lucida Sans Unicode", null, null, null, null),
			new SpreadThemeFontScheme("Couture", "Garamond", "Garamond", null, null, null, null),
			new SpreadThemeFontScheme("Elemental", "Palatino Linotype", "Palatino Linotype", null, null, null, null),
			new SpreadThemeFontScheme("Equity", "Franklin Gothic Book", "Perpetua", null, null, null, null),
			new SpreadThemeFontScheme("Essential", "Arial Black", "Arial", null, null, null, null),
			new SpreadThemeFontScheme("Executive", "Century Gothic", "Palatino Linotype", null, null, null, null),
			new SpreadThemeFontScheme("Flow", "Calibri", "Constantia", null, null, null, null),
			new SpreadThemeFontScheme("Foundry", "Rockwell", "Rockwell", null, null, null, null),
			new SpreadThemeFontScheme("Grid", "Franklin Gothic Medium", "Franklin Gothic Medium", null, null, null, null),
			new SpreadThemeFontScheme("Hardcover", "Book Antiqua", "Book Antiqua", null, null, null, null),
			new SpreadThemeFontScheme("Horizon", "Arial Narrow", "Arial Narrow", null, null, null, null),
			new SpreadThemeFontScheme("Median", "Tw Cen MT", "Tw Cen MT", null, null, null, null),
			new SpreadThemeFontScheme("Metro", "Consolas", "Corbel", null, null, null, null),
			new SpreadThemeFontScheme("Module", "Corbel", "Corbel", null, null, null, null),
			new SpreadThemeFontScheme("Newsprint", "Impact", "Times New Roman", null, null, null, null),
			new SpreadThemeFontScheme("Opulent", "Trebuchet MS", "Trebuchet MS", null, null, null, null),
			new SpreadThemeFontScheme("Oriel", "Century Schoolbook", "Century Schoolbook", null, null, null, null),
			new SpreadThemeFontScheme("Origin", "Bookman Old Style", "Gill Sans MT", null, null, null, null),
			new SpreadThemeFontScheme("Paper", "Constantia", "Constantia", null, null, null, null),
			new SpreadThemeFontScheme("Perspective", "Arial", "Arial", null, null, null, null),
			new SpreadThemeFontScheme("Pushpin", "Constantia", "Franklin Gothic Book", null, null, null, null),
			new SpreadThemeFontScheme("Slipstream", "Trebuchet MS", "Trebuchet MS", null, null, null, null),
			new SpreadThemeFontScheme("Solstice", "Gill Sans MT", "Gill Sans MT", null, null, null, null),
			new SpreadThemeFontScheme("Technic", "Franklin Gothic Book", "Arial", null, null, null, null),
			new SpreadThemeFontScheme("Thatch", "Tw Cen MT", "Tw Cen MT", null, null, null, null),
			new SpreadThemeFontScheme("Trek", "Franklin Gothic Medium", "Franklin Gothic Book", null, null, null, null),
			new SpreadThemeFontScheme("Urban", "Trebuchet MS", "Georgia", null, null, null, null),
			new SpreadThemeFontScheme("Verve", "Century Gothic", "Century Gothic", null, null, null, null),
			new SpreadThemeFontScheme("Waveform", "Candara", "Candara", null, null, null, null)
		});
	}
}
