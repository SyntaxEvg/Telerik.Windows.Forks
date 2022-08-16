using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	abstract class StrokePathBase : PaintPathBase
	{
		protected override void ExecuteOverride(ContentStreamInterpreter interpreter, IPdfContentImportContext context, Path path)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Path>(path, "path");
			path.IsStroked = true;
			this.ExecuteStrokeOverride(interpreter, context, path);
		}

		protected abstract void ExecuteStrokeOverride(ContentStreamInterpreter interpreter, IPdfContentImportContext context, Path path);
	}
}
