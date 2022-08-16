using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class TitleElement : HtmlElementBase
	{
		public TitleElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "title";
			}
		}

		protected override bool ForceFullEndElement
		{
			get
			{
				return true;
			}
		}
	}
}
