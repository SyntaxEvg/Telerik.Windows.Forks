﻿using System;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class ModifyClippingPathEvenOdd : ModifyClippingPathBase
	{
		public override string Name
		{
			get
			{
				return "W*";
			}
		}

		protected override FillRule FillRule
		{
			get
			{
				return FillRule.EvenOdd;
			}
		}
	}
}
