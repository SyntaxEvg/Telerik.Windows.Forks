using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text
{
	class CenterElement : ParagraphElement
	{
		public CenterElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "center";
			}
		}
	}
}
