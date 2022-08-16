using System;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class SheetViewStateBase : ISheetViewState
	{
		public ThemableColor TabColor
		{
			get
			{
				return this.tabColor;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ThemableColor>(value, "value");
				this.tabColor = value;
			}
		}

		public bool IsInvalidated
		{
			get
			{
				return this.isInvalidated;
			}
			set
			{
				Guard.ThrowExceptionIfNull<bool>(value, "value");
				this.isInvalidated = value;
			}
		}

		protected SheetViewStateBase()
		{
			this.TabColor = new ThemableColor(ColorsHelper.HexStringToColor("#FFE4E8EB"));
		}

		ThemableColor tabColor;

		bool isInvalidated;
	}
}
