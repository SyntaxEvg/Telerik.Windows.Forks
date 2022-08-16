using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class ClippingWriter : ContentElementWriter<Clipping>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, Clipping clipping)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Clipping>(clipping, "element");
			using (ContentStreamOperators.PushGraphicsState(writer, context))
			{
				if (clipping.Clip != null)
				{
					bool flag = !clipping.Transform.IsIdentity && clipping.Transform.HasInverseMatrix();
					if (flag)
					{
						ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, new MatrixPosition(clipping.Transform));
					}
					ContentElementWriters.ClippingGeometryWriter.Write(writer, context, clipping.Clip);
					if (flag)
					{
						ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, new MatrixPosition(clipping.Transform.InverseMatrix()));
					}
				}
				IEnumerable<ContentElementBase> clippingChildren = context.GetClippingChildren(clipping);
				foreach (ContentElementBase element in clippingChildren)
				{
					ContentElementWriterBase.WriteElement(writer, context, element);
				}
			}
		}
	}
}
