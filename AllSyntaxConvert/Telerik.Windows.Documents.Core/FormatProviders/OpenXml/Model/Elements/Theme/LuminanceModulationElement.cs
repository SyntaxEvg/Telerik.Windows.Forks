using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class LuminanceModulationElement : ThemeElementBase
	{
		public LuminanceModulationElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("val", false));
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public int Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "lumMod";
			}
		}

		readonly IntOpenXmlAttribute value;
	}
}
