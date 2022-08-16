using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	interface IShapeSourceFactory<T> where T : FloatingShapeBase
	{
		IShapeSource GetShapeSource(T shape);
	}
}
