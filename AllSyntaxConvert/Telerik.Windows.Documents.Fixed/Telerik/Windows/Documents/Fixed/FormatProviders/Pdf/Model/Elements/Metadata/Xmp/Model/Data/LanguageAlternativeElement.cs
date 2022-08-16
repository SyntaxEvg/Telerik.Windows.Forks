using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data
{
	class LanguageAlternativeElement : XmpDataElementBase
	{
		public LanguageAlternativeElement(string defaultLanguageValue)
		{
			Guard.ThrowExceptionIfNull<string>(defaultLanguageValue, "defaultLanguageValue");
			this.defaultLanguageValue = defaultLanguageValue;
		}

		public override string Name
		{
			get
			{
				return "Alt";
			}
		}

		protected override IEnumerable<XmpElementBase> EnumerateInnerElements()
		{
			yield return new ListItemElement(this.defaultLanguageValue, "x-default");
			yield break;
		}

		readonly string defaultLanguageValue;
	}
}
