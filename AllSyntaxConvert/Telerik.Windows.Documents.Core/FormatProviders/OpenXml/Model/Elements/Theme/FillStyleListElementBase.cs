using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	abstract class FillStyleListElementBase : ThemeElementBase
	{
		public FillStyleListElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			for (int i = 0; i < 3; i++)
			{
				SolidFillElement solidFillElement = base.CreateElement<SolidFillElement>("solidFill");
				solidFillElement.SetDefaultProperties();
				yield return solidFillElement;
			}
			yield break;
		}
	}
}
