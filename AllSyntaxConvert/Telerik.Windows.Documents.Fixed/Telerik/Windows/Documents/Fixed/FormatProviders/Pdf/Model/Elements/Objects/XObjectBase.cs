using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects
{
	abstract class XObjectBase : PdfStreamObjectBase
	{
		public abstract XObjectType XObjectType { get; }
	}
}
