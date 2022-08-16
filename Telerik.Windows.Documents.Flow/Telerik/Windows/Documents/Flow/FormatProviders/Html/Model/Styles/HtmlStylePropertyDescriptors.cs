using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	static class HtmlStylePropertyDescriptors
	{
		static HtmlStylePropertyDescriptors()
		{
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.ColorPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.FontSizePropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.FontFamilyPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.TextAlignPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.FontWeightPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.FontStylePropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.TextDecorationPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BackgroundColorPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.VerticalAlignmentPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.MarginLeftPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.MarginTopPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.MarginRightPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.MarginBottomPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.DirectionPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.TextIndentPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.LineHeightPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.WidthPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderSpacingPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderCollapsePropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderLeftPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderTopPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderRightPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderBottomPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.BorderColorPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.TableLayoutPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.RowHeightPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.PaddingPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.PaddingLeftPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.PaddingRightPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.PaddingTopPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.PaddingBottomPropertyDescriptor);
			HtmlStylePropertyDescriptors.RegisterPropertyDescriptor(HtmlStylePropertyDescriptors.ListStyleTypeDescriptor);
		}

		public static bool TryGetPropertyDescriptor(string propertyName, out HtmlStylePropertyDescriptor result)
		{
			return HtmlStylePropertyDescriptors.propertyDescriptors.TryGetValue(propertyName, out result);
		}

		static void RegisterPropertyDescriptor(HtmlStylePropertyDescriptor descriptor)
		{
			Guard.ThrowExceptionIfNull<HtmlStylePropertyDescriptor>(descriptor, "descriptor");
			HtmlStylePropertyDescriptors.propertyDescriptors[descriptor.Name] = descriptor;
		}

		public static readonly HtmlStylePropertyDescriptor ColorPropertyDescriptor = new HtmlStylePropertyDescriptor("color", true);

		public static readonly HtmlStylePropertyDescriptor FontSizePropertyDescriptor = new HtmlStylePropertyDescriptor("font-size", true, true, false);

		public static readonly HtmlStylePropertyDescriptor FontFamilyPropertyDescriptor = new HtmlStylePropertyDescriptor("font-family", true);

		public static readonly HtmlStylePropertyDescriptor TextAlignPropertyDescriptor = new HtmlStylePropertyDescriptor("text-align", true);

		public static readonly HtmlStylePropertyDescriptor FontWeightPropertyDescriptor = new HtmlStylePropertyDescriptor("font-weight", true);

		public static readonly HtmlStylePropertyDescriptor FontStylePropertyDescriptor = new HtmlStylePropertyDescriptor("font-style", true);

		public static readonly HtmlStylePropertyDescriptor TextDecorationPropertyDescriptor = new HtmlStylePropertyDescriptor("text-decoration", true);

		public static readonly HtmlStylePropertyDescriptor BackgroundColorPropertyDescriptor = new HtmlStylePropertyDescriptor("background-color", false);

		public static readonly HtmlStylePropertyDescriptor VerticalAlignmentPropertyDescriptor = new HtmlStylePropertyDescriptor("vertical-align", true);

		public static readonly HtmlStylePropertyDescriptor MarginLeftPropertyDescriptor = new HtmlStylePropertyDescriptor("margin-left", false);

		public static readonly HtmlStylePropertyDescriptor MarginTopPropertyDescriptor = new HtmlStylePropertyDescriptor("margin-top", false);

		public static readonly HtmlStylePropertyDescriptor MarginRightPropertyDescriptor = new HtmlStylePropertyDescriptor("margin-right", false);

		public static readonly HtmlStylePropertyDescriptor MarginBottomPropertyDescriptor = new HtmlStylePropertyDescriptor("margin-bottom", false);

		public static readonly HtmlStylePropertyDescriptor TextIndentPropertyDescriptor = new HtmlStylePropertyDescriptor("text-indent", true);

		public static readonly HtmlStylePropertyDescriptor LineHeightPropertyDescriptor = new HtmlStylePropertyDescriptor("line-height", true);

		public static readonly HtmlStylePropertyDescriptor DirectionPropertyDescriptor = new HtmlStylePropertyDescriptor("direction", true);

		public static readonly HtmlStylePropertyDescriptor WidthPropertyDescriptor = new HtmlStylePropertyDescriptor("width", false);

		public static readonly HtmlStylePropertyDescriptor BorderSpacingPropertyDescriptor = new HtmlStylePropertyDescriptor("border-spacing", true);

		public static readonly HtmlStylePropertyDescriptor BorderCollapsePropertyDescriptor = new HtmlStylePropertyDescriptor("border-collapse", true);

		public static readonly HtmlStylePropertyDescriptor BorderColorPropertyDescriptor = new HtmlStylePropertyDescriptor("border-color", false);

		public static readonly HtmlStylePropertyDescriptor BorderPropertyDescriptor = new HtmlStylePropertyDescriptor("border", false);

		public static readonly HtmlStylePropertyDescriptor BorderLeftPropertyDescriptor = new HtmlStylePropertyDescriptor("border-left", false);

		public static readonly HtmlStylePropertyDescriptor BorderTopPropertyDescriptor = new HtmlStylePropertyDescriptor("border-top", false);

		public static readonly HtmlStylePropertyDescriptor BorderRightPropertyDescriptor = new HtmlStylePropertyDescriptor("border-right", false);

		public static readonly HtmlStylePropertyDescriptor BorderBottomPropertyDescriptor = new HtmlStylePropertyDescriptor("border-bottom", false);

		public static readonly HtmlStylePropertyDescriptor TableLayoutPropertyDescriptor = new HtmlStylePropertyDescriptor("table-layout", false);

		public static readonly HtmlStylePropertyDescriptor RowHeightPropertyDescriptor = new HtmlStylePropertyDescriptor("height", false, false, true);

		public static readonly HtmlStylePropertyDescriptor PaddingPropertyDescriptor = new HtmlStylePropertyDescriptor("padding", false);

		public static readonly HtmlStylePropertyDescriptor PaddingLeftPropertyDescriptor = new HtmlStylePropertyDescriptor("padding-left", false);

		public static readonly HtmlStylePropertyDescriptor PaddingRightPropertyDescriptor = new HtmlStylePropertyDescriptor("padding-right", false);

		public static readonly HtmlStylePropertyDescriptor PaddingTopPropertyDescriptor = new HtmlStylePropertyDescriptor("padding-top", false);

		public static readonly HtmlStylePropertyDescriptor PaddingBottomPropertyDescriptor = new HtmlStylePropertyDescriptor("padding-bottom", false);

		public static readonly HtmlStylePropertyDescriptor ListStyleTypeDescriptor = new HtmlStylePropertyDescriptor("list-style-type", true);

		static readonly Dictionary<string, HtmlStylePropertyDescriptor> propertyDescriptors = new Dictionary<string, HtmlStylePropertyDescriptor>();
	}
}
