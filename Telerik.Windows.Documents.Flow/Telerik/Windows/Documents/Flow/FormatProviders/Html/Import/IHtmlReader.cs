using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	interface IHtmlReader
	{
		bool BeginReadElement();

		void EndReadElement();

		bool HasChildElements();

		bool IsInsideSpanElement();

		bool MoveToNextAttribute();

		string GetAttributeName();

		string GetAttributeValue();

		bool MoveToNextChildElement();

		string GetElementName();

		HtmlElementType GetCurrentChildElementType();

		string GetCurrentChildElementName();

		string GetInnerText();
	}
}
