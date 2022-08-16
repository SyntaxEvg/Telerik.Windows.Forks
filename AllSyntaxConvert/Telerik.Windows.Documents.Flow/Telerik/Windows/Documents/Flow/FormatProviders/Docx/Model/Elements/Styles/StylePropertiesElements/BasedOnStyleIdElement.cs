﻿using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.StylePropertiesElements
{
	class BasedOnStyleIdElement : DocxRequiredValueElementBase<string>
	{
		public BasedOnStyleIdElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "basedOn";
			}
		}
	}
}
