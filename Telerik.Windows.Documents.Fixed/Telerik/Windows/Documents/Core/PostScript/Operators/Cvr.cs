﻿using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Cvr : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return x;
		}
	}
}
