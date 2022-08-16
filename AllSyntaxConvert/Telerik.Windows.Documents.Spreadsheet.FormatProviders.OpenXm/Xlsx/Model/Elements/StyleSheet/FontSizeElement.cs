﻿using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class FontSizeElement : RequiredValueElementBase<double>
	{
		public FontSizeElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "sz";
			}
		}
	}
}
