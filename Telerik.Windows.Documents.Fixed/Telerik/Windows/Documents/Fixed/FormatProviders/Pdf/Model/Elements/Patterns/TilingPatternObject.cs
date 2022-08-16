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
	class TilingPatternObject : PatternColorObject
	{
		public TilingPatternObject()
		{
			this.tiling = new Tiling();
		}

		public TilingPatternObject(Tiling tiling)
		{
			this.tiling = tiling;
			base.Matrix = tiling.Matrix;
		}

		public Tiling Tiling
		{
			get
			{
				return this.tiling;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			this.tiling.Write(writer, context);
		}

		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, PatternColor patternColor, Matrix transformMatrix)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PatternColor>(patternColor, "patternColor");
			Guard.ThrowExceptionIfNull<Matrix>(transformMatrix, "transformMatrix");
			TilingBase tilingBase = patternColor as TilingBase;
			this.tiling.CopyPropertiesFrom(context.Owner, this, tilingBase);
		}

		public override ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			if (this.tilingColor == null)
			{
				this.tilingColor = new Tiling();
				this.tilingColor.HorizontalSpacing = this.Tiling.XStep.Value;
				this.tilingColor.VerticalSpacing = this.Tiling.YStep.Value;
				this.tilingColor.TilingType = (TilingType)this.Tiling.TilingType.Value;
				Matrix m = base.CreateInitialMatrix(context.ContentRoot);
				this.tilingColor.Position = new MatrixPosition(base.Matrix.ToMatrix(reader, context.Owner).MultiplyBy(m.InverseMatrix()));
				this.tilingColor.BoundingBox = this.Tiling.BBox.ToRect(reader, context.Owner);
				this.Tiling.CopyPropertiesTo(context.Owner, this.Tiling, this.tilingColor);
			}
			if (this.Tiling.PaintType.Value == 2)
			{
				return new UncoloredTiling(this.tilingColor, base.UnderlyingColor.ToColor(reader, context) as SimpleColor);
			}
			return this.tilingColor;
		}

		readonly Tiling tiling;

		Tiling tilingColor;
	}
}
