using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class ClosePath : Operator
	{
		public override void Execute(BuildChar buildChar)
		{
			buildChar.CurrentPathFigure.IsClosed = true;
		}
	}
}
