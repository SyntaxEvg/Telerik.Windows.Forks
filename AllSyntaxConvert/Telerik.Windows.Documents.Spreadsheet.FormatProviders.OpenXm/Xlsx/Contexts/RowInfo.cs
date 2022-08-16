using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class RowInfo
	{
		public RowInfo()
		{
		}

		public RowInfo(int rowIndex, Rows rows)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfNull<Rows>(rows, "rows");
			this.RowIndex = new int?(rowIndex);
			ValueBox<RowHeight> valueInternal = rows.PropertyBag.GetPropertyValueCollection<RowHeight>(RowsPropertyBag.HeightProperty).GetValueInternal((long)rowIndex);
			this.IsDefault = valueInternal == null;
			RowHeight rowHeight = (this.IsDefault ? RowsPropertyBag.HeightProperty.DefaultValue : valueInternal.Value);
			this.Height = new double?(rowHeight.Value);
			this.IsCustom = rowHeight.IsCustom;
			this.OutlineLevel = rows.PropertyBag.GetPropertyValueCollection<int>(RowColumnPropertyBagBase.OutlineLevelProperty).GetValue((long)rowIndex);
			this.Hidden = rows.PropertyBag.GetPropertyValueCollection<bool>(RowColumnPropertyBagBase.HiddenProperty).GetValue((long)rowIndex);
		}

		public int? RowIndex
		{
			get
			{
				return this.rowIndex;
			}
			set
			{
				if (value != null)
				{
					Guard.ThrowExceptionIfInvalidRowIndex(value.Value);
				}
				this.rowIndex = value;
			}
		}

		public bool IsCustom { get; set; }

		public bool IsDefault { get; set; }

		public double? Height { get; set; }

		public bool Hidden { get; set; }

		public int OutlineLevel { get; set; }

		public int StyleIndex { get; set; }

		public override bool Equals(object obj)
		{
			RowInfo rowInfo = (RowInfo)obj;
			if (this.RowIndex == rowInfo.RowIndex && this.IsCustom == rowInfo.IsCustom && this.IsDefault == rowInfo.IsDefault)
			{
				double? height = this.Height;
				double? height2 = rowInfo.Height;
				if (height.GetValueOrDefault() == height2.GetValueOrDefault() && height != null == (height2 != null) && this.Hidden == rowInfo.Hidden && this.OutlineLevel == rowInfo.OutlineLevel)
				{
					return this.StyleIndex == rowInfo.StyleIndex;
				}
			}
			return false;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.RowIndex.GetHashCode(), this.IsCustom.GetHashCode(), this.IsDefault.GetHashCode(), this.Height.GetHashCode(), this.Hidden.GetHashCode(), this.OutlineLevel.GetHashCode(), this.StyleIndex.GetHashCode());
		}

		int? rowIndex;
	}
}
