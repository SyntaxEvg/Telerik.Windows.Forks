using System;
using Telerik.Documents.SpreadsheetStreaming.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	abstract class PartBase : EntityBase
	{
		public PartBase(string name)
		{
			this.name = name;
		}

		public abstract string ContentType { get; }

		protected abstract string PartPath { get; }

		public string GetPartFullPath()
		{
			return string.Format(this.PartPath, this.name);
		}

		readonly string name;
	}
}
