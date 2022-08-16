﻿using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class RLineCurve : Operator
	{
		public override void Execute(BuildChar interpreter)
		{
			OperandsCollection operands = interpreter.Operands;
			while (operands.Count > 6)
			{
				Operator.LineTo(interpreter, operands.GetFirstAsInt(), operands.GetFirstAsInt());
			}
			int firstAsInt = operands.GetFirstAsInt();
			int firstAsInt2 = operands.GetFirstAsInt();
			int firstAsInt3 = operands.GetFirstAsInt();
			int firstAsInt4 = operands.GetFirstAsInt();
			int firstAsInt5 = operands.GetFirstAsInt();
			int firstAsInt6 = operands.GetFirstAsInt();
			Operator.CurveTo(interpreter, firstAsInt, firstAsInt2, firstAsInt3, firstAsInt4, firstAsInt5, firstAsInt6);
			operands.Clear();
		}
	}
}
