using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Copying
{
	class CopyContext
	{
		public CopyContext(Worksheet target, Worksheet source)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(target, "target");
			Guard.ThrowExceptionIfNull<Worksheet>(source, "source");
			this.targetWorksheet = target;
			this.sourceWorksheet = source;
			this.originalToRenamedStyleNames = new Dictionary<string, string>();
		}

		public Worksheet TargetWorksheet
		{
			get
			{
				return this.targetWorksheet;
			}
		}

		public Worksheet SourceWorksheet
		{
			get
			{
				return this.sourceWorksheet;
			}
		}

		public Dictionary<string, string> OriginalToRenamedStyleNames
		{
			get
			{
				return this.originalToRenamedStyleNames;
			}
		}

		public bool SpreadsheetNameExistsInTargetWorkbook { get; set; }

		public FloatingShapeBase SourceFloatingShape { get; set; }

		readonly Dictionary<string, string> originalToRenamedStyleNames;

		readonly Worksheet targetWorksheet;

		readonly Worksheet sourceWorksheet;
	}
}
