using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class GraphicsStatePusher : IDisposable
	{
		public GraphicsStatePusher(PdfWriter writer, IPdfContentExportContext context)
		{
			this.writer = writer;
			this.context = context;
			this.SaveState();
		}

		public void Dispose()
		{
			this.RestoreGraphicsState();
		}

		void SaveState()
		{
			ContentStreamOperators.SaveGraphicsStateOperator.Write(this.writer, this.context);
		}

		void RestoreGraphicsState()
		{
			ContentStreamOperators.RestoreGraphicsStateOperator.Write(this.writer, this.context);
		}

		readonly PdfWriter writer;

		readonly IPdfContentExportContext context;
	}
}
