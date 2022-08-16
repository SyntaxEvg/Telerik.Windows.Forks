using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	class NonVisualDrawingPropertiesElement : OpenXmlElementBase
	{
		public NonVisualDrawingPropertiesElement(OpenXmlPartsManager partsManager, string elementName, OpenXmlNamespace ns)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlNamespace>(ns, "ns");
			this.elementName = elementName;
			this.ns = ns;
			this.id = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("id", true));
			this.name = base.RegisterAttribute<string>("name", true);
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

		public int Id
		{
			get
			{
				return this.id.Value;
			}
			set
			{
				this.id.Value = value;
			}
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		readonly string elementName;

		readonly OpenXmlNamespace ns;

		readonly IntOpenXmlAttribute id;

		readonly OpenXmlAttribute<string> name;
	}
}
