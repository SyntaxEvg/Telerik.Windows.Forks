using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data
{
	class SequenceElement : XmpDataElementBase
	{
		public SequenceElement(params string[] sequence)
		{
			Guard.ThrowExceptionIfNull<string[]>(sequence, "sequence");
			this.sequence = sequence;
		}

		public override string Name
		{
			get
			{
				return "Seq";
			}
		}

		protected override IEnumerable<XmpElementBase> EnumerateInnerElements()
		{
			foreach (string item in this.sequence)
			{
				yield return new ListItemElement(item);
			}
			yield break;
		}

		readonly string[] sequence;
	}
}
