using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	class CssStyleWriter : CssWriter
	{
		public CssStyleWriter()
		{
			base.Writer.AppendLine();
		}

		public void WriteStyleStart(string name, SelectorType type, string forElementType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			base.Writer.AppendLine(string.Format("{0} {{", CssStyleWriter.BuildSelectorName(type, name, forElementType)));
		}

		public void WriteStyleEnd()
		{
			base.Writer.AppendLine("}");
		}

		protected override void WriteStylePropertyOverride(string name, string value)
		{
			base.Writer.AppendLine();
		}

		static string BuildSelectorName(SelectorType type, string name, string forElementType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			string result;
			switch (type)
			{
			case SelectorType.Style:
				result = string.Format(".{0}", name);
				break;
			case SelectorType.Elements:
				result = string.Format("{0}", name);
				break;
			case SelectorType.StyleForElements:
				result = string.Format("{0}.{1}", forElementType, name);
				break;
			default:
				throw new NotSupportedException("Selector type is not supported.");
			}
			return result;
		}

		const string CssStyleSelectorNameFormat = ".{0}";

		const string CssStyleSelectorForElementNameFormat = "{0}.{1}";

		const string CssElementSelectorNameFormat = "{0}";

		const string CssClassStartFormat = "{0} {{";

		const string CssClassFromatEndFormat = "}";
	}
}
