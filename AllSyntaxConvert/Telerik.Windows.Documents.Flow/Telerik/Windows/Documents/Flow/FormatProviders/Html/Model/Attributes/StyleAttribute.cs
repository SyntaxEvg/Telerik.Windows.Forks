using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes
{
	class StyleAttribute : HtmlAttribute<HtmlStyleProperties>
	{
		public StyleAttribute()
			: base("style", false)
		{
			base.Value = new HtmlStyleProperties();
		}

		public override bool ShouldExport()
		{
			return base.Value.HasProperties;
		}

		public override string GetValue()
		{
			CssWriter cssWriter = new CssWriter();
			base.Value.Write(cssWriter);
			return cssWriter.GetData();
		}

		public override void SetValue(string value)
		{
			InlineCssReader inlineCssReader = new InlineCssReader();
			inlineCssReader.ReadStyleProperties(base.Value, value);
		}

		public override void ResetValue()
		{
			base.Value.ClearProperties();
		}
	}
}
