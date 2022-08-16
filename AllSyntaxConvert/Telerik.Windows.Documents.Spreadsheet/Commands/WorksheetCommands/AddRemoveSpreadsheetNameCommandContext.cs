using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddRemoveSpreadsheetNameCommandContext : WorksheetCommandContextBase
	{
		public ISpreadsheetName Name
		{
			get
			{
				return this.name;
			}
		}

		public SpreadsheetNameCollectionScope Owner
		{
			get
			{
				return this.owner;
			}
		}

		public AddRemoveSpreadsheetNameCommandContext(SpreadsheetNameCollectionScope owner, ISpreadsheetName name)
			: base(owner.CurrentWorksheet)
		{
			Guard.ThrowExceptionIfNull<SpreadsheetNameCollectionScope>(owner, "owner");
			Guard.ThrowExceptionIfNull<ISpreadsheetName>(name, "name");
			this.owner = owner;
			this.name = name;
		}

		readonly SpreadsheetNameCollectionScope owner;

		readonly ISpreadsheetName name;
	}
}
