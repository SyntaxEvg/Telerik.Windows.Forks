using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet
{
	class MergedCellElementWriter : ConsecutiveElementBase
	{
		public MergedCellElementWriter()
		{
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<Ref>>(new ConvertedOpenXmlAttribute<Ref>("ref", Converters.RefConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "mergeCell";
			}
		}

		Ref Reference
		{
			set
			{
				this.reference.Value = value;
			}
		}

		public void Write(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			this.Reference = new Ref(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
			base.EnsureWritingStarted();
			base.EnsureWritingEnded();
		}

		readonly ConvertedOpenXmlAttribute<Ref> reference;
	}
}
