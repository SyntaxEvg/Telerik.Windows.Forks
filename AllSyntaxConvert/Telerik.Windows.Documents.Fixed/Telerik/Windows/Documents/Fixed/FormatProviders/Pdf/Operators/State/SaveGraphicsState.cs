﻿using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class SaveGraphicsState : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "q";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			interpreter.SaveGraphicState();
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context)
		{
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[0]);
		}
	}
}
