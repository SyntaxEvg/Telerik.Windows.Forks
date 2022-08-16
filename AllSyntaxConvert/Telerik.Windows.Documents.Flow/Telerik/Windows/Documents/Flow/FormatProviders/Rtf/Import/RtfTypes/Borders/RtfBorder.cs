using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Borders
{
	class RtfBorder
	{
		public RtfBorder(bool isNone = false)
		{
			if (isNone)
			{
				this.thickness = 0f;
				this.thicknessModifier = 1f;
				this.borderStyle = Border.EmptyBorder.Style;
				this.color = Border.EmptyBorder.Color.LocalValue;
				return;
			}
			this.thickness = 0f;
			this.thicknessModifier = 1f;
			this.borderStyle = BorderStyle.Inherit;
			this.color = Border.DefaultBorder.Color.LocalValue;
		}

		public bool HasValue { get; set; }

		public void HandleTag(RtfTag tag, RtfImportContext context)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "brdrw")
				{
					this.thickness = Math.Max(Unit.TwipToDipF((double)tag.ValueAsNumber), 1f);
					return;
				}
				if (name == "brdrth")
				{
					this.thicknessModifier = 2f;
					this.borderStyle = BorderStyle.Single;
					return;
				}
				if (name == "brdrcf")
				{
					RtfColor rtfColor;
					if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
					{
						this.color = rtfColor.Color;
						return;
					}
					return;
				}
			}
			this.borderStyle = RtfHelper.BorderMapper.GetToValue(tag.Name);
		}

		public Border CreateBorder()
		{
			return new Border((double)(this.thickness * this.thicknessModifier), this.borderStyle, new ThemableColor(this.color));
		}

		public RtfBorder CreateCopy()
		{
			RtfBorder rtfBorder = new RtfBorder(false);
			if (this.HasValue)
			{
				rtfBorder.HasValue = true;
				rtfBorder.thickness = this.thickness;
				rtfBorder.borderStyle = this.borderStyle;
				rtfBorder.color = this.color;
				rtfBorder.thicknessModifier = this.thicknessModifier;
			}
			return rtfBorder;
		}

		float thickness;

		BorderStyle borderStyle;

		Color color;

		float thicknessModifier;
	}
}
