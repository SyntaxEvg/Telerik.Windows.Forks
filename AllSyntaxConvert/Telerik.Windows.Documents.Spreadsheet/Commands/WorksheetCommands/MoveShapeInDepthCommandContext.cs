using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class MoveShapeInDepthCommandContext : WorksheetCommandContextBase
	{
		public IEnumerable<FloatingShapeBase> Shapes
		{
			get
			{
				return this.shapes;
			}
		}

		public Dictionary<FloatingShapeBase, int> OldShapeIndices
		{
			get
			{
				return this.oldShapeIndices;
			}
		}

		public MoveShapeInDepthCommandContext(Worksheet worksheet, IEnumerable<FloatingShapeBase> shapes)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
			this.shapes = shapes;
			this.oldShapeIndices = new Dictionary<FloatingShapeBase, int>();
		}

		readonly Dictionary<FloatingShapeBase, int> oldShapeIndices;

		readonly IEnumerable<FloatingShapeBase> shapes;
	}
}
