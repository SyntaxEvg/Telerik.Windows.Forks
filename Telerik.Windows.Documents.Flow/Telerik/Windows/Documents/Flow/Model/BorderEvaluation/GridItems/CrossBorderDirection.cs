using System;

namespace Telerik.Windows.Documents.Flow.Model.BorderEvaluation.GridItems
{
	[Flags]
	enum CrossBorderDirection
	{
		None = 0,
		Left = 1,
		Top = 2,
		Right = 4,
		Bottom = 8
	}
}
