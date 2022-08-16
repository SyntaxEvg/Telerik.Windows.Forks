using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class OneDimentionOffsetElement : OpenXmlElementBase
	{
		public OneDimentionOffsetElement(OpenXmlPartsManager partsManager, string elementName, OpenXmlNamespace ns)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlNamespace>(ns, "ns");
			this.elementName = elementName;
			this.ns = ns;
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		readonly string elementName;

		readonly OpenXmlNamespace ns;
	}
}
