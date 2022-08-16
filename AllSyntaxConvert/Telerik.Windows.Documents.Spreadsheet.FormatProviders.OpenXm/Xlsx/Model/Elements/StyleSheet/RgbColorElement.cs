using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class RgbColorElement : StyleSheetElementBase
	{
		public RgbColorElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.rgb = base.RegisterAttribute<ConvertedOpenXmlAttribute<UnsignedIntHex>>(new ConvertedOpenXmlAttribute<UnsignedIntHex>("rgb", Converters.UnsignedIntHexConverter, false));
		}

		public UnsignedIntHex Rgb
		{
			get
			{
				return this.rgb.Value;
			}
			set
			{
				this.rgb.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "rgbColor";
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			context.StyleSheet.IndexedColorTable.Add(this.Rgb.Color);
		}

		readonly ConvertedOpenXmlAttribute<UnsignedIntHex> rgb;
	}
}
