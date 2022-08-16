﻿using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ValueElement : ChartElementBase
	{
		public ValueElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "v";
			}
		}
	}
}
