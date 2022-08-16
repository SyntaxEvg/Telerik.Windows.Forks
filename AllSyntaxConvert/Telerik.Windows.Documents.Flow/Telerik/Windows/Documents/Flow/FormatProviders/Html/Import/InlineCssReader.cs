using System;
using System.Collections.Generic;
using PreMailer.Net;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class InlineCssReader
	{
		public InlineCssReader()
		{
			this.parser = new CssParser();
		}

		public void ReadStyleProperties(HtmlStyleProperties styleProperties, string style)
		{
			StyleClass styleClass = this.parser.ParseStyleClass("class", style);
			foreach (KeyValuePair<string, string> keyValuePair in styleClass.Attributes)
			{
				styleProperties.RegisterProperty(keyValuePair.Key, keyValuePair.Value);
			}
		}

		readonly CssParser parser;
	}
}
