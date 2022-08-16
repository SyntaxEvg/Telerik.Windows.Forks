using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class ColumnsElement : ConsecutiveElementBase
	{
		public ColumnsElement()
		{
			base.RegisterChildElement<ColumnElement>();
		}

		public override string ElementName
		{
			get
			{
				return "cols";
			}
		}

		public IEnumerable<ColumnElement> Columns
		{
			get
			{
				while (base.ReadToElement<ColumnElement>())
				{
					ColumnElement columnElement = base.CreateChildElement<ColumnElement>();
					yield return columnElement;
				}
				yield break;
			}
		}

		public ColumnElement CreateColumnElementWriter()
		{
			return base.CreateChildElement<ColumnElement>();
		}
	}
}
