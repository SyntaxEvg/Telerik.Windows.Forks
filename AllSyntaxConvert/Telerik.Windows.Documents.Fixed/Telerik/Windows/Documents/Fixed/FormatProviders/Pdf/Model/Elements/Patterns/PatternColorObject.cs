using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	abstract class PatternColorObject : ColorObjectBase
	{
		public PatternColorObject()
		{
			this.matrix = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Matrix"), PdfArray.MatrixIdentity);
		}

		public PdfArray Matrix
		{
			get
			{
				return this.matrix.GetValue();
			}
			set
			{
				this.matrix.SetValue(value);
			}
		}

		public ColorObjectBase UnderlyingColor { get; set; }

		public static PatternColorObject CreateInstance(PdfInt type)
		{
			Guard.ThrowExceptionIfNull<PdfInt>(type, "type");
			return PatternColorObject.CreateInstance(type.Value);
		}

		public static PatternColorObject CreateInstance(int type)
		{
			switch (type)
			{
			case 1:
				return new TilingPatternObject();
			case 2:
				return new ShadingPatternObject();
			default:
				throw new NotSupportedException("Pattern type is not supported.");
			}
		}

		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, ColorBase color)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ColorBase>(color, "color");
			RadFixedPage element = context.ContentRoot as RadFixedPage;
			PatternColor patternColor = (PatternColor)color;
			Matrix matrix = this.CreateInitialMatrix(element);
			this.Matrix = patternColor.Position.Matrix.MultiplyBy(matrix).ToPdfArray();
			this.CopyPropertiesFromOverride(context, patternColor, matrix);
		}

		protected Matrix CreateInitialMatrix(IContentRootElement element)
		{
			RadFixedPage radFixedPage = element as RadFixedPage;
			Matrix result = default(Matrix);
			if (radFixedPage != null)
			{
				result = PdfExportContext.CalculateDipToPdfPointTransformation(radFixedPage);
			}
			return result;
		}

		protected abstract void CopyPropertiesFromOverride(IPdfContentExportContext context, PatternColor patternColor, Matrix transformMatrix);

		readonly DirectProperty<PdfArray> matrix;
	}
}
