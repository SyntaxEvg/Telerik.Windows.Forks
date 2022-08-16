using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Core
{
	abstract class NamedObjectBase : INamedObject
	{
		protected NamedObjectBase(string name)
		{
			Guard.ThrowExceptionIfNull<string>(name, "name");
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
