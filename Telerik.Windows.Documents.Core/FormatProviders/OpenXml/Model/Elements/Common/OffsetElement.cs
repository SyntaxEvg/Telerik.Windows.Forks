using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	class OffsetElement : OpenXmlElementBase
	{
		public OffsetElement(OpenXmlPartsManager partsManager, string elementName, OpenXmlNamespace ns)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlNamespace>(ns, "ns");
			this.elementName = elementName;
			this.ns = ns;
			this.x = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("x", true));
			this.y = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("y", true));
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

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public int X
		{
			get
			{
				return this.x.Value;
			}
			set
			{
				this.x.Value = value;
			}
		}

		public int Y
		{
			get
			{
				return this.y.Value;
			}
			set
			{
				this.y.Value = value;
			}
		}

		readonly IntOpenXmlAttribute x;

		readonly IntOpenXmlAttribute y;

		readonly string elementName;

		readonly OpenXmlNamespace ns;
	}
}
