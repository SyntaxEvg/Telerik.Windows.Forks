using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	abstract class WrapTextElementBase : DrawingElementBase
	{
		public WrapTextElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.wrapText = new MappedOpenXmlAttribute<TextWrap>("wrapText", null, TypeMappers.TextWrapMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<TextWrap>>(this.wrapText);
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public TextWrap WrapText
		{
			get
			{
				return this.wrapText.Value;
			}
			set
			{
				this.wrapText.Value = value;
			}
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeWrapping shapeWrapping)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeWrapping>(shapeWrapping, "shapeWrapping");
			this.WrapText = shapeWrapping.TextWrap;
		}

		public void CopyPropertiesTo(IDocxImportContext context, ShapeWrapping shapeWrapping)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeWrapping>(shapeWrapping, "shapeWrapping");
			shapeWrapping.TextWrap = this.WrapText;
		}

		readonly MappedOpenXmlAttribute<TextWrap> wrapText;
	}
}
