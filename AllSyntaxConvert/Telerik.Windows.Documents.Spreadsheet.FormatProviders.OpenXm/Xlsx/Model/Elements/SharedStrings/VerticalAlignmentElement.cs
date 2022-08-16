﻿using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings
{
	class VerticalAlignmentElement : RequiredValueElementBase<string>
	{
		public VerticalAlignmentElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "vertAlign";
			}
		}
	}
}
