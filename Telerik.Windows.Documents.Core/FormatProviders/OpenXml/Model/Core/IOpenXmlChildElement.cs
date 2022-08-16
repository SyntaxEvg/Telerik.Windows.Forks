using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	interface IOpenXmlChildElement
	{
		string ElementName { get; }

		string UniquePoolId { get; }

		bool HasElement { get; }

		void SetElement(OpenXmlElementBase element);

		OpenXmlElementBase GetElement();

		void ClearElement();
	}
}
