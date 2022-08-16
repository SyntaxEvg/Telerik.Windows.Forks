using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	abstract class OpenXmlPartRootElementBase : OpenXmlElementBase
	{
		public OpenXmlPartRootElementBase(OpenXmlPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			base.Part = part;
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}
	}
}
