using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	class ShadingPatternObject : PatternColorObject
	{
		public ShadingPatternObject()
		{
			this.shading = base.RegisterReferenceProperty<Shading>(new PdfPropertyDescriptor("Shading", true));
		}

		public Shading Shading
		{
			get
			{
				return this.shading.GetValue();
			}
			set
			{
				this.shading.SetValue(value);
			}
		}

		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, PatternColor patternColor, Matrix transformMatrix)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PatternColor>(patternColor, "patternColor");
			Guard.ThrowExceptionIfNull<Matrix>(transformMatrix, "transformMatrix");
			Gradient gradient = patternColor as Gradient;
			this.Shading = Shading.CreateInstance((int)gradient.GradientType);
			this.Shading.CopyPropertiesFrom(context, gradient);
		}

		public override ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			return this.Shading.ToColor(reader, context, base.Matrix.ToMatrix(reader, context.Owner));
		}

		readonly ReferenceProperty<Shading> shading;
	}
}
