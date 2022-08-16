using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	class TilingPatternObject : global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.PatternColorObject
	{
		public TilingPatternObject()
		{
			this.tiling = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.Tiling();
		}

		public TilingPatternObject(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.Tiling tiling)
		{
			this.tiling = tiling;
			base.Matrix = tiling.Matrix;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.Tiling Tiling
		{
			get
			{
				return this.tiling;
			}
		}

		public override void Write(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter writer, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context)
		{
			this.tiling.Write(writer, context);
		}

		protected override void CopyPropertiesFromOverride(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext context, global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.PatternColor patternColor, global::System.Windows.Media.Matrix transformMatrix)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext>(context, "context");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.PatternColor>(patternColor, "patternColor");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::System.Windows.Media.Matrix>(transformMatrix, "transformMatrix");
			global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.TilingBase tilingBase = patternColor as global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.TilingBase;
			this.tiling.CopyPropertiesFrom(context.Owner, this, tilingBase);
		}

		public override global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase ToColor(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfContentImportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader>(reader, "reader");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfContentImportContext>(context, "context");
			if (this.tilingColor == null)
			{
				this.tilingColor = new global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.Tiling();
				this.tilingColor.HorizontalSpacing = this.Tiling.XStep.Value;
				this.tilingColor.VerticalSpacing = this.Tiling.YStep.Value;
				this.tilingColor.TilingType = (global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.TilingType)this.Tiling.TilingType.Value;
				global::System.Windows.Media.Matrix m = base.CreateInitialMatrix(context.ContentRoot);
				this.tilingColor.Position = new global::Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition(base.Matrix.ToMatrix(reader, context.Owner).MultiplyBy(m.InverseMatrix()));
				this.tilingColor.BoundingBox = this.Tiling.BBox.ToRect(reader, context.Owner);
				this.Tiling.CopyPropertiesTo(context.Owner, this.Tiling, this.tilingColor);
			}
			if (this.Tiling.PaintType.Value == 2)
			{
				return new global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.UncoloredTiling(this.tilingColor, base.UnderlyingColor.ToColor(reader, context) as global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.SimpleColor);
			}
			return this.tilingColor;
		}

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.Tiling tiling;

		private global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.Tiling tilingColor;
	}
}
