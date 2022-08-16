using System;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	class CellStyleCollectionsMerger
	{
		public CellStyleCollectionsMerger(CellStyleCollection targetStyles, CellStyleCollection sourceStyles, CopyContext context)
		{
			Guard.ThrowExceptionIfNull<CellStyleCollection>(targetStyles, "targetStyles");
			Guard.ThrowExceptionIfNull<CellStyleCollection>(sourceStyles, "sourceStyles");
			Guard.ThrowExceptionIfNull<CopyContext>(context, "context");
			this.targetStyles = targetStyles;
			this.sourceStyles = sourceStyles;
			this.context = context;
		}

		public void Merge()
		{
			foreach (CellStyle cellStyle in this.sourceStyles)
			{
				switch (this.GetMergeOperationForStyle(cellStyle))
				{
				case CellStyleCollectionsMerger.MergeOperation.Add:
				{
					CellStyle cellStyle2 = this.targetStyles.Add(cellStyle.Name, cellStyle.Category, cellStyle.IsRemovable);
					cellStyle2.CopyPropertiesFrom(cellStyle);
					break;
				}
				case CellStyleCollectionsMerger.MergeOperation.RenameAndAdd:
				{
					string text = this.ComputeRenamedStyleName(cellStyle.Name);
					CellStyle cellStyle3 = this.targetStyles.Add(text, cellStyle.Category, cellStyle.IsRemovable);
					cellStyle3.CopyPropertiesFrom(cellStyle);
					this.context.OriginalToRenamedStyleNames.Add(cellStyle.Name, text);
					break;
				}
				}
			}
		}

		string ComputeRenamedStyleName(string styleName)
		{
			int num = 1;
			string text;
			do
			{
				text = string.Format("{0} {1}", styleName, num);
				num++;
			}
			while (this.targetStyles.Contains(text));
			return text;
		}

		CellStyleCollectionsMerger.MergeOperation GetMergeOperationForStyle(CellStyle style)
		{
			if (!this.targetStyles.Contains(style.Name))
			{
				return CellStyleCollectionsMerger.MergeOperation.Add;
			}
			CellStyle cellStyle = this.targetStyles[style.Name];
			if (!cellStyle.Equals(style))
			{
				return CellStyleCollectionsMerger.MergeOperation.RenameAndAdd;
			}
			return CellStyleCollectionsMerger.MergeOperation.None;
		}

		readonly CopyContext context;

		readonly CellStyleCollection targetStyles;

		readonly CellStyleCollection sourceStyles;

		enum MergeOperation
		{
			None,
			Add,
			RenameAndAdd
		}
	}
}
