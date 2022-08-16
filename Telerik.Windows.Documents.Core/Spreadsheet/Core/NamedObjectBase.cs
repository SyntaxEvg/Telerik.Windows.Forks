using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	public abstract class NamedObjectBase : INamedObject
	{
		protected NamedObjectBase(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		readonly string name;
	}
}
