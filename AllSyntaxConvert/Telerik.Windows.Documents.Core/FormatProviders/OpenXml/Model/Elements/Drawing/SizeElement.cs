using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class SizeElement : OpenXmlElementBase
	{
		public SizeElement(OpenXmlPartsManager partsManager, string elementName, OpenXmlNamespace ns)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlNamespace>(ns, "ns");
			this.elementName = elementName;
			this.ns = ns;
			this.extentHeight = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("cy", Converters.EmuOrUniversalMeasureToDipConverter, true));
			this.extentWidth = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("cx", Converters.EmuOrUniversalMeasureToDipConverter, true));
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

		public double ExtentWidth
		{
			get
			{
				return this.extentWidth.Value;
			}
			set
			{
				this.extentWidth.Value = value;
			}
		}

		public double ExtentHeight
		{
			get
			{
				return this.extentHeight.Value;
			}
			set
			{
				this.extentHeight.Value = value;
			}
		}

		readonly string elementName;

		readonly OpenXmlNamespace ns;

		readonly ConvertedOpenXmlAttribute<double> extentHeight;

		readonly ConvertedOpenXmlAttribute<double> extentWidth;
	}
}
